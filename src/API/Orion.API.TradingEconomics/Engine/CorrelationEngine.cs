using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Engine for analyzing correlations between financial time series data.
    /// </summary>
    public sealed class CorrelationEngine : ICorrelationEngine
    {
        private const int MinValuesRequired = 2;
        private const decimal NeutralThreshold = 0.10m;

        /// <inheritdoc />
        public async Task<CorrelationResult> AnalyzeAsync(CorrelationRequest request, CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);

            var primarySeries = GetPrimarySeries(request);
            var correlations = await CalculateCorrelationsAsync(request, primarySeries, cancellationToken);
            var averageCorrelation = CalculateAverageAbsoluteCorrelation(correlations);

            return BuildResult(request.PrimaryPair, correlations, averageCorrelation);
        }

        private static void ValidateRequest(CorrelationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.PrimaryPair))
                throw new ArgumentException("Primary pair is required.", nameof(request));

            if (request.Series == null || request.Series.Count < MinValuesRequired)
                throw new ArgumentException($"At least {MinValuesRequired} series are required for correlation analysis.", nameof(request));
        }

        private static DataSeries GetPrimarySeries(CorrelationRequest request)
        {
            var primary = request.Series.FirstOrDefault(x =>
                string.Equals(x.Symbol, request.PrimaryPair, StringComparison.OrdinalIgnoreCase));

            if (primary == null)
                throw new ArgumentException("Primary pair series was not supplied.", nameof(request));

            if (primary.Values == null || primary.Values.Count < MinValuesRequired)
                throw new ArgumentException($"Primary pair requires at least {MinValuesRequired} values.", nameof(request));

            return primary;
        }

        private static async Task<List<CorrelationItem>> CalculateCorrelationsAsync(
            CorrelationRequest request,
            DataSeries primarySeries,
            CancellationToken cancellationToken)
        {
            var correlations = new List<CorrelationItem>();

            foreach (var series in request.Series)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (IsPrimarySeries(series, request.PrimaryPair))
                    continue;

                if (!HasValidValues(series))
                    continue;

                var correlation = await Task.Run(() => CalculateSeriesCorrelation(primarySeries, series, request.LookbackPeriods), cancellationToken);

                if (correlation.HasValue)
                {
                    correlations.Add(new CorrelationItem
                    {
                        Symbol = series.Symbol,
                        Correlation = correlation.Value,
                        Strength = GetCorrelationStrength(correlation.Value),
                        Direction = GetCorrelationDirection(correlation.Value)
                    });
                }
            }

            return correlations;
        }

        private static bool IsPrimarySeries(DataSeries series, string primaryPair) =>
            string.Equals(series.Symbol, primaryPair, StringComparison.OrdinalIgnoreCase);

        private static bool HasValidValues(DataSeries series) =>
            series.Values != null && series.Values.Count >= MinValuesRequired;

        private static decimal? CalculateSeriesCorrelation(DataSeries primary, DataSeries secondary, int lookbackPeriods)
        {
            var alignedPrimary = AlignValues(primary.Values, secondary.Values, lookbackPeriods);
            var alignedSecondary = AlignValues(secondary.Values, primary.Values, lookbackPeriods);

            if (alignedPrimary.Count < MinValuesRequired || alignedSecondary.Count < MinValuesRequired)
                return null;

            return CalculatePearsonCorrelation(alignedPrimary, alignedSecondary);
        }

        private static List<decimal> AlignValues(List<decimal> primary, List<decimal> secondary, int lookbackPeriods)
        {
            var maxCount = Math.Min(primary.Count, secondary.Count);
            var takeCount = lookbackPeriods > 0 ? Math.Min(maxCount, lookbackPeriods) : maxCount;

            return primary
                .Skip(primary.Count - takeCount)
                .Take(takeCount)
                .ToList();
        }

        private static decimal CalculatePearsonCorrelation(List<decimal> xValues, List<decimal> yValues)
        {
            var count = Math.Min(xValues.Count, yValues.Count);

            if (count < MinValuesRequired)
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
            var correlation = denominator == 0 ? 0 : numerator / denominator;

            return Math.Round((decimal)correlation, 4);
        }

        private static string GetCorrelationStrength(decimal correlation)
        {
            var abs = Math.Abs(correlation);

            return abs >= 0.80m ? "VERY_STRONG" :
                   abs >= 0.60m ? "STRONG" :
                   abs >= 0.40m ? "MODERATE" :
                   abs >= 0.20m ? "WEAK" :
                   "VERY_WEAK";
        }

        private static string GetCorrelationDirection(decimal correlation) =>
            correlation > NeutralThreshold ? "POSITIVE" :
            correlation < -NeutralThreshold ? "NEGATIVE" :
            "NEUTRAL";

        private static decimal CalculateAverageAbsoluteCorrelation(List<CorrelationItem> correlations) =>
            correlations.Count == 0
                ? 0m
                : Math.Round(correlations.Average(x => Math.Abs(x.Correlation)), 4);

        private static string GetRiskSummary(decimal averageAbsCorrelation) =>
            averageAbsCorrelation >= 0.75m ? "HIGH_CORRELATION_RISK" :
            averageAbsCorrelation >= 0.50m ? "MODERATE_CORRELATION_RISK" :
            averageAbsCorrelation >= 0.25m ? "LOW_CORRELATION_RISK" :
            "DIVERSIFIED";

        private static CorrelationResult BuildResult(string primaryPair, List<CorrelationItem> correlations, decimal averageCorrelation) =>
            new CorrelationResult
            {
                PrimaryPair = primaryPair,
                Correlations = correlations.OrderByDescending(x => Math.Abs(x.Correlation)).ToList(),
                AverageAbsCorrelation = averageCorrelation,
                RiskSummary = GetRiskSummary(averageCorrelation),
                TimestampUtc = DateTime.UtcNow
            };
    }
}