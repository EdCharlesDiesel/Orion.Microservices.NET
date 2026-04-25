//using CsvHelper;
//using Microsoft.Extensions.Caching.Memory;
//using Orion.WebApps.AanalysisDashboard.Interfaces;
//using Orion.WebApps.AanalysisDashboard.Models;
//using Orion.WebApps.AanalysisDashboardBlazor.Models;
//using Polly;
//using Polly.Retry;
//using System.Globalization;
//using YahooFinanceApi;

//namespace Orion.WebApps.AanalysisDashboard.Services
//{
//    public class DataService
//    {
//        private readonly IMemoryCache _cache;
//        private readonly IMarketDataProvider _dataProvider;
//        private readonly ILogger<DataService> _logger;
//        private List<PriceData> _allData = new();
//        private List<DateOnly> _allDates = new();        
//        private readonly AsyncRetryPolicy _retryPolicy;
//        private readonly string _fredApiKey;

//        // Cache durations in seconds
//        private static readonly TimeSpan FetchDataCacheDuration = TimeSpan.FromSeconds(300);
//        private static readonly TimeSpan MacroDataCacheDuration = TimeSpan.FromSeconds(3600);

//        public DataService(IMemoryCache cache, IMarketDataProvider dataProvider, ILogger<DataService> logger)
//        {
//            _cache = cache;
//            _dataProvider = dataProvider;
//            _logger = logger;
//        }

//        public async Task<(Dictionary<string, Dictionary<string, List<MarketData>>> DataByTimeframe,Dictionary<string, List<MarketData>> DxyByTimeframe)>LoadAllTimeframesAsync()
//        {
//            var cacheKey = "AllTimeframesData";

//            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
//            {
//                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

//                var dataByTimeframe = new Dictionary<string, Dictionary<string, List<MarketData>>>();
//                var dxyByTimeframe = new Dictionary<string, List<MarketData>>();

//                foreach (var tf in TimeframeConfigs.Mappings)
//                {
//                    dataByTimeframe[tf.Key] = new Dictionary<string, List<MarketData>>();

//                    // Load DXY
//                    try
//                    {
//                        var dxyData = await _dataProvider.GetDataAsync("DX-Y.NYB", tf.Value.Interval, tf.Value.Period);
//                        dxyByTimeframe[tf.Key] = dxyData;
//                    }
//                    catch (Exception ex)
//                    {
//                        _logger.LogWarning(ex, $"Failed to load DXY for {tf.Key}");
//                        dxyByTimeframe[tf.Key] = new List<MarketData>();
//                    }

//                    // Load all forex pairs
//                    foreach (var asset in Assets.All)
//                    {
//                        try
//                        {
//                            var data = await _dataProvider.GetDataAsync(asset.Value, tf.Value.Interval, tf.Value.Period);
//                            dataByTimeframe[tf.Key][asset.Key] = data;
//                        }
//                        catch (Exception ex)
//                        {
//                            _logger.LogWarning(ex, $"Failed to load {asset.Key} for {tf.Key}");
//                            dataByTimeframe[tf.Key][asset.Key] = new List<MarketData>();
//                        }
//                    }
//                }

//                return (dataByTimeframe, dxyByTimeframe);
//            });
//        }

//        public async Task<MacroDataCollection> GetMacroDataAsync()
//        {
//            var cacheKey = "MacroData";

//            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
//            {
//                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

//                // Default values
//                var macroData = new MacroDataCollection
//                {
//                    USD = new MacroData { GDP = 2.5m, Inflation = 3.2m, Rates = 5.5m, Unemployment = 3.8m },
//                    ZAR = new MacroData { GDP = 1.2m, Inflation = 5.0m, Rates = 8.25m, Unemployment = 32.1m },
//                    JPY = new MacroData { GDP = 1.1m, Inflation = 2.8m, Rates = -0.1m, Unemployment = 2.6m },
//                    AUD = new MacroData { GDP = 2.0m, Inflation = 4.1m, Rates = 4.35m, Unemployment = 3.9m },
//                    CAD = new MacroData { GDP = 1.5m, Inflation = 3.4m, Rates = 5.0m, Unemployment = 5.1m },
//                    EUR = new MacroData { GDP = 0.8m, Inflation = 2.9m, Rates = 4.5m, Unemployment = 6.5m },
//                    GBP = new MacroData { GDP = 0.6m, Inflation = 3.4m, Rates = 5.25m, Unemployment = 4.2m },
//                    CHF = new MacroData { GDP = 0.9m, Inflation = 2.1m, Rates = 1.75m, Unemployment = 2.0m }
//                };

