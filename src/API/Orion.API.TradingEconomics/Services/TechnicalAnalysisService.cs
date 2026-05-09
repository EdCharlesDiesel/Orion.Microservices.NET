//using Microsoft.Extensions.Options;
//using Orion.API.TradingEconomics.Configuration;
//using Orion.API.TradingEconomics.Entities;
//using Orion.API.TradingEconomics.Interfaces;
//using System.Collections.Concurrent;

//namespace Orion.API.TradingEconomics.Services
//{
//    public partial class TechnicalAnalysisService : ITechnicalAnalysisService
//    {
//        private readonly AppConfiguration _config;
//        private readonly ILogger<TechnicalAnalysisService> _logger;

//        public TechnicalAnalysisService(
//            IOptions<AppConfiguration> config,
//            ILogger<TechnicalAnalysisService> logger)
//        {
//            _config = config.Value;
//            _logger = logger;
//        }

//        #region Public API - Indicator Calculation

//        public TechnicalIndicators CalculateIndicators(List<OHLCVData> data)
//        {
//            if (data == null || data.Count < IndicatorParams.BB_WINDOW)
//                return new TechnicalIndicators();

//            var closes = data.Select(d => d.Close).ToList();
//            var highs = data.Select(d => d.High).ToList();
//            var lows = data.Select(d => d.Low).ToList();

//            var result = new TechnicalIndicators
//            {
//                RSI = CalculateRSI(closes, IndicatorParams.RSI_WINDOW),
//                SMA20 = CalculateSMA(closes, IndicatorParams.SMA_SHORT),
//                SMA50 = CalculateSMA(closes, IndicatorParams.SMA_LONG),
//                EMA20 = CalculateEMA(closes, IndicatorParams.SMA_SHORT),
//                EMA50 = CalculateEMA(closes, IndicatorParams.SMA_LONG),
//                ATR = CalculateATR(highs, lows, closes, IndicatorParams.ATR_WINDOW),
//                Support20 = CalculateRollingMin(lows, 20),
//                Resistance20 = CalculateRollingMax(highs, 20)
//            };

//            // MACD
//            var (macd, signal, histogram) = CalculateMACD(
//                closes,
//                IndicatorParams.MACD_FAST,
//                IndicatorParams.MACD_SLOW,
//                IndicatorParams.MACD_SIGNAL);

//            result.MACD = macd;
//            result.MACDSignal = signal;
//            result.MACDHistogram = histogram;

//            // Bollinger Bands
//            var (upper, middle, lower) = CalculateBollingerBands(
//                closes,
//                IndicatorParams.BB_WINDOW,
//                IndicatorParams.BB_STD_DEV);

//            result.BBUpper = upper;
//            result.BBMiddle = middle;
//            result.BBLower = lower;

//            // Stochastic
//            var (stochK, stochD) = CalculateStochastic(
//                highs, lows, closes,
//                IndicatorParams.STOCH_WINDOW,
//                IndicatorParams.STOCH_SMOOTH);

//            result.StochK = stochK;
//            result.StochD = stochD;

//            // ADX
//            var (adx, adxPos, adxNeg) = CalculateADX(
//                highs, lows, closes,
//                IndicatorParams.ADX_WINDOW);

//            result.ADX = adx;
//            result.ADXPos = adxPos;
//            result.ADXNeg = adxNeg;

//            return result;
//        }

//        #endregion

//        #region Public API - Entry Signals

//        public EntrySignalResult GetEntrySignal(List<OHLCVData> data15m, string bias)
//        {
//            if (data15m == null || data15m.Count < 5)
//                return new EntrySignalResult
//                {
//                    Signal = 0,
//                    Confidence = 0,
//                    Reasons = new List<string> { "Insufficient 15-min data" }
//                };

//            var indicators = CalculateIndicators(data15m);
//            var latest = data15m.Last();

//            var (k, d, prevK, prevD) = GetStochasticValues(indicators);
//            var rsi = indicators.RSI.LastOrDefault() ?? 50;
//            var bbLower = indicators.BBLower.LastOrDefault() ?? latest.Close * 0.99m;
//            var bbUpper = indicators.BBUpper.LastOrDefault() ?? latest.Close * 1.01m;

//            var result = new EntrySignalResult
//            {
//                Price = latest.Close,
//                StochK = k,
//                StochD = d,
//                RSI = rsi
//            };

//            EvaluateEntryConditions(bias, result, latest, k, d, prevK, prevD, rsi, bbLower, bbUpper);

//            result.Confidence = Math.Min(result.Confidence, 5);

//            if (result.Reasons.Count == 0)
//                result.Reasons.Add($"Awaiting trigger - K={k:F1}, RSI={rsi:F1}");

//            return result;
//        }

//        #endregion

//        #region Public API - Bias Analysis

//        public async Task<List<BiasAnalysisResult>> GenerateBiasDashboardAsync(
//            Dictionary<string, Dictionary<string, MarketDataResponse>> allData,
//            CancellationToken cancellationToken)
//        {
//            if (allData == null || allData.Count == 0)
//            {
//                _logger.LogWarning("No data provided for bias analysis");
//                return new List<BiasAnalysisResult>();
//            }

//            var results = new ConcurrentBag<BiasAnalysisResult>();

//            await Parallel.ForEachAsync(allData, cancellationToken, async (pairData, ct) =>
//            {
//                var (pair, timeframes) = pairData;

//                try
//                {
//                    var biasResult = await AnalyzePairBiasesAsync(pair, timeframes, ct);
//                    results.Add(biasResult);

//                    _logger.LogDebug(
//                        "Bias analysis complete for {Pair}: Overall={Bias}, Tradeable={Tradeable}",
//                        pair, biasResult.OverallBias.Bias, biasResult.IsTradeable);
//                }
//                catch (OperationCanceledException)
//                {
//                    _logger.LogInformation("Bias analysis cancelled for {Pair}", pair);
//                    throw;
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error analyzing bias for {Pair}", pair);
//                    results.Add(CreateErrorResult(pair, ex.Message));
//                }
//            });

//            return results.ToList();
//        }

//        #endregion

//        #region Public API - Trading Ideas Generation

//        public async Task<List<TradingIdea>> GenerateTradingIdeasAsync(
//            Dictionary<string, Dictionary<string, MarketDataResponse>> allData,
//            CancellationToken cancellationToken)
//        {
//            // Implementation would parallel the Python multi-timeframe analysis logic
//            await Task.CompletedTask;
//            return new List<TradingIdea>();
//        }

//        public async Task<List<SwingTradingIdea>> GenerateSwingIdeasAsync(
//            Dictionary<string, Dictionary<string, MarketDataResponse>> allData,
//            CancellationToken cancellationToken)
//        {
//            await Task.CompletedTask;
//            return new List<SwingTradingIdea>();
//        }

