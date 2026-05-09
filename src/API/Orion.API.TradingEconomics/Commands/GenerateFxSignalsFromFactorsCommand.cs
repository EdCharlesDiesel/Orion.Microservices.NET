using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{    

    public record GenerateFxSignalsFromFactorsCommand(List<CurrencyFactorScore> Factors) : IRequest<List<FxSignal>>;

    public class GenerateFxSignalsFromFactorsHandler
        : IRequestHandler<GenerateFxSignalsFromFactorsCommand, List<FxSignal>>
    {
        public Task<List<FxSignal>> Handle(
            GenerateFxSignalsFromFactorsCommand request,
            CancellationToken cancellationToken)
        {
            var factors = request.Factors
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            var signals = new List<FxSignal>();

            for (var i = 0; i < factors.Count; i++)
            {
                for (var j = i + 1; j < factors.Count; j++)
                {
                    var stronger = factors[i];
                    var weaker = factors[j];

                    var strength = Math.Abs(stronger.TotalScore - weaker.TotalScore);

                    if (strength <= 0)
                        continue;

                    signals.Add(new FxSignal
                    {
                        BaseCurrency = stronger.Currency,
                        QuoteCurrency = weaker.Currency,
                        BaseScore = stronger.TotalScore,
                        QuoteScore = weaker.TotalScore,
                        Confidence = CalculateConfidence(strength)
                    });
                }
            }

            return Task.FromResult(
                signals
                    .OrderByDescending(x => x.SignalStrength)
                    .ToList());
        }

        private static decimal CalculateConfidence(decimal strength)
        {
            var value = 1 - Math.Exp(-(double)strength);
            return Math.Round((decimal)value, 4);
        }
    }
}
