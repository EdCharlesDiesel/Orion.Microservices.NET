using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class TechnicalIndicatorService
    {
        public List<EnrichedMarketData> AddIndicators(List<MarketData> data)
        {
            if (data == null || data.Count < 20)
                return data?.Select(d => new EnrichedMarketData
                {
                    Timestamp = d.Timestamp,
                    Open = d.Open,
                    High = d.High,
                    Low = d.Low,
                    Close = d.Close,
                    Volume = d.Volume,
                    Indicators = new TechnicalIndicators()
                }).ToList() ?? new List<EnrichedMarketData>();

            var enriched = data.Select(d => new EnrichedMarketData
            {
                Timestamp = d.Timestamp,
                Open = d.Open,
                High = d.High,
                Low = d.Low,
                Close = d.Close,
                Volume = d.Volume,
                Indicators = new TechnicalIndicators()
            }).ToList();

            var closes = enriched.Select(e => (double)e.Close).ToList();
            var highs = enriched.Select(e => (double)e.High).ToList();
            var lows = enriched.Select(e => (double)e.Low).ToList();

            // RSI
            var rsi = CalculateRSI(closes, 14);
            for (int i = 0; i < enriched.Count; i++)
                enriched[i].Indicators.RSI = rsi[i];

            // Moving Averages
            var sma20 = CalculateSMA(closes, 20);
            var sma50 = CalculateSMA(closes, 50);
            var ema9 = CalculateEMA(closes, 9);
            var ema20 = CalculateEMA(closes, 20);
            var ema50 = CalculateEMA(closes, 50);
            var ema200 = CalculateEMA(closes, 200);

            for (int i = 0; i < enriched.Count; i++)
            {
                enriched[i].Indicators.SMA20 = sma20[i].HasValue ? (decimal)sma20[i].Value : null;
                enriched[i].Indicators.SMA50 = sma50[i].HasValue ? (decimal)sma50[i].Value : null;
                enriched[i].Indicators.EMA9 = ema9[i].HasValue ? (decimal)ema9[i].Value : null;
                enriched[i].Indicators.EMA20 = ema20[i].HasValue ? (decimal)ema20[i].Value : null;
                enriched[i].Indicators.EMA50 = ema50[i].HasValue ? (decimal)ema50[i].Value : null;
                enriched[i].Indicators.EMA200 = ema200[i].HasValue ? (decimal)ema200[i].Value : null;
            }

            // Bollinger Bands
            var bb = CalculateBollingerBands(closes, 20, 2);
            for (int i = 0; i < enriched.Count; i++)
            {
                enriched[i].Indicators.BBUpper = bb.Upper[i].HasValue ? (decimal)bb.Upper[i].Value : null;
                enriched[i].Indicators.BBMiddle = bb.Middle[i].HasValue ? (decimal)bb.Middle[i].Value : null;
                enriched[i].Indicators.BBLower = bb.Lower[i].HasValue ? (decimal)bb.Lower[i].Value : null;
                if (enriched[i].Indicators.BBMiddle.HasValue && enriched[i].Close != 0)
                    enriched[i].Indicators.BBWidth = (enriched[i].Indicators.BBUpper - enriched[i].Indicators.BBLower) / enriched[i].Close;
            }

            // ATR
            var atr = CalculateATR(highs, lows, closes, 14);
            for (int i = 0; i < enriched.Count; i++)
                enriched[i].Indicators.ATR = atr[i].HasValue ? (decimal)atr[i].Value : null;

            // Stochastic
            var stoch = CalculateStochastic(highs, lows, closes, 14, 3);
            for (int i = 0; i < enriched.Count; i++)
            {
                enriched[i].Indicators.StochK = stoch.K[i];
                enriched[i].Indicators.StochD = stoch.D[i];
            }

            // ADX
            var adx = CalculateADX(highs, lows, closes, 14);
            for (int i = 0; i < enriched.Count; i++)
            {
                enriched[i].Indicators.ADX = adx.ADX[i];
                enriched[i].Indicators.ADXPos = adx.PosDI[i];
                enriched[i].Indicators.ADXNeg = adx.NegDI[i];
            }

            // Support/Resistance
            for (int i = 0; i < enriched.Count; i++)
            {
                var window = Math.Max(0, i - 20);
                enriched[i].Indicators.Resistance20 = enriched.Skip(window).Take(20).Max(x => x.High);
                enriched[i].Indicators.Support20 = enriched.Skip(window).Take(20).Min(x => x.Low);
                enriched[i].Indicators.PivotPoint = (enriched[i].High + enriched[i].Low + enriched[i].Close) / 3;
            }

            return enriched;
        }

        private List<double?> CalculateRSI(List<double> prices, int period)
        {
            var rsi = new List<double?>();
            for (int i = 0; i < prices.Count; i++)
                rsi.Add(null);

            if (prices.Count < period + 1)
                return rsi;

            double avgGain = 0, avgLoss = 0;

            for (int i = 1; i <= period; i++)
            {
                var change = prices[i] - prices[i - 1];
                if (change >= 0) avgGain += change;
                else avgLoss -= change;
            }

            avgGain /= period;
            avgLoss /= period;

            rsi[period] = 100 - (100 / (1 + avgGain / avgLoss));

            for (int i = period + 1; i < prices.Count; i++)
            {
                var change = prices[i] - prices[i - 1];
                if (change >= 0)
                {
                    avgGain = (avgGain * (period - 1) + change) / period;
                    avgLoss = (avgLoss * (period - 1)) / period;
                }
                else
                {
                    avgGain = (avgGain * (period - 1)) / period;
                    avgLoss = (avgLoss * (period - 1) - change) / period;
                }
                rsi[i] = 100 - (100 / (1 + avgGain / avgLoss));
            }

            return rsi;
        }

        private List<double?> CalculateSMA(List<double> prices, int period)
        {
            var sma = new List<double?>();
            for (int i = 0; i < prices.Count; i++)
            {
                if (i < period - 1)
                    sma.Add(null);
                else
                    sma.Add(prices.Skip(i - period + 1).Take(period).Average());
            }
            return sma;
        }

        private List<double?> CalculateEMA(List<double> prices, int period)
        {
            var ema = new List<double?>();
            for (int i = 0; i < prices.Count; i++)
                ema.Add(null);

            if (prices.Count < period)
                return ema;

            double multiplier = 2.0 / (period + 1);
            ema[period - 1] = prices.Take(period).Average();

            for (int i = period; i < prices.Count; i++)
                ema[i] = (prices[i] - ema[i - 1]) * multiplier + ema[i - 1];

            return ema;
        }

        private (List<double?> Upper, List<double?> Middle, List<double?> Lower) CalculateBollingerBands(List<double> prices, int period, double stdDev)
        {
            var upper = new List<double?>();
            var middle = new List<double?>();
            var lower = new List<double?>();

            for (int i = 0; i < prices.Count; i++)
            {
                if (i < period - 1)
                {
                    upper.Add(null);
                    middle.Add(null);
                    lower.Add(null);
                    continue;
                }

                var slice = prices.Skip(i - period + 1).Take(period);
                var avg = slice.Average();
                var std = Math.Sqrt(slice.Select(x => Math.Pow(x - avg, 2)).Average());

                middle.Add(avg);
                upper.Add(avg + stdDev * std);
                lower.Add(avg - stdDev * std);
            }

            return (upper, middle, lower);
        }

        private List<double?> CalculateATR(List<double> highs, List<double> lows, List<double> closes, int period)
        {
            var tr = new List<double>();
            var atr = new List<double?>();

            for (int i = 0; i < highs.Count; i++)
            {
                if (i == 0)
                    tr.Add(highs[i] - lows[i]);
                else
                {
                    var hl = highs[i] - lows[i];
                    var hc = Math.Abs(highs[i] - closes[i - 1]);
                    var lc = Math.Abs(lows[i] - closes[i - 1]);
                    tr.Add(new[] { hl, hc, lc }.Max());
                }
            }

            for (int i = 0; i < tr.Count; i++)
            {
                if (i < period - 1)
                    atr.Add(null);
                else if (i == period - 1)
                    atr.Add(tr.Take(period).Average());
                else
                    atr.Add(((atr[i - 1] ?? 0) * (period - 1) + tr[i]) / period);
            }

            return atr;
        }

        private (List<double?> K, List<double?> D) CalculateStochastic(List<double> highs, List<double> lows, List<double> closes, int kPeriod, int dPeriod)
        {
            var k = new List<double?>();
            var d = new List<double?>();

            for (int i = 0; i < closes.Count; i++)
            {
                if (i < kPeriod - 1)
                {
                    k.Add(null);
                    d.Add(null);
                    continue;
                }

                var highestHigh = highs.Skip(i - kPeriod + 1).Take(kPeriod).Max();
                var lowestLow = lows.Skip(i - kPeriod + 1).Take(kPeriod).Min();
                var kValue = 100 * (closes[i] - lowestLow) / (highestHigh - lowestLow);
                k.Add(kValue);

                if (i >= kPeriod - 1 + dPeriod - 1)
                    d.Add(k.Skip(i - dPeriod + 1).Take(dPeriod).Where(x => x.HasValue).Average());
                else
                    d.Add(null);
            }

            return (k, d);
        }

        private (List<double?> ADX, List<double?> PosDI, List<double?> NegDI) CalculateADX(List<double> highs, List<double> lows, List<double> closes, int period)
        {
            var adx = new List<double?>();
            var posDI = new List<double?>();
            var negDI = new List<double?>();
            var tr = new List<double>();
            var plusDM = new List<double>();
            var minusDM = new List<double>();

            for (int i = 0; i < highs.Count; i++)
            {
                if (i == 0)
                {
                    tr.Add(highs[i] - lows[i]);
                    plusDM.Add(0);
                    minusDM.Add(0);
                }
                else
                {
                    var hl = highs[i] - lows[i];
                    var hc = Math.Abs(highs[i] - closes[i - 1]);
                    var lc = Math.Abs(lows[i] - closes[i - 1]);
                    tr.Add(new[] { hl, hc, lc }.Max());

                    var upMove = highs[i] - highs[i - 1];
                    var downMove = lows[i - 1] - lows[i];
                    plusDM.Add(upMove > downMove && upMove > 0 ? upMove : 0);
                    minusDM.Add(downMove > upMove && downMove > 0 ? downMove : 0);
                }
            }

            var smoothedTR = new List<double>();
            var smoothedPlusDM = new List<double>();
            var smoothedMinusDM = new List<double>();

            for (int i = 0; i < tr.Count; i++)
            {
                if (i < period - 1)
                {
                    smoothedTR.Add(0);
                    smoothedPlusDM.Add(0);
                    smoothedMinusDM.Add(0);
                    posDI.Add(null);
                    negDI.Add(null);
                    adx.Add(null);
                }
                else if (i == period - 1)
                {
                    smoothedTR.Add(tr.Take(period).Average());
                    smoothedPlusDM.Add(plusDM.Take(period).Average());
                    smoothedMinusDM.Add(minusDM.Take(period).Average());

                    var plusDI = 100 * smoothedPlusDM[i] / smoothedTR[i];
                    var minusDI = 100 * smoothedMinusDM[i] / smoothedTR[i];
                    posDI.Add(plusDI);
                    negDI.Add(minusDI);

                    var dx = 100 * Math.Abs(plusDI - minusDI) / (plusDI + minusDI);
                    adx.Add(dx);
                }
                else
                {
                    smoothedTR.Add(smoothedTR[i - 1] * (period - 1) / period + tr[i] / period);
                    smoothedPlusDM.Add(smoothedPlusDM[i - 1] * (period - 1) / period + plusDM[i] / period);
                    smoothedMinusDM.Add(smoothedMinusDM[i - 1] * (period - 1) / period + minusDM[i] / period);

                    var plusDI = 100 * smoothedPlusDM[i] / smoothedTR[i];
                    var minusDI = 100 * smoothedMinusDM[i] / smoothedTR[i];
                    posDI.Add(plusDI);
                    negDI.Add(minusDI);

                    var dx = 100 * Math.Abs(plusDI - minusDI) / (plusDI + minusDI);
                    adx.Add((adx[i - 1] ?? 0) * (period - 1) / period + dx / period);
                }
            }

            return (adx, posDI, negDI);
        }
    }
}
