using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBitRestClient
    {
        Task<Ticker> GetTicker(string pair);
        Task<IEnumerable<Trade>>GetTrades(string pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandles(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);

    }
}