//        public async Task<TradingIdea?> AnalyzeMultiTimeframeAsync(
//            MarketDataResponse daily,
//            MarketDataResponse fourHour,
//            MarketDataResponse oneHour,
//            MarketDataResponse fifteenMin,
//            string pairName,
//            CancellationToken cancellationToken = default)
//        {
//            await Task.Yield();
//            cancellationToken.ThrowIfCancellationRequested();

//            var dailyIndicators = CalculateIndicators(daily.OHLCVData);
//            var h4Indicators = CalculateIndicators(fourHour.OHLCVData);
//            var h1Indicators = CalculateIndicators(oneHour.OHLCVData);

//            var scoring = ScoreMultiTimeframe(
//                daily, fourHour, oneHour,
//                dailyIndicators, h4Indicators, h1Indicators);

//            if (scoring.LongScore == scoring.ShortScore)
//                return null;

//            return BuildTradingIdea(
//                pairName, scoring, fifteenMin, oneHour, fourHour, h1Indicators);
//        }

//        #endregion

//        #region Public API - Stop Loss and Take Profit

//        public StopLossResult CalculateStopLoss(
//            List<OHLCVData> df,
//            string pair,
//            string bias,
//            decimal currentPrice,
//            decimal atr,
//            int lookback = IndicatorParams.SWING_LOOKBACK)
//        {
//            var atrMult = _config.PairATRMultipliers?.GetValueOrDefault(pair, _config.ATRSLMult) ?? _config.ATRSLMult;
//            var minDist = _config.PairMinStop?.GetValueOrDefault(pair, 0.0010m) ?? 0.0010m;
//            var buffer = atr * 0.25m;

//            var atrStop = CalculateATRStop(bias, currentPrice, atr, atrMult);
//            var swing = GetSwingStop(df, bias, lookback);

//            var (stop, method) = DetermineOptimalStop(
//                bias, currentPrice, atrStop, swing, buffer);

//            // Enforce minimum distance
//            var rawDist = Math.Abs(currentPrice - stop);
//            if (rawDist < minDist)
//            {
//                stop = bias == "Long"
//                    ? currentPrice - minDist
//                    : currentPrice + minDist;
//                method += " + min-dist enforced";
//            }

//            return new StopLossResult
//            {
//                Stop = stop,
//                Method = method,
//                DistancePips = PriceToPips(pair, Math.Abs(currentPrice - stop)),
//                DistancePrice = Math.Abs(currentPrice - stop),
//                IsValid = true
//            };
//        }

//        public TakeProfitResult CalculateTakeProfit(
//            List<OHLCVData> df,
//            string pair,
//            string bias,
//            decimal currentPrice,
//            decimal atr,
//            decimal stopLoss,
//            int lookback = IndicatorParams.SWING_LOOKBACK)
//        {
//            var stopDist = Math.Abs(currentPrice - stopLoss);
//            if (stopDist == 0) stopDist = atr;

//            var swing = GetSwingTarget(df, bias, lookback);

//            return bias == "Long"
//                ? CalculateLongTakeProfit(currentPrice, atr, stopDist, swing)
//                : CalculateShortTakeProfit(currentPrice, atr, stopDist, swing);
//        }

//        #endregion

//        #region Public API - Utility Methods

//        public decimal GetPipSize(string pair)
//        {
//            if (pair.Contains("JPY")) return 0.01m;
//            if (pair == "XAU/USD") return 0.10m;
//            if (pair == "BTC/USD") return 1.0m;
//            if (pair.Contains("ZAR")) return 0.001m;
//            return 0.0001m;
//        }

//        public decimal PriceToPips(string pair, decimal distance)
//        {
//            var pipSize = GetPipSize(pair);
//            return pipSize > 0 ? Math.Round(distance / pipSize, 1) : 0;
//        }

 

//        public TrendBiasResult DetermineTrendBias(
//            List<OHLCVData> daily,
//            List<OHLCVData> fourHour,
//            List<OHLCVData> oneHour)
//        {
//            var result = new TrendBiasResult();
//            var dailyIndicators = CalculateIndicators(daily);
//            var h4Indicators = CalculateIndicators(fourHour);
//            var h1Indicators = CalculateIndicators(oneHour);

//            var scoring = ScoreTrendBias(daily, fourHour, oneHour,
//                dailyIndicators, h4Indicators, h1Indicators);

//            result.ADX = scoring.ADX;
//            result.IsTrending = scoring.IsTrending;
//            result.Strength = scoring.Strength;
//            result.Bias = scoring.Bias;
//            result.Reasons = scoring.Reasons;

//            return result;
//        }

//        public bool HasValidEntrySetup(List<OHLCVData> data15m, string bias, out List<string> reasons)
//        {
//            reasons = new List<string>();
//            var signal = GetEntrySignal(data15m, bias);

//            if (signal.Signal == 0)
//            {
//                reasons = signal.Reasons;
//                return false;
//            }

//            var isValidDirection = (bias == "Long" && signal.Signal == 1) ||
//                                   (bias == "Short" && signal.Signal == -1);

//            if (isValidDirection)
//            {
//                reasons.Add($"Valid {bias} entry signal with confidence {signal.Confidence}/5");
//                reasons.AddRange(signal.Reasons);
//                return true;
//            }

//            reasons.Add($"Signal direction ({signal.Signal}) doesn't match bias ({bias})");
//            return false;
//        }

//        #endregion

//        #region Private - Bias Analysis Helpers

//        private async Task<BiasAnalysisResult> AnalyzePairBiasesAsync(
//            string pair,
//            Dictionary<string, MarketDataResponse> timeframes,
//            CancellationToken cancellationToken)
//        {
//            var biasResult = new BiasAnalysisResult
//            {
//                Pair = pair,
//                Timestamp = DateTime.UtcNow,
//                TimeframeBiases = new Dictionary<string, BiasInfo>()
//            };

//            foreach (var (timeframe, data) in timeframes)
//            {
//                cancellationToken.ThrowIfCancellationRequested();

//                if (data?.OHLCVData == null || data.OHLCVData.Count < IndicatorParams.MIN_DATA_POINTS)
//                {
//                    biasResult.TimeframeBiases[timeframe] = new BiasInfo
//                    {
//                        Bias = "Neutral",
//                        Strength = 0,
//                        Reasons = new List<string> { "Insufficient data" }
//                    };
//                    continue;
//                }

//                var bias = AnalyzeBias(data.OHLCVData);
//                biasResult.TimeframeBiases[timeframe] = bias;
//            }

//            biasResult.OverallBias = CalculateOverallBias(biasResult.TimeframeBiases);
//            biasResult.IsTradeable = DetermineTradeability(biasResult);

//            return biasResult;
//        }