//                // Note: FRED API integration would go here
//                // You can add HTTP client calls to FRED API

//                return macroData;
//            });
//        }

//        public async Task LoadDataAsync(string filePath)
//        {
//            try
//            {
//                using var reader = new StreamReader(filePath);
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

//                csv.Context.RegisterClassMap<PriceDataMap>();
//                var records = csv.GetRecords<PriceData>().ToList();

//                _allData = records
//                    .OrderBy(r => r.DateTime)
//                    .ToList();

//                _allDates = _allData
//                    .Select(r => r.Date)
//                    .Distinct()
//                    .OrderBy(d => d)
//                    .ToList();
//            }
//            catch (Exception ex)
//            {
//                // Fallback to manual parsing if CSV helper fails
//                await LoadDataManuallyAsync(filePath);
//            }
//        }

//        private async Task LoadDataManuallyAsync(string filePath)
//        {
//            var lines = await File.ReadAllLinesAsync(filePath);
//            _allData = new List<PriceData>();

//            foreach (var line in lines)
//            {
//                var parts = line.Split(';');
//                if (parts.Length >= 6)
//                {
//                    var dateStr = parts[0].Trim();
//                    if (DateTime.TryParseExact(dateStr, "yyyyMMdd HHmmss",
//                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
//                    {
//                        _allData.Add(new PriceData
//                        {
//                            DateTime = dt,
//                            Date = DateOnly.FromDateTime(dt),
//                            Open = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
//                            High = decimal.Parse(parts[2], CultureInfo.InvariantCulture),
//                            Low = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
//                            Close = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
//                            Volume = long.Parse(parts[5])
//                        });
//                    }
//                }
//            }

//            _allData = _allData.OrderBy(d => d.DateTime).ToList();
//            _allDates = _allData.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
//        }

//        public List<DateOnly> GetAllDates() => _allDates;

//        public List<PriceData> GetDataForDate(DateOnly date)
//        {
//            return _allData.Where(d => d.Date == date).ToList();
//        }

//        public List<PriceData> ResampleData(List<PriceData> minuteData, string timeframe)
//        {
//            var rule = timeframe switch
//            {
//                "M5" => TimeSpan.FromMinutes(5),
//                "M15" => TimeSpan.FromMinutes(15),
//                "M30" => TimeSpan.FromMinutes(30),
//                "H1" => TimeSpan.FromHours(1),
//                _ => TimeSpan.FromMinutes(1)
//            };

//            var result = new List<PriceData>();
//            var grouped = minuteData.GroupBy(d =>
//                new DateTime(d.DateTime.Year, d.DateTime.Month, d.DateTime.Day,
//                    d.DateTime.Hour, (d.DateTime.Minute / (int)rule.TotalMinutes) * (int)rule.TotalMinutes, 0));

//            foreach (var group in grouped)
//            {
//                var items = group.ToList();
//                if (items.Any())
//                {
//                    result.Add(new PriceData
//                    {
//                        DateTime = group.Key,
//                        Date = DateOnly.FromDateTime(group.Key),
//                        Open = items.First().Open,
//                        High = items.Max(i => i.High),
//                        Low = items.Min(i => i.Low),
//                        Close = items.Last().Close,
//                        Volume = items.Sum(i => i.Volume)
//                    });
//                }
//            }

//            return result.OrderBy(r => r.DateTime).ToList();
//        }

//        public DataService(IMemoryCache cache, string fredApiKey = null)
//        {
//            _cache = cache;
//            _fredApiKey = fredApiKey;

//            // Configure retry policy: 3 attempts with 2 second fixed delay
//            _retryPolicy = Policy
//                .Handle<Exception>()
//                .WaitAndRetryAsync(
//                    3,
//                    _ => TimeSpan.FromSeconds(2),
//                    onRetry: (exception, timeSpan, retryCount, context) =>
//                    {
//                        Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds}s delay");
//                    });
//        }

//        public async Task<MarketDataFrame> FetchDataAsync(
//            string symbol,
//            string interval,
//            string period)
//        {
//            string cacheKey = $"FetchData_{symbol}_{interval}_{period}_{yfInterval}";

//            if (_cache.TryGetValue(cacheKey, out MarketDataFrame cachedData))
//            {
//                return cachedData;
//            }

//            var result = await _retryPolicy.ExecuteAsync(async () =>
//            {
//                try
//                {
//                    var ticker = Yahoo.Symbols(symbol);
//                    if (ticker == null)
//                    {
//                        return MarketDataFrame.Empty;
//                    }

//                    IEnumerable<Candle> history;

