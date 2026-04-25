//using Orion.WebApps.AanalysisDashboard.Interfaces;
//using Orion.WebApps.AanalysisDashboard.Models;
//using Orion.WebApps.AanalysisDashboardBlazor.Models;

//namespace Orion.WebApps.AanalysisDashboard.Services
//{
//    /// <summary>
//    /// Service for generating trading ideas based on technical analysis, 
//    /// multi-timeframe analysis, and macroeconomic data
//    /// </summary>
//    public class TradingIdeaService : ITradingIdeaService
//    {
//        // Configuration constants
//        private const int AdxTrendMin = 25;           // Minimum ADX value for trend strength
//        private const double AtrSlMult = 1.5;         // ATR multiplier for stop loss calculation
//        private const int HighConvictionThreshold = 8;  // Strength score for high conviction
//        private const int MediumConvictionThreshold = 4; // Strength score for medium conviction
//        private const decimal DxyBullishThreshold = 0.002m;   // DXY change for bullish bias (0.2%)
//        private const decimal DxyBearishThreshold = -0.002m;  // DXY change for bearish bias (-0.2%)
//        private const decimal DefaultAtrPercentage = 0.005m;  // Default ATR as % of price when missing

//        private readonly TechnicalIndicatorService _indicatorService;
//        private readonly EntrySignalService _entrySignalService;

//        public TradingIdeaService(TechnicalIndicatorService indicatorService, EntrySignalService entrySignalService)
//        {
//            _indicatorService = indicatorService ?? throw new ArgumentNullException(nameof(indicatorService));
//            _entrySignalService = entrySignalService ?? throw new ArgumentNullException(nameof(entrySignalService));
//        }

//        /// <summary>
//        /// Generates trading ideas based on market data, macroeconomic data, and DXY data.
//        /// Performs multi-timeframe analysis and generates entry signals with conviction scoring.
//        /// </summary>
//        /// <param name="dataByTimeframe">Market data organized by timeframe and asset</param>
//        /// <param name="macro">Macroeconomic data collection</param>
//        /// <param name="dxyByTimeframe">DXY (US Dollar Index) data by timeframe</param>
//        /// <returns>List of trading ideas ranked by conviction and strength</returns>
//        public async Task<List<TradingSignal>> GenerateTradingIdeasAsync(
//            Dictionary<string, Dictionary<string, List<MarketData>>> dataByTimeframe,
//            MacroDataCollection macro,
//            Dictionary<string, List<MarketData>> dxyByTimeframe)
//        {
//            var ideas = new List<TradingIdeaSignal>();

//            // Validate input
//            if (dataByTimeframe == null || macro == null || dxyByTimeframe == null)
//                return ideas;

//            foreach (var asset in Assets.All)
//            {
//                var enrichedData = new Dictionary<string, List<EnrichedMarketData>>();

//                // Add technical indicators for each available timeframe
//                foreach (var tf in TimeframeConfigs.Mappings)
//                {
//                    if (dataByTimeframe.ContainsKey(tf.Key) &&
//                        dataByTimeframe[tf.Key].ContainsKey(asset.Key) &&
//                        dataByTimeframe[tf.Key][asset.Key].Any())
//                    {
//                        enrichedData[tf.Key] = _indicatorService.AddIndicators(dataByTimeframe[tf.Key][asset.Key]);
//                    }
//                }

//                // Ensure required timeframes are available
//                if (!enrichedData.ContainsKey("Daily") || !enrichedData.ContainsKey("4 Hour") ||
//                    !enrichedData.ContainsKey("Hourly") || !enrichedData.ContainsKey("15 Minute"))
//                    continue;

//                // Extract timeframe data
//                var daily = enrichedData["Daily"];
//                var fourHour = enrichedData["4 Hour"];
//                var hourly = enrichedData["Hourly"];
//                var fifteenMin = enrichedData["15 Minute"];
//                var weekly = enrichedData.ContainsKey("Weekly") ? enrichedData["Weekly"] : null;

//                // Analyze each timeframe
//                var signals = new Dictionary<string, TimeframeSignal>
//                {
//                    ["weekly"] = AnalyzeTimeframe(weekly?.LastOrDefault()),
//                    ["daily"] = AnalyzeTimeframe(daily.LastOrDefault()),
//                    ["4h"] = AnalyzeTimeframe(fourHour.LastOrDefault()),
//                    ["1h"] = AnalyzeTimeframe(hourly.LastOrDefault())
//                };

//                // Calculate weighted bias (higher timeframes have more weight)
//                var weights = new Dictionary<string, int>
//                {
//                    ["weekly"] = 4,
//                    ["daily"] = 3,
//                    ["4h"] = 2,
//                    ["1h"] = 1
//                };

