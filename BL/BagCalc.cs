using Core.Interfaces;

namespace BL
{
    public class BagCalc
    {
        private readonly ITestConnector _connector;

        public BagCalc(ITestConnector connector)
        {
            _connector = connector;
        }
        public async Task<Dictionary<string, Dictionary<string, decimal>>>Calculate()
        {
            var bag = new Dictionary<string, decimal>
            {
                { "BTC", 1 },
                { "XRP", 15000 },
                { "XMR", 50 },
                { "DSH", 30 }
            };

            var targetCurrencies = new[] { "USD", "BTC", "XRP", "XMR", "DSH" };

            var prices = new Dictionary<string, decimal>();

            foreach (var from in bag.Keys) 
            {
                foreach (var currency in targetCurrencies) 
                {
                    if (from == currency)
                    {
                        prices[$"{from}/{currency}"] = 1;
                        continue;
                    }
                    try
                    {
                        var ticker = await _connector.GetTicker($"t{from}{currency}");
                        prices[$"{from}/{currency}"] = ticker.LastPrice;
                    }
                    catch
                    {
                        var ticker1 = await _connector.GetTicker($"t{from}USD");
                        var ticker2 = await _connector.GetTicker($"t{currency}USD");
                        prices[$"{from}/{currency}"] = ticker1.LastPrice / ticker2.LastPrice;
                    }
                }
            }

            var result = new Dictionary<string, Dictionary<string, decimal>>();
            foreach (var currency in targetCurrencies)
            {
                result[currency] = new Dictionary<string, decimal>();

                decimal total = 0;
                foreach (var asset in bag)
                {
                    var key = $"{asset.Key}/{currency}";
                    if (prices.ContainsKey(key))
                    {
                        var converted = asset.Value * prices[key];
                        result[currency][asset.Key] = converted;
                        total += converted;
                    }
                }

                result[currency]["Total"] = total;
            }
            return result;
        }
    }
}
