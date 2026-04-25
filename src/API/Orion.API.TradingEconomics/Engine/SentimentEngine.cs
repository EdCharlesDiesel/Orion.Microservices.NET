using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class SentimentEngine : ISentimentEngine
    {
        private static readonly string[] BullishWords =
        {
            "hawkish", "strong", "growth", "beat", "surge", "rally",
            "higher", "inflation rising", "rate hike", "risk-on",
            "resilient", "expansion", "upside"
        };

        private static readonly string[] BearishWords =
        {
            "dovish", "weak", "miss", "drop", "fall", "recession",
            "lower", "rate cut", "risk-off", "slowdown",
            "contraction", "downside", "crisis"
        };

        public Task<SentimentResult> AnalyzeAsync(SentimentRequest request,CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Pair))
                throw new ArgumentException("Pair is required.", nameof(request));

            if (request.Items == null || request.Items.Count == 0)
            {
                return Task.FromResult(new SentimentResult
                {
                    Pair = request.Pair,
                    Score = 0m,
                    Bias = "NEUTRAL",
                    Confidence = 0m,
                    Reasons = new List<string> { "No sentiment items supplied." },
                    TimestampUtc = DateTime.UtcNow
                });
            }

            decimal weightedScore = 0m;
            decimal totalWeight = 0m;
            var reasons = new List<string>();

            foreach (var item in request.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var text = $"{item.Title} {item.Text}".ToLowerInvariant();

                var bullishHits = BullishWords.Count(word => text.Contains(word));
                var bearishHits = BearishWords.Count(word => text.Contains(word));

                var rawScore = bullishHits - bearishHits;

                var normalizedScore = rawScore switch
                {
                    > 3 => 1m,
                    3 => 0.75m,
                    2 => 0.50m,
                    1 => 0.25m,
                    0 => 0m,
                    -1 => -0.25m,
                    -2 => -0.50m,
                    -3 => -0.75m,
                    < -3 => -1m
                };

                var weight = item.Weight <= 0 ? 1m : item.Weight;

                weightedScore += normalizedScore * weight;
                totalWeight += weight;

                if (normalizedScore > 0)
                    reasons.Add($"Bullish sentiment from {item.Source}: {item.Title}");

                if (normalizedScore < 0)
                    reasons.Add($"Bearish sentiment from {item.Source}: {item.Title}");
            }

            var finalScore = totalWeight > 0
                ? weightedScore / totalWeight
                : 0m;

            finalScore = Clamp(finalScore, -1m, 1m);

            var confidence = Math.Abs(finalScore) * 100m;

            return Task.FromResult(new SentimentResult
            {
                Pair = request.Pair,
                Score = finalScore,
                Bias = ResolveBias(finalScore),
                Confidence = confidence,
                Reasons = reasons.Count == 0
                    ? new List<string> { "No strong directional sentiment detected." }
                    : reasons.Take(10).ToList(),
                TimestampUtc = DateTime.UtcNow
            });
        }

        private static string ResolveBias(decimal score)
        {
            return score switch
            {
                >= 0.60m => "STRONGLY_BULLISH",
                >= 0.20m => "BULLISH",
                <= -0.60m => "STRONGLY_BEARISH",
                <= -0.20m => "BEARISH",
                _ => "NEUTRAL"
            };
        }

        private static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}