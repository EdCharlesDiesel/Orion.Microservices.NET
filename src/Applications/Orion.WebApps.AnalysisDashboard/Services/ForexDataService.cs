using Microsoft.Extensions.Caching.Memory;
using Skender.Stock.Indicators;
using System.Globalization;
using Orion.WebApps.AnalysisDashboard.Models;

namespace Orion.WebApps.AnalysisDashboard.Services;

public class ForexDataService
{
    private readonly IMemoryCache _cache;
    private const string CACHE_KEY = "ForexData";
    private const string CANDLES_CACHE_KEY = "ForexCandles";
    private const string INDICATORS_CACHE_KEY = "ForexIndicators";

    private List<Candle> _candles = new();

    public ForexDataService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<UploadResult> ProcessFileAsync(IFormFile file)
    {
        try
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var candles = new List<Candle>();
            string? line;
            var lineNumber = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length < 6) continue;

                var dateTimeStr = parts[0].Trim();
                if (dateTimeStr.Length < 15) continue;

                try
                {
                    var year = int.Parse(dateTimeStr.Substring(0, 4));
                    var month = int.Parse(dateTimeStr.Substring(4, 2));
                    var day = int.Parse(dateTimeStr.Substring(6, 2));
                    var hour = int.Parse(dateTimeStr.Substring(9, 2));
                    var minute = int.Parse(dateTimeStr.Substring(11, 2));
                    var second = int.Parse(dateTimeStr.Substring(13, 2));

                    var timestamp = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

                    var candle = new Candle
                    {
                        Date = timestamp,
                        Open = decimal.Parse(parts[1].Trim(), CultureInfo.InvariantCulture),
                        High = decimal.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                        Low = decimal.Parse(parts[3].Trim(), CultureInfo.InvariantCulture),
                        Close = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                        Volume = decimal.Parse(parts[5].Trim(), CultureInfo.InvariantCulture)
                    };

                    candles.Add(candle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line {lineNumber}: {ex.Message}");
                }
            }

            _candles = candles.OrderBy(d => d.Date).ToList();
            _cache.Set(CANDLES_CACHE_KEY, _candles, TimeSpan.FromHours(24));

            // Convert to ForexCandles for display and cache
            var forexCandles = _candles.Select(c => ForexCandle.FromCandle(c)).ToList();
            _cache.Set(CACHE_KEY, forexCandles, TimeSpan.FromHours(24));

            // Calculate and cache indicators directly from Candle (IQuote) list
            CalculateAndCacheIndicators(_candles);

            var stats = CalculateStatistics(forexCandles);

