//namespace Orion.WebApps.AanalysisDashboard.Models
//{
//    public class SignalGenerator
//    {
//        private readonly Dictionary<string, int> _timeframeWeights = new()
//        {
//            ["Daily"] = 3,
//            ["4 Hour"] = 2,
//            ["Hourly"] = 1
//        };

//        public TradingSignal GenerateSignals(
//            MarketDataFrame df,
//            string pairName,
//            Dictionary<string, double> macroData,
//            MarketDataFrame dxyDf = null)
//        {
//            if (df.IsEmpty || df.Rows.Count < 20)
//            {
//                return null;
//            }

//            // Calculate indicators
//            df = TechnicalIndicators.CalculateIndicators(df);
//            var latest = df.Rows.Last();

//            var signalData = new SignalAccumulator();

//            // RSI Signals
//            var rsi = GetIndicatorValue(latest, "RSI", 50);
//            if (rsi < 30)
//            {
//                signalData.BullishScore += 2;
//                signalData.Reasons.Add($"Oversold RSI ({rsi:F1})");
//            }
//            else if (rsi > 70)
//            {
//                signalData.BearishScore += 2;
//                signalData.Reasons.Add($"Overbought RSI ({rsi:F1})");
//            }

//            // Trend Signals
//            var price = (double)latest.Close;
//            var sma20 = GetIndicatorValue(latest, "SMA_20", price);
//            var sma50 = GetIndicatorValue(latest, "SMA_50", price);

//            if (price > sma20 && sma20 > sma50)
//            {
//                signalData.BullishScore += 1;
//                signalData.Reasons.Add("Bullish trend structure");
//            }
//            else if (price < sma20 && sma20 < sma50)
//            {
//                signalData.BearishScore += 1;
//                signalData.Reasons.Add("Bearish trend structure");
//            }

//            // MACD Signals
//            var macd = GetIndicatorValue(latest, "MACD", 0);
//            var macdSignal = GetIndicatorValue(latest, "MACD_Signal", 0);
//            if (macd > macdSignal)
//            {
//                signalData.BullishScore += 1;
//                signalData.Reasons.Add("MACD bullish crossover");
//            }
//            else
//            {
//                signalData.BearishScore += 1;
//                signalData.Reasons.Add("MACD bearish crossover");
//            }

//            // Bollinger Bands
//            var bbLower = GetIndicatorValue(latest, "BB_Lower", price * 0.98);
//            var bbUpper = GetIndicatorValue(latest, "BB_Upper", price * 1.02);
//            if (price <= bbLower)
//            {
//                signalData.BullishScore += 1;
//                signalData.Reasons.Add("At lower Bollinger Band");
//            }
//            else if (price >= bbUpper)
//            {
//                signalData.BearishScore += 1;
//                signalData.Reasons.Add("At upper Bollinger Band");
//            }

//            // Gold Macro Signals
//            if (pairName == "XAU/USD" && dxyDf != null && !dxyDf.IsEmpty)
//            {
//                var dxyChange = CalculateDxyChange(dxyDf);
//                var rates = macroData.GetValueOrDefault("Rates", 5.25);

//                if (dxyChange < -0.002 && rates < 4)
//                {
//                    signalData.BullishScore += 2;
//                    signalData.Reasons.Add("Gold macro bullish (DXY falling, low rates)");
//                }
//                else if (dxyChange > 0.002 && rates > 4)
//                {
//                    signalData.BearishScore += 2;
//                    signalData.Reasons.Add("Gold macro bearish (DXY rising, high rates)");
//                }
//            }

//            // Determine bias
//            string bias;
//            int strength;

//            if (signalData.BullishScore > signalData.BearishScore)
//            {
//                bias = "Long";
//                strength = signalData.BullishScore;
//            }
//            else if (signalData.BearishScore > signalData.BullishScore)
//            {
//                bias = "Short";
//                strength = signalData.BearishScore;
//            }
//            else
//            {
//                return null; // Neutral - no signal
//            }

//            // Conviction
//            string conviction = DetermineConviction(strength);

//            // Price levels based on ATR
//            var atr = GetIndicatorValue(latest, "ATR", price * 0.01);

//            var signal = CreateTradingSignal(
//                pairName,
//                bias,
//                conviction,
//                strength,
//                signalData.Reasons,
//                price,
//                atr,
//                rsi);

//            return signal;
//        }

