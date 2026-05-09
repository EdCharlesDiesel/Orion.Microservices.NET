using MediatR;
using Orion.API.TradingEconomics.Application;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Runs scenario analysis by applying macro shocks and recalculating factors, signals and portfolio impact.
    /// </summary>
    public sealed class ScenarioEngine(IMediator mediator) : IScenarioEngine
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        /// <inheritdoc />
        public async Task<ScenarioResult> RunAsync(
            Scenario scenario,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(scenario);

            if (string.IsNullOrWhiteSpace(scenario.Name))
                throw new ArgumentException("Scenario name is required.", nameof(scenario));

            scenario.Shocks ??= [];

            var baseline = await _mediator.Send(
                new GetNormalizedMacroDataQuery(),
                cancellationToken);

            var shockedData = ApplyShocks(baseline, scenario.Shocks);

            var factors = await _mediator.Send(
                new CalculateCurrencyFactorsWithOverrideCommand(shockedData),
                cancellationToken);

            var signals = await _mediator.Send(
                new GenerateFxSignalsFromFactorsCommand(factors),
                cancellationToken);

            var portfolio = await _mediator.Send(
                new BuildPortfolioFromSignalsCommand(signals),
                cancellationToken);

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

        /// <inheritdoc />
        public ScenarioResult Build(
            NormalizedIndicator normalized,
            RegimeResult regime)
        {
            ArgumentNullException.ThrowIfNull(normalized);
            ArgumentNullException.ThrowIfNull(regime);

            var direction = normalized.ZScore >= 0m ? "BULLISH" : "BEARISH";
            var strength = Math.Min(100m, Math.Abs(normalized.ZScore) * 25m);

            return new ScenarioResult
            {
                ScenarioName = $"{normalized.Country}-{normalized.Indicator}-{regime.Regime}",
                Impact = new ScenarioImpact
                {
                    ExpectedReturnChange = strength,
                    RiskChange = Math.Abs(normalized.Surprise),
                    KeyDrivers =
                    [
                        $"{normalized.Country} {normalized.Indicator} is {direction} with ZScore={normalized.ZScore:F2}.",
                        $"Detected regime is {regime.Regime} with confidence {regime.Confidence:F0}%."
                    ]
                }
            };
        }

        private static List<NormalizedIndicator> ApplyShocks(
            IEnumerable<NormalizedIndicator> data,
            IEnumerable<ScenarioShock> shocks)
        {
            var result = data
                .Select(x => new NormalizedIndicator
                {
                    Id = x.Id,
                    Country = x.Country,
                    Indicator = x.Indicator,
                    Date = x.Date,
                    Value = x.Value,
                    Previous = x.Previous,
                    Forecast = x.Forecast,
                    YoY = x.YoY,
                    MoM = x.MoM,
                    ZScore = x.ZScore,
                    RollingMean = x.RollingMean,
                    RollingStdDev = x.RollingStdDev,
                    Surprise = x.Surprise,
                    Frequency = x.Frequency,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            foreach (var shock in shocks)
            {
                var affected = result.Where(x =>
                    string.Equals(x.Country, shock.Country, StringComparison.OrdinalIgnoreCase) &&
                    x.Indicator.Contains(shock.Indicator, StringComparison.OrdinalIgnoreCase));

                foreach (var item in affected)
                {
                    if (shock.Type == ShockType.Absolute)
                        item.Value += shock.ShockValue;
                    else
                        item.Value *= 1m + shock.ShockValue;

                    item.ZScore += shock.ShockValue * 0.5m;
                }
            }

            return result;
        }

        private static ScenarioImpact ComputeImpact(
            IReadOnlyCollection<CurrencyFactorScore> factors,
            IReadOnlyCollection<FxSignal> signals)
        {
            var topFactor = factors
                .OrderByDescending(x => Math.Abs(x.TotalScore))
                .FirstOrDefault();

            var topSignal = signals
                .OrderByDescending(x => x.SignalStrength)
                .FirstOrDefault();

            var averageSignalStrength = signals.Count == 0
                ? 0m
                : signals.Average(x => x.SignalStrength);

            var averageRisk = factors.Count == 0
                ? 0m
                : factors.Average(x => Math.Abs(x.Risk));

            return new ScenarioImpact
            {
                ExpectedReturnChange = averageSignalStrength,
                RiskChange = averageRisk,
                KeyDrivers =
                [
                    topFactor is null
                        ? "No dominant macro factor found."
                        : $"{topFactor.Currency} has the strongest total macro score: {topFactor.TotalScore:N2}",

                    topSignal is null
                        ? "No FX signal generated."
                        : $"Strongest FX signal is {topSignal.Pair} with strength {topSignal.SignalStrength:N2}"
                ]
            };
        }
    }
}