//        private BiasInfo AnalyzeBias(List<OHLCVData> data)
//        {
//            if (data == null || data.Count < 200)
//            {
//                return new BiasInfo
//                {
//                    Bias = "Neutral",
//                    Confidence = 0,
//                    Strength = 0,
//                    Reasons = new List<string> { "Not enough data to calculate bias" }
//                };
//            }

//            var closes = data.Select(d => d.Close).ToArray();

//            var sma20 = CalculateSMAArray(closes, 20);
//            var sma50 = CalculateSMAArray(closes, 50);
//            var sma200 = CalculateSMAArray(closes, 200);
//            var rsi = CalculateRSIArray(closes, 14);

//            var adxResult = CalculateADXWithDi(data, 14);

//            var currentPrice = closes.Last();
//            var currentSMA20 = sma20.Last();
//            var currentSMA50 = sma50.Last();
//            var currentSMA200 = sma200.Last();
//            var currentADX = adxResult.Adx;
//            var currentRSI = rsi.Last();
//            var currentPlusDi = adxResult.PlusDi;
//            var currentMinusDi = adxResult.MinusDi;

//            return DetermineBiasFromValues(
//                currentPrice,
//                currentSMA20,
//                currentSMA50,
//                currentSMA200,
//                currentADX,
//                currentRSI,
//                currentPlusDi,
//                currentMinusDi);
//        }

//        private static AdxResult CalculateADXWithDi(List<OHLCVData> data, int period)
//        {
//            if (data == null || data.Count < period + 2)
//                return new AdxResult();

//            var plusDm = new List<decimal>();
//            var minusDm = new List<decimal>();
//            var trueRanges = new List<decimal>();

//            for (var i = 1; i < data.Count; i++)
//            {
//                var upMove = data[i].High - data[i - 1].High;
//                var downMove = data[i - 1].Low - data[i].Low;

//                plusDm.Add(upMove > downMove && upMove > 0 ? upMove : 0);
//                minusDm.Add(downMove > upMove && downMove > 0 ? downMove : 0);

//                var tr1 = data[i].High - data[i].Low;
//                var tr2 = Math.Abs(data[i].High - data[i - 1].Close);
//                var tr3 = Math.Abs(data[i].Low - data[i - 1].Close);

//                trueRanges.Add(Math.Max(tr1, Math.Max(tr2, tr3)));
//            }

//            var atr = trueRanges.Take(period).Sum();
//            var plusDmSmoothed = plusDm.Take(period).Sum();
//            var minusDmSmoothed = minusDm.Take(period).Sum();

//            var dxValues = new List<decimal>();

//            decimal latestPlusDi = 0;
//            decimal latestMinusDi = 0;

//            for (var i = period; i < trueRanges.Count; i++)
//            {
//                atr = atr - (atr / period) + trueRanges[i];
//                plusDmSmoothed = plusDmSmoothed - (plusDmSmoothed / period) + plusDm[i];
//                minusDmSmoothed = minusDmSmoothed - (minusDmSmoothed / period) + minusDm[i];

//                latestPlusDi = atr == 0 ? 0 : 100m * (plusDmSmoothed / atr);
//                latestMinusDi = atr == 0 ? 0 : 100m * (minusDmSmoothed / atr);

//                var diSum = latestPlusDi + latestMinusDi;
//                var dx = diSum == 0
//                    ? 0
//                    : 100m * Math.Abs(latestPlusDi - latestMinusDi) / diSum;

//                dxValues.Add(dx);
//            }

//            if (dxValues.Count == 0)
//                return new AdxResult
//                {
//                    PlusDi = latestPlusDi,
//                    MinusDi = latestMinusDi,
//                    Adx = 0
//                };

//            var adx = dxValues.Count < period
//                ? dxValues.Average()
//                : dxValues.TakeLast(period).Average();

//            return new AdxResult
//            {
//                Adx = adx,
//                PlusDi = latestPlusDi,
//                MinusDi = latestMinusDi
//            };
//        }

//        private BiasInfo DetermineBiasFromValues(
//            decimal price, decimal sma20, decimal sma50, decimal sma200,
//            decimal adx, decimal rsi, decimal plusDi, decimal minusDi)
//        {
//            var biasInfo = new BiasInfo();
//            var reasons = new List<string>();
//            var bullishScore = 0;
//            var bearishScore = 0;

//            // Price vs Moving Averages
//            if (price > sma20) { bullishScore++; reasons.Add("Price above 20 SMA"); }
//            else { bearishScore++; reasons.Add("Price below 20 SMA"); }

//            if (price > sma50) { bullishScore++; reasons.Add("Price above 50 SMA"); }
//            else { bearishScore++; reasons.Add("Price below 50 SMA"); }

//            // SMA Alignment
//            if (sma20 > sma50 && sma50 > sma200)
//            {
//                bullishScore += 2;
//                reasons.Add("Bullish SMA alignment");
//            }
//            else if (sma20 < sma50 && sma50 < sma200)
//            {
//                bearishScore += 2;
//                reasons.Add("Bearish SMA alignment");
//            }

//            // ADX Trend Strength
//            if (adx > _config.ADXTrendMin)
//            {
//                if (plusDi > minusDi)
//                {
//                    bullishScore += 2;
//                    reasons.Add($"Strong bullish trend (ADX: {adx:F1})");
//                }
//                else
//                {
//                    bearishScore += 2;
//                    reasons.Add($"Strong bearish trend (ADX: {adx:F1})");
//                }
//            }
//            else
//            {
//                reasons.Add($"Weak trend (ADX: {adx:F1})");
//            }

//            // RSI
//            if (rsi < _config.RSI_OS) { bullishScore++; reasons.Add($"RSI oversold ({rsi:F1})"); }
//            else if (rsi > _config.RSI_OB) { bearishScore++; reasons.Add($"RSI overbought ({rsi:F1})"); }

//            // Determine final bias
//            var totalScore = bullishScore - bearishScore;
//            biasInfo.Strength = Math.Abs(totalScore);
//            biasInfo.Reasons = reasons;

//            if (totalScore >= 2)
//            {
//                biasInfo.Bias = "Bullish";
//                biasInfo.Confidence = CalculateConfidence(bullishScore, bearishScore);
//            }
//            else if (totalScore <= -2)
//            {
//                biasInfo.Bias = "Bearish";
//                biasInfo.Confidence = CalculateConfidence(bearishScore, bullishScore);
//            }
//            else
//            {
//                biasInfo.Bias = "Neutral";
//                biasInfo.Confidence = 0;
//            }

//            return biasInfo;
//        }

//        public decimal CalculateADX(List<OHLCVData> data, int window = IndicatorParams.ADX_WINDOW)
//        {
//            if (data == null || data.Count < window * 2) return 0;

