using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Websocket.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Models;

namespace Infrastructure
{
    public class BitWsClient
    {
        private readonly Uri _url = new Uri("wss://api-pub.bitfinex.com/ws/2");
        private WebsocketClient _client;

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;

        public async Task SubscribeTrades(string pair)
        {
            _client = new WebsocketClient(_url);

            _client.MessageReceived
                .Where(msg => !string.IsNullOrEmpty(msg.Text))
                .Subscribe(msg => HandleTradeMessage(msg.Text, pair));

            await _client.Start();

            var subscribeMessage = new
            {
                @event = "subscribe",
                channel = "trades",
                symbol = pair
            };

            string jsonMessage = JsonConvert.SerializeObject(subscribeMessage);
            _client.Send(jsonMessage);
        }

        private void HandleTradeMessage(string message, string pair)
        {
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
                    Pair = pair
                };

                if (trade.Side == "buy")
                {
                    NewBuyTrade?.Invoke(trade);
                }
                else
                {
                    NewSellTrade?.Invoke(trade);
                }
            }
        }

        public void UnsubscribeTrades(string pair)
        {
            var unsubscribeMessage = new
            {
                @event = "unsubscribe",
                channel = "trades",
                symbol = pair
            };

            string jsonMessage = JsonConvert.SerializeObject(unsubscribeMessage);
            _client.Send(jsonMessage);
        }

        public void Disconnect()
        {
            _client.Dispose();
        }
    }
}
