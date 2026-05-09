//using Orion.WebApps.AnalysisDashboard.Interfaces;
//using Microsoft.Extensions.Logging;
//using YahooFinanceApi;
//using Orion.WebApps.AnalysisDashboard.Models;

//namespace Orion.WebApps.AnalysisDashboard.Services
//{
//    /// <summary>
//    /// Market data provider that fetches financial data from Yahoo Finance API
//    /// Implements IMarketDataProvider interface for standardized data access
//    /// </summary>
//    public class YahooFinanceProvider : IMarketDataProvider
//    {
//        private readonly ILogger<YahooFinanceProvider> _logger;

//        // Maximum retry attempts for failed requests
//        private const int MaxRetryAttempts = 3;

//        // Delay between retries in milliseconds
//        private const int RetryDelayMilliseconds = 1000;

//        public YahooFinanceProvider(ILogger<YahooFinanceProvider> logger)
//        {
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        /// <summary>
//        /// Fetches historical market data from Yahoo Finance
//        /// </summary>
//        /// <param name="symbol">Trading symbol (e.g., "AAPL", "EURUSD=X")</param>
//        /// <param name="interval">Time interval between data points (1m, 5m, 15m, 30m, 1h, 1d, 1wk)</param>
//        /// <param name="period">Time period to fetch (5d, 1mo, 3mo, 6mo, 1y)</param>
//        /// <returns>List of market data points, or empty list if fetch fails</returns>
//        public async Task<List<MarketData>> GetDataAsync(string symbol, string interval, string period)
//        {
//            // Validate input parameters
//            if (string.IsNullOrWhiteSpace(symbol))
//            {
//                _logger.LogWarning("GetDataAsync called with null or empty symbol");
//                return new List<MarketData>();
//            }

//            if (string.IsNullOrWhiteSpace(interval))
//            {
//                _logger.LogWarning("GetDataAsync called with null or empty interval for symbol {Symbol}", symbol);
//                return new List<MarketData>();
//            }

//            if (string.IsNullOrWhiteSpace(period))
//            {
//                _logger.LogWarning("GetDataAsync called with null or empty period for symbol {Symbol}", symbol);
//                return new List<MarketData>();
//            }

//            try
//            {
//                _logger.LogInformation("Fetching data for {Symbol} with interval {Interval} and period {Period}",
//                    symbol, interval, period);

//                // Calculate date range based on period
//                var days = GetDaysFromPeriod(period);
//                var startDate = DateTime.Now.AddDays(-days);
//                var endDate = DateTime.Now;

//                // Get Yahoo interval as Period enum
//                var yahooInterval = GetYahooIntervalEnum(interval);

//                _logger.LogDebug("Fetching {Symbol} from {StartDate} to {EndDate} with interval {Interval}",
//                    symbol, startDate, endDate, yahooInterval);

//                // Fetch data from Yahoo Finance with retry logic
//                var history = await FetchWithRetryAsync(symbol, startDate, endDate, yahooInterval);

//                if (history == null || !history.Any())
//                {
//                    _logger.LogWarning("No data returned from Yahoo Finance for {Symbol}", symbol);
//                    return new List<MarketData>();
//                }

//                // Convert Yahoo Finance data to our MarketData model
//                var marketData = history.Select(h => new MarketData
//                {
//                    Timestamp = h.DateTime,
//                    Open = (decimal)h.Open,
//                    High = (decimal)h.High,
//                    Low = (decimal)h.Low,
//                    Close = (decimal)h.Close,
//                    Volume = h.Volume
//                }).ToList();

//                _logger.LogInformation("Successfully fetched {Count} data points for {Symbol}",
//                    marketData.Count, symbol);

//                return marketData;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error fetching data for {Symbol} with interval {Interval} and period {Period}",
//                    symbol, interval, period);
//                return new List<MarketData>();
//            }
//        }