//            var highs = data.Select(d => d.High).ToList();
//            var lows = data.Select(d => d.Low).ToList();
//            var closes = data.Select(d => d.Close).ToList();

//            var (adx, _, _) = CalculateADX(highs, lows, closes, window);
//            return adx.LastOrDefault() ?? 0;
//        }

//        private BiasInfo CalculateOverallBias(Dictionary<string, BiasInfo> timeframeBiases)
//        {
//            var overall = new BiasInfo();
//            var reasons = new List<string>();

//            var weights = new Dictionary<string, int>
//            {
//                ["Weekly"] = 3,
//                ["Daily"] = 3,
//                ["4 Hour"] = 2,
//                ["Hourly"] = 1,
//                ["15 Minute"] = 1
//            };

//            var bullishWeight = 0m;
//            var bearishWeight = 0m;
//            var totalWeight = 0m;

//            foreach (var (timeframe, bias) in timeframeBiases)
//            {
//                var weight = weights.GetValueOrDefault(timeframe, 1);
//                totalWeight += weight;

//                if (bias.Bias == "Bullish")
//                {
//                    bullishWeight += weight;
//                    reasons.Add($"{timeframe}: Bullish");
//                }
//                else if (bias.Bias == "Bearish")
//                {
//                    bearishWeight += weight;
//                    reasons.Add($"{timeframe}: Bearish");
//                }
//            }

//            if (bullishWeight > bearishWeight && bullishWeight > totalWeight * 0.5m)
//            {
//                overall.Bias = "Bullish";
//                overall.Confidence = bullishWeight / totalWeight;
//            }
//            else if (bearishWeight > bullishWeight && bearishWeight > totalWeight * 0.5m)
//            {
//                overall.Bias = "Bearish";
//                overall.Confidence = bearishWeight / totalWeight;
//            }
//            else
//            {
//                overall.Bias = "Neutral";
//                overall.Confidence = 0;
//            }

//            overall.Reasons = reasons;
//            overall.Strength = (int)Math.Abs(bullishWeight - bearishWeight);

//            return overall;
//        }

//        private bool DetermineTradeability(BiasAnalysisResult result)
//        {
//            if (result.OverallBias.Bias == "Neutral")
//                return false;

//            if (result.OverallBias.Confidence < IndicatorParams.MIN_CONFIDENCE)
//                return false;

//            var higherTimeframes = new[] { "Weekly", "Daily" };
//            return higherTimeframes.All(tf =>
//                !result.TimeframeBiases.TryGetValue(tf, out var bias) ||
//                bias.Bias == result.OverallBias.Bias);
//        }

//        private BiasAnalysisResult CreateErrorResult(string pair, string errorMessage)
//        {
//            return new BiasAnalysisResult
//            {
//                Pair = pair,
//                Timestamp = DateTime.UtcNow,
//                OverallBias = new BiasInfo { Bias = "Error", Strength = 0 },
//                IsTradeable = false,
//                Error = errorMessage
//            };
//        }

//        #endregion

//        #region Private - Entry Signal Helpers

//        private (decimal k, decimal d, decimal prevK, decimal prevD) GetStochasticValues(
//            TechnicalIndicators indicators)
//        {
//            var kIdx = indicators.StochK.Count - 1;
//            var dIdx = indicators.StochD.Count - 1;
//            var prevKIdx = kIdx - 1;
//            var prevDIdx = dIdx - 1;

//            return (
//                kIdx >= 0 ? indicators.StochK[kIdx] ?? 50 : 50,
//                dIdx >= 0 ? indicators.StochD[dIdx] ?? 50 : 50,
//                prevKIdx >= 0 ? indicators.StochK[prevKIdx] ?? 50 : 50,
//                prevDIdx >= 0 ? indicators.StochD[prevDIdx] ?? 50 : 50
//            );
//        }

//        private void EvaluateEntryConditions(
//            string bias,
//            EntrySignalResult result,
//            OHLCVData latest,
//            decimal k, decimal d, decimal prevK, decimal prevD,
//            decimal rsi, decimal bbLower, decimal bbUpper)
//        {
//            if (bias == "Long")
//            {
//                EvaluateLongEntry(result, latest, k, d, prevK, prevD, rsi, bbLower);
//            }
//            else if (bias == "Short")
//            {
//                EvaluateShortEntry(result, latest, k, d, prevK, prevD, rsi, bbUpper);
//            }
//        }

//        private void EvaluateLongEntry(
//            EntrySignalResult result, OHLCVData latest,
//            decimal k, decimal d, decimal prevK, decimal prevD,
//            decimal rsi, decimal bbLower)
//        {
//            if (prevK <= prevD && d < k && k < _config.StochOS)
//            {
//                result.Signal = 1;
//                result.Confidence += 2;
//                result.Reasons.Add($"Stochastic bullish crossover (K={k:F1})");
//            }

//            if (rsi < _config.RSI_OS)
//            {
//                result.Confidence += 1;
//                result.Reasons.Add($"RSI oversold ({rsi:F1})");
//            }

//            if (latest.Close <= bbLower * 1.002m)
//            {
//                result.Confidence += 1;
//                result.Reasons.Add("Price at lower Bollinger Band");
//            }
//        }

//        private void EvaluateShortEntry(
//            EntrySignalResult result, OHLCVData latest,
//            decimal k, decimal d, decimal prevK, decimal prevD,
//            decimal rsi, decimal bbUpper)
//        {
//            if (prevK >= prevD && d > k && k > _config.StochOB)
//            {
//                result.Signal = -1;
//                result.Confidence += 2;
//                result.Reasons.Add($"Stochastic bearish crossover (K={k:F1})");
//            }

//            if (rsi > _config.RSI_OB)
//            {
//                result.Confidence += 1;
//                result.Reasons.Add($"RSI overbought ({rsi:F1})");
//            }

//            if (latest.Close >= bbUpper * 0.998m)
//            {
//                result.Confidence += 1;
//                result.Reasons.Add("Price at upper Bollinger Band");
//            }
//        }

//        #endregion

//        #region Private - Multi-Timeframe Scoring

//        private MultiTimeframeScore ScoreMultiTimeframe(
//            MarketDataResponse daily,
//            MarketDataResponse fourHour,
//            MarketDataResponse oneHour,
//            TechnicalIndicators dailyIndicators,
//            TechnicalIndicators h4Indicators,
//            TechnicalIndicators h1Indicators)
//        {
//            var score = new MultiTimeframeScore();
//            var dailyLatest = daily.OHLCVData.Last();
//            var h4Latest = fourHour.OHLCVData.Last();
//            var h1Latest = oneHour.OHLCVData.Last();

