using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{


    public record BuildPortfolioFromSignalsCommand(List<FxSignal> Signals, decimal Capital = 100000m) : IRequest<List<PortfolioPosition>>;

    public class BuildPortfolioFromSignalsHandler
        : IRequestHandler<BuildPortfolioFromSignalsCommand, List<PortfolioPosition>>
    {
        public Task<List<PortfolioPosition>> Handle(
            BuildPortfolioFromSignalsCommand request,
            CancellationToken cancellationToken)
        {
            var selected = EnforceExposureLimits(
                request.Signals
                    .Where(x => x.SignalStrength >= 0.50m)
                    .OrderByDescending(x => x.SignalStrength)
                    .Take(10)
                    .ToList(),
                maxPerCurrency: 2);

            if (selected.Count == 0)
                return Task.FromResult(new List<PortfolioPosition>());

            var totalStrength = selected.Sum(x => x.SignalStrength);

            var portfolio = selected.Select(signal =>
            {
                var weight = totalStrength == 0
                    ? 1m / selected.Count
                    : signal.SignalStrength / totalStrength;

                return new PortfolioPosition
                {
                    Pair = $"{signal.BaseCurrency}/{signal.QuoteCurrency}",
                    BaseCurrency = signal.BaseCurrency,
                    QuoteCurrency = signal.QuoteCurrency,
                    Direction = signal.Direction,
                    SignalStrength = signal.SignalStrength,
                    Confidence = signal.Confidence,
                    Volatility = 1m,
                    Weight = weight,
                    PositionSize = request.Capital * weight
                };
            }).ToList();

            return Task.FromResult(portfolio);
        }

        private static List<FxSignal> EnforceExposureLimits(
            List<FxSignal> signals,
            int maxPerCurrency)
        {
            var result = new List<FxSignal>();
            var exposure = new Dictionary<string, int>();

            foreach (var signal in signals)
            {
                if (!CanAdd(signal.BaseCurrency, exposure, maxPerCurrency))
                    continue;

                if (!CanAdd(signal.QuoteCurrency, exposure, maxPerCurrency))
                    continue;

                result.Add(signal);

                Increment(signal.BaseCurrency, exposure);
                Increment(signal.QuoteCurrency, exposure);
            }

            return result;
        }

        private static bool CanAdd(string currency, Dictionary<string, int> exposure, int max)
        {
            return !exposure.TryGetValue(currency, out var count) || count < max;
        }

        private static void Increment(string currency, Dictionary<string, int> exposure)
        {
            exposure[currency] = exposure.TryGetValue(currency, out var count) ? count + 1 : 1;
        }
    }
}
