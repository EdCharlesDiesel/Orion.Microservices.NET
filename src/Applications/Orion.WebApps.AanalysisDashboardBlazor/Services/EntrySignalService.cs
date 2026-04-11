using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class EntrySignalService
    {
        private const int StochOs = 25;
        private const int StochOb = 75;
        private const int LondonStart = 9;
        private const int LondonEnd = 12;
        private const int NyStart = 13;
        private const int NyEnd = 16;

        public EntrySignal Get15mEntrySignal(List<EnrichedMarketData> data, string bias)
        {
            if (data == null || data.Count < 5)
                return new EntrySignal { Signal = 0, Confidence = 0, Reasons = { "Insufficient data" } };

            var last = data[^1];
            var prev = data.Count > 1 ? data[^2] : last;

            var k = last.Indicators.StochK ?? 50;
            var d = last.Indicators.StochD ?? 50;
            var prevK = prev.Indicators.StochK ?? 50;
            var prevD = prev.Indicators.StochD ?? 50;
            var rsi = last.Indicators.RSI ?? 50;
            var price = last.Close;
            var bbLower = last.Indicators.BBLower ?? price * 0.99m;
            var bbUpper = last.Indicators.BBUpper ?? price * 1.01m;

            var signal = 0;
            var confidence = 0;
            var reasons = new List<string>();

            if (bias == "Long")
            {
                if (prevK <= prevD && k > d && k < StochOs)
                {
                    signal = 1;
                    confidence += 2;
                    reasons.Add($"Stochastic bullish crossover (K={k:F1})");
                }

                if (rsi < 35)
                {
                    confidence += 1;
                    reasons.Add($"RSI oversold ({rsi:F1})");
                }

                if (price <= bbLower * 1.002m)
                {
                    confidence += 1;
                    reasons.Add("Price at lower Bollinger Band");
                }

                var currentHour = DateTime.Now.Hour;
                if (currentHour >= LondonStart && currentHour <= LondonEnd ||
                    currentHour >= NyStart && currentHour <= NyEnd)
                {
                    confidence += 1;
                    reasons.Add("Active trading session");
                }
            }
            else if (bias == "Short")
            {
                if (prevK >= prevD && k < d && k > StochOb)
                {
                    signal = -1;
                    confidence += 2;
                    reasons.Add($"Stochastic bearish crossover (K={k:F1})");
                }

                if (rsi > 65)
                {
                    confidence += 1;
                    reasons.Add($"RSI overbought ({rsi:F1})");
                }

                if (price >= bbUpper * 0.998m)
                {
                    confidence += 1;
                    reasons.Add("Price at upper Bollinger Band");
                }

                var currentHour = DateTime.Now.Hour;
                if (currentHour >= LondonStart && currentHour <= LondonEnd ||
                    currentHour >= NyStart && currentHour <= NyEnd)
                {
                    confidence += 1;
                    reasons.Add("Active trading session");
                }
            }

            return new EntrySignal
            {
                Signal = signal,
                Confidence = Math.Min(confidence, 5),
                Reasons = reasons,
                StochK = k,
                StochD = d,
                RSI = rsi,
                Price = price
            };
        }
    }
}