//            // Daily analysis
//            var dClose = dailyLatest.Close;
//            var dEma20 = dailyIndicators.EMA20.LastOrDefault() ?? dClose;
//            var dTrend = dClose > dEma20 ? "Long" : "Short";
//            var dRsi = dailyIndicators.RSI.LastOrDefault() ?? 50;
//            var dAdx = dailyIndicators.ADX.LastOrDefault() ?? 0;

//            if (dTrend == "Long") ScoreLong(score, 2, "Daily: Bullish EMA alignment");
//            else ScoreShort(score, 2, "Daily: Bearish EMA alignment");

//            if (dRsi < 40) ScoreLong(score, 1, $"Daily RSI oversold ({dRsi:F1})");
//            else if (dRsi > 60) ScoreShort(score, 1, $"Daily RSI overbought ({dRsi:F1})");

//            if (dAdx > _config.ADXTrendMin)
//            {
//                if (dTrend == "Long") ScoreLong(score, 1, $"Strong trend (ADX={dAdx:F1})");
//                else ScoreShort(score, 1, $"Strong trend (ADX={dAdx:F1})");
//            }

//            // 4H analysis
//            var h4Ema20 = h4Indicators.EMA20.LastOrDefault() ?? h4Latest.Close;
//            var h4Ema50 = h4Indicators.EMA50.LastOrDefault() ?? h4Latest.Close;
//            var h4Trend = h4Ema20 > h4Ema50 ? "Long" : "Short";
//            var h4Macd = h4Indicators.MACD.LastOrDefault() ?? 0;
//            var h4Sig = h4Indicators.MACDSignal.LastOrDefault() ?? 0;

//            if (h4Trend == "Long") ScoreLong(score, 1, "4H: EMA20 > EMA50");
//            else ScoreShort(score, 1, "4H: EMA20 < EMA50");

//            if (h4Macd > h4Sig) ScoreLong(score, 1, "4H: MACD bullish");
//            else ScoreShort(score, 1, "4H: MACD bearish");

//            // 1H analysis
//            var h1Ema20 = h1Indicators.EMA20.LastOrDefault() ?? h1Latest.Close;
//            var h1Ema50 = h1Indicators.EMA50.LastOrDefault() ?? h1Latest.Close;
//            var h1Trend = h1Ema20 > h1Ema50 ? "Long" : "Short";
//            var h1Rsi = h1Indicators.RSI.LastOrDefault() ?? 50;

//            if (h1Trend == "Long") ScoreLong(score, 1, "1H: Bullish EMA alignment");
//            else ScoreShort(score, 1, "1H: Bearish EMA alignment");

//            if (h1Rsi < 45) ScoreLong(score, 1, $"1H RSI supportive ({h1Rsi:F1})");
//            else if (h1Rsi > 55) ScoreShort(score, 1, $"1H RSI resistive ({h1Rsi:F1})");

//            return score;
//        }

//        private TrendBiasScore ScoreTrendBias(
//            List<OHLCVData> daily,
//            List<OHLCVData> fourHour,
//            List<OHLCVData> oneHour,
//            TechnicalIndicators dailyIndicators,
//            TechnicalIndicators h4Indicators,
//            TechnicalIndicators h1Indicators)
//        {
//            var score = new TrendBiasScore();
//            var dailyLatest = daily.Last();
//            var dClose = dailyLatest.Close;
//            var dEma20 = dailyIndicators.EMA20.LastOrDefault() ?? dClose;
//            var dEma50 = dailyIndicators.EMA50.LastOrDefault() ?? dClose;
//            var dAdx = dailyIndicators.ADX.LastOrDefault() ?? 0;

//            score.ADX = dAdx;
//            score.IsTrending = dAdx > _config.ADXTrendMin;

//            var bullishScore = 0;
//            var bearishScore = 0;

//            // Daily trend
//            if (dClose > dEma20) { bullishScore += 2; score.Reasons.Add("Daily: Price > EMA20"); }
//            else { bearishScore += 2; score.Reasons.Add("Daily: Price < EMA20"); }

//            if (dEma20 > dEma50) { bullishScore += 1; score.Reasons.Add("Daily: EMA20 > EMA50"); }
//            else { bearishScore += 1; score.Reasons.Add("Daily: EMA20 < EMA50"); }

//            if (score.IsTrending)
//            {
//                if (dClose > dEma20) bullishScore += 1;
//                else bearishScore += 1;
//                score.Reasons.Add($"Daily ADX trending ({dAdx:F1})");
//            }

//            // 4H trend
//            var h4Latest = fourHour.Last();
//            var h4Ema20 = h4Indicators.EMA20.LastOrDefault() ?? h4Latest.Close;
//            if (h4Latest.Close > h4Ema20) { bullishScore += 1; score.Reasons.Add("4H: Price > EMA20"); }
//            else { bearishScore += 1; score.Reasons.Add("4H: Price < EMA20"); }

//            // 1H trend
//            var h1Latest = oneHour.Last();
//            var h1Ema20 = h1Indicators.EMA20.LastOrDefault() ?? h1Latest.Close;
//            if (h1Latest.Close > h1Ema20) { bullishScore += 1; score.Reasons.Add("1H: Price > EMA20"); }
//            else { bearishScore += 1; score.Reasons.Add("1H: Price < EMA20"); }

//            score.Strength = Math.Max(bullishScore, bearishScore);
//            score.Bias = bullishScore > bearishScore ? "Long" :
//                        (bearishScore > bullishScore ? "Short" : "Neutral");

//            return score;
//        }

//        private void ScoreLong(MultiTimeframeScore score, int points, string reason)
//        {
//            score.LongScore += points;
//            score.Reasons.Add(reason);
//        }

//        private void ScoreShort(MultiTimeframeScore score, int points, string reason)
//        {
//            score.ShortScore += points;
//            score.Reasons.Add(reason);
//        }

//        #endregion

//        #region Private - Stop Loss / Take Profit Helpers

//        private decimal CalculateATRStop(string bias, decimal currentPrice, decimal atr, decimal atrMult)
//        {
//            return bias == "Long"
//                ? currentPrice - atr * atrMult
//                : currentPrice + atr * atrMult;
//        }

//        private (decimal stop, string method) DetermineOptimalStop(
//            string bias, decimal currentPrice, decimal atrStop,
//            decimal? swing, decimal buffer)
//        {
//            if (!swing.HasValue)
//                return (atrStop, "ATR");

//            if (bias == "Long")
//            {
//                var structStop = swing.Value - buffer;
//                if (structStop < currentPrice && structStop <= atrStop)
//                {
//                    return (structStop, "Swing Low");
//                }
//                return (atrStop, "ATR (struct too tight)");
//            }
//            else
//            {
//                var structStop = swing.Value + buffer;
//                if (structStop > currentPrice && structStop >= atrStop)
//                {
//                    return (structStop, "Swing High");
//                }
//                return (atrStop, "ATR (struct too tight)");
//            }
//        }

