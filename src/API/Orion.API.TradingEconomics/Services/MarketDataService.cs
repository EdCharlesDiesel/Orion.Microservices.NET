using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Configuration;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Orion.API.TradingEconomics.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly AppConfiguration _config;
        private readonly ILogger<MarketDataService> _logger;
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public MarketDataService(
            IOptions<AppConfiguration> config,
            ILogger<MarketDataService> logger,
            IMemoryCache cache,
            HttpClient httpClient)
        {
            _config = config.Value;
            _logger = logger;
            _cache = cache;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets historical OHLCV data for a trading pair
        /// </summary>
        public async Task<List<OhlcvBar>> GetHistoricalDataAsync(
            string pair,
            string timeframe = "1d",
            int count = 100)
        {
            var cacheKey = $"ohlcv_{pair}_{timeframe}_{count}";

            if (_cache.TryGetValue(cacheKey, out List<OhlcvBar> cachedData))
            {
                _logger.LogDebug("Cache hit for {Pair} {Timeframe}", pair, timeframe);
                return cachedData;
            }

            var lockKey = $"{pair}_{timeframe}";
            var semaphore = _locks.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            try
            {
                // Double-check cache after acquiring lock
                if (_cache.TryGetValue(cacheKey, out cachedData))
                    return cachedData;

                var data = await FetchHistoricalDataAsync(pair, timeframe, count);

                if (data != null && data.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(GetCacheDuration(timeframe))
                        .SetSlidingExpiration(GetCacheDuration(timeframe) / 2);

                    _cache.Set(cacheKey, data, cacheOptions);
                    _logger.LogInformation(
                        "Fetched {Count} candles for {Pair} {Timeframe}",
                        data.Count, pair, timeframe);
                }

                return data ?? new List<OhlcvBar>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching historical data for {Pair} {Timeframe}", pair, timeframe);
                return new List<OhlcvBar>();
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Gets latest price for a trading pair
        /// </summary>
        public async Task<decimal> GetLatestPriceAsync(string pair)
        {
            try
            {
                var data = await GetHistoricalDataAsync(pair, "1m", 2);
                return data.LastOrDefault()?.Close ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching latest price for {Pair}", pair);
                return 0;
            }
        }

        /// <summary>
        /// Gets multi-timeframe data for a pair
        /// </summary>
        public async Task<Dictionary<string, List<OhlcvBar>>> GetMultiTimeframeDataAsync(
            string pair,
            string[] timeframes,
            int count = 100)
        {
            var result = new Dictionary<string, List<OhlcvBar>>();

            var tasks = timeframes.Select(async timeframe =>
            {
                var data = await GetHistoricalDataAsync(pair, timeframe, count);
                return (timeframe, data);
            });

            var results = await Task.WhenAll(tasks);

            foreach (var (timeframe, data) in results)
            {
                result[timeframe] = data;
            }

            return result;
        }

        /// <summary>
        /// Gets data for multiple pairs
        /// </summary>
        public async Task<Dictionary<string, List<OhlcvBar>>> GetMultiplePairsDataAsync(
            string[] pairs,
            string timeframe = "1d",
            int count = 100)
        {
            var result = new ConcurrentDictionary<string, List<OhlcvBar>>();

            await Parallel.ForEachAsync(pairs, async (pair, ct) =>
            {
                var data = await GetHistoricalDataAsync(pair, timeframe, count);
                result.TryAdd(pair, data);
            });

            return result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Gets current spread for a pair
        /// </summary>
        public async Task<decimal> GetSpreadAsync(string pair)
        {
            try
            {
                // Try to get from cache first
                var cacheKey = $"spread_{pair}";
                if (_cache.TryGetValue(cacheKey, out decimal cachedSpread))
                    return cachedSpread;

                var spread = await FetchSpreadAsync(pair);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                _cache.Set(cacheKey, spread, cacheOptions);

                return spread;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching spread for {Pair}", pair);
                return GetDefaultSpread(pair);
            }
        }

        /// <summary>
        /// Gets current trading session info
        /// </summary>
        public async Task<MarketSession> GetCurrentSessionAsync(string pair)
        {
            try
            {
                var now = DateTime.UtcNow;
                var session = DetermineSession(now, pair);
                return await Task.FromResult(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error determining session for {Pair}", pair);
                return new MarketSession { Session = "Unknown", IsOpen = false };
            }
        }

        /// <summary>
        /// Validates if market is open for a pair
        /// </summary>
        public async Task<bool> IsMarketOpenAsync(string pair)
        {
            try
            {
                var session = await GetCurrentSessionAsync(pair);

                // Check weekends
                var now = DateTime.UtcNow;
                if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                {
                    // Crypto markets are always open
                    if (IsCrypto(pair))
                        return true;

                    return false;
                }

                return session.IsOpen;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking market status for {Pair}", pair);
                return true; // Default to open on error
            }
        }

        #region Private Data Fetching Methods

        private async Task<List<OhlcvBar>> FetchHistoricalDataAsync(
            string pair,
            string timeframe,
            int count)
        {
            // This is where you'd integrate with your actual data provider
            // Examples: OANDA, TradingView, Alpha Vantage, etc.

            if (_config.UseMockData)
            {
                return GenerateMockData(pair, timeframe, count);
            }

            try
            {
                var formattedPair = FormatPair(pair);
                var url = BuildDataUrl(formattedPair, timeframe, count);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<MarketDataResponse>(JsonOptions);

                return data?.OhlcvBar ?? new List<OhlcvBar>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "HTTP error fetching data. Falling back to mock data.");
                return GenerateMockData(pair, timeframe, count);
            }
        }

        private async Task<decimal> FetchSpreadAsync(string pair)
        {
            if (_config.UseMockData)
            {
                return GetDefaultSpread(pair);
            }

            try
            {
                var formattedPair = FormatPair(pair);
                var url = $"{_config.ApiBaseUrl}/spread?symbol={formattedPair}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var spreadData = await response.Content.ReadFromJsonAsync<SpreadResponse>(JsonOptions);
                return spreadData?.Spread ?? GetDefaultSpread(pair);
            }
            catch (Exception)
            {
                return GetDefaultSpread(pair);
            }
        }

        private string BuildDataUrl(string pair, string timeframe, int count)
        {
            // Adapt this to your data provider's API format
            return $"{_config.ApiBaseUrl}/ohlcv?symbol={pair}&timeframe={timeframe}&limit={count}";
        }

        private string FormatPair(string pair)
        {
            // Convert "EUR/USD" to "EURUSD" or whatever format your API expects
            return pair.Replace("/", "");
        }

        #endregion

        #region Mock Data Generation

        private List<OhlcvBar> GenerateMockData(string pair, string timeframe, int count)
        {
            var data = new List<OhlcvBar>();
            var random = new Random(pair.GetHashCode());

            var timeframeMinutes = GetTimeframeMinutes(timeframe);
            var basePrice = GetBasePrice(pair);
            var volatility = (double)GetDefaultATR(pair);

            var currentPrice = (double)basePrice;
            var now = DateTime.UtcNow;

            for (int i = count - 1; i >= 0; i--)
            {
                var change = (random.NextDouble() - 0.5) * 2 * volatility;
                var open = (decimal)currentPrice;
                var close = (decimal)(currentPrice + change);
                var high = (decimal)(Math.Max(currentPrice, currentPrice + change) + random.NextDouble() * volatility * 0.5);
                var low = (decimal)(Math.Min(currentPrice, currentPrice + change) - random.NextDouble() * volatility * 0.5);
                var volume = (long)(random.NextDouble() * 1000 + 100);

                data.Add(new OhlcvBar
                {
                    TimestampUtc = now.AddMinutes(-timeframeMinutes * i),
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = volume
                });

                currentPrice = currentPrice + change;
            }

            return data;
        }

        #endregion

        #region Session Management

        private MarketSession DetermineSession(DateTime time, string pair)
        {
            var utcHour = time.Hour;
            var sessions = new List<string>();
            var isOpen = false;

            // Asian Session (00:00 - 09:00 UTC)
            if (utcHour >= 0 && utcHour < 9)
            {
                sessions.Add("Asian");
                if (pair.Contains("JPY") || pair.Contains("AUD") || pair.Contains("NZD"))
                    isOpen = true;
            }

            // European Session (07:00 - 16:00 UTC)
            if (utcHour >= 7 && utcHour < 16)
            {
                sessions.Add("European");
                if (pair.Contains("EUR") || pair.Contains("GBP") || pair.Contains("CHF"))
                    isOpen = true;
            }

            // US Session (12:00 - 21:00 UTC)
            if (utcHour >= 12 && utcHour < 21)
            {
                sessions.Add("US");
                if (pair.Contains("USD") || pair.Contains("CAD"))
                    isOpen = true;
            }

            // Crypto and Gold are always active
            if (IsCrypto(pair) || pair.Contains("XAU"))
                isOpen = true;

            return new MarketSession
            {
                Session = string.Join("/", sessions),
                IsOpen = isOpen,
                NextSessionChange = GetNextSessionChange(time)
            };
        }

        private DateTime GetNextSessionChange(DateTime currentTime)
        {
            var hour = currentTime.Hour;

            if (hour < 7) return currentTime.Date.AddHours(7); // Tokyo open
            if (hour < 12) return currentTime.Date.AddHours(12); // London open
            if (hour < 21) return currentTime.Date.AddHours(21); // NY close

            return currentTime.Date.AddDays(1).AddHours(7); // Next Tokyo open
        }

        #endregion

        #region Helper Methods

        private TimeSpan GetCacheDuration(string timeframe)
        {
            return timeframe switch
            {
                "1m" or "5m" => TimeSpan.FromSeconds(30),
                "15m" or "30m" => TimeSpan.FromMinutes(1),
                "1h" or "4h" => TimeSpan.FromMinutes(5),
                "1d" => TimeSpan.FromMinutes(15),
                "1w" => TimeSpan.FromHours(1),
                _ => TimeSpan.FromMinutes(5)
            };
        }

        private int GetTimeframeMinutes(string timeframe)
        {
            return timeframe switch
            {
                "1m" => 1,
                "5m" => 5,
                "15m" => 15,
                "30m" => 30,
                "1h" => 60,
                "4h" => 240,
                "1d" => 1440,
                "1w" => 10080,
                _ => 1440
            };
        }

        private decimal GetBasePrice(string pair)
        {
            var prices = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["EUR/USD"] = 1.0850m,
                ["GBP/USD"] = 1.2650m,
                ["USD/JPY"] = 151.50m,
                ["AUD/USD"] = 0.6550m,
                ["USD/CAD"] = 1.3550m,
                ["NZD/USD"] = 0.6050m,
                ["XAU/USD"] = 2025.00m,
                ["BTC/USD"] = 67500.00m,
                ["ETH/USD"] = 3450.00m,
            };

            return prices.GetValueOrDefault(pair, 100.00m);
        }

        private decimal GetDefaultATR(string pair)
        {
            var atrs = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["EUR/USD"] = 0.0050m,
                ["GBP/USD"] = 0.0070m,
                ["USD/JPY"] = 0.6000m,
                ["AUD/USD"] = 0.0060m,
                ["USD/CAD"] = 0.0050m,
                ["NZD/USD"] = 0.0060m,
                ["XAU/USD"] = 15.0000m,
                ["BTC/USD"] = 2000.0000m,
                ["ETH/USD"] = 150.0000m,
            };

            return atrs.GetValueOrDefault(pair, 0.0050m);
        }

        private decimal GetDefaultSpread(string pair)
        {
            // Default spread in pips
            var spreads = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["EUR/USD"] = 1.0m,
                ["GBP/USD"] = 1.5m,
                ["USD/JPY"] = 1.2m,
                ["AUD/USD"] = 1.8m,
                ["XAU/USD"] = 25.0m,
            };

            return spreads.GetValueOrDefault(pair, 1.5m);
        }

        private bool IsCrypto(string pair)
        {
            return pair.Contains("BTC") ||
                   pair.Contains("ETH") ||
                   pair.Contains("XRP") ||
                   pair.Contains("CRYPTO");
        }

        private string ExtractBaseCurrency(string pair)
        {
            return pair.Split('/')[0];
        }

        private string ExtractQuoteCurrency(string pair)
        {
            return pair.Split('/')[1];
        }

        #endregion
    }
}