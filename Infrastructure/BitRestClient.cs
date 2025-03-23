using RestSharp;
using Core.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class BitRestClient
    {
        private readonly IMapper _mapper;
        public BitRestClient(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<Candle>> GetCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var options = new RestClientOptions($"https://api-pub.bitfinex.com/v2/candles/trade:{{periodInSec}}m:{{pair}}/hist");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception("API call failed: " + response.ErrorMessage);
            }

            var rawData = JsonConvert.DeserializeObject<List<List<object>>>(response.Content);

            var candles = rawData.Select(data =>
            {
                var candle = _mapper.Map<Candle>(data);
                candle.Pair = pair;
                candle.TotalPrice = candle.ClosePrice * candle.TotalVolume;
                return candle;
            }).ToList();

            return candles;
        }
        public async Task<List<Trade>> GetTrades(string pair, int maxCount)
        {
            var options = new RestClientOptions("https://api-pub.bitfinex.com/v2/trades/{pair}/hist?limit={maxCount}&sort=-1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);


            if (!response.IsSuccessful)
            {
                throw new Exception("API call failed: " + response.ErrorMessage);
            }

            var rawData = JsonConvert.DeserializeObject<List<List<object>>>(response.Content);

            var trades = rawData.Select(data =>
            {
                var trade = _mapper.Map<Trade>(data);
                trade.Pair = pair;
                trade.Side = trade.Amount > 0 ? "buy" : "sell";
                return trade;
            }).ToList();

            return trades;
        }

        public async Task<Ticker> GetTikcker(string pair)
        {
            var options = new RestClientOptions($"https://api-pub.bitfinex.com/v2/ticker/{pair}");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception("API call failed: " + response.ErrorMessage);
            }

            var rawData = JsonConvert.DeserializeObject<List<object>>(response.Content);

            var ticker = _mapper.Map<Ticker>(rawData);
            ticker.Pair = pair;

            return ticker;
        }
    }
}