//        private TakeProfitResult CalculateLongTakeProfit(
//            decimal currentPrice, decimal atr, decimal stopDist, decimal? swing)
//        {
//            var tp1Atr = currentPrice + atr * _config.TP1ATRMult;
//            var tp2Atr = currentPrice + atr * _config.TP2ATRMult;
//            var (tp1, m1) = DetermineTP1(tp1Atr, swing, "Swing High", currentPrice);
//            var (tp2, m2) = DetermineTP2(tp2Atr, tp1, swing, "Swing High (ext)");

//            return new TakeProfitResult
//            {
//                TP1 = tp1,
//                TP2 = tp2,
//                MethodTP1 = m1,
//                MethodTP2 = m2,
//                RR1 = Math.Round((tp1 - currentPrice) / stopDist, 2),
//                RR2 = Math.Round((tp2 - currentPrice) / stopDist, 2),
//                TP1Valid = (tp1 - currentPrice) / stopDist >= _config.MinRR,
//                TP2Valid = (tp2 - currentPrice) / stopDist >= _config.MinRR
//            };
//        }

//        private TakeProfitResult CalculateShortTakeProfit(
//            decimal currentPrice, decimal atr, decimal stopDist, decimal? swing)
//        {
//            var tp1Atr = currentPrice - atr * _config.TP1ATRMult;
//            var tp2Atr = currentPrice - atr * _config.TP2ATRMult;
//            var (tp1, m1) = DetermineTP1(tp1Atr, swing, "Swing Low", currentPrice, true);
//            var (tp2, m2) = DetermineTP2(tp2Atr, tp1, swing, "Swing Low (ext)", true);

//            return new TakeProfitResult
//            {
//                TP1 = tp1,
//                TP2 = tp2,
//                MethodTP1 = m1,
//                MethodTP2 = m2,
//                RR1 = Math.Round((currentPrice - tp1) / stopDist, 2),
//                RR2 = Math.Round((currentPrice - tp2) / stopDist, 2),
//                TP1Valid = (currentPrice - tp1) / stopDist >= _config.MinRR,
//                TP2Valid = (currentPrice - tp2) / stopDist >= _config.MinRR
//            };
//        }

//        private (decimal price, string method) DetermineTP1(
//            decimal atrTP, decimal? swing, string swingMethod,
//            decimal currentPrice, bool isShort = false)
//        {
//            if (!swing.HasValue)
//                return (atrTP, $"ATR ×{_config.TP1ATRMult}");

//            var useSwing = isShort
//                ? atrTP < swing.Value && swing.Value < currentPrice
//                : currentPrice < swing.Value && swing.Value < atrTP;

//            return useSwing
//                ? (swing.Value, swingMethod)
//                : (atrTP, $"ATR ×{_config.TP1ATRMult}");
//        }

//        private (decimal price, string method) DetermineTP2(
//            decimal atrTP, decimal tp1, decimal? swing,
//            string swingMethod, bool isShort = false)
//        {
//            if (!swing.HasValue)
//                return (atrTP, $"ATR ×{_config.TP2ATRMult}");

//            var useSwing = isShort
//                ? atrTP < swing.Value && swing.Value < tp1
//                : tp1 < swing.Value && swing.Value < atrTP;

//            return useSwing
//                ? (swing.Value, swingMethod)
//                : (atrTP, $"ATR ×{_config.TP2ATRMult}");
//        }

//        public decimal? GetSwingStop(List<OHLCVData> df, string bias, int lookback = 20)
//        {
//            if (df == null || df.Count < lookback) return null;

//            var recent = df.TakeLast(lookback).ToList();
//            return bias == "Long" ? recent.Min(d => d.Low) :
//                   bias == "Short" ? recent.Max(d => d.High) : null;
//        }

//        public decimal? GetSwingTarget(List<OHLCVData> df, string bias, int lookback = 20)
//        {
//            if (df == null || df.Count < lookback) return null;

//            var recent = df.TakeLast(lookback).ToList();
//            return bias == "Long" ? recent.Max(d => d.High) :
//                   bias == "Short" ? recent.Min(d => d.Low) : null;
//        }

//        #endregion

//        #region Private - Trading Idea Builders

//        private TradingIdea BuildTradingIdea(
//            string pairName,
//            MultiTimeframeScore scoring,
//            MarketDataResponse fifteenMin,
//            MarketDataResponse oneHour,
//            MarketDataResponse fourHour,
//            TechnicalIndicators h1Indicators)
//        {
//            var bias = scoring.LongScore > scoring.ShortScore ? "Long" : "Short";
//            var strength = scoring.LongScore > scoring.ShortScore ? scoring.LongScore : scoring.ShortScore;
//            var conviction = strength >= 6 ? "High" : (strength >= 3 ? "Medium" : "Low");

//            var entrySignal = GetEntrySignal(fifteenMin.OHLCVData, bias);
//            var m15Latest = fifteenMin.OHLCVData.Last();
//            var atr = h1Indicators.ATR.LastOrDefault() ?? m15Latest.Close * 0.005m;
//            if (atr <= 0) atr = m15Latest.Close * 0.005m;

//            var currentPrice = m15Latest.Close;
//            var slResult = CalculateStopLoss(oneHour.OHLCVData, pairName, bias, currentPrice, atr);
//            var tpResult = CalculateTakeProfit(fourHour.OHLCVData, pairName, bias, currentPrice, atr, slResult.Stop);

//            var thesis = string.Join(" | ", scoring.Reasons);
//            if (entrySignal.Signal != 0)
//            {
//                thesis += $" | Entry: {string.Join(", ", entrySignal.Reasons.Take(2))}";
//            }

//            return new TradingIdea
//            {
//                Pair = pairName,
//                Bias = bias,
//                Conviction = conviction,
//                StrengthScore = strength,
//                Thesis = thesis,
//                Entry = currentPrice,
//                TakeProfit1 = tpResult.TP1,
//                TakeProfit2 = tpResult.TP2,
//                Tp1Method = tpResult.MethodTP1,
//                Tp2Method = tpResult.MethodTP2,
//                Tp1Valid = tpResult.TP1Valid,
//                Tp2Valid = tpResult.TP2Valid,
//                StopLoss = slResult.Stop,
//                StopLossMethod = slResult.Method,
//                StopLossPips = slResult.DistancePips,
//                RiskReward1 = tpResult.RR1,
//                RiskReward2 = tpResult.RR2,
//                ATR = atr,
//                EntrySignal = entrySignal
//            };
//        }

//        #endregion

//        #region Private - Indicator Calculations