//                    // Handle 4H interval by fetching hourly data and resampling
//                    if (interval == "4h")
//                    {
//                        var periodEnd = DateTime.Now;
//                        var periodStart = GetPeriodStart(periodEnd, period);

//                        history = (IEnumerable<Candle>)await ticker.QueryAsync();
//                            //periodStart,
//                            //periodEnd,
//                            //MapInterval(yfInterval));

//                        var df = new MarketDataFrame(history);

//                        if (!df.IsEmpty)
//                        {
//                            // Resample to 4 hours
//                            df = df.Resample(TimeSpan.FromHours(4));
//                        }

//                        return df;
//                    }
//                    else
//                    {
//                        var periodEnd = DateTime.Now;
//                        var periodStart = GetPeriodStart(periodEnd, period);

//                        history = (IEnumerable<Candle>)await ticker.QueryAsync();
//                            //periodStart,
//                            //periodEnd,
//                            //MapInterval(yfInterval));

//                        return new MarketDataFrame(history);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error fetching {symbol}: {ex.Message}");
//                    return MarketDataFrame.Empty;
//                }
//            });
            
//            _cache.Set(cacheKey, result, FetchDataCacheDuration);

//            return result;
//        }

//        public async Task<Dictionary<string, double>> FetchMacroDataAsync()
//        {
//            const string cacheKey = "MacroData";

//            if (_cache.TryGetValue(cacheKey, out Dictionary<string, double> cachedData))
//            {
//                return cachedData;
//            }

//            var macroData = new Dictionary<string, double>
//            {
//                ["GDP"] = 2.8,
//                ["Inflation"] = 3.2,
//                ["Rates"] = 5.25,
//                ["Unemployment"] = 3.8
//            };
            
//            if (!string.IsNullOrEmpty(_fredApiKey))
//            {
//                try
//                {
//                    using var fredClient = new FredClient(_fredApiKey);

//                    // GDP
//                    try
//                    {
//                        var gdp = await fredClient.GetLatestValueAsync("GDP");
//                        if (gdp.HasValue)
//                        {
//                            macroData["GDP"] = gdp.Value;
//                        }
//                    }
//                    catch { /* Skip if fails */ }

                    
//                    try
//                    {
//                        var cpi = await fredClient.GetSeriesAsync("CPIAUCSL");
//                        if (cpi.Count > 1)
//                        {
//                            var lastValue = cpi.Last();
//                            var previousValue = cpi.ElementAt(cpi.Count - 2);
//                            macroData["Inflation"] = ((lastValue - previousValue) / previousValue) * 100;
//                        }
//                    }
//                    catch { /* Skip if fails */ }

//                    // Fed Funds Rate
//                    try
//                    {
//                        var rates = await fredClient.GetLatestValueAsync("DFF");
//                        if (rates.HasValue)
//                        {
//                            macroData["Rates"] = rates.Value;
//                        }
//                    }
//                    catch { /* Skip if fails */ }

//                    // Unemployment
//                    try
//                    {
//                        var unemp = await fredClient.GetLatestValueAsync("UNRATE");
//                        if (unemp.HasValue)
//                        {
//                            macroData["Unemployment"] = unemp.Value;
//                        }
//                    }
//                    catch { /* Skip if fails */ }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"FRED API error: {ex.Message}");
//                }
//            }

//            // Cache the result
//            _cache.Set(cacheKey, macroData, MacroDataCacheDuration);

//            return macroData;
//        }

//        private DateTime GetPeriodStart(DateTime end, string period)
//        {
//            return period.ToLower() switch
//            {
//                "1d" => end.AddDays(-1),
//                "5d" => end.AddDays(-5),
//                "1mo" => end.AddMonths(-1),
//                "3mo" => end.AddMonths(-3),
//                "6mo" => end.AddMonths(-6),
//                "1y" => end.AddYears(-1),
//                "2y" => end.AddYears(-2),
//                "5y" => end.AddYears(-5),
//                "10y" => end.AddYears(-10),
//                "ytd" => new DateTime(end.Year, 1, 1),
//                "max" => end.AddYears(-20), // Default max
//                _ => end.AddMonths(-1)
//            };
//        }

//        private Period MapInterval(string interval)
//        {
//            return interval.ToLower() switch
//            {
//                //"1m" => Period.Minute,
//                //"5m" => Period.FiveMinutes,
//                //"15m" => Period.FifteenMinutes,
//                //"30m" => Period.ThirtyMinutes,
//                //"1h" => Period.Hourly,
//                "1d" => Period.Daily,
//                "1wk" => Period.Weekly,
//                "1mo" => Period.Monthly,
//                _ => Period.Daily
//            };
//        }
//    }
//}

