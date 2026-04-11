using Orion.WebApps.AanalysisDashboardBlazor.Interfaces;
using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class TradingIdeaService : ITradingIdeaService
    {
        private const int AdxTrendMin = 25;
        private const double AtrSlMult = 1.5;

        private readonly TechnicalIndicatorService _indicatorService;
        private readonly EntrySignalService _entrySignalService;

        public TradingIdeaService(TechnicalIndicatorService indicatorService, EntrySignalService entrySignalService)
        {
            _indicatorService = indicatorService;
            _entrySignalService = entrySignalService;
        }

        /// <summary>
        /// Generates trading ideas based on market data, macroeconomic data, and DXY data.
        /// </summary>
        /// <param name="dataByTimeframe"></param>
        /// <param name="macro"></param>
        /// <param name="dxyByTimeframe"></param>
        /// <returns></returns>
        public async Task<List<TradingIdea>> GenerateTradingIdeasAsync(
            Dictionary<string, Dictionary<string, List<MarketData>>> dataByTimeframe,  
            MacroDataCollection macro,
            Dictionary<string, List<MarketData>> dxyByTimeframe)
        {
            var ideas = new List<TradingIdea>();

            foreach (var asset in Assets.All)
            {
                var enrichedData = new Dictionary<string, List<EnrichedMarketData>>();

                foreach (var tf in TimeframeConfigs.Mappings)
                {
                    if (dataByTimeframe.ContainsKey(tf.Key) &&
                        dataByTimeframe[tf.Key].ContainsKey(asset.Key) &&
                        dataByTimeframe[tf.Key][asset.Key].Any())
                    {
                        enrichedData[tf.Key] = _indicatorService.AddIndicators(dataByTimeframe[tf.Key][asset.Key]);
                    }
                }

                if (!enrichedData.ContainsKey("Daily") || !enrichedData.ContainsKey("4 Hour") ||
                    !enrichedData.ContainsKey("Hourly") || !enrichedData.ContainsKey("15 Minute"))
                    continue;

                var daily = enrichedData["Daily"];
                var fourHour = enrichedData["4 Hour"];
                var hourly = enrichedData["Hourly"];
                var fifteenMin = enrichedData["15 Minute"];
                var weekly = enrichedData.ContainsKey("Weekly") ? enrichedData["Weekly"] : null;

                var signals = new Dictionary<string, TimeframeSignal>
                {
                    ["weekly"] = AnalyzeTimeframe(weekly?.LastOrDefault()),
                    ["daily"] = AnalyzeTimeframe(daily.LastOrDefault()),
                    ["4h"] = AnalyzeTimeframe(fourHour.LastOrDefault()),
                    ["1h"] = AnalyzeTimeframe(hourly.LastOrDefault())
                };

                var weights = new Dictionary<string, int> { ["weekly"] = 4, ["daily"] = 3, ["4h"] = 2, ["1h"] = 1 };
                var totalLongStrength = 0;
                var totalShortStrength = 0;

                foreach (var tf in signals)
                {
                    if (tf.Value.Bias == "Long")
                        totalLongStrength += tf.Value.Strength * weights.GetValueOrDefault(tf.Key, 1);
                    else if (tf.Value.Bias == "Short")
                        totalShortStrength += tf.Value.Strength * weights.GetValueOrDefault(tf.Key, 1);
                }

                var finalBias = totalLongStrength > totalShortStrength ? "Long" :
                               totalShortStrength > totalLongStrength ? "Short" : "Neutral";
                var finalStrength = Math.Max(totalLongStrength, totalShortStrength);

                if (finalBias == "Neutral")
                    continue;

                var entrySignal = _entrySignalService.Get15mEntrySignal(fifteenMin, finalBias);

                var conviction = finalStrength >= 8 ? "High" : finalStrength >= 4 ? "Medium" : "Low";
                if (entrySignal.Signal != 0 && entrySignal.Confidence >= 3 && conviction == "Medium")
                    conviction = "High";

                var thesisParts = new List<string>();
                foreach (var tf in signals)
                {
                    if (tf.Value.Reasons.Any())
                        thesisParts.Add($"{tf.Key.ToUpper()}: {string.Join(", ", tf.Value.Reasons.Take(2))}");
                }

                if (entrySignal.Signal != 0 && entrySignal.Reasons.Any())
                {
                    thesisParts.Add($"15M Entry: {string.Join(", ", entrySignal.Reasons.Take(2))}");
                    thesisParts.Add($"Entry Confidence: {entrySignal.Confidence}/5");
                }

                var thesis = thesisParts.Any() ? string.Join(" | ", thesisParts) : "No clear signals";

                // Gold macro adjustment
                if (asset.Key == "XAU/USD" && dxyByTimeframe.ContainsKey("Daily") && dxyByTimeframe["Daily"].Any())
                {
                    var dxyData = dxyByTimeframe["Daily"];
                    var dxyChange = dxyData.Count > 5 ?
                        (dxyData[^1].Close - dxyData[^6].Close) / dxyData[^6].Close : 0m;
                    var rates = macro.USD.Rates;

                    if (dxyChange > 0.002m && rates > 3)
                    {
                        thesis += " | Macro: Bearish ❌";
                        if (finalBias != "Short")
                        {
                            finalBias = "Short";
                            conviction = "High";
                        }
                    }
                    else if (dxyChange < -0.002m && rates < 3)
                    {
                        thesis += " | Macro: Bullish ✅";
                        if (finalBias != "Long")
                        {
                            finalBias = "Long";
                            conviction = "High";
                        }
                    }
                    else
                    {
                        thesis += " | Macro: Neutral ⚖️";
                    }
                }

                var currentPrice = fifteenMin.Last().Close;
                var atr = fifteenMin.Last().Indicators.ATR ?? currentPrice * 0.005m;

                decimal entry, stopLoss, tp1, tp2, riskReward;

                if (finalBias == "Long")
                {
                    entry = currentPrice;
                    stopLoss = currentPrice - (atr * (decimal)AtrSlMult);
                    tp1 = currentPrice + (atr * 1.0m);
                    tp2 = currentPrice + (atr * 2.0m);
                    riskReward = (entry - stopLoss) != 0 ? (tp1 - entry) / (entry - stopLoss) : 0;
                }
                else
                {
                    entry = currentPrice;
                    stopLoss = currentPrice + (atr * (decimal)AtrSlMult);
                    tp1 = currentPrice - (atr * 1.0m);
                    tp2 = currentPrice - (atr * 2.0m);
                    riskReward = (stopLoss - entry) != 0 ? (entry - tp1) / (stopLoss - entry) : 0;
                }

                ideas.Add(new TradingIdea
                {
                    Pair = asset.Key,
                    Bias = finalBias,
                    Conviction = conviction,
                    StrengthScore = finalStrength,
                    Thesis = thesis,
                    Entry = entry,
                    TakeProfit1 = tp1,
                    TakeProfit2 = tp2,
                    StopLoss = stopLoss,
                    RiskReward = riskReward,
                    EntrySignal = entrySignal.Signal != 0 ? entrySignal : null,
                    TimeframeSignals = signals
                });
            }

            return ideas.OrderByDescending(x => x.Conviction == "High")
                       .ThenByDescending(x => x.StrengthScore)
                       .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private TimeframeSignal AnalyzeTimeframe(EnrichedMarketData? data)
        {
            if (data == null)
                return new TimeframeSignal { Bias = null, Strength = 0, Reasons = new List<string>() };

            var bias = (string?)null;
            var strength = 0;
            var reasons = new List<string>();

            var adx = data.Indicators.ADX ?? 0;
            if (adx > AdxTrendMin)
            {
                strength++;
                reasons.Add($"Strong trend (ADX={adx:F1})");
            }

            var rsi = data.Indicators.RSI ?? 50;
            if (rsi < 30)
            {
                bias = "Long";
                strength += rsi < 25 ? 2 : 1;
                reasons.Add($"Oversold RSI ({rsi:F1})");
            }
            else if (rsi > 70)
            {
                bias = "Short";
                strength += rsi > 75 ? 2 : 1;
                reasons.Add($"Overbought RSI ({rsi:F1})");
            }

            var price = data.Close;
            var ema20 = data.Indicators.EMA20 ?? price;
            var ema50 = data.Indicators.EMA50 ?? price;

            if (price > ema20 && ema20 > ema50)
            {
                if (bias != "Short")
                {
                    bias = "Long";
                    strength++;
                    reasons.Add("Bullish EMA alignment");
                }
            }
            else if (price < ema20 && ema20 < ema50)
            {
                if (bias != "Long")
                {
                    bias = "Short";
                    strength++;
                    reasons.Add("Bearish EMA alignment");
                }
            }

            var macd = data.Indicators.MACD ?? 0;
            var macdSignal = data.Indicators.MACDSignal ?? 0;
            if (macd > macdSignal)
            {
                if (bias != "Short")
                {
                    bias = "Long";
                    strength++;
                    reasons.Add("MACD bullish crossover");
                }
            }
            else if (macd < macdSignal)
            {
                if (bias != "Long")
                {
                    bias = "Short";
                    strength++;
                    reasons.Add("MACD bearish crossover");
                }
            }

            return new TimeframeSignal { Bias = bias, Strength = strength, Reasons = reasons };
        }
    }
}