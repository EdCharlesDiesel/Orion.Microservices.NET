//using System;
//using System.Collections.Generic;
//using System.Linq;
//using YahooFinanceApi;

//public static class TechnicalIndicators
//{
//    public static MarketDataFrame CalculateIndicators(MarketDataFrame df)
//    {
//        if (df.IsEmpty || df.Rows.Count < 20)
//        {
//            return df;
//        }

//        var result = new MarketDataFrame();
//        result.Rows = df.Rows.Select(r => r.Clone()).ToList();

//        var closes = df.Rows.Select(r => (double)r.Close).ToArray();
//        var highs = df.Rows.Select(r => (double)r.High).ToArray();
//        var lows = df.Rows.Select(r => (double)r.Low).ToArray();
//        var volumes = df.Rows.Select(r => (double)r.Volume).ToArray();

//        // Trend Indicators
//        var sma20 = CalculateSMA(closes, 20);
//        var sma50 = CalculateSMA(closes, 50);
//        var ema9 = CalculateEMA(closes, 9);
//        var ema21 = CalculateEMA(closes, 21);

//        // Momentum Indicators
//        var rsi = CalculateRSI(closes, 14);

//        // MACD
//        var (macd, macdSignal, macdHistogram) = CalculateMACD(closes);

//        // Bollinger Bands
//        var (bbUpper, bbLower, bbMiddle, bbWidth) = CalculateBollingerBands(closes, 20);

//        // Volatility - ATR
//        var atr = CalculateATR(highs, lows, closes, 14);

//        // Support & Resistance
//        var resistance = CalculateRollingMax(highs, 20);
//        var support = CalculateRollingMin(lows, 20);

//        // Volume SMA
//        var volumeSma = volumes.Length > 0 ? CalculateSMA(volumes, 20) : new double[volumes.Length];

//        // Assign all indicators to the dataframe
//        for (int i = 0; i < result.Rows.Count; i++)
//        {
//            var indicators = result.Rows[i].Indicators;

//            // Trend Indicators
//            indicators["SMA_20"] = sma20[i];
//            indicators["SMA_50"] = sma50[i];
//            indicators["EMA_9"] = ema9[i];
//            indicators["EMA_21"] = ema21[i];

//            // Momentum Indicators
//            indicators["RSI"] = rsi[i];

//            // MACD
//            indicators["MACD"] = macd[i];
//            indicators["MACD_Signal"] = macdSignal[i];
//            indicators["MACD_Histogram"] = macdHistogram[i];

//            // Bollinger Bands
//            indicators["BB_Upper"] = bbUpper[i];
//            indicators["BB_Lower"] = bbLower[i];
//            indicators["BB_Middle"] = bbMiddle[i];
//            indicators["BB_Width"] = bbWidth[i];

//            // Volatility
//            indicators["ATR"] = atr[i];

//            // Support & Resistance
//            indicators["Resistance"] = resistance[i];
//            indicators["Support"] = support[i];

//            // Volume
//            if (volumes.Length > 0)
//            {
//                indicators["Volume_SMA"] = volumeSma[i];
//            }
//        }

//        return result;
//    }

//    private static double[] CalculateSMA(double[] values, int period)
//    {
//        var result = new double[values.Length];

//        for (int i = 0; i < values.Length; i++)
//        {
//            if (i < period - 1)
//            {
//                result[i] = double.NaN;
//                continue;
//            }

//            double sum = 0;
//            for (int j = 0; j < period; j++)
//            {
//                sum += values[i - j];
//            }
//            result[i] = sum / period;
//        }

//        return result;
//    }

//    private static double[] CalculateEMA(double[] values, int period)
//    {
//        var result = new double[values.Length];

//        if (values.Length == 0) return result;

//        double multiplier = 2.0 / (period + 1);

//        // First EMA is SMA
//        double sum = 0;
//        for (int i = 0; i < Math.Min(period, values.Length); i++)
//        {
//            sum += values[i];
//        }

//        result[period - 1] = sum / period;

