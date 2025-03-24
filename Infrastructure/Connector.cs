using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Clients;

namespace Infrastructure
{
    public class Connector : ITestConnector
    {
        private readonly BitRestClient _bitRestClient;
        private readonly BitWsClient _bitWsClient;
        public Connector(IMapper mapper)
        {
            _bitRestClient = new(mapper);
            _bitWsClient = new();
        }
        #region REST
        public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount) 
        {
            return _bitRestClient.GetTrades(pair, maxCount);
        }
        public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            return _bitRestClient.GetCandles(pair, periodInSec, from, to, count);
        }
        #endregion

        #region WebSocket
        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;

        public void SubscribeTrades(string pair, int maxCount = 100)
        {
            _bitWsClient.SubscribeTrades(pair, maxCount);
        }

        public void UnsubscribeTrades(string pair)
        {
            _bitWsClient.UnsubscribeTrades(pair);
        }

        public event Action<Candle> CandleSeriesProcessing;

        public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            _bitWsClient.SubscribeCandles(pair, periodInSec, from, to, count);
        }

        public void UnsubscribeCandles(string pair)
        {
            _bitWsClient.UnsubscribeCandles(pair);
        }

        #endregion
    }
}
