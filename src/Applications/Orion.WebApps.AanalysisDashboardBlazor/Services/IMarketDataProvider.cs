using Orion.WebApps.AanalysisDashboardBlazor.Interfaces;
using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class YahooFinanceProvider : IMarketDataProvider
    {
        private readonly ILogger<YahooFinanceProvider> _logger;

        public YahooFinanceProvider(ILogger<YahooFinanceProvider> logger)
        {
            _logger = logger;
        }

        public async Task<List<MarketData>> GetDataAsync(string symbol, string interval, string period)
        {
            try
            {
                // Using YahooFinanceApi
                var history = await YahooFinanceApi.Yahoo.GetHistoricalAsync(symbol,
                    start: DateTime.Now.AddDays(-GetDaysFromPeriod(period)),
                    end: DateTime.Now,
                    interval: GetYahooInterval(interval));

                return history.Select(h => new MarketData
                {
                    Timestamp = h.DateTime,
                    Open = (decimal)h.Open,
                    High = (decimal)h.High,
                    Low = (decimal)h.Low,
                    Close = (decimal)h.Close,
                    Volume = h.Volume
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching data for {symbol}");
                return new List<MarketData>();
            }
        }

        private int GetDaysFromPeriod(string period)
        {
            return period switch
            {
                "5d" => 5,
                "1mo" => 30,
                "3mo" => 90,
                "6mo" => 180,
                "1y" => 365,
                _ => 90
            };
        }

        private string GetYahooInterval(string interval)
        {
            return interval switch
            {
                "1m" => "1m",
                "5m" => "5m",
                "15m" => "15m",
                "30m" => "30m",
                "1h" => "1h",
                "1d" => "1d",
                "1wk" => "1wk",
                _ => "1d"
            };
        }
    }
}