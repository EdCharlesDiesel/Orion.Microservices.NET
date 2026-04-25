using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class CorrelationEngine : ICorrelationEngine
    {
        public Task<CorrelationResult> AnalyzeAsync(CorrelationRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.PrimaryPair))
                throw new ArgumentException("Primary pair is required.", nameof(request));

            if (request.Series == null || request.Series.Count < 2)
                throw new ArgumentException("At least two series are required for correlation analysis.", nameof(request));

            var primary = request.Series.FirstOrDefault(x =>
                string.Equals(x.Symbol, request.PrimaryPair, StringComparison.OrdinalIgnoreCase));

            if (primary == null)
                throw new ArgumentException("Primary pair series was not supplied.", nameof(request));

            if (primary.Values == null || primary.Values.Count < 2)
                throw new ArgumentException("Primary pair requires at least two values.", nameof(request));

            var correlations = new List<CorrelationItem>();

            foreach (var series in request.Series)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.Equals(series.Symbol, request.PrimaryPair, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (series.Values == null || series.Values.Count < 2)
                    continue;

                var primaryValues = TakeAlignedLookback(primary.Values, series.Values, request.LookbackPeriods);
                var comparisonValues = TakeAlignedLookback(series.Values, primary.Values, request.LookbackPeriods);

                if (primaryValues.Count < 2 || comparisonValues.Count < 2)
                    continue;

                var correlation = CalculatePearsonCorrelation(primaryValues, comparisonValues);

                correlations.Add(new CorrelationItem
                {
                    Symbol = series.Symbol,
                    Correlation = correlation,
                    Strength = ResolveStrength(correlation),
                    Direction = ResolveDirection(correlation)
                });
            }

            var averageAbsCorrelation = correlations.Count == 0
                ? 0m
                : correlations.Average(x => Math.Abs(x.Correlation));

            return Task.FromResult(new CorrelationResult
            {
                PrimaryPair = request.PrimaryPair,
                Correlations = correlations
                    .OrderByDescending(x => Math.Abs(x.Correlation))
                    .ToList(),
                AverageAbsCorrelation = Math.Round(averageAbsCorrelation, 4),
                RiskSummary = ResolveRiskSummary(averageAbsCorrelation),
                TimestampUtc = DateTime.UtcNow
            });
        }

        private static List<decimal> TakeAlignedLookback(List<decimal> first, List<decimal> second, int lookbackPeriods)
        {
            var count = Math.Min(first.Count, second.Count);

            if (lookbackPeriods > 0)
                count = Math.Min(count, lookbackPeriods);

            return first
                .Skip(first.Count - count)
                .Take(count)
                .ToList();
        }

        private static decimal CalculatePearsonCorrelation(List<decimal> xValues, List<decimal> yValues)
        {
            var count = Math.Min(xValues.Count, yValues.Count);

            if (count < 2)
                return 0m;

            var x = xValues.TakeLast(count).Select(v => (double)v).ToArray();
            var y = yValues.TakeLast(count).Select(v => (double)v).ToArray();

            var avgX = x.Average();
            var avgY = y.Average();

            double numerator = 0;
            double sumXSquared = 0;
            double sumYSquared = 0;

            for (var i = 0; i < count; i++)
            {
                var xDiff = x[i] - avgX;
                var yDiff = y[i] - avgY;

                numerator += xDiff * yDiff;
                sumXSquared += xDiff * xDiff;
                sumYSquared += yDiff * yDiff;
            }

            var denominator = Math.Sqrt(sumXSquared * sumYSquared);

            if (denominator == 0)
                return 0m;

            var correlation = numerator / denominator;

            return Math.Round((decimal)correlation, 4);
        }

        private static string ResolveStrength(decimal correlation)
        {
            var abs = Math.Abs(correlation);

            return abs switch
            {
                >= 0.80m => "VERY_STRONG",
                >= 0.60m => "STRONG",
                >= 0.40m => "MODERATE",
                >= 0.20m => "WEAK",
                _ => "VERY_WEAK"
            };
        }

        private static string ResolveDirection(decimal correlation)
        {
            return correlation switch
            {
                > 0.10m => "POSITIVE",
                < -0.10m => "NEGATIVE",
                _ => "NEUTRAL"
            };
        }

        private static string ResolveRiskSummary(decimal averageAbsCorrelation)
        {
            return averageAbsCorrelation switch
            {
                >= 0.75m => "HIGH_CORRELATION_RISK",
                >= 0.50m => "MODERATE_CORRELATION_RISK",
                >= 0.25m => "LOW_CORRELATION_RISK",
                _ => "DIVERSIFIED"
            };
        }
    }
}