//        /// <summary>
//        /// Fetches data from Yahoo Finance with automatic retry logic
//        /// </summary>
//        /// <param name="symbol">Trading symbol</param>
//        /// <param name="startDate">Start date for historical data</param>
//        /// <param name="endDate">End date for historical data</param>
//        /// <param name="interval">Yahoo Finance period interval enum</param>
//        /// <returns>Collection of candles or null if all retries fail</returns>
//        private async Task<IEnumerable<Candle>?> FetchWithRetryAsync(
//            string symbol, DateTime startDate, DateTime endDate, Period interval)
//        {
//            int attempt = 0;
//            while (attempt < MaxRetryAttempts)
//            {
//                try
//                {
//                    return await Yahoo.GetHistoricalAsync(symbol,
//                        startTime: startDate,
//                        endTime: endDate,
//                        period: interval);
//                }
//                catch (Exception ex) when (attempt < MaxRetryAttempts - 1)
//                {
//                    attempt++;
//                    _logger.LogWarning(ex,
//                        "Attempt {Attempt}/{MaxAttempts} failed for {Symbol}. Retrying in {Delay}ms...",
//                        attempt, MaxRetryAttempts, symbol, RetryDelayMilliseconds * attempt);

//                    await Task.Delay(RetryDelayMilliseconds * attempt); // Exponential backoff
//                }
//            }

//            _logger.LogError("All {MaxAttempts} retry attempts failed for {Symbol}", MaxRetryAttempts, symbol);
//            return null;
//        }

//        /// <summary>
//        /// Converts period string to number of days for historical data fetch
//        /// </summary>
//        /// <param name="period">Period string (5d, 1mo, 3mo, 6mo, 1y)</param>
//        /// <returns>Number of days corresponding to the period</returns>
//        private int GetDaysFromPeriod(string period)
//        {
//            return period?.ToLowerInvariant() switch
//            {
//                "5d" => 5,
//                "1mo" => 30,      // Approximate month
//                "3mo" => 90,      // 3 months
//                "6mo" => 180,     // 6 months
//                "1y" => 365,      // 1 year
//                "2y" => 730,      // 2 years
//                "5y" => 1825,     // 5 years
//                _ => 90           // Default to 3 months
//            };
//        }

//        /// <summary>
//        /// Converts our interval string to Yahoo Finance API Period enum
//        /// </summary>
//        /// <param name="interval">Interval string (1m, 5m, 15m, 30m, 1h, 1d, 1wk, 1mo)</param>
//        /// <returns>Yahoo Finance Period enum value</returns>
//        private Period GetYahooIntervalEnum(string interval)
//        {
//            return interval?.ToLowerInvariant() switch
//            {
//                "1d" => Period.Daily,
//                "1wk" => Period.Weekly,
//                "1mo" => Period.Monthly


//                //"1m" => Period.Daily - 20000,
//                //"2m" => Period.Minute2,
//                //"5m" => Period.Minute5,
//                //"15m" => Period.Minute15,
//                //"30m" => Period.Minute30,
//                //"1h" => Period.Hour,
//                //"2h" => Period.Hour2,
//                //"4h" => Period.Hour4,
//                //"5d" => Period.Day5,
//                //"3mo" => Period.Month3,
//                //_ => Period.Day      // Default to daily
//            };
//        }

//        /// <summary>
//        /// Converts our interval format to Yahoo Finance API interval string (legacy method for compatibility)
//        /// </summary>
//        /// <param name="interval">Interval string (1m, 5m, 15m, 30m, 1h, 1d, 1wk)</param>
//        /// <returns>Yahoo Finance compatible interval string</returns>
//        private string GetYahooInterval(string interval)
//        {
//            return interval?.ToLowerInvariant() switch
//            {
//                "1m" => "1m",
//                "2m" => "2m",
//                "5m" => "5m",
//                "15m" => "15m",
//                "30m" => "30m",
//                "1h" => "1h",
//                "2h" => "2h",
//                "4h" => "4h",
//                "1d" => "1d",
//                "5d" => "5d",
//                "1wk" => "1wk",
//                "1mo" => "1mo",
//                "3mo" => "3mo",
//                _ => "1d"         // Default to daily
//            };
//        }
//    }
//}