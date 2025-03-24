using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Models;
using Websocket.Client;
using Core.Interfaces;

namespace Infrastructure.Clients
{
    public class BitWsClient : IBitWsClient
    {
        private readonly Uri _url = new Uri("wss://api-pub.bitfinex.com/ws/2");
        private WebsocketClient _client;

        private string _tradePair;
        private string _candlePair;
        private int _maxTradeCount;
        private int _curTradeCount;

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        public void SubscribeTrades(string pair, int maxCount = 100)
        {
            _tradePair = pair;
            _maxTradeCount = maxCount;
            _curTradeCount = 0;

            _client = new WebsocketClient(_url);
            _client.MessageReceived
                .Where(msg => !string.IsNullOrEmpty(msg.Text))
                .Subscribe(msg => HandleTradeMessage(msg.Text));

            _client.Start().Wait();

            var subscribeMessage = new
            {
                @event = "subscribe",
                channel = "trades",
                symbol = pair
            };

            string jsonMessage = JsonConvert.SerializeObject(subscribeMessage);
            _client.Send(jsonMessage);
        }

        private void HandleTradeMessage(string message)
        {
            if (_curTradeCount == _maxTradeCount)
            {
                UnsubscribeTrades(_tradePair);
                return;
            }
            if (message.StartsWith("{")) return;

            var parsed = JsonConvert.DeserializeObject<JArray>(message);
            if (parsed.Count > 2 && (parsed[1]?.ToString() == "te" || parsed[1]?.ToString() == "tu"))
            {
                var tradeData = parsed[2].ToObject<List<object>>();
                var amount = Convert.ToDecimal(tradeData[2]);

                var trade = new Trade
                {
                    Id = tradeData[0].ToString(),
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(tradeData[1])),
                    Amount = Math.Abs(amount),
                    Side = amount > 0 ? "buy" : "sell",
                    Price = Convert.ToDecimal(tradeData[3]),
                    Pair = _tradePair
                };

                if (amount > 0)
                    NewBuyTrade?.Invoke(trade);
                else
                    NewSellTrade?.Invoke(trade);
            }
        }

        public void UnsubscribeTrades(string pair)
        {
            _client.Dispose();
        }

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            _candlePair = pair;
            _client = new WebsocketClient(_url);
            _client.MessageReceived
                .Where(msg => !string.IsNullOrEmpty(msg.Text))
                .Subscribe(msg => HandleCandleMessage(msg.Text));

            _client.Start().Wait();

            var subscribeMessage = new
            {
                @event = "subscribe",
                channel = "candles",
                key = $"trade:{periodInSec / 60}m:{pair}:a{count}:p{from}:p{to}"
            };

            string jsonMessage = JsonConvert.SerializeObject(subscribeMessage);
            _client.Send(jsonMessage);
        }

        private void HandleCandleMessage(string message)
        {
            if (message.StartsWith("{")) return;

            var parsed = JsonConvert.DeserializeObject<JArray>(message);
            if (parsed.Count > 1 && parsed[1] is JArray candleData)
            {
                var data = candleData.ToObject<List<object>>();

                var candle = new Candle
                {
                    OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(data[0])),
                    OpenPrice = Convert.ToDecimal(data[1]),
                    ClosePrice = Convert.ToDecimal(data[2]),
                    HighPrice = Convert.ToDecimal(data[3]),
                    LowPrice = Convert.ToDecimal(data[4]),
                    TotalVolume = Convert.ToDecimal(data[5]),
                    TotalPrice = Convert.ToDecimal(data[2]) * Convert.ToDecimal(data[5]),
                    Pair = _candlePair
                };

                CandleSeriesProcessing?.Invoke(candle);
            }
        }

        public void UnsubscribeCandles(string pair)
        {
            _client.Dispose();
        }
    }
}
