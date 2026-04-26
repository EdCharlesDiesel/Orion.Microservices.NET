using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Handlers
{
    public class GenerateFxSignalsHandler(IMediator mediator)
        : IRequestHandler<GenerateFxSignalsCommand, List<FxSignal>>
    {
        public async Task<List<FxSignal>> Handle(GenerateFxSignalsCommand request,CancellationToken ct)
        {
            var scores = await mediator.Send(new CalculateCurrencyFactorsCommand());

            var signals = new List<FxSignal>();

            // Generate all combinations
            for (int i = 0; i < scores.Count; i++)
            {
                for (int j = i + 1; j < scores.Count; j++)
                {
                    var a = scores[i];
                    var b = scores[j];

                    var diff = a.TotalScore - b.TotalScore;

                    var direction = diff > 0 ? "LONG" : "SHORT";

                    var baseCurrency = diff > 0 ? a.Currency : b.Currency;
                    var quoteCurrency = diff > 0 ? b.Currency : a.Currency;

                    var strength = Math.Abs(diff);

                    var confidence = CalculateConfidence(strength);

                    signals.Add(new FxSignal
                    {
                        BaseCurrency = baseCurrency,
                        QuoteCurrency = quoteCurrency,
                        BaseScore = (decimal)(diff > 0 ? a.TotalScore : b.TotalScore),
                        QuoteScore = diff > 0 ? b.TotalScore : a.TotalScore,
                        SignalStrength = strength,
                        Direction = direction,
                        Confidence = confidence
                    });
                }
            }

            return signals
                .OrderByDescending(x => x.SignalStrength)
                .ToList();
        }

        private decimal CalculateConfidence(decimal strength)
        {
            // Sigmoid-like scaling (0 → 1)
            return (decimal)(1 - Math.Exp((double)-strength));
        }
    }
}
