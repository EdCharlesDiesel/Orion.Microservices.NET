using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class NormalizationEngine : INormalizationEngine
    {
        private readonly NormalizationOptions _options;

        public NormalizationEngine(NormalizationOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public List<NormalizedIndicator> Normalize(IEnumerable<EconomicIndicator> raw)
        {
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

                var frequency = FrequencyResolver.Resolve(ordered.Last().Frequency);
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

                    var z = std == 0 ? 0 : (value - mean) / std;

                    if (_options.WinsorizeOutliers)
                        z = Math.Clamp(z, (decimal)-_options.WinsorizeZLimit, (decimal)_options.WinsorizeZLimit);

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

                        ZScore = z,
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

        private static decimal CalculatePeriodChange(List<EconomicIndicator> data,int index,int lag)
        {
            if (index < lag)
                return 0;

            var current = data[index].Value;
            var previous = data[index - lag].Value;

            if (!current.HasValue || !previous.HasValue || previous.Value == 0)
                return 0;

            return (current.Value - previous.Value) / Math.Abs(previous.Value);
        }

        private static decimal CalculateSurprise(EconomicIndicator x)
        {
            if (!x.Value.HasValue || !x.Forecast.HasValue || x.Forecast.Value == 0)
                return 0;

            return (x.Value.Value - x.Forecast.Value) / Math.Abs(x.Forecast.Value);
        }

        private static decimal StandardDeviation(List<decimal> values, decimal mean)
        {
            if (values.Count <= 1)
                return 0;

            var variance = values.Sum(v => Math.Pow((double)(v - mean), 2)) / (values.Count - 1);
            return (decimal)Math.Sqrt(variance);
        }
    }
}