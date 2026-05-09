using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Converts raw economic indicators into normalized macro signals.
    /// </summary>
    public sealed class NormalizationEngine(NormalizationOptions options) : INormalizationEngine
    {
        private readonly NormalizationOptions _options = options ?? throw new ArgumentNullException(nameof(options));

        /// <inheritdoc />
        public List<NormalizedIndicator> Normalize(IEnumerable<EconomicIndicator> raw)
        {
            ArgumentNullException.ThrowIfNull(raw);

            var results = new List<NormalizedIndicator>();

            var groups = raw
                .Where(x => x.Value.HasValue)
                .GroupBy(x => new { x.Country, x.Indicator })
                .ToList();

            foreach (var group in groups)
            {
                var ordered = group
                    .OrderBy(x => x.Date)
                    .ToList();

                if (ordered.Count < _options.MinimumWindowSize)
                    continue;

                var frequency = FrequencyResolver.Resolve(ordered[^1].Frequency);
                var yoyLag = FrequencyResolver.YoYLag(frequency);
                var rollingWindow = FrequencyResolver.DefaultRollingWindow(frequency);

                for (var i = 0; i < ordered.Count; i++)
                {
                    var current = ordered[i];

                    var window = ordered
                        .Take(i + 1)
                        .TakeLast(rollingWindow)
                        .Where(x => x.Value.HasValue)
                        .Select(x => x.Value!.Value)
                        .ToList();

                    if (window.Count < _options.MinimumWindowSize)
                        continue;

                    var value = current.Value!.Value;
                    var mean = window.Average();
                    var std = StandardDeviation(window, mean);

                    var zScore = std == 0 ? 0 : (value - mean) / std;

                    if (_options.WinsorizeOutliers)
                    {
                        zScore = Math.Clamp(
                            zScore,
                            -_options.WinsorizeZLimit,
                            _options.WinsorizeZLimit);
                    }

                    results.Add(new NormalizedIndicator
                    {
                        Id = Guid.NewGuid(),
                        Country = current.Country,
                        Indicator = current.Indicator,
                        Date = current.Date,
                        Value = value,
                        Previous = current.Previous,
                        Forecast = current.Forecast,
                        MoM = CalculatePeriodChange(ordered, i, 1),
                        YoY = CalculatePeriodChange(ordered, i, yoyLag),
                        ZScore = zScore,
                        RollingMean = mean,
                        RollingStdDev = std,
                        Surprise = CalculateSurprise(current),
                        Frequency = current.Frequency,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            return results;
        }

        private static decimal CalculatePeriodChange(
            IReadOnlyList<EconomicIndicator> data,
            int index,
            int lag)
        {
            if (index < lag)
                return 0m;

            var current = data[index].Value;
            var previous = data[index - lag].Value;

            if (!current.HasValue || !previous.HasValue || previous.Value == 0m)
                return 0m;

            return (current.Value - previous.Value) / Math.Abs(previous.Value);
        }

        private static decimal CalculateSurprise(EconomicIndicator indicator)
        {
            if (!indicator.Value.HasValue || !indicator.Forecast.HasValue || indicator.Forecast.Value == 0m)
                return 0m;

            return (indicator.Value.Value - indicator.Forecast.Value) / Math.Abs(indicator.Forecast.Value);
        }

        private static decimal StandardDeviation(IReadOnlyCollection<decimal> values, decimal mean)
        {
            if (values.Count <= 1)
                return 0m;

            var variance = values.Sum(value =>
                Math.Pow((double)(value - mean), 2)) / (values.Count - 1);

            return (decimal)Math.Sqrt(variance);
        }
    }
}