//        // Calculate EMA for remaining values
//        for (int i = period; i < values.Length; i++)
//        {
//            result[i] = (values[i] - result[i - 1]) * multiplier + result[i - 1];
//        }

//        // Fill NaN for initial periods
//        for (int i = 0; i < period - 1; i++)
//        {
//            result[i] = double.NaN;
//        }

//        return result;
//    }

//    private static double[] CalculateRSI(double[] values, int period)
//    {
//        var result = new double[values.Length];

//        if (values.Length < period + 1)
//        {
//            return result;
//        }

//        var gains = new double[values.Length];
//        var losses = new double[values.Length];

//        // Calculate price changes
//        for (int i = 1; i < values.Length; i++)
//        {
//            double change = values[i] - values[i - 1];
//            gains[i] = change > 0 ? change : 0;
//            losses[i] = change < 0 ? -change : 0;
//        }

//        // Calculate initial average gain/loss
//        double avgGain = 0;
//        double avgLoss = 0;

//        for (int i = 1; i <= period; i++)
//        {
//            avgGain += gains[i];
//            avgLoss += losses[i];
//        }

//        avgGain /= period;
//        avgLoss /= period;

//        // First RSI value
//        if (avgLoss == 0)
//        {
//            result[period] = 100;
//        }
//        else
//        {
//            double rs = avgGain / avgLoss;
//            result[period] = 100 - (100 / (1 + rs));
//        }

//        // Calculate remaining RSI values using smoothing
//        for (int i = period + 1; i < values.Length; i++)
//        {
//            avgGain = ((avgGain * (period - 1)) + gains[i]) / period;
//            avgLoss = ((avgLoss * (period - 1)) + losses[i]) / period;

//            if (avgLoss == 0)
//            {
//                result[i] = 100;
//            }
//            else
//            {
//                double rs = avgGain / avgLoss;
//                result[i] = 100 - (100 / (1 + rs));
//            }
//        }

//        // Fill NaN for initial periods
//        for (int i = 0; i < period; i++)
//        {
//            result[i] = double.NaN;
//        }

//        return result;
//    }

//    private static (double[] macd, double[] signal, double[] histogram) CalculateMACD(
//        double[] values,
//        int fastPeriod = 12,
//        int slowPeriod = 26,
//        int signalPeriod = 9)
//    {
//        var fastEma = CalculateEMA(values, fastPeriod);
//        var slowEma = CalculateEMA(values, slowPeriod);

//        var macd = new double[values.Length];
//        for (int i = 0; i < values.Length; i++)
//        {
//            if (double.IsNaN(fastEma[i]) || double.IsNaN(slowEma[i]))
//            {
//                macd[i] = double.NaN;
//            }
//            else
//            {
//                macd[i] = fastEma[i] - slowEma[i];
//            }
//        }

//        var signal = CalculateEMA(macd, signalPeriod);
//        var histogram = new double[values.Length];

//        for (int i = 0; i < values.Length; i++)
//        {
//            if (double.IsNaN(macd[i]) || double.IsNaN(signal[i]))
//            {
//                histogram[i] = double.NaN;
//            }
//            else
//            {
//                histogram[i] = macd[i] - signal[i];
//            }
//        }

//        return (macd, signal, histogram);
//    }

//    private static (double[] upper, double[] lower, double[] middle, double[] width)
//        CalculateBollingerBands(double[] values, int period, double standardDeviations = 2.0)
//    {
//        var middle = CalculateSMA(values, period);
//        var upper = new double[values.Length];
//        var lower = new double[values.Length];
//        var width = new double[values.Length];

//        for (int i = 0; i < values.Length; i++)
//        {
//            if (i < period - 1)
//            {
//                upper[i] = double.NaN;
//                lower[i] = double.NaN;
//                width[i] = double.NaN;
//                continue;
//            }

//            // Calculate standard deviation
//            double sum = 0;
//            for (int j = 0; j < period; j++)
//            {
//                sum += Math.Pow(values[i - j] - middle[i], 2);
//            }
//            double stdDev = Math.Sqrt(sum / period);

