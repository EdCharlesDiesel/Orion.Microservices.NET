using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Calculates Pearson correlation between a primary series and other series.
    /// </summary>
    public sealed class CorrelationEngine : ICorrelationEngine
    {
        private const int MinValuesRequired = 2;
        private const decimal NeutralThreshold = 0.10m;

        /// <inheritdoc />
        public async Task<CorrelationResult> AnalyzeAsync(CorrelationRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            ValidateRequest(request);

            var primary = GetPrimarySeries(request);

            var correlations = await CalculateCorrelationsAsync(
                primary,
                request,
                cancellationToken);

            var avg = CalculateAverageAbsoluteCorrelation(correlations);

            return BuildResult(request.PrimaryPair, correlations, avg);
        }

        private static void ValidateRequest(CorrelationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PrimaryPair))
                throw new ArgumentException("Primary pair is required.", nameof(request));

            if (request.Series == null || request.Series.Count < MinValuesRequired)
                throw new ArgumentException($"At least {MinValuesRequired} series required.", nameof(request));
        }

        private static CorrelationSeries GetPrimarySeries(CorrelationRequest request)
        {
            var primary = request.Series.FirstOrDefault(x =>
                string.Equals(x.Symbol, request.PrimaryPair, StringComparison.OrdinalIgnoreCase));

            if (primary == null)
                throw new ArgumentException("Primary pair series not supplied.", nameof(request));

            if (primary.Values == null || primary.Values.Count < MinValuesRequired)
                throw new ArgumentException("Primary series has insufficient data.", nameof(request));

            return primary;
        }

        private static async Task<List<CorrelationItem>> CalculateCorrelationsAsync(
            CorrelationSeries primary,
            CorrelationRequest request,
            CancellationToken cancellationToken)
        {
            var result = new List<CorrelationItem>();

            foreach (var series in request.Series)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (IsPrimary(series, request.PrimaryPair))
                    continue;

                if (!HasValidValues(series))
                    continue;

                var correlation = await Task.Run(() =>
                    CalculateSeriesCorrelation(primary, series, request.LookbackPeriods),
                    cancellationToken);

                if (!correlation.HasValue)
                    continue;

                result.Add(new CorrelationItem
                {
                    Symbol = series.Symbol,
                    Correlation = correlation.Value,
                    Strength = GetStrength(correlation.Value),
                    Direction = GetDirection(correlation.Value)
                });
            }

            return result;
        }

        private static bool IsPrimary(CorrelationSeries s, string primary) =>
            string.Equals(s.Symbol, primary, StringComparison.OrdinalIgnoreCase);

        private static bool HasValidValues(CorrelationSeries s) =>
            s.Values != null && s.Values.Count >= MinValuesRequired;

        private static decimal? CalculateSeriesCorrelation(
            CorrelationSeries primary,
            CorrelationSeries secondary,
            int lookback)
        {
            var x = Align(primary.Values, secondary.Values, lookback);
            var y = Align(secondary.Values, primary.Values, lookback);

            if (x.Count < MinValuesRequired || y.Count < MinValuesRequired)
                return null;

            return Pearson(x, y);
        }

        private static List<decimal> Align(List<decimal> a, List<decimal> b, int lookback)
        {
            var max = Math.Min(a.Count, b.Count);
            var take = lookback > 0 ? Math.Min(max, lookback) : max;

            return a.Skip(a.Count - take).Take(take).ToList();
        }

        private static decimal Pearson(List<decimal> xVals, List<decimal> yVals)
        {
            var n = Math.Min(xVals.Count, yVals.Count);

            var x = xVals.TakeLast(n).Select(v => (double)v).ToArray();
            var y = yVals.TakeLast(n).Select(v => (double)v).ToArray();

            var avgX = x.Average();
            var avgY = y.Average();

            double num = 0, sx = 0, sy = 0;

            for (int i = 0; i < n; i++)
            {
                var dx = x[i] - avgX;
                var dy = y[i] - avgY;

                num += dx * dy;
                sx += dx * dx;
                sy += dy * dy;
            }

            var denom = Math.Sqrt(sx * sy);
            return denom == 0 ? 0 : Math.Round((decimal)(num / denom), 4);
        }

        private static string GetStrength(decimal c)
        {
            var abs = Math.Abs(c);

            return abs >= 0.80m ? "VERY_STRONG" :
                   abs >= 0.60m ? "STRONG" :
                   abs >= 0.40m ? "MODERATE" :
                   abs >= 0.20m ? "WEAK" :
                   "VERY_WEAK";
        }

        private static string GetDirection(decimal c) =>
            c > NeutralThreshold ? "POSITIVE" :
            c < -NeutralThreshold ? "NEGATIVE" :
            "NEUTRAL";

        private static decimal CalculateAverageAbsoluteCorrelation(List<CorrelationItem> list) =>
            list.Count == 0 ? 0m : Math.Round(list.Average(x => Math.Abs(x.Correlation)), 4);

        private static string GetRisk(decimal avg) =>
            avg >= 0.75m ? "HIGH_CORRELATION_RISK" :
            avg >= 0.50m ? "MODERATE_CORRELATION_RISK" :
            avg >= 0.25m ? "LOW_CORRELATION_RISK" :
            "DIVERSIFIED";

        private static CorrelationResult BuildResult(
            string pair,
            List<CorrelationItem> list,
            decimal avg)
        {
            return new CorrelationResult
            {
                PrimaryPair = pair,
                Correlations = list.OrderByDescending(x => Math.Abs(x.Correlation)).ToList(),
                AverageAbsCorrelation = avg,
                RiskSummary = GetRisk(avg),
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}