//                var totalLongStrength = 0;
//                var totalShortStrength = 0;

//                foreach (var tf in signals)
//                {
//                    if (tf.Value.Bias == "Long")
//                        totalLongStrength += tf.Value.Strength * weights.GetValueOrDefault(tf.Key);
//                    else if (tf.Value.Bias == "Short")
//                        totalShortStrength += tf.Value.Strength * weights.GetValueOrDefault(tf.Key);
//                }

//                // Determine final bias based on weighted strengths
//                var finalBias = totalLongStrength > totalShortStrength ? "Long" :
//                               totalShortStrength > totalLongStrength ? "Short" : "Neutral";
//                var finalStrength = Math.Max(totalLongStrength, totalShortStrength);

//                // Skip neutral bias
//                if (finalBias == "Neutral")
//                    continue;

//                // Get entry signal from 15-minute timeframe
//                var entrySignal = _entrySignalService.Get15mEntrySignal(fifteenMin, finalBias);

//                // Calculate conviction level
//                var conviction = finalStrength >= HighConvictionThreshold ? "High" :
//                                finalStrength >= MediumConvictionThreshold ? "Medium" : "Low";

//                // Upgrade conviction if entry signal is strong
//                if (entrySignal.Signal != 0 && entrySignal.Confidence >= 3 && conviction == "Medium")
//                    conviction = "High";

//                // Build thesis from all timeframes
//                var thesisParts = new List<string>();
//                foreach (var tf in signals)
//                {
//                    if (tf.Value.Reasons.Any())
//                        thesisParts.Add($"{tf.Key.ToUpper()}: {string.Join(", ", tf.Value.Reasons.Take(2))}");
//                }

//                // Add entry signal to thesis
//                if (entrySignal.Signal != 0 && entrySignal.Reasons.Any())
//                {
//                    thesisParts.Add($"15M Entry: {string.Join(", ", entrySignal.Reasons.Take(2))}");
//                    thesisParts.Add($"Entry Confidence: {entrySignal.Confidence}/5");
//                }

//                var thesis = thesisParts.Any() ? string.Join(" | ", thesisParts) : "No clear signals";

//                // Apply gold macro adjustment based on DXY and interest rates
//                if (asset.Key == "XAU/USD" && dxyByTimeframe.ContainsKey("Daily") && dxyByTimeframe["Daily"].Any())
//                {
//                    var dxyData = dxyByTimeframe["Daily"];
//                    var dxyChange = dxyData.Count > 5 ?
//                        (dxyData[^1].Close - dxyData[^6].Close) / dxyData[^6].Close : 0m;
//                    var rates = macro.USD.Rates;

//                    // Bearish macro conditions for Gold
//                    if (dxyChange > DxyBullishThreshold && rates > 3)
//                    {
//                        thesis += " | Macro: Bearish ❌ (Strong USD + High Rates)";
//                        if (finalBias != "Short")
//                        {
//                            finalBias = "Short";
//                            conviction = "High";
//                        }
//                    }
//                    // Bullish macro conditions for Gold
//                    else if (dxyChange < DxyBearishThreshold && rates < 3)
//                    {
//                        thesis += " | Macro: Bullish ✅ (Weak USD + Low Rates)";
//                        if (finalBias != "Long")
//                        {
//                            finalBias = "Long";
//                            conviction = "High";
//                        }
//                    }
//                    else
//                    {
//                        thesis += " | Macro: Neutral ⚖️";
//                    }
//                }

//                // Calculate position sizing with ATR
//                var currentPrice = fifteenMin.Last().Close;
//                var atr = fifteenMin.Last().Indicators.ATR ?? currentPrice * DefaultAtrPercentage;

//                decimal entry, stopLoss, tp1, tp2, riskReward;

//                // Calculate levels based on bias
//                if (finalBias == "Long")
//                {
//                    entry = currentPrice;
//                    stopLoss = currentPrice - (atr * (decimal)AtrSlMult);
//                    tp1 = currentPrice + (atr * 1.0m);
//                    tp2 = currentPrice + (atr * 2.0m);
//                    riskReward = (entry - stopLoss) != 0 ? (tp1 - entry) / (entry - stopLoss) : 0;
//                }
//                else // Short bias
//                {
//                    entry = currentPrice;
//                    stopLoss = currentPrice + (atr * (decimal)AtrSlMult);
//                    tp1 = currentPrice - (atr * 1.0m);
//                    tp2 = currentPrice - (atr * 2.0m);
//                    riskReward = (stopLoss - entry) != 0 ? (entry - tp1) / (stopLoss - entry) : 0;
//                }

