using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class IndicatorService
    {
        public void AddIndicators(List<PriceData> data)
        {
            if (data.Count < 3) return;

            var closes = data.Select(d => (double)d.Close).ToArray();

            // Calculate EMAs
            data[0].Ema9 = data[0].Close;
            data[0].Ema21 = data[0].Close;
            data[0].Ema50 = data[0].Close;

            for (int i = 1; i < data.Count; i++)
            {
                data[i].Ema9 = CalculateEma(data[i].Close, data[i - 1].Ema9 ?? data[i].Close, 9);
                data[i].Ema21 = CalculateEma(data[i].Close, data[i - 1].Ema21 ?? data[i].Close, 21);
                data[i].Ema50 = CalculateEma(data[i].Close, data[i - 1].Ema50 ?? data[i].Close, 50);
            }

            // Calculate RSI
            CalculateRsi(data, 14);

            // Calculate MACD
            CalculateMacd(data);

            // Calculate Bollinger Bands
            CalculateBollingerBands(data, 20);
        }

        private decimal CalculateEma(decimal price, decimal previousEma, int period)
        {
            double multiplier = 2.0 / (period + 1);
            return price * (decimal)multiplier + previousEma * (1 - (decimal)multiplier);
        }

        private void CalculateRsi(List<PriceData> data, int period)
        {
            var gains = new List<double>();
            var losses = new List<double>();

            for (int i = 1; i < data.Count; i++)
            {
                var change = (double)(data[i].Close - data[i - 1].Close);
                gains.Add(change > 0 ? change : 0);
                losses.Add(change < 0 ? -change : 0);
            }

            for (int i = period; i < data.Count; i++)
            {
                var avgGain = gains.Skip(i - period).Take(period).Average();
                var avgLoss = losses.Skip(i - period).Take(period).Average();

                if (avgLoss == 0)
                    data[i].Rsi = 100;
                else
                {
                    var rs = avgGain / avgLoss;
                    data[i].Rsi = 100 - (100 / (1 + rs));
                }
            }
        }

        private void CalculateMacd(List<PriceData> data)
        {
            var closes = data.Select(d => (double)d.Close).ToArray();
            var ema12 = new double[data.Count];
            var ema26 = new double[data.Count];

            ema12[0] = closes[0];
            ema26[0] = closes[0];

            for (int i = 1; i < data.Count; i++)
            {
                ema12[i] = closes[i] * (2.0 / 13) + ema12[i - 1] * (1 - 2.0 / 13);
                ema26[i] = closes[i] * (2.0 / 27) + ema26[i - 1] * (1 - 2.0 / 27);
                data[i].Macd = (decimal)(ema12[i] - ema26[i]);
            }

            // Signal line (EMA9 of MACD)
            data[0].Signal = data[0].Macd;
            for (int i = 1; i < data.Count; i++)
            {
                data[i].Signal = data[i].Macd * (decimal)(2.0 / 10) + data[i - 1].Signal * (1 - (decimal)(2.0 / 10));
            }
        }

        private void CalculateBollingerBands(List<PriceData> data, int period)
        {
            for (int i = period - 1; i < data.Count; i++)
            {
                var window = data.Skip(i - period + 1).Take(period).Select(d => (double)d.Close).ToList();
                var mean = window.Average();
                var stdDev = Math.Sqrt(window.Select(x => Math.Pow(x - mean, 2)).Average());

                data[i].BbMid = (decimal)mean;
                data[i].BbUpper = (decimal)(mean + 2 * stdDev);
                data[i].BbLower = (decimal)(mean - 2 * stdDev);
            }
        }

        public (string Bias, double Confidence) ComputeDailyBias(List<PriceData> dayData)
        {
            if (dayData.Count < 5) return ("Neutral", 50.0);

            var open = dayData.First().Open;
            var close = dayData.Last().Close;
            var high = dayData.Max(d => d.High);
            var low = dayData.Min(d => d.Low);
            var mid = (high + low) / 2;
            var pctChange = (double)((close - open) / open) * 100;

            double bullScore = 0;

            if (close > open) bullScore += 2;
            if (close > mid) bullScore += 1;

            // Check 4-hour structure
            var hourlyData = ResampleToHourly(dayData);
            if (hourlyData.Count >= 2)
            {
                if (hourlyData.Last().Close > hourlyData.First().Close) bullScore += 2;
                if (hourlyData.Count >= 2 && hourlyData.Last().High > hourlyData[^2].High) bullScore += 1;
                if (hourlyData.Count >= 2 && hourlyData.Last().Low > hourlyData[^2].Low) bullScore += 1;
            }

            if (pctChange > 0.1) bullScore += 1;

            var bullPct = (bullScore / 8) * 100;

            if (bullPct >= 62) return ("Bullish", bullPct);
            if (bullPct <= 38) return ("Bearish", 100 - bullPct);
            return ("Neutral", 50.0);
        }

        private List<PriceData> ResampleToHourly(List<PriceData> minuteData)
        {
            return minuteData
                .GroupBy(d => new DateTime(d.DateTime.Year, d.DateTime.Month, d.DateTime.Day, d.DateTime.Hour, 0, 0))
                .Select(g => new PriceData
                {
                    DateTime = g.Key,
                    Open = g.First().Open,
                    High = g.Max(x => x.High),
                    Low = g.Min(x => x.Low),
                    Close = g.Last().Close
                })
                .ToList();
        }
    }
}