using MediatR;
using Moq;
using Orion.API.TradingEconomics.Application;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class ScenarioEngineTests
    {
        private readonly Mock<IMediator> _mediator = new();

        [Fact]
        public void Constructor_ShouldThrow_WhenMediatorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ScenarioEngine(null!));
        }

        [Fact]
        public async Task RunAsync_ShouldThrow_WhenScenarioIsNull()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                engine.RunAsync(null!));
        }

        [Fact]
        public async Task RunAsync_ShouldThrow_WhenScenarioNameIsMissing()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.RunAsync(new Scenario { Name = "" }));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnScenarioResult()
        {
            var baseline = new List<NormalizedIndicator>
            {
                new()
                {
                    Country = "US",
                    Indicator = "CPI",
                    Value = 100m,
                    ZScore = 1m
                }
            };

            var factors = new List<CurrencyFactorScore>
            {
                new()
                {
                    Currency = "USD",
                    TotalScore = 2m,
                    Risk = 0.25m
                }
            };

            var signals = new List<FxSignal>
            {
                new()
                {
                    Pair = "EUR/USD",
                    SignalStrength = 75m
                }
            };

            var portfolio = new List<PortfolioPosition>
            {
                new()
                {
                    Pair = "EUR/USD",
                    Direction = "SHORT",
                    PositionSize = 10000m
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetNormalizedMacroDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(baseline);

            _mediator
                .Setup(x => x.Send(It.IsAny<CalculateCurrencyFactorsWithOverrideCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(factors);

            _mediator
                .Setup(x => x.Send(It.IsAny<GenerateFxSignalsFromFactorsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(signals);

            _mediator
                .Setup(x => x.Send(It.IsAny<BuildPortfolioFromSignalsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(portfolio);

            var engine = CreateEngine();

            var result = await engine.RunAsync(new Scenario
            {
                Name = "Inflation Shock",
                Shocks =
                [
                    new ScenarioShock
                    {
                        Country = "US",
                        Indicator = "CPI",
                        Type = ShockType.Absolute,
                        ShockValue = 1m
                    }
                ]
            });

            Assert.Equal("Inflation Shock", result.ScenarioName);
            Assert.Same(factors, result.Factors);
            Assert.Same(signals, result.Signals);
            Assert.Same(portfolio, result.Portfolio);
            Assert.NotNull(result.Impact);
            Assert.Equal(75m, result.Impact.ExpectedReturnChange);
            Assert.Equal(0.25m, result.Impact.RiskChange);
        }

        [Fact]
        public async Task RunAsync_ShouldHandleNullShocks()
        {
            _mediator
                .Setup(x => x.Send(It.IsAny<GetNormalizedMacroDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _mediator
                .Setup(x => x.Send(It.IsAny<CalculateCurrencyFactorsWithOverrideCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _mediator
                .Setup(x => x.Send(It.IsAny<GenerateFxSignalsFromFactorsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _mediator
                .Setup(x => x.Send(It.IsAny<BuildPortfolioFromSignalsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            var engine = CreateEngine();

            var result = await engine.RunAsync(new Scenario
            {
                Name = "Base Scenario",
                Shocks = null!
            });

            Assert.Equal("Base Scenario", result.ScenarioName);
            Assert.NotNull(result.Impact);
            Assert.Equal(0m, result.Impact.ExpectedReturnChange);
            Assert.Equal(0m, result.Impact.RiskChange);
        }

        [Fact]
        public void Build_ShouldThrow_WhenNormalizedIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Build(null!, new RegimeResult()));
        }

        [Fact]
        public void Build_ShouldThrow_WhenRegimeIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Build(new NormalizedIndicator(), null!));
        }

        [Fact]
        public void Build_ShouldReturnScenarioResult()
        {
            var engine = CreateEngine();

            var result = engine.Build(
                new NormalizedIndicator
                {
                    Country = "US",
                    Indicator = "GDP",
                    ZScore = 2m,
                    Surprise = 0.10m
                },
                new RegimeResult
                {
                    Regime = MarketRegime.RiskOn,
                    Confidence = 80m
                });

            Assert.Equal("US-GDP-RiskOn", result.ScenarioName);
            Assert.NotNull(result.Impact);
            Assert.Equal(50m, result.Impact.ExpectedReturnChange);
            Assert.Equal(0.10m, result.Impact.RiskChange);
            Assert.Equal(2, result.Impact.KeyDrivers.Count);
        }

        private ScenarioEngine CreateEngine()
        {
            return new ScenarioEngine(_mediator.Object);
        }
    }
}