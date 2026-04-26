using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{
    public class CalculateCurrencyFactorsHandler
        : IRequestHandler<CalculateCurrencyFactorsCommand, List<CurrencyFactorScore>>
    {
        private readonly IRepository<NormalizedIndicator> _repo;

        public CalculateCurrencyFactorsHandler(IRepository<NormalizedIndicator> repo)
        {
            _repo = repo;
        }

        public async Task<List<CurrencyFactorScore>> Handle(CalculateCurrencyFactorsCommand request,CancellationToken ct)
        {
            var data = await _repo.GetAllAsync();

            var grouped = data.GroupBy(x => x.Country);

            var results = new List<CurrencyFactorScore>();

            foreach (var g in grouped)
            {
                var latest = g.OrderByDescending(x => x.Date).Take(50).ToList();

                var carry = CalculateCarry(latest);
                var growth = CalculateGrowth(latest);
                var inflation = CalculateInflation(latest);
                var risk = CalculateRisk(latest);

                var total = WeightedScore(carry, growth, inflation, risk);

                results.Add(new CurrencyFactorScore
                {
                    Currency = MapCountryToCurrency(g.Key),
                    Date = DateTime.UtcNow,
                    Carry = carry,
                    Growth = growth,
                    Inflation = inflation,
                    Risk = risk,
                    TotalScore = total
                });
            }

            return results.OrderByDescending(x => x.TotalScore).ToList();
        }

        // ================= FACTORS =================

        private decimal CalculateCarry(List<NormalizedIndicator> data)
        {
            var rates = data
                .Where(x => x.Indicator.Contains("Interest Rate"))
                .Select(x => x.Value);

            return Z(rates);
        }

        private decimal CalculateGrowth(List<NormalizedIndicator> data)
        {
            var gdp = data
                .Where(x => x.Indicator.Contains("GDP"))
                .Select(x => x.ZScore);

            return gdp.DefaultIfEmpty(0).Average();
        }

        private decimal CalculateInflation(List<NormalizedIndicator> data)
        {
            var cpi = data
                .Where(x => x.Indicator.Contains("Inflation") || x.Indicator.Contains("CPI"))
                .Select(x => x.ZScore);

            return -cpi.DefaultIfEmpty(0).Average(); // inverse effect
        }

        private decimal CalculateRisk(List<NormalizedIndicator> data)
        {
            var unemployment = data
                .Where(x => x.Indicator.Contains("Unemployment"))
                .Select(x => x.ZScore);

            return -unemployment.DefaultIfEmpty(0).Average();
        }

        private decimal WeightedScore(decimal carry, decimal growth, decimal inflation, decimal risk)
        {
            return
                0.35M * carry +
                0.30M * growth +
                0.20M * inflation +
                0.15M * risk;
        }

        private decimal Z(IEnumerable<decimal> values)
        {
            var list = values.ToList();
            if (!list.Any()) return 0;

            var mean = list.Average();
            var std = Math.Sqrt(list.Sum(v => Math.Pow((double)(v - mean), 2)) / list.Count);

            return std == 0 ? 0 : (list.Last() - mean) / (decimal)std;
        }

        private string MapCountryToCurrency(string country) => country switch
        {
            "United States" => "USD",
            "Euro Area" => "EUR",
            "Japan" => "JPY",
            "United Kingdom" => "GBP",
            "South Africa" => "ZAR",
            _ => country
        };
    }
}