//        private static List<decimal?> CalculateRSI(List<decimal> closes, int window)
//        {
//            var result = new List<decimal?>();
//            if (closes.Count <= window)
//                return Enumerable.Repeat<decimal?>(null, closes.Count).ToList();

//            // Add nulls for initial period
//            for (int i = 0; i < window; i++)
//                result.Add(null);

//            // Calculate initial average gain/loss
//            decimal avgGain = 0, avgLoss = 0;
//            for (int i = 1; i <= window; i++)
//            {
//                var change = closes[i] - closes[i - 1];
//                avgGain += change > 0 ? change : 0;
//                avgLoss += change < 0 ? -change : 0;
//            }
//            avgGain /= window;
//            avgLoss /= window;

//            result.Add(CalculateRSIValue(avgGain, avgLoss));

//            // Calculate subsequent values using smoothing
//            for (int i = window + 1; i < closes.Count; i++)
//            {
//                var change = closes[i] - closes[i - 1];
//                avgGain = ((avgGain * (window - 1)) + (change > 0 ? change : 0)) / window;
//                avgLoss = ((avgLoss * (window - 1)) + (change < 0 ? -change : 0)) / window;
//                result.Add(CalculateRSIValue(avgGain, avgLoss));
//            }

//            return result;
//        }

//        private static decimal CalculateRSIValue(decimal avgGain, decimal avgLoss)
//        {
//            if (avgLoss == 0) return 100;
//            var rs = avgGain / avgLoss;
//            return 100 - (100 / (1 + rs));
//        }

//        private static List<decimal?> CalculateSMA(List<decimal> values, int window)
//        {
//            var result = new List<decimal?>();
//            for (int i = 0; i < values.Count; i++)
//            {
//                if (i < window - 1)
//                    result.Add(null);
//                else
//                    result.Add(values.Skip(i - window + 1).Take(window).Average());
//            }
//            return result;
//        }

//        private static decimal[] CalculateSMAArray(decimal[] values, int window)
//        {
//            var result = new decimal[values.Length];
//            for (int i = window - 1; i < values.Length; i++)
//            {
//                result[i] = values.Skip(i - window + 1).Take(window).Average();
//            }
//            return result;
//        }

//        private static decimal[] CalculateRSIArray(decimal[] prices, int window)
//        {
//            var list = prices.ToList();
//            var rsiList = CalculateRSI(list, window);
//            return rsiList.Select(x => x ?? 50).ToArray();
//        }

//        private static List<decimal?> CalculateEMA(List<decimal> values, int window)
//        {
//            var result = new List<decimal?>();
//            if (values.Count == 0) return result;

//            var multiplier = 2m / (window + 1);

//            for (int i = 0; i < values.Count; i++)
//            {
//                if (i < window - 1)
//                {
//                    result.Add(null);
//                }
//                else if (i == window - 1)
//                {
//                    result.Add(values.Take(window).Average());
//                }
//                else
//                {
//                    var prevEma = result.Last()!.Value;
//                    result.Add((values[i] - prevEma) * multiplier + prevEma);
//                }
//            }
//            return result;
//        }

//        private static (List<decimal?> macd, List<decimal?> signal, List<decimal?> histogram)
//            CalculateMACD(List<decimal> closes, int fast, int slow, int signalPeriod)
//        {
//            var emaFast = CalculateEMA(closes, fast);
//            var emaSlow = CalculateEMA(closes, slow);

//            var macd = new List<decimal?>();
//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (emaFast[i].HasValue && emaSlow[i].HasValue)
//                    macd.Add(emaFast[i]!.Value - emaSlow[i]!.Value);
//                else
//                    macd.Add(null);
//            }

//            var validMacdValues = macd.Where(x => x.HasValue).Select(x => x!.Value).ToList();
//            var signal = CalculateEMA(validMacdValues, signalPeriod);

//            // Pad signal to match original length
//            while (signal.Count < closes.Count)
//                signal.Insert(0, null);

//            var histogram = new List<decimal?>();
//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (macd[i].HasValue && signal[i].HasValue)
//                    histogram.Add(macd[i]!.Value - signal[i]!.Value);
//                else
//                    histogram.Add(null);
//            }

//            return (macd, signal, histogram);
//        }

//        private static (List<decimal?> upper, List<decimal?> middle, List<decimal?> lower)
//            CalculateBollingerBands(List<decimal> closes, int window, decimal stdDev)
//        {
//            var middle = CalculateSMA(closes, window);
//            var upper = new List<decimal?>();
//            var lower = new List<decimal?>();

//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (i < window - 1)
//                {
//                    upper.Add(null);
//                    lower.Add(null);
//                }
//                else
//                {
//                    var slice = closes.Skip(i - window + 1).Take(window).ToList();
//                    var avg = slice.Average();
//                    var variance = slice.Sum(x => (double)(x - avg) * (double)(x - avg)) / window;
//                    var std = (decimal)Math.Sqrt(variance);

//                    upper.Add(avg + stdDev * std);
//                    lower.Add(avg - stdDev * std);
//                }
//            }

//            return (upper, middle, lower);
//        }

//        private static List<decimal?> CalculateATR(
//            List<decimal> highs, List<decimal> lows, List<decimal> closes, int window)
//        {
//            var tr = CalculateTrueRange(highs, lows, closes);
//            var atr = new List<decimal?>();

//            for (int i = 0; i < tr.Count; i++)
//            {
//                if (i < window - 1)
//                {
//                    atr.Add(null);
//                }
//                else if (i == window - 1)
//                {
//                    atr.Add(tr.Take(window).Average());
//                }
//                else
//                {
//                    var prevAtr = atr.Last()!.Value;
//                    atr.Add((prevAtr * (window - 1) + tr[i]) / window);
//                }
//            }

//            return atr;
//        }

//        private static List<decimal> CalculateTrueRange(
//            List<decimal> highs, List<decimal> lows, List<decimal> closes)
//        {
//            var tr = new List<decimal>();
//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (i == 0)
//                {
//                    tr.Add(highs[i] - lows[i]);
//                }
//                else
//                {
//                    var highLow = highs[i] - lows[i];
//                    var highClose = Math.Abs(highs[i] - closes[i - 1]);
//                    var lowClose = Math.Abs(lows[i] - closes[i - 1]);
//                    tr.Add(Math.Max(highLow, Math.Max(highClose, lowClose)));
//                }
//            }
//            return tr;
//        }

//        private static (List<decimal?> k, List<decimal?> d) CalculateStochastic(
//            List<decimal> highs, List<decimal> lows, List<decimal> closes,
//            int window, int smooth)
//        {
//            var k = new List<decimal?>();

//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (i < window - 1)
//                {
//                    k.Add(null);
//                }
//                else
//                {
//                    var highest = highs.Skip(i - window + 1).Take(window).Max();
//                    var lowest = lows.Skip(i - window + 1).Take(window).Min();
//                    var denominator = highest - lowest;
//                    k.Add(denominator == 0 ? 50 : 100 * (closes[i] - lowest) / denominator);
//                }
//            }

