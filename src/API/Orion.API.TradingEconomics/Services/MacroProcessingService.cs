using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Services
{
    public class MacroProcessingService
    {
        public IEnumerable<NormalizedIndicator> Normalize(IEnumerable<EconomicIndicator> data)
        {
            var grouped = data.GroupBy(x => new { x.Country, x.Indicator });

            foreach (var group in grouped)
            {
                var ordered = group.OrderBy(x => x.Date).ToList();

                for (int i = 1; i < ordered.Count; i++)
                {
                    var current = ordered[i];
                    var prev = ordered[i - 1];

                    var yoy = CalculateYoY(ordered, i);
                    var mom = (current.Value - prev.Value) / prev.Value;
                    var surprise = CalculateSurprise(current);

                    yield return new NormalizedIndicator
                    {
                        Id = Guid.NewGuid(),
                        Country = current.Country,
                        Indicator = current.Indicator,
                        Date = current.Date,
                        Value = (decimal)(current.Value ?? 0),
                        YoY = yoy,
                        MoM = (decimal)mom,
                        ZScore = 0, // computed later
                        Surprise = surprise
                    };
                }
            }
        }

        private decimal CalculateYoY(List<EconomicIndicator> data, int index)
        {
            if (index < 12) return 0;

            var current = (decimal)(data[index].Value ?? 0);
            var lastYear = (decimal)(data[index - 12].Value ?? 0);

            return (current - lastYear) / lastYear;
        }

        private decimal CalculateSurprise(EconomicIndicator x)
        {
            if (x.Forecast == null || x.Value == null) return 0;
            return ((decimal)x.Value.Value - (decimal)x.Forecast.Value) / Math.Abs((decimal)x.Forecast.Value);
        }
    }
}
