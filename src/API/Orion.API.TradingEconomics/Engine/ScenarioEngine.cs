using MediatR;
using Orion.API.TradingEconomics.Application;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public abstract class ScenarioEngine(IMediator mediator) : IScenarioEngine
    {
        public async Task<ScenarioResult> RunAsync(Scenario scenario)
        {
            // 1. Get baseline normalized data
            var baseline = await mediator.Send(new GetNormalizedMacroDataQuery());

            // 2. Apply shocks
            var shockedData = ApplyShocks(baseline, scenario.Shocks);

            // 3. Recompute factors
            var factors = await mediator.Send(
                new CalculateCurrencyFactorsWithOverrideCommand(shockedData));

            // 4. Generate signals
            var signals = await mediator.Send(
                new GenerateFxSignalsFromFactorsCommand(factors));

            // 5. Build portfolio
            var portfolio = await mediator.Send(
                new BuildPortfolioFromSignalsCommand(signals));

            // 6. Compute impact
            var impact = ComputeImpact(factors, signals);

            return new ScenarioResult
            {
                ScenarioName = scenario.Name,
                Factors = factors,
                Signals = signals,
                Portfolio = portfolio,
                Impact = impact
            };
        }

        private List<NormalizedIndicator> ApplyShocks(List<NormalizedIndicator> data, List<ScenarioShock> shocks)
        {
            var result = data.Select(x => new NormalizedIndicator
            {
                Id = x.Id,
                Country = x.Country,
                Indicator = x.Indicator,
                Date = x.Date,
                Value = x.Value,
                YoY = x.YoY,
                MoM = x.MoM,
                ZScore = x.ZScore,
                Surprise = x.Surprise
            }).ToList();

            foreach (var shock in shocks)
            {
                var affected = result
                    .Where(x => x.Country == shock.Country &&
                                x.Indicator.Contains(shock.Indicator));

                foreach (var item in affected)
                {
                    if (shock.Type == ShockType.Absolute)
                        item.Value += shock.ShockValue;
                    else
                        item.Value *= (1 + shock.ShockValue);

                    // Recompute ZScore (simplified)
                    item.ZScore += shock.ShockValue * 0.5m;
                }
            }

            return result;
        }

        private static ScenarioImpact ComputeImpact(List<CurrencyFactorScore> factors, List<FxSignal> signals)
        {
            var topFactor = factors
                .OrderByDescending(x => Math.Abs(x.TotalScore))
                .FirstOrDefault();

            var topSignal = signals
                .OrderByDescending(x => x.SignalStrength)
                .FirstOrDefault();

            var avgSignalStrength = signals.Count == 0
                ? 0
                : signals.Average(x => x.SignalStrength);

            var avgRisk = factors.Count == 0
                ? 0
                : factors.Average(x => Math.Abs(x.Risk));

            return new ScenarioImpact
            {
                ExpectedReturnChange = avgSignalStrength,
                RiskChange = avgRisk,
                KeyDrivers = new List<string>
                {
                    topFactor is null
                        ? "No dominant macro factor found."
                        : $"{topFactor.Currency} has the strongest total macro score: {topFactor.TotalScore:N2}",

                    topSignal is null
                        ? "No FX signal generated."
                        : $"Strongest FX signal is {topSignal.Pair} with strength {topSignal.SignalStrength:N2}"
                }
            };
        }

        internal ScenarioResult Build(NormalizedIndicator normalized, RegimeResult regime)
        {
            throw new NotImplementedException();
        }
    }
}
