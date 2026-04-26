using Moq;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class DynamicMacroSimulationEngineTests
    {
        private readonly Mock<IRegimeEngine> _mockRegime = new();
        private readonly Mock<ICorrelatedShockGenerator> _mockShock = new();
        private readonly Mock<IMacroTransitionModel> _mockTransition = new();

        [Fact]
        public void Run_WithNullInitialState_ThrowsArgumentNullException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            Assert.Throws<ArgumentNullException>(() => engine.Run(null!, 5));
        }

        [Fact]
        public void Run_WithZeroSteps_ThrowsArgumentException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            var initialState = new MacroState();
            Assert.Throws<ArgumentException>(() => engine.Run(initialState, 0));
        }

        [Fact]
        public void Run_WithNegativeSteps_ThrowsArgumentException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            var initialState = new MacroState();
            Assert.Throws<ArgumentException>(() => engine.Run(initialState, -5));
        }

        [Fact]
        public void Run_WithValidInputs_ReturnsCorrectNumberOfStates()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            var initialState = new MacroState
            {
                GdpGrowth = 0.02m,
                Inflation = 0.03m,
                Sentiment = 0.5m
            };

            var result = engine.Run(initialState, 10);

            Assert.Equal(11, result.Count); // initial + 10 steps
            Assert.Equal(initialState.GdpGrowth, result[0].GdpGrowth);
        }

        [Fact]
        public void Run_WithDependencies_UsesInjectedDependencies()
        {
            _mockRegime.Setup(r => r.Next(It.IsAny<MarketRegime>()))
                .Returns(MarketRegime.RiskOff);
            
            _mockShock.Setup(s => s.Generate())
                .Returns(new ShockResult { GrowthShock = 0.01m });
            
            _mockTransition.Setup(t => t.Next(It.IsAny<MacroState>(), It.IsAny<ShockResult>(), It.IsAny<MarketRegime>()))
                .Returns(new MacroState { GdpGrowth = 0.03m });

            var engine = new DynamicMacroSimulationEngine(
                _mockRegime.Object,
                _mockShock.Object,
                _mockTransition.Object);

            var result = engine.Run(new MacroState(), 3);

            Assert.Equal(4, result.Count);
            _mockRegime.Verify(r => r.Next(It.IsAny<MarketRegime>()), Times.Exactly(3));
            _mockShock.Verify(s => s.Generate(), Times.Exactly(3));
            _mockTransition.Verify(t => t.Next(It.IsAny<MacroState>(), It.IsAny<ShockResult>(), It.IsAny<MarketRegime>()), Times.Exactly(3));
        }

        [Fact]
        public void Simulate_WithNullNormalized_ThrowsArgumentNullException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            Assert.Throws<ArgumentNullException>(() => 
                engine.Simulate(null!, new RegimeResult(), new ProbabilisticScenarioResult()));
        }

        [Fact]
        public void Simulate_WithNullRegime_ThrowsArgumentNullException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            Assert.Throws<ArgumentNullException>(() => 
                engine.Simulate(new NormalizedIndicator(), null!, new ProbabilisticScenarioResult()));
        }

        [Fact]
        public void Simulate_WithNullProbabilities_ThrowsArgumentNullException()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            Assert.Throws<ArgumentNullException>(() => 
                engine.Simulate(new NormalizedIndicator(), new RegimeResult(), null!));
        }

        [Fact]
        public void Simulate_WithValidInputs_ReturnsResult()
        {
            _mockRegime.Setup(r => r.Next(It.IsAny<MarketRegime>()))
                .Returns(MarketRegime.RiskOn);
            
            _mockShock.Setup(s => s.GenerateWithProbabilities(It.IsAny<ProbabilisticScenarioResult>()))
                .Returns(new ShockResult());
            
            _mockTransition.Setup(t => t.NextWithNormalization(It.IsAny<NormalizedIndicator>(), It.IsAny<ShockResult>(), It.IsAny<MarketRegime>()))
                .Returns(new MacroState { IsStable = true });

            var engine = new DynamicMacroSimulationEngine(
                _mockRegime.Object,
                _mockShock.Object,
                _mockTransition.Object);

            var probabilities = new ProbabilisticScenarioResult { ScenarioCount = 5 };
            var result = engine.Simulate(new NormalizedIndicator(), new RegimeResult(), probabilities);

            Assert.NotNull(result);
            Assert.Equal(5, result.States.Count);
            Assert.Equal(MarketRegime.RiskOn, result.FinalRegime);
            Assert.Equal(100, result.SuccessRate);
        }

        [Fact]
        public void Simulate_CalculatesSuccessRateCorrectly()
        {
            var engine = new DynamicMacroSimulationEngine(
                CreateMockRegime(3),
                CreateMockShock(),
                CreateMockTransitionWithMixedStability());

            var result = engine.Simulate(new NormalizedIndicator(), new RegimeResult(), new ProbabilisticScenarioResult { ScenarioCount = 3 });

            Assert.Equal(66.67, result.SuccessRate);
        }

        [Fact]
        public void Constructor_WithNullRegimeEngine_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DynamicMacroSimulationEngine(null!, new Mock<ICorrelatedShockGenerator>().Object, new Mock<IMacroTransitionModel>().Object));
        }

        [Fact]
        public void Constructor_WithDefault_SuccessfullyCreatesInstance()
        {
            var engine = new DynamicMacroSimulationEngine(_mockRegime.Object,_mockShock.Object, _mockTransition.Object);
            Assert.NotNull(engine);
        }

        private static IRegimeEngine CreateMockRegime(int steps)
        {
            var mock = new Mock<IRegimeEngine>();
            mock.Setup(r => r.Next(It.IsAny<MarketRegime>()))
                .Returns(MarketRegime.RiskOn);
            return mock.Object;
        }

        private static ICorrelatedShockGenerator CreateMockShock()
        {
            var mock = new Mock<ICorrelatedShockGenerator>();
            mock.Setup(s => s.GenerateWithProbabilities(It.IsAny<ProbabilisticScenarioResult>()))
                .Returns(new ShockResult());
            return mock.Object;
        }

        private static IMacroTransitionModel CreateMockTransitionWithMixedStability()
        {
            var mock = new Mock<IMacroTransitionModel>();
            var callCount = 0;
            mock.Setup(t => t.NextWithNormalization(It.IsAny<NormalizedIndicator>(), It.IsAny<ShockResult>(), It.IsAny<MarketRegime>()))
                .Returns(() => new MacroState { IsStable = callCount++ != 1 }); // second call returns false
            return mock.Object;
        }
    }
}