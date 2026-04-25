using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Configuration;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using System.Collections.Concurrent;

namespace Orion.API.TradingEconomics.Services
{
    public class VolatilityService : IVolatilityService
    {
        private readonly AppConfiguration _config;
        private readonly ILogger<VolatilityService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMarketDataService _marketDataService;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);
        private const int DEFAULT_ATR_WINDOW = 14;
        private const int MIN_DATA_POINTS = 20;

        public VolatilityService(
            IOptions<AppConfiguration> config,
            ILogger<VolatilityService> logger,
            IMemoryCache cache,
            IMarketDataService marketDataService)
        {
            _config = config.Value;
            _logger = logger;
            _cache = cache;
            _marketDataService = marketDataService;
        }

        /// <summary>
        /// Gets current volatility as a decimal (e.g., 0.005 = 0.5%)
        /// </summary>
        public async Task<decimal> GetVolatilityAsync(string pair)
        {
            try
            {
                var cacheKey = $"volatility_{pair}";

                if (_cache.TryGetValue(cacheKey, out decimal cachedVolatility))
                {
                    _logger.LogDebug("Volatility cache hit for {Pair}: {Volatility:P2}", pair, cachedVolatility);
                    return cachedVolatility;
                }

                var volatility = await CalculateVolatilityAsync(pair);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DefaultCacheDuration)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, volatility, cacheOptions);

