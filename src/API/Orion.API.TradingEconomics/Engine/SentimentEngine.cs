using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Performs keyword-based weighted sentiment analysis for forex pairs.
    /// </summary>
    public sealed class SentimentEngine : ISentimentEngine
    {
        private static readonly string[] BullishWords =
        [
            "hawkish", "strong", "growth", "beat", "surge", "rally",
            "higher", "inflation rising", "rate hike", "risk-on",
            "resilient", "expansion", "upside"
        ];

        private static readonly string[] BearishWords =
        [
            "dovish", "weak", "miss", "drop", "fall", "recession",
            "lower", "rate cut", "risk-off", "slowdown",
            "contraction", "downside", "crisis"
        ];

        /// <inheritdoc />
        public Task<SentimentResult> AnalyzeAsync(
            SentimentRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Pair))
                throw new ArgumentException("Pair is required.", nameof(request));

            if (request.Items == null || request.Items.Count == 0)
            {
                return Task.FromResult(new SentimentResult
                {
                    Pair = request.Pair.Trim().ToUpperInvariant(),
                    Score = 0m,
                    Bias = "NEUTRAL",
                    Confidence = 0m,
                    Reasons = ["No sentiment items supplied."],
                    TimestampUtc = DateTime.UtcNow
                });
            }

            var weightedScore = 0m;
            var totalWeight = 0m;
            var reasons = new List<string>();

            foreach (var item in request.Items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var text = $"{item.Title} {item.Text}".ToLowerInvariant();

                var bullishHits = BullishWords.Count(text.Contains);
                var bearishHits = BearishWords.Count(text.Contains);

                var normalizedScore = NormalizeScore(bullishHits - bearishHits);
                var weight = item.Weight <= 0m ? 1m : item.Weight;

                weightedScore += normalizedScore * weight;
                totalWeight += weight;

                if (normalizedScore > 0m)
                    reasons.Add($"Bullish sentiment from {item.Source}: {item.Title}");

                if (normalizedScore < 0m)
                    reasons.Add($"Bearish sentiment from {item.Source}: {item.Title}");
            }

            var finalScore = totalWeight > 0m
                ? Clamp(weightedScore / totalWeight, -1m, 1m)
                : 0m;

            return Task.FromResult(new SentimentResult
            {
                Pair = request.Pair.Trim().ToUpperInvariant(),
                Score = Math.Round(finalScore, 4),
                Bias = ResolveBias(finalScore),
                Confidence = Math.Round(Math.Abs(finalScore) * 100m, 2),
                Reasons = reasons.Count == 0
                    ? ["No strong directional sentiment detected."]
                    : reasons.Take(10).ToList(),
                TimestampUtc = DateTime.UtcNow
            });
        }

        private static decimal NormalizeScore(int rawScore)
        {
            return rawScore switch
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