//        public TradingSignal MultiTimeframeAnalysis(
//            Dictionary<string, Dictionary<string, MarketDataFrame>> dataDict,
//            string pairName,
//            Dictionary<string, double> macroData)
//        {
//            var timeframesData = new Dictionary<string, TradingSignal>();

//            foreach (var tfConfig in dataDict)
//            {
//                var tfName = tfConfig.Key;
//                var tfData = tfConfig.Value;

//                if (tfData.TryGetValue(pairName, out var df) && !df.IsEmpty)
//                {
//                    tfData.TryGetValue("DXY", out var dxyDf);
//                    var signal = GenerateSignals(df, pairName, macroData, dxyDf);

//                    if (signal != null)
//                    {
//                        timeframesData[tfName] = signal;
//                    }
//                }
//            }

//            if (timeframesData.Count == 0)
//            {
//                return null;
//            }

//            // Weighted combination
//            double totalBullish = 0;
//            double totalBearish = 0;
//            var combinedReasons = new List<string>();

//            foreach (var tfSignal in timeframesData)
//            {
//                var tf = tfSignal.Key;
//                var signal = tfSignal.Value;
//                var weight = _timeframeWeights.GetValueOrDefault(tf, 1);

//                if (signal.Bias == "Long")
//                {
//                    totalBullish += signal.Strength * weight;
//                }
//                else if (signal.Bias == "Short")
//                {
//                    totalBearish += signal.Strength * weight;
//                }

//                var truncatedThesis = signal.Thesis.Length > 50
//                    ? signal.Thesis.Substring(0, 50)
//                    : signal.Thesis;
//                combinedReasons.Add($"{tf}: {truncatedThesis}");
//            }

//            // Final bias
//            string bias;
//            double strength;

//            if (totalBullish > totalBearish)
//            {
//                bias = "Long";
//                strength = totalBullish;
//            }
//            else if (totalBearish > totalBullish)
//            {
//                bias = "Short";
//                strength = totalBearish;
//            }
//            else
//            {
//                return null;
//            }

//            // Conviction for multi-timeframe
//            string conviction = strength >= 10 ? "High" :
//                               strength >= 5 ? "Medium" : "Low";

//            // Use daily data for price levels, fallback to first available
//            var dailySignal = timeframesData.GetValueOrDefault("Daily", timeframesData.Values.First());

//            return new TradingSignal
//            {
//                Pair = pairName,
//                Bias = bias,
//                Conviction = conviction,
//                Strength = strength,
//                Thesis = string.Join(" | ", combinedReasons.Take(3)),
//                Entry = dailySignal.Entry,
//                StopLoss = dailySignal.StopLoss,
//                TakeProfit1 = dailySignal.TakeProfit1,
//                TakeProfit2 = dailySignal.TakeProfit2,
//                RiskReward = dailySignal.RiskReward,
//                Timeframes = timeframesData.Count,
//                Rsi = dailySignal.Rsi,
//                Atr = dailySignal.Atr,
//                Price = dailySignal.Price
//            };
//        }

//        private double GetIndicatorValue(MarketDataRow row, string indicatorName, double defaultValue)
//        {
//            return row.Indicators.TryGetValue(indicatorName, out var value) && !double.IsNaN(value)
//                ? value
//                : defaultValue;
//        }

//        private double CalculateDxyChange(MarketDataFrame dxyDf)
//        {
//            if (dxyDf.Rows.Count < 6) return 0;

//            var closes = dxyDf.Rows.Select(r => (double)r.Close).ToArray();
//            var changes = new List<double>();

//            for (int i = 1; i < closes.Length; i++)
//            {
//                changes.Add((closes[i] - closes[i - 1]) / closes[i - 1]);
//            }

//            // 5-period rolling average
//            var lastChanges = changes.Skip(Math.Max(0, changes.Count - 5)).Take(5);
//            return lastChanges.Any() ? lastChanges.Average() : 0;
//        }

//        private string DetermineConviction(int strength)
//        {
//            return strength >= 4 ? "High" :
//                   strength >= 2 ? "Medium" : "Low";
//        }

//        private TradingSignal CreateTradingSignal(
//            string pairName,
//            string bias,
//            string conviction,
//            int strength,
//            List<string> reasons,
//            double price,
//            double atr,
//            double rsi)
//        {
//            double entry = price;
//            double stopLoss;
//            double takeProfit1;
//            double takeProfit2;
//            double riskReward;

