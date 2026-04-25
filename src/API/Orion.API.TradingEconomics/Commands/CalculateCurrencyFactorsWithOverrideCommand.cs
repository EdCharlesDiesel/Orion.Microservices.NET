using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{

    public record CalculateCurrencyFactorsWithOverrideCommand(List<NormalizedIndicator> Data) : IRequest<List<CurrencyFactorScore>>;

    public class CalculateCurrencyFactorsWithOverrideHandler
        : IRequestHandler<CalculateCurrencyFactorsWithOverrideCommand, List<CurrencyFactorScore>>
    {
        public Task<List<CurrencyFactorScore>> Handle(
            CalculateCurrencyFactorsWithOverrideCommand request,
            CancellationToken cancellationToken)
        {
            var results = request.Data
                .GroupBy(x => x.Country)
                .Select(g =>
                {
                    var latest = g.OrderByDescending(x => x.Date).ToList();

                    var carry = AvgZ(latest, "Interest Rate", "Policy Rate", "Rate");
                    var growth = AvgZ(latest, "GDP", "Growth");
                    var inflation = -AvgZ(latest, "CPI", "Inflation");
                    var risk = -AvgZ(latest, "Unemployment", "Risk");

                    var total =
                        0.35m * carry +
                        0.30m * growth +
                        0.20m * inflation +
                        0.15m * risk;

                    return new CurrencyFactorScore
                    {
                        Currency = MapCountryToCurrency(g.Key),
                        Date = DateTime.UtcNow,
                        Carry = carry,
                        Growth = growth,
                        Inflation = inflation,
                        Risk = risk,
                        TotalScore = total
                    };
                })
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            return Task.FromResult(results);
        }

        private static decimal AvgZ(
            IEnumerable<NormalizedIndicator> data,
            params string[] keywords)
        {
            var values = data
                .Where(x => keywords.Any(k =>
                    x.Indicator.Contains(k, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.ZScore)
                .ToList();

            return values.Count == 0 ? 0 : values.Average();
        }

        private static string MapCountryToCurrency(string country)
        {
            return country switch
            {
                "United States" => "USD",
                "Euro Area" => "EUR",
                "Japan" => "JPY",
                "United Kingdom" => "GBP",
                "South Africa" => "ZAR",
                "Australia" => "AUD",
                "New Zealand" => "NZD",
                "Canada" => "CAD",
                "Switzerland" => "CHF",
                _ => country
            };
        }
    }
}