//            var d = new List<decimal?>();
//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (i < window + smooth - 2)
//                {
//                    d.Add(null);
//                }
//                else
//                {
//                    d.Add(k.Skip(i - smooth + 1).Take(smooth).Average());
//                }
//            }

//            return (k, d);
//        }

//        private static (List<decimal?> adx, List<decimal?> plusDi, List<decimal?> minusDi)
//            CalculateADX(List<decimal> highs, List<decimal> lows, List<decimal> closes, int window)
//        {
//            var (plusDm, minusDm, tr) = CalculateDirectionalMovement(highs, lows, closes);

//            var smoothedTr = CalculateEMAList(tr, window);
//            var smoothedPlusDm = CalculateEMAList(plusDm, window);
//            var smoothedMinusDm = CalculateEMAList(minusDm, window);

//            var (plusDi, minusDi, dx) = CalculateDILines(
//                closes.Count, smoothedTr, smoothedPlusDm, smoothedMinusDm);

//            var validDxValues = dx.Where(x => x.HasValue).Select(x => x!.Value).ToList();
//            var adx = CalculateEMAList(validDxValues, window);

//            // Pad ADX to match original length
//            while (adx.Count < closes.Count)
//                adx.Insert(0, null);

//            return (adx, plusDi, minusDi);
//        }

//        private static (List<decimal> plusDm, List<decimal> minusDm, List<decimal> tr)
//            CalculateDirectionalMovement(List<decimal> highs, List<decimal> lows, List<decimal> closes)
//        {
//            var plusDm = new List<decimal>();
//            var minusDm = new List<decimal>();
//            var tr = new List<decimal>();

//            for (int i = 0; i < closes.Count; i++)
//            {
//                if (i == 0)
//                {
//                    plusDm.Add(0);
//                    minusDm.Add(0);
//                    tr.Add(highs[i] - lows[i]);
//                }
//                else
//                {
//                    var upMove = highs[i] - highs[i - 1];
//                    var downMove = lows[i - 1] - lows[i];
//                    plusDm.Add(upMove > downMove && upMove > 0 ? upMove : 0);
//                    minusDm.Add(downMove > upMove && downMove > 0 ? downMove : 0);

//                    var hl = highs[i] - lows[i];
//                    var hc = Math.Abs(highs[i] - closes[i - 1]);
//                    var lc = Math.Abs(lows[i] - closes[i - 1]);
//                    tr.Add(Math.Max(hl, Math.Max(hc, lc)));
//                }
//            }

//            return (plusDm, minusDm, tr);
//        }

//        private static (List<decimal?> plusDi, List<decimal?> minusDi, List<decimal?> dx)
//            CalculateDILines(
//                int count,
//                List<decimal?> smoothedTr,
//                List<decimal?> smoothedPlusDm,
//                List<decimal?> smoothedMinusDm)
//        {
//            var plusDi = new List<decimal?>();
//            var minusDi = new List<decimal?>();
//            var dx = new List<decimal?>();

//            for (int i = 0; i < count; i++)
//            {
//                if (!smoothedTr[i].HasValue || smoothedTr[i] == 0)
//                {
//                    plusDi.Add(null);
//                    minusDi.Add(null);
//                    dx.Add(null);
//                }
//                else
//                {
//                    var pDi = (smoothedPlusDm[i] ?? 0) / smoothedTr[i]!.Value * 100;
//                    var mDi = (smoothedMinusDm[i] ?? 0) / smoothedTr[i]!.Value * 100;
//                    plusDi.Add(pDi);
//                    minusDi.Add(mDi);

//                    var sum = pDi + mDi;
//                    dx.Add(sum == 0 ? 0 : Math.Abs(pDi - mDi) / sum * 100);
//                }
//            }

//            return (plusDi, minusDi, dx);
//        }

//        private static List<decimal?> CalculateEMAList(List<decimal> values, int window)
//        {
//            return CalculateEMA(values, window);
//        }

//        private static List<decimal?> CalculateRollingMin(List<decimal> values, int window)
//        {
//            var result = new List<decimal?>();
//            for (int i = 0; i < values.Count; i++)
//            {
//                if (i < window - 1)
//                    result.Add(null);
//                else
//                    result.Add(values.Skip(i - window + 1).Take(window).Min());
//            }
//            return result;
//        }

//        private static List<decimal?> CalculateRollingMax(List<decimal> values, int window)
//        {
//            var result = new List<decimal?>();
//            for (int i = 0; i < values.Count; i++)
//            {
//                if (i < window - 1)
//                    result.Add(null);
//                else
//                    result.Add(values.Skip(i - window + 1).Take(window).Max());
//            }
//            return result;
//        }

//        #endregion

//        #region Private - Utility Methods

//        private decimal CalculateConfidence(int winningScore, int losingScore)
//        {
//            var total = winningScore + losingScore;
//            return total > 0 ? (decimal)winningScore / total : 0;
//        }

//        //Task<List<Entities.BiasAnalysisResult>> ITechnicalAnalysisService.GenerateBiasDashboardAsync(Dictionary<string, Dictionary<string, MarketDataResponse>> allData, CancellationToken cancellationToken)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public Task<List<TradingIdea>> GenerateTradingIdeasAsync(Dictionary<string, Dictionary<string, Entities.MarketDataResponse>> allData, CancellationToken cancellationToken = default)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public Task<List<SwingTradingIdea>> GenerateSwingIdeasAsync(Dictionary<string, Dictionary<string, Entities.MarketDataResponse>> allData, CancellationToken cancellationToken = default)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public Task<List<Entities.BiasAnalysisResult>> GenerateBiasDashboardAsync(Dictionary<string, Dictionary<string, Entities.MarketDataResponse>> allData, CancellationToken cancellationToken = default)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public Task<TradingIdea?> AnalyzeMultiTimeframeAsync(Entities.MarketDataResponse daily, Entities.MarketDataResponse fourHour, Entities.MarketDataResponse oneHour, Entities.MarketDataResponse fifteenMin, string pairName, CancellationToken cancellationToken = default)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        #endregion

//        #region Private - Score Classes

//        private class TrendBiasScore
//        {
//            public decimal ADX { get; set; }
//            public bool IsTrending { get; set; }
//            public int Strength { get; set; }
//            public string Bias { get; set; } = "Neutral";
//            public List<string> Reasons { get; } = new();
//        }

//        private sealed class AdxResult
//        {
//            public decimal Adx { get; set; }
//            public decimal PlusDi { get; set; }
//            public decimal MinusDi { get; set; }
//        }

//        #endregion
//    }
//}