//            if (bias == "Long")
//            {
//                stopLoss = price - atr * 0.75;
//                takeProfit1 = price + atr * 1.0;
//                takeProfit2 = price + atr * 1.5;
//                riskReward = entry - stopLoss > 0
//                    ? (takeProfit1 - entry) / (entry - stopLoss)
//                    : 0;
//            }
//            else // Short
//            {
//                stopLoss = price + atr * 0.75;
//                takeProfit1 = price - atr * 1.0;
//                takeProfit2 = price - atr * 1.5;
//                riskReward = stopLoss - entry > 0
//                    ? (entry - takeProfit1) / (stopLoss - entry)
//                    : 0;
//            }

//            return new TradingSignal
//            {
//                Pair = pairName,
//                Bias = bias,
//                Conviction = conviction,
//                Strength = strength,
//                Thesis = string.Join(" | ", reasons.Take(4)),
//                Entry = Math.Round(entry, 2),
//                StopLoss = Math.Round(stopLoss, 2),
//                TakeProfit1 = Math.Round(takeProfit1, 2),
//                TakeProfit2 = Math.Round(takeProfit2, 2),
//                RiskReward = Math.Round(riskReward, 2),
//                Rsi = Math.Round(rsi, 1),
//                Atr = Math.Round(atr, 2),
//                Price = Math.Round(price, 2)
//            };
//        }

//        // Helper class for accumulating signal data
//        private class SignalAccumulator
//        {
//            public int BullishScore { get; set; }
//            public int BearishScore { get; set; }
//            public List<string> Reasons { get; set; } = new();
//        }
//    }

//    // Trading Signal Model


//    // Extension method for dictionary
//    public static class DictionaryExtensions
//    {
//        public static TValue GetValueOrDefault<TKey, TValue>(
//            this IDictionary<TKey, TValue> dictionary,
//            TKey key,
//            TValue defaultValue = default)
//        {
//            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
//        }
//    }

//    // Example usage class
//    public class SignalAnalysisService
//    {
//        private readonly SignalGenerator _signalGenerator;

//        // Define timeframes configuration (similar to Python TIMEFRAMES dict)
//        private readonly Dictionary<string, TimeframeConfig> _timeframes = new()
//        {
//            ["Daily"] = new TimeframeConfig { Period = "3mo", Interval = "1d" },
//            ["4 Hour"] = new TimeframeConfig { Period = "1mo", Interval = "1h" },
//            ["Hourly"] = new TimeframeConfig { Period = "5d", Interval = "1h" }
//        };

//        public SignalAnalysisService()
//        {
//            _signalGenerator = new SignalGenerator();
//        }

//        public List<TradingSignal> AnalyzeAllPairs(
//            Dictionary<string, Dictionary<string, MarketDataFrame>> dataDict,
//            List<string> pairs,
//            Dictionary<string, double> macroData)
//        {
//            var signals = new List<TradingSignal>();

//            foreach (var pair in pairs)
//            {
//                var signal = _signalGenerator.MultiTimeframeAnalysis(dataDict, pair, macroData);
//                if (signal != null && signal.IsValid)
//                {
//                    signals.Add(signal);
//                }
//            }

//            // Sort by conviction and strength
//            return signals
//                .OrderByDescending(s => s.Conviction == "High" ? 3 : s.Conviction == "Medium" ? 2 : 1)
//                .ThenByDescending(s => s.Strength)
//                .ToList();
//        }

//        public void DisplaySignals(List<TradingSignal> signals)
//        {
//            Console.WriteLine("\n=== TRADING SIGNALS ===\n");

//            var grouped = signals.GroupBy(s => s.Conviction);

//            foreach (var group in grouped.OrderByDescending(g => g.Key))
//            {
//                Console.WriteLine($"\n--- {group.Key} Conviction Signals ---");

//                foreach (var signal in group)
//                {
//                    Console.WriteLine($"\n{signal}");
//                    Console.WriteLine($"  Entry: {signal.Entry:F2} | SL: {signal.StopLoss:F2} | TP1: {signal.TakeProfit1:F2} | TP2: {signal.TakeProfit2:F2}");
//                    Console.WriteLine($"  Price: {signal.Price:F2} | RSI: {signal.Rsi:F1} | ATR: {signal.Atr:F2}");
//                }
//            }
//        }
//    }
//}