                _logger.LogDebug("Volatility calculated for {Pair}: {Volatility:P2}", pair, volatility);
                return volatility;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating volatility for {Pair}", pair);
                return GetDefaultVolatility(pair);
            }
        }

        /// <summary>
        /// Gets ATR (Average True Range) for a pair
        /// </summary>
        public async Task<decimal> GetAtrAsync(string pair, int window = DEFAULT_ATR_WINDOW)
        {
            try
            {
                var data = await GetOHLCVDataAsync(pair, window + 10);

                if (data == null || data.Count < window)
                {
                    _logger.LogWarning("Insufficient data for ATR calculation for {Pair}", pair);
                    return GetDefaultATR(pair);
                }

                var highs = data.Select(d => d.High).ToList();
                var lows = data.Select(d => d.Low).ToList();
                var closes = data.Select(d => d.Close).ToList();

                return CalculateATR(highs, lows, closes, window);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating ATR for {Pair}", pair);
                return GetDefaultATR(pair);
            }
        }

        /// <summary>
        /// Gets volatility metrics for multiple timeframes
        /// </summary>
        public async Task<VolatilityMetrics> GetVolatilityMetricsAsync(string pair)
        {
            var metrics = new VolatilityMetrics
            {
                Pair = pair,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                // Get data for different timeframes
                var dailyData = await GetOHLCVDataAsync(pair, 30);
                var hourlyData = await GetOHLCVDataAsync(pair, 24, "1h");

                if (dailyData != null && dailyData.Count >= MIN_DATA_POINTS)
                {
                    metrics.DailyATR = CalculateATR(
                        dailyData.Select(d => d.High).ToList(),
                        dailyData.Select(d => d.Low).ToList(),
                        dailyData.Select(d => d.Close).ToList(),
                        DEFAULT_ATR_WINDOW);

                    metrics.DailyVolatility = (decimal)CalculateHistoricalVolatility(
                        dailyData.Select(d => d.Close).ToList());

                    metrics.VolatilityRegime = DetermineVolatilityRegime(
                        dailyData, metrics.DailyATR);

                    metrics.IsHighVolatility = IsHighVolatility(dailyData, metrics.DailyATR);
                }

                if (hourlyData != null && hourlyData.Count >= MIN_DATA_POINTS)
                {
                    metrics.HourlyATR = CalculateATR(
                        hourlyData.Select(d => d.High).ToList(),
                        hourlyData.Select(d => d.Low).ToList(),
                        hourlyData.Select(d => d.Close).ToList(),
                        DEFAULT_ATR_WINDOW);
                }

                // Calculate additional metrics
                if (dailyData != null && dailyData.Count >= 2)
                {
                    metrics.CurrentPrice = dailyData.Last().Close;
                    metrics.DailyRange = dailyData.Last().High - dailyData.Last().Low;
                    metrics.AverageDailyRange = dailyData
                        .TakeLast(10)
                        .Average(d => d.High - d.Low);
                    metrics.VolatilityPercentile = CalculateVolatilityPercentile(
                        dailyData, metrics.DailyVolatility);
                }

                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating volatility metrics for {Pair}", pair);
                metrics.Error = ex.Message;
                return metrics;
            }
        }

        /// <summary>
        /// Adjusts position size based on volatility
        /// </summary>
        public async Task<decimal> GetVolatilityAdjustedSizeAsync(
            string pair,
            decimal baseSize,
            decimal maxVolatility = 0.02m)
        {
            var volatility = (decimal)await GetVolatilityAsync(pair);
            var adjustment = Math.Min(1, maxVolatility / Math.Max(volatility, 0.0001m));
            var adjustedSize = baseSize * adjustment;

            _logger.LogDebug(
                "Position size adjusted for {Pair}: Base={BaseSize}, Volatility={Vol:P2}, " +
                "Adjustment={Adj:P2}, Final={Final}",
                pair, baseSize, volatility, adjustment, adjustedSize);

            return Math.Round(adjustedSize, 4);
        }

        /// <summary>
        /// Gets stop loss distance based on ATR
        /// </summary>
        public async Task<decimal> GetAtrBasedStopDistanceAsync(
            string pair,
            decimal atrMultiplier = 1.5m)
        {
            var atr = await GetAtrAsync(pair);
            var stopDistance = atr * atrMultiplier;

            _logger.LogDebug(
                "ATR-based stop distance for {Pair}: ATR={ATR}, Multiplier={Mult}, Distance={Dist}",
                pair, atr, atrMultiplier, stopDistance);

            return stopDistance;
        }

        /// <summary>
        /// Compares volatility across multiple pairs
        /// </summary>
        public async Task<Dictionary<string, decimal>> GetVolatilityRankingAsync(
            IEnumerable<string> pairs)
        {
            var results = new ConcurrentDictionary<string, decimal>();

            await Parallel.ForEachAsync(pairs, async (pair, ct) =>
            {
                try
                {
                    var volatility = await GetVolatilityAsync(pair);
                    results.TryAdd(pair, (decimal)volatility);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting volatility for {Pair}", pair);
                    results.TryAdd(pair, 0);
                }
            });

            return results
                .OrderByDescending(kvp => kvp.Value)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        #region Private Calculation Methods

        private async Task<decimal> CalculateVolatilityAsync(string pair)
        {
            var data = await GetOHLCVDataAsync(pair, 30);

            if (data == null || data.Count < MIN_DATA_POINTS)
            {
                _logger.LogWarning("Insufficient data for volatility calculation for {Pair}", pair);
                return GetDefaultVolatility(pair);
            }

            return CalculateHistoricalVolatility(data.Select(d => d.Close).ToList());
        }

        private decimal CalculateHistoricalVolatility(List<decimal> prices)
        {
            if (prices.Count < 2)
                return 0;

            var returns = new List<decimal>();
            for (int i = 1; i < prices.Count; i++)
            {
                var dailyReturn = (decimal)Math.Log((double)(prices[i] / prices[i - 1]));
                returns.Add(dailyReturn);
            }

            var mean = returns.Average();
            var variance = returns.Sum(r => (r - mean) * (r - mean)) / (returns.Count - 1);
            var dailyVolatility = (decimal)Math.Sqrt((double)variance);

            // Annualize volatility (assuming daily data)
            var annualizedVolatility = dailyVolatility * (decimal)Math.Sqrt(252);
            return annualizedVolatility;
        }

        private decimal CalculateATR(List<decimal> highs, List<decimal> lows, List<decimal> closes, int window)
        {
            if (highs.Count < window || lows.Count < window || closes.Count < window)
                return 0;

            var trueRanges = new List<decimal>();

            for (int i = 1; i < closes.Count; i++)
            {
                var highLow = highs[i] - lows[i];
                var highClose = Math.Abs(highs[i] - closes[i - 1]);
                var lowClose = Math.Abs(lows[i] - closes[i - 1]);

                trueRanges.Add(Math.Max(highLow, Math.Max(highClose, lowClose)));
            }

            // Wilder's smoothing method
            if (trueRanges.Count == window - 1)
            {
                return trueRanges.Average();
            }

            var initialATR = trueRanges.Take(window).Average();
            var currentATR = initialATR;

            for (int i = window; i < trueRanges.Count; i++)
            {
                currentATR = ((currentATR * (window - 1)) + trueRanges[i]) / window;
            }

            return currentATR;
        }

        private string DetermineVolatilityRegime(List<OhlcvBar> data, decimal currentATR)
        {
            if (data.Count < 20) return "Unknown";

            var historicalATRs = new List<decimal>();
            var highs = data.Select(d => d.High).ToList();
            var lows = data.Select(d => d.Low).ToList();
            var closes = data.Select(d => d.Close).ToList();

            // Calculate rolling ATRs for comparison
            for (int i = DEFAULT_ATR_WINDOW; i <= data.Count; i++)
            {
                var slice = data.Take(i).ToList();
                var sliceHighs = slice.Select(d => d.High).ToList();
                var sliceLows = slice.Select(d => d.Low).ToList();
                var sliceCloses = slice.Select(d => d.Close).ToList();

                historicalATRs.Add(CalculateATR(sliceHighs, sliceLows, sliceCloses, DEFAULT_ATR_WINDOW));
            }

            if (!historicalATRs.Any()) return "Unknown";

            var avgATR = historicalATRs.Average();
            var stdATR = CalculateStandardDeviation(historicalATRs.Select(a => a).ToList());

            if (currentATR > avgATR + (decimal)stdATR * 2)
                return "Extreme High";
            if (currentATR > avgATR + (decimal)stdATR)
                return "High";
            if (currentATR < avgATR - (decimal)stdATR)
                return "Low";

            return "Normal";
        }

        private bool IsHighVolatility(List<OhlcvBar> data, decimal currentATR)
        {
            if (data.Count < 20) return false;

            var avgRange = data.TakeLast(10).Average(d => d.High - d.Low);
            var currentRange = data.Last().High - data.Last().Low;

            return currentRange > avgRange * 1.5m || currentATR > avgRange;
        }

        private decimal CalculateVolatilityPercentile(List<OhlcvBar> data, decimal currentVolatility)
        {
            if (data.Count < 10) return 50;

            var volatilities = new List<decimal>();
            var closes = data.Select(d => d.Close).ToList();

            for (int i = 20; i <= data.Count; i++)
            {
                var slice = closes.Take(i).Select(c => (decimal)c).ToList();
                volatilities.Add(CalculateHistoricalVolatility(slice));
            }

            if (!volatilities.Any()) return 50;

            var count = volatilities.Count(v => v <= currentVolatility);
            return (decimal)count / volatilities.Count * 100;
        }

        private decimal CalculateStandardDeviation(List<decimal> values)
        {
            if (values.Count < 2) return 0;

            var mean = values.Average();
            var variance = values.Sum(v => (v - mean) * (v - mean)) / (values.Count - 1);
            return (decimal)Math.Sqrt((double)variance);
        }

        #endregion

        #region Private Helper Methods

        private async Task<List<OhlcvBar>> GetOHLCVDataAsync(string pair, int count, string timeframe = "1d")
        {
            var lockKey = $"{pair}_{timeframe}_volatility";
            var semaphore = _locks.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            try
            {
                var cacheKey = $"ohlcv_{pair}_{timeframe}_{count}";

                if (_cache.TryGetValue(cacheKey, out List<OhlcvBar> cachedData))
                {
                    return cachedData;
                }

                var data = await _marketDataService.GetHistoricalDataAsync(pair, timeframe, count);

                if (data != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(DefaultCacheDuration.TotalMinutes / 2));

                    _cache.Set(cacheKey, data, cacheOptions);
                }

                return data;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private decimal GetDefaultVolatility(string pair)
        {
            // Default volatilities for common pairs (annualized)
            var defaults = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
            {
                ["EUR/USD"] = 0.08m,
                ["GBP/USD"] = 0.10m,
                ["USD/JPY"] = 0.09m,
                ["AUD/USD"] = 0.11m,
                ["USD/CAD"] = 0.09m,
                ["NZD/USD"] = 0.11m,
                ["XAU/USD"] = 0.15m,
                ["BTC/USD"] = 0.60m,
                ["ETH/USD"] = 0.70m,
            };

            return defaults.GetValueOrDefault(pair, 0.10m);
        }

        private decimal GetDefaultATR(string pair)
        {
            var defaults = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
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

            return defaults.GetValueOrDefault(pair, 0.0050m);
        }
        #endregion
    }
}