//            upper[i] = middle[i] + (standardDeviations * stdDev);
//            lower[i] = middle[i] - (standardDeviations * stdDev);
//            width[i] = (upper[i] - lower[i]) / values[i];
//        }

//        return (upper, lower, middle, width);
//    }

//    private static double[] CalculateATR(double[] highs, double[] lows, double[] closes, int period)
//    {
//        var result = new double[highs.Length];

//        if (highs.Length < period)
//        {
//            return result;
//        }

//        var trueRanges = new double[highs.Length];

//        // Calculate True Range
//        trueRanges[0] = highs[0] - lows[0];

//        for (int i = 1; i < highs.Length; i++)
//        {
//            double tr1 = highs[i] - lows[i];
//            double tr2 = Math.Abs(highs[i] - closes[i - 1]);
//            double tr3 = Math.Abs(lows[i] - closes[i - 1]);

//            trueRanges[i] = Math.Max(tr1, Math.Max(tr2, tr3));
//        }

//        // Calculate first ATR as simple average
//        double sum = 0;
//        for (int i = 0; i < period; i++)
//        {
//            sum += trueRanges[i];
//        }
//        result[period - 1] = sum / period;

//        // Calculate remaining ATR using smoothing
//        for (int i = period; i < highs.Length; i++)
//        {
//            result[i] = ((result[i - 1] * (period - 1)) + trueRanges[i]) / period;
//        }

//        // Fill NaN for initial periods
//        for (int i = 0; i < period - 1; i++)
//        {
//            result[i] = double.NaN;
//        }

//        return result;
//    }

//    private static double[] CalculateRollingMax(double[] values, int period)
//    {
//        var result = new double[values.Length];

//        for (int i = 0; i < values.Length; i++)
//        {
//            if (i < period - 1)
//            {
//                result[i] = double.NaN;
//                continue;
//            }

//            double max = double.MinValue;
//            for (int j = 0; j < period; j++)
//            {
//                max = Math.Max(max, values[i - j]);
//            }
//            result[i] = max;
//        }

//        return result;
//    }

//    private static double[] CalculateRollingMin(double[] values, int period)
//    {
//        var result = new double[values.Length];

//        for (int i = 0; i < values.Length; i++)
//        {
//            if (i < period - 1)
//            {
//                result[i] = double.NaN;
//                continue;
//            }

//            double min = double.MaxValue;
//            for (int j = 0; j < period; j++)
//            {
//                min = Math.Min(min, values[i - j]);
//            }
//            result[i] = min;
//        }

//        return result;
//    }
//}

//// Updated MarketDataFrame to work with indicators
//public class MarketDataFrame
//{
//    public static MarketDataFrame Empty => new MarketDataFrame();

//    public bool IsEmpty => Rows.Count == 0;

//    public List<MarketDataRow> Rows { get; set; } = new();

//    public MarketDataFrame() { }

//    public MarketDataFrame(IEnumerable<Candle> candles)
//    {
//        Rows = candles.Select(c => new MarketDataRow
//        {
//            DateTime = c.DateTime,
//            Open = c.Open,
//            High = c.High,
//            Low = c.Low,
//            Close = c.Close,
//            Volume = c.Volume
//        }).ToList();
//    }

//    public MarketDataFrame Resample(TimeSpan interval)
//    {
//        var grouped = Rows.GroupBy(r => RoundDownToInterval(r.DateTime, interval))
//                         .OrderBy(g => g.Key);

//        var resampled = new MarketDataFrame();

//        foreach (var group in grouped)
//        {
//            var rows = group.ToList();
//            resampled.Rows.Add(new MarketDataRow
//            {
//                DateTime = group.Key,
//                Open = rows.First().Open,
//                High = rows.Max(r => r.High),
//                Low = rows.Min(r => r.Low),
//                Close = rows.Last().Close,
//                Volume = rows.Sum(r => r.Volume)
//            });
//        }

//        return resampled;
//    }

//    private DateTime RoundDownToInterval(DateTime dt, TimeSpan interval)
//    {
//        long ticks = dt.Ticks / interval.Ticks;
//        return new DateTime(ticks * interval.Ticks, dt.Kind);
//    }
//}