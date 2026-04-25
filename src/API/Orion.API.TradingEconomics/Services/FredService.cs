using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace Orion.API.TradingEconomics.Services
{
    public class FredService : IFredService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<FredService> _logger;
        private readonly IConfiguration _configuration;
        private readonly FredServiceOptions _options;
        private readonly AuditTrailEngine _auditTrail;

        private const string CACHE_KEY_PREFIX = "FRED_MACRO_DATA";
        private const string ApiKey = "b95428dde387943cd19ecac1cf71d38f";
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        // Series metadata for better error handling and monitoring
        private static readonly Dictionary<string, Dictionary<string, FredSeriesInfo>> FredSeries = new()
        {
            ["USD"] = new()
            {
                ["GDP"] = new("A191RL1Q225SBEA", "Real GDP Growth Rate", "%", Frequency.Quarterly),
                ["CPI"] = new("CPIAUCSL", "Consumer Price Index", "Index", Frequency.Monthly),
                ["Rates"] = new("FEDFUNDS", "Federal Funds Rate", "%", Frequency.Monthly),
                ["Unemployment"] = new("UNRATE", "Unemployment Rate", "%", Frequency.Monthly),
                ["Debt"] = new("GFDEBTN", "Federal Debt", "Millions", Frequency.Quarterly),
                ["IndustrialProduction"] = new("INDPRO", "Industrial Production", "Index", Frequency.Monthly),
                ["RetailSales"] = new("RSXFS", "Retail Sales", "Millions", Frequency.Monthly),
                ["TradeBalance"] = new("BOPGSTB", "Trade Balance", "Millions", Frequency.Monthly)
            },
            ["EUR"] = new()
            {
                ["GDP"] = new("CLVMNACSCAB1GQEA19", "GDP Euro Area", "Index", Frequency.Quarterly),
                ["CPI"] = new("CP0000EZ19M086NEST", "CPI Euro Area", "Index", Frequency.Monthly),
                ["Rates"] = new("ECBDFR", "ECB Deposit Facility Rate", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTEZM156S", "Unemployment Rate Euro Area", "%", Frequency.Monthly)
            },
            ["GBP"] = new()
            {
                ["GDP"] = new("CLVMNACSCAB1GQGB", "GDP UK", "Index", Frequency.Quarterly),
                ["CPI"] = new("GBRCPIALLMINMEI", "CPI UK", "Index", Frequency.Monthly),
                ["Rates"] = new("BOERUKM", "BOE Official Rate", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTGBM156S", "Unemployment Rate UK", "%", Frequency.Monthly)
            },
            ["JPY"] = new()
            {
                ["GDP"] = new("JPNRGDPEXP", "GDP Japan", "Index", Frequency.Quarterly),
                ["CPI"] = new("JPNCPIALLMINMEI", "CPI Japan", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01JPM156N", "Interest Rate Japan", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTJPM156S", "Unemployment Rate Japan", "%", Frequency.Monthly)
            },
            ["ZAR"] = new()
            {
                ["GDP"] = new("ZAFGDPRQPSMEI", "GDP South Africa", "Index", Frequency.Quarterly),
                ["CPI"] = new("ZAFCPIALLMINMEI", "CPI South Africa", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01ZAM156N", "Interest Rate South Africa", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTZAM156S", "Unemployment Rate SA", "%", Frequency.Monthly)
            },
            ["AUD"] = new()
            {
                ["GDP"] = new("AUSGDPRQPSMEI", "GDP Australia", "Index", Frequency.Quarterly),
                ["CPI"] = new("AUSCPIALLMINMEI", "CPI Australia", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01AUM156N", "Interest Rate Australia", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTAUM156S", "Unemployment Rate Australia", "%", Frequency.Monthly)
            },
            ["NZD"] = new()
            {
                ["GDP"] = new("NZLGDPRQPSMEI", "GDP New Zealand", "Index", Frequency.Quarterly),
                ["CPI"] = new("NZLCPIALLMINMEI", "CPI New Zealand", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01NZM156N", "Interest Rate New Zealand", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTNZM156S", "Unemployment Rate NZ", "%", Frequency.Monthly)
            },
            ["CAD"] = new()
            {
                ["GDP"] = new("CANGDPRQPSMEI", "GDP Canada", "Index", Frequency.Quarterly),
                ["CPI"] = new("CANCPIALLMINMEI", "CPI Canada", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01CAM156N", "Interest Rate Canada", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTCAM156S", "Unemployment Rate Canada", "%", Frequency.Monthly)
            },
            ["CHF"] = new()
            {
                ["GDP"] = new("CHEGDPRQPSMEI", "GDP Switzerland", "Index", Frequency.Quarterly),
                ["CPI"] = new("CHECPIALLMINMEI", "CPI Switzerland", "Index", Frequency.Monthly),
                ["Rates"] = new("IRSTCI01CHM156N", "Interest Rate Switzerland", "%", Frequency.Monthly),
                ["Unemployment"] = new("LRHUTTTTCHM156S", "Unemployment Rate Switzerland", "%", Frequency.Monthly)
            }
        };

        // Fallback data with timestamps for staleness tracking
        private static readonly Dictionary<string, CurrencyMacroData> FallbackData = new()
        {
            ["USD"] = new()
            {
                GDP = 2.5m, Inflation = 3.2m, Rates = 5.50m, Unemployment = 3.8m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["EUR"] = new()
            {
                GDP = 0.8m, Inflation = 2.9m, Rates = 4.50m, Unemployment = 6.5m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["GBP"] = new()
            {
                GDP = 0.6m, Inflation = 3.4m, Rates = 5.25m, Unemployment = 4.2m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["JPY"] = new()
            {
                GDP = 1.1m, Inflation = 2.8m, Rates = -0.10m, Unemployment = 2.6m,
                LastUpdated = new DateTime(2024, 6, 1)
            },
            ["ZAR"] = new()
            {
                GDP = 1.2m, Inflation = 5.0m, Rates = 8.25m, Unemployment = 32.1m,
                LastUpdated = new DateTime(2024, 6, 1)
            },
            ["AUD"] = new()
            {
                GDP = 2.0m, Inflation = 4.1m, Rates = 4.35m, Unemployment = 3.9m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["NZD"] = new()
            {
                GDP = 2.2m, Inflation = 3.8m, Rates = 5.50m, Unemployment = 3.9m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["CAD"] = new()
            {
                GDP = 1.5m, Inflation = 3.4m, Rates = 5.00m, Unemployment = 5.1m, LastUpdated = new DateTime(2024, 6, 1)
            },
            ["CHF"] = new()
            {
                GDP = 0.9m, Inflation = 2.1m, Rates = 1.75m, Unemployment = 2.0m, LastUpdated = new DateTime(2024, 6, 1)
            }
        };

        // Resilience policies
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        public FredService(IHttpClientFactory httpClientFactory, IMemoryCache cache, ILogger<FredService> logger, IConfiguration configuration, IOptions<FredServiceOptions> options = null, AuditTrailEngine auditTrail = null)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _options = options?.Value ?? new FredServiceOptions();
            // _healthEngine = healthEngine;
            _auditTrail = auditTrail;

            // Configure Polly policies
            _retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    _options.MaxRetries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            "Retry {RetryCount} after {Delay}s due to {Error}",
                            retryCount, timeSpan.TotalSeconds, outcome.Exception?.Message);
                    });

            _circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    _options.CircuitBreakerThreshold,
                    TimeSpan.FromSeconds(_options.CircuitBreakerDurationSeconds),
                    onBreak: (exception, duration) =>
                    {
                        _logger.LogError("Circuit breaker opened for {Duration}s", duration.TotalSeconds);
                    },
                    onReset: () => { _logger.LogInformation("Circuit breaker reset"); });
        }

        public async Task<MacroData> GetMacroDataAsync(string[] currencies = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(currencies);

            // Try cache first
            if (_cache.TryGetValue<MacroData>(cacheKey, out var cached))
            {
                if (!IsStale(cached))
                {
                    _logger.LogDebug("Cache hit for macro data");
                    return cached;
                }

                _logger.LogDebug("Cache hit but data is stale, refreshing in background");
                _ = RefreshMacroDataAsync(currencies, CancellationToken.None); // Fire and forget
                return cached; // Return stale data rather than blocking
            }

            return await FetchAndCacheMacroDataAsync(currencies, cancellationToken);
        }

        public async Task<MacroData> RefreshMacroDataAsync(string[] currencies = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(currencies);
            _cache.Remove(cacheKey);
            return await FetchAndCacheMacroDataAsync(currencies, cancellationToken);
        }

        public async Task<CurrencyMacroData> GetCurrencyMacroDataAsync(string currency, CancellationToken cancellationToken = default)
        {
            var macroData = await GetMacroDataAsync(new[] { currency }, cancellationToken);
            return macroData.Data.GetValueOrDefault(currency);
        }

        private async Task<MacroData> FetchAndCacheMacroDataAsync(string[] currencies = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(currencies);
            var lockKey = cacheKey;
            var semaphore = _locks.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync(cancellationToken);
            try
            {
                // Double-check cache after acquiring lock
                if (_cache.TryGetValue<MacroData>(cacheKey, out var cached) && !IsStale(cached))
                    return cached;

                var apiKey = ResolveApiKey();
                var targetCurrencies = currencies ?? FredSeries.Keys.ToArray();

                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogWarning("No FRED API key configured - using fallback data");
                    return CreateMacroData(
                        targetCurrencies.Where(FallbackData.ContainsKey)
                            .ToDictionary(c => c, c => FallbackData[c]),
                        false,
                        "API key not configured");
                }

                var result = new Dictionary<string, CurrencyMacroData>();
                var anySuccess = false;
                var failedCurrencies = new List<string>();

                var httpClient = CreateHttpClient();

                // Process currencies in parallel with throttling
                var semaphoreSlim = new SemaphoreSlim(_options.MaxConcurrentRequests);
                var tasks = targetCurrencies.Select(async currency =>
                {
                    await semaphoreSlim.WaitAsync(cancellationToken);
                    try
                    {
                        if (FredSeries.TryGetValue(currency, out var seriesMap))
                        {
                            var (success, currencyData) = await FetchCurrencyDataAsync(
                                httpClient, currency, seriesMap, cancellationToken);

                            if (success)
                            {
                                anySuccess = true;
                                result[currency] = currencyData;
                            }
                            else
                            {
                                failedCurrencies.Add(currency);
                                result[currency] = FallbackData.GetValueOrDefault(currency);
                            }
                        }
                        else
                        {
                            _logger.LogWarning("No FRED series mapping for {Currency}", currency);
                            result[currency] = FallbackData.GetValueOrDefault(currency);
                        }
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                });

                await Task.WhenAll(tasks);

                var macroData = CreateMacroData(
                    result,
                    anySuccess,
                    failedCurrencies.Count > 0
                        ? $"Failed currencies: {string.Join(", ", failedCurrencies)}"
                        : null);

                // Cache with expiration
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_options.CacheExpirationSeconds))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(_options.CacheSlidingExpirationSeconds));

                _cache.Set(cacheKey, macroData, cacheOptions);

                await _auditTrail?.RecordEventAsync(Guid.NewGuid(), "FredDataFetched",
                    new Dictionary<string, object>
                    {
                        ["Currencies"] = targetCurrencies.Length,
                        ["Successful"] = result.Count - failedCurrencies.Count,
                        ["Failed"] = failedCurrencies.Count
                    })!;

                return macroData;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<(bool Success, CurrencyMacroData Data)> FetchCurrencyDataAsync(HttpClient client, string currency, Dictionary<string, FredSeriesInfo> seriesMap,  CancellationToken cancellationToken)
        {
            var currencyData = new CurrencyMacroData();
            var success = false;

            try
            {
                // Fetch all indicators for this currency in parallel
                var fetchTasks = new Dictionary<string, Task<decimal?>>();

                foreach (var (indicator, seriesInfo) in seriesMap)
                {
                    fetchTasks[indicator] = indicator switch
                    {
                        "CPI" => FetchYoYChangeAsync(client, seriesInfo.SeriesId,  cancellationToken),
                        _ => FetchLatestValueAsync(client, seriesInfo.SeriesId, cancellationToken)
                    };
                }

                await Task.WhenAll(fetchTasks.Values);

                // Map results
                currencyData.GDP = fetchTasks.GetValueOrDefault("GDP")?.Result
                                   ?? FallbackData[currency]?.GDP ?? 0;
                currencyData.Inflation = fetchTasks.GetValueOrDefault("CPI")?.Result
                                         ?? FallbackData[currency]?.Inflation ?? 0;
                currencyData.Rates = fetchTasks.GetValueOrDefault("Rates")?.Result
                                     ?? FallbackData[currency]?.Rates ?? 0;
                currencyData.Unemployment = fetchTasks.GetValueOrDefault("Unemployment")?.Result
                                            ?? FallbackData[currency]?.Unemployment ?? 0;

                // Check if at least one value was successfully fetched
                success = fetchTasks.Any(t => t.Value.Result.HasValue);

                if (success)
                {
                    currencyData.LastUpdated = DateTime.UtcNow;
                    currencyData.IsLiveData = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch FRED data for {Currency}, using fallback", currency);
                if (FallbackData.TryGetValue(currency, out var fallback))
                {
                    currencyData = new CurrencyMacroData
                    {
                        GDP = fallback.GDP,
                        Inflation = fallback.Inflation,
                        Rates = fallback.Rates,
                        Unemployment = fallback.Unemployment,
                        LastUpdated = fallback.LastUpdated,
                        IsLiveData = false
                    };
                }
            }

            return (success, currencyData);
        }

        private async Task<decimal?> FetchLatestValueAsync(HttpClient client, string seriesId, CancellationToken cancellationToken)
        {
            try
            {
                var url =
                    $"series/observations?series_id={seriesId}&api_key={ApiKey}&file_type=json&limit=1&sort_order=desc";

                var response = await _retryPolicy
                    .WrapAsync(_circuitBreakerPolicy)
                    .ExecuteAsync(() => client.GetAsync(url, cancellationToken));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("FRED API returned {StatusCode} for series {SeriesId}",
                        response.StatusCode, seriesId);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                using var doc = JsonDocument.Parse(json);

                var observations = doc.RootElement.GetProperty("observations");
                if (observations.GetArrayLength() == 0)
                    return null;

                var valueElement = observations[0].GetProperty("value");
                if (valueElement.ValueKind == JsonValueKind.String &&
                    decimal.TryParse(valueElement.GetString(), out var value))
                {
                    return value;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch latest value for series {SeriesId}", seriesId);
                return null;
            }
        }

        private async Task<decimal?> FetchYoYChangeAsync(HttpClient client, string seriesId, CancellationToken cancellationToken)
        {
            try
            {
                // Get 13 months of data for YoY calculation
                var url =
                    $"series/observations?series_id={seriesId}&api_key={ApiKey}&file_type=json&limit=13&sort_order=desc";

                var response = await _retryPolicy
                    .WrapAsync(_circuitBreakerPolicy)
                    .ExecuteAsync(() => client.GetAsync(url, cancellationToken));

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                using var doc = JsonDocument.Parse(json);

                var observations = doc.RootElement.GetProperty("observations");
                var values = new List<decimal>();

                foreach (var obs in observations.EnumerateArray())
                {
                    var valueElement = obs.GetProperty("value");
                    if (valueElement.ValueKind == JsonValueKind.String &&
                        decimal.TryParse(valueElement.GetString(), out var value))
                    {
                        values.Add(value);
                    }
                }

                // Calculate YoY change
                if (values.Count >= 13)
                {
                    var current = values[0];
                    var yearAgo = values[12];
                    if (yearAgo != 0)
                        return Math.Round(((current - yearAgo) / yearAgo) * 100, 2);
                }
                else if (values.Count >= 2)
                {
                    // Fallback: calculate monthly change annualized
                    var current = values[0];
                    var previous = values[1];
                    if (previous != 0)
                    {
                        var monthlyChange = (current - previous) / previous;
                        return Math.Round(monthlyChange * 12 * 100, 2);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to calculate YoY change for series {SeriesId}", seriesId);
                return null;
            }
        }

        public Task<MacroData> GetMacroDataAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MacroData> RefreshMacroDataAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Dictionary<string, string>> GetFredSeriesMappings()
        {
            throw new NotImplementedException();
        }

        Task<DTO.FredStatusResponse> IFredService.CheckStatusAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<FredStatusResponse> CheckStatusAsync(CancellationToken cancellationToken = default)
        {
            var resolvedApiKey = ApiKey ?? ResolveApiKey();

            var response = new FredStatusResponse
            {
                IsConfigured = !string.IsNullOrEmpty(resolvedApiKey),
                ApiKeyProvided = !string.IsNullOrEmpty(resolvedApiKey) ? "Yes (masked)" : "No",
                CheckedAt = DateTime.UtcNow
            };

            if (!response.IsConfigured)
            {
                response.Message =
                    "FRED API key not configured. Add 'FRED_API_KEY' to appsettings.json or environment variables.";
                return response;
            }

            try
            {
                var client = CreateHttpClient();
                var testUrl = $"series/observations?series_id=FEDFUNDS&api_key={resolvedApiKey}&file_type=json&limit=1";

                var testResponse = await client.GetAsync(testUrl, cancellationToken);

                response.IsConnected = testResponse.IsSuccessStatusCode;
                response.Message = testResponse.IsSuccessStatusCode
                    ? "Successfully connected to FRED API"
                    : $"FRED API error: {testResponse.StatusCode}";

                if (testResponse.IsSuccessStatusCode)
                {
                    // Check sample series availability
                    response.SeriesAvailability = new Dictionary<string, bool>();
                    var sampleSeries = new[] { "FEDFUNDS", "UNRATE", "CPIAUCSL" };

                    foreach (var seriesId in sampleSeries)
                    {
                        try
                        {
                            var url = $"series?series_id={seriesId}&api_key={resolvedApiKey}&file_type=json";
                            var seriesResponse = await client.GetAsync(url, cancellationToken);
                            response.SeriesAvailability[seriesId] = seriesResponse.IsSuccessStatusCode;
                        }
                        catch
                        {
                            response.SeriesAvailability[seriesId] = false;
                        }
                    }
                }

                // Check rate limits
                if (testResponse.Headers.TryGetValues("X-RateLimit-Remaining", out var rateLimit))
                {
                    response.RateLimitRemaining =
                        int.TryParse(rateLimit.FirstOrDefault(), out var limit) ? limit : null;
                }
            }
            catch (Exception ex)
            {
                response.IsConnected = false;
                response.Message = $"Connection error: {ex.Message}";
            }

            return response;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("FRED");
            client.BaseAddress = new Uri("https://api.stlouisfed.org/fred/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(_options.RequestTimeoutSeconds);
            return client;
        }

        private string ResolveApiKey()
        {
            // Check multiple sources in order of priority
            return _configuration["FRED_API_KEY"]
                   ?? _configuration["FredApi:Key"]
                   ?? _options.ApiKey
                   ?? Environment.GetEnvironmentVariable("FRED_API_KEY");
        }

        private bool IsStale(MacroData data)
        {
            if (!data.IsLive) return false; // Fallback data never goes stale
            return DateTime.UtcNow - data.LastUpdated > TimeSpan.FromSeconds(_options.StaleDataThresholdSeconds);
        }

        private static string BuildCacheKey(string[] currencies)
        {
            return currencies?.Any() == true
                ? $"{CACHE_KEY_PREFIX}_{string.Join("_", currencies.OrderBy(c => c))}"
                : CACHE_KEY_PREFIX;
        }

        private static MacroData CreateMacroData(Dictionary<string, CurrencyMacroData> data, bool isLive, string? warning = null)
        {
            return new MacroData
            {
                Data = data,
                IsLive = isLive,
                LastUpdated = DateTime.UtcNow,
                Warning = warning,
                DataSource = isLive ? "FRED API" : "Fallback Data"
            };
        }

       

    
    }
}