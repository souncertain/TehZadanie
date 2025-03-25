using Core.Models;

namespace Core.Interfaces
{
    public interface IBitRestClient
    {
        Task<Ticker> GetTicker(string pair);
        Task<IEnumerable<Trade>>GetTrades(string pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);

    }
}