//                // Create and add the trading idea
//                ideas.Add(new TradingIdeaSignal
//                {
//                    Pair = asset.Key,
//                    Bias = finalBias,
//                    Conviction = conviction,
//                    StrengthScore = finalStrength,
//                    Thesis = thesis,
//                    Entry = entry,
//                    TakeProfit1 = tp1,
//                    TakeProfit2 = tp2,
//                    StopLoss = stopLoss,
//                    RiskReward = riskReward,
//                    EntrySignal = entrySignal.Signal != 0 ? entrySignal : null,
//                    TimeframeSignals = signals
//                });
//            }

//            // Sort by conviction (High first) then by strength score
//            return ideas.OrderByDescending(x => x.Conviction == "High")
//                       .ThenByDescending(x => x.StrengthScore)
//                       .ToList();
//        }

//        /// <summary>
//        /// Analyzes a single timeframe to determine bias, strength, and reasons
//        /// Uses multiple indicators: ADX, RSI, EMA alignment, and MACD
//        /// </summary>
//        /// <param name="data">Enriched market data for the timeframe</param>
//        /// <returns>Timeframe signal with bias, strength score, and reasons</returns>
//        private TimeframeSignal AnalyzeTimeframe(EnrichedMarketData? data)
//        {
//            if (data == null)
//                return new TimeframeSignal { Bias = null, Strength = 0, Reasons = new List<string>() };

//            var bias = (string?)null;
//            var strength = 0;
//            var reasons = new List<string>();

//            // ADX Analysis - Trend Strength
//            var adx = data.Indicators.ADX ?? 0;
//            if (adx > AdxTrendMin)
//            {
//                strength++;
//                reasons.Add($"Strong trend (ADX={adx:F1})");
//            }

//            // RSI Analysis - Overbought/Oversold conditions
//            var rsi = data.Indicators.RSI ?? 50;
//            if (rsi < 30) // Oversold - Bullish signal
//            {
//                bias = "Long";
//                strength += rsi < 25 ? 2 : 1; // Extra strength for extreme oversold
//                reasons.Add($"Oversold RSI ({rsi:F1})");
//            }
//            else if (rsi > 70) // Overbought - Bearish signal
//            {
//                bias = "Short";
//                strength += rsi > 75 ? 2 : 1; // Extra strength for extreme overbought
//                reasons.Add($"Overbought RSI ({rsi:F1})");
//            }

//            // EMA Alignment Analysis
//            var price = data.Close;
//            var ema20 = data.Indicators.EMA20 ?? price;
//            var ema50 = data.Indicators.EMA50 ?? price;

//            if (price > ema20 && ema20 > ema50) // Bullish alignment
//            {
//                if (bias != "Short") // Don't override if already bearish
//                {
//                    bias = "Long";
//                    strength++;
//                    reasons.Add("Bullish EMA alignment (Price > EMA20 > EMA50)");
//                }
//            }
//            else if (price < ema20 && ema20 < ema50) // Bearish alignment
//            {
//                if (bias != "Long") // Don't override if already bullish
//                {
//                    bias = "Short";
//                    strength++;
//                    reasons.Add("Bearish EMA alignment (Price < EMA20 < EMA50)");
//                }
//            }

//            // MACD Analysis - Momentum and Crossovers
//            var macd = data.Indicators.MACD ?? 0;
//            var macdSignal = data.Indicators.MACDSignal ?? 0;

//            if (macd > macdSignal) // Bullish crossover
//            {
//                if (bias != "Short")
//                {
//                    bias = "Long";
//                    strength++;
//                    reasons.Add("MACD bullish crossover (MACD > Signal)");
//                }
//            }
//            else if (macd < macdSignal) // Bearish crossover
//            {
//                if (bias != "Long")
//                {
//                    bias = "Short";
//                    strength++;
//                    reasons.Add("MACD bearish crossover (MACD < Signal)");
//                }
//            }

//            return new TimeframeSignal { Bias = bias, Strength = strength, Reasons = reasons };
//        }

//        /// <summary>
//        /// Legacy method - kept for interface compatibility
//        /// Use the async version instead
//        /// </summary>
//        Task<List<TradingSignal>> ITradingIdeaService.GenerateTradingIdeasAsync(
//            Dictionary<string, Dictionary<string, List<MarketData>>> dataByTimeframe,
//            MacroDataCollection macro,
//            Dictionary<string, List<MarketData>> dxyByTimeframe)
//        {
//            // Call the async implementation
//            return GenerateTradingIdeasAsync(dataByTimeframe, macro, dxyByTimeframe);
//        }
//    }
//}