            return new UploadResult
            {
                Success = true,
                Message = $"Successfully loaded {_candles.Count:N0} candles",
                Statistics = stats,
                TotalRows = _candles.Count
            };
        }
        catch (Exception ex)
        {
            return new UploadResult
            {
                Success = false,
                Message = $"Error processing file: {ex.Message}"
            };
        }
    }

    private void CalculateAndCacheIndicators(List<Candle> candles)
    {
        var indicators = new Dictionary<string, object>();

        try
        {
            // SMA - Simple Moving Averages
            indicators["SMA20"] = candles.GetSma(20).ToList();
            indicators["SMA50"] = candles.GetSma(50).ToList();
            indicators["SMA200"] = candles.GetSma(200).ToList();

            // EMA - Exponential Moving Averages
            indicators["EMA12"] = candles.GetEma(12).ToList();
            indicators["EMA26"] = candles.GetEma(26).ToList();

            // MACD (12, 26, 9)
            indicators["MACD"] = candles.GetMacd(12, 26, 9).ToList();

            // RSI (14)
            indicators["RSI"] = candles.GetRsi(14).ToList();

            // Bollinger Bands (20, 2)
            indicators["BollingerBands"] = candles.GetBollingerBands(20, 2).ToList();

            // ATR - Average True Range (14)
            indicators["ATR"] = candles.GetAtr(14).ToList();

            // Stochastic Oscillator (14, 3, 3)
            indicators["Stochastic"] = candles.GetStoch(14, 3, 3).ToList();

            // OBV - On Balance Volume
            indicators["OBV"] = candles.GetObv().ToList();

            // Ichimoku Cloud (9, 26, 52)
            indicators["Ichimoku"] = candles.GetIchimoku(9, 26, 52).ToList();

            // Parabolic SAR (0.02, 0.2)
            indicators["ParabolicSAR"] = candles.GetParabolicSar(0.02, 0.2).ToList();

            // ADX - Average Directional Index (14)
            indicators["ADX"] = candles.GetAdx(14).ToList();

            // Awesome Oscillator
            indicators["Awesome"] = candles.GetAwesome().ToList();

            // Williams %R (14)
            indicators["WilliamsR"] = candles.GetWilliamsR(14).ToList();

            // CCI - Commodity Channel Index (20)
            indicators["CCI"] = candles.GetCci(20).ToList();

            _cache.Set(INDICATORS_CACHE_KEY, indicators, TimeSpan.FromHours(24));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating indicators: {ex.Message}");
        }
    }

    public List<Candle> GetCandles(DateTime? startDate = null, DateTime? endDate = null)
    {
        var candles = _cache.Get<List<Candle>>(CANDLES_CACHE_KEY) ?? _candles;

        if (startDate.HasValue && endDate.HasValue)
        {
            return candles.Where(c => c.Date >= startDate.Value && c.Date <= endDate.Value).ToList();
        }

        return candles;
    }

    public List<ForexCandle> GetData(DateTime? startDate = null, DateTime? endDate = null)
    {
        var candles = GetCandles(startDate, endDate);
        return candles.Select(c => ForexCandle.FromCandle(c)).ToList();
    }

    public Dictionary<string, object> GetIndicators()
    {
        return _cache.Get<Dictionary<string, object>>(INDICATORS_CACHE_KEY) ?? new Dictionary<string, object>();
    }

    public ForexStatistics GetStatistics(List<ForexCandle>? data = null)
    {
        data ??= GetData();
        return CalculateStatistics(data);
    }

    private ForexStatistics CalculateStatistics(List<ForexCandle> data)
    {
        if (data.Count == 0)
        {
            return new ForexStatistics();
        }

        var first = data[0];
        var last = data[^1];
        var high = data.Max(d => d.High);
        var low = data.Min(d => d.Low);
        var change = last.Close - first.Open;
        var changePercent = first.Open != 0 ? (change / first.Open) * 100 : 0;

        return new ForexStatistics
        {
            TotalCandles = data.Count,
            DateRange = $"{first.Date} - {last.Date}",
            FirstPrice = first.Open,
            LastPrice = last.Close,
            Change = change,
            ChangePercent = changePercent,
            High = high,
            Low = low,
            MinDate = first.Timestamp,
            MaxDate = last.Timestamp
        };
    }

    public List<ChartDataPoint> GetChartData(DateTime? startDate = null, DateTime? endDate = null, int? maxPoints = 5000)
    {
        var data = GetData(startDate, endDate);

        if (maxPoints.HasValue && data.Count > maxPoints.Value)
        {
            var step = data.Count / maxPoints.Value;
            var sampled = new List<ForexCandle>();
            for (int i = 0; i < data.Count; i += step)
            {
                sampled.Add(data[i]);
            }
            if (sampled.Count > 0 && sampled[^1] != data[^1])
            {
                sampled.Add(data[^1]);
            }
            data = sampled;
        }

        return data.Select(d => new ChartDataPoint
        {
            Timestamp = d.Timestamp,
            Open = d.Open,
            High = d.High,
            Low = d.Low,
            Close = d.Close,
            Volume = d.Volume
        }).ToList();
    }

    public IndicatorChartData GetIndicatorChartData(string indicatorName, DateTime? startDate = null, DateTime? endDate = null)
    {
        var indicators = GetIndicators();
        var candles = GetCandles(startDate, endDate);

        var result = new IndicatorChartData
        {
            Dates = candles.Select(c => c.Date).ToList(),
            Values = new Dictionary<string, List<double?>>()
        };

        if (!indicators.TryGetValue(indicatorName, out var indicatorData))
        {
            return result;
        }

        var datesList = result.Dates;

        switch (indicatorName)
        {
            case "SMA20":
            case "SMA50":
            case "SMA200":
                var smaResults = (List<SmaResult>)indicatorData;
                var smaDict = smaResults.ToDictionary(r => r.Date, r => r.Sma);
                result.Values["SMA"] = datesList.Select(d => smaDict.GetValueOrDefault(d)).ToList();
                break;

            case "EMA12":
            case "EMA26":
                var emaResults = (List<EmaResult>)indicatorData;
                var emaDict = emaResults.ToDictionary(r => r.Date, r => r.Ema);
                result.Values["EMA"] = datesList.Select(d => emaDict.GetValueOrDefault(d)).ToList();
                break;

            case "MACD":
                var macdResults = (List<MacdResult>)indicatorData;
                var macdDict = macdResults.ToDictionary(r => r.Date, r => r.Macd);
                var signalDict = macdResults.ToDictionary(r => r.Date, r => r.Signal);
                var histogramDict = macdResults.ToDictionary(r => r.Date, r => r.Histogram);
                result.Values["MACD"] = datesList.Select(d => macdDict.GetValueOrDefault(d)).ToList();
                result.Values["Signal"] = datesList.Select(d => signalDict.GetValueOrDefault(d)).ToList();
                result.Values["Histogram"] = datesList.Select(d => histogramDict.GetValueOrDefault(d)).ToList();
                break;

            case "RSI":
                var rsiResults = (List<RsiResult>)indicatorData;
                var rsiDict = rsiResults.ToDictionary(r => r.Date, r => r.Rsi);
                result.Values["RSI"] = datesList.Select(d => rsiDict.GetValueOrDefault(d)).ToList();
                break;

            case "BollingerBands":
                var bbResults = (List<BollingerBandsResult>)indicatorData;
                var bbDict = bbResults.ToDictionary(r => r.Date, r => r);
                result.Values["UpperBand"] = datesList.Select(d => bbDict.GetValueOrDefault(d)?.UpperBand).ToList();
                result.Values["Sma"] = datesList.Select(d => bbDict.GetValueOrDefault(d)?.Sma).ToList();
                result.Values["LowerBand"] = datesList.Select(d => bbDict.GetValueOrDefault(d)?.LowerBand).ToList();
                result.Values["ZScore"] = datesList.Select(d => bbDict.GetValueOrDefault(d)?.ZScore).ToList();
                result.Values["Width"] = datesList.Select(d => bbDict.GetValueOrDefault(d)?.Width).ToList();
                break;

            case "ATR":
                var atrResults = (List<AtrResult>)indicatorData;
                var atrDict = atrResults.ToDictionary(r => r.Date, r => r.Atr);
                result.Values["ATR"] = datesList.Select(d => atrDict.GetValueOrDefault(d)).ToList();
                break;

            case "Stochastic":
                var stochResults = (List<StochResult>)indicatorData;
                var stochDict = stochResults.ToDictionary(r => r.Date, r => r);
                result.Values["Oscillator"] = datesList.Select(d => stochDict.GetValueOrDefault(d)?.Oscillator).ToList();
                result.Values["Signal"] = datesList.Select(d => stochDict.GetValueOrDefault(d)?.Signal).ToList();
                result.Values["PercentJ"] = datesList.Select(d => stochDict.GetValueOrDefault(d)?.PercentJ).ToList();
                break;

            case "ADX":
                var adxResults = (List<AdxResult>)indicatorData;
                var adxDict = adxResults.ToDictionary(r => r.Date, r => r);
                result.Values["ADX"] = datesList.Select(d => adxDict.GetValueOrDefault(d)?.Adx).ToList();
                result.Values["PDI"] = datesList.Select(d => adxDict.GetValueOrDefault(d)?.Pdi).ToList();
                result.Values["MDI"] = datesList.Select(d => adxDict.GetValueOrDefault(d)?.Mdi).ToList();
                break;

            case "WilliamsR":
                var williamsResults = (List<WilliamsResult>)indicatorData;
                var williamsDict = williamsResults.ToDictionary(r => r.Date, r => r.WilliamsR);
                result.Values["WilliamsR"] = datesList.Select(d => williamsDict.GetValueOrDefault(d)).ToList();
                break;

            case "CCI":
                var cciResults = (List<CciResult>)indicatorData;
                var cciDict = cciResults.ToDictionary(r => r.Date, r => r.Cci);
                result.Values["CCI"] = datesList.Select(d => cciDict.GetValueOrDefault(d)).ToList();
                break;
        }

        return result;
    }
}