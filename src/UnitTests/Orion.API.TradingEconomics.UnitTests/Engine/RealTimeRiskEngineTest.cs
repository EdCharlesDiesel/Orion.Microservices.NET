using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class RealTimeRiskEngineTests
    {
        [Fact]
        public void Evaluate_ShouldThrow_WhenAccountIsNull()
        {
            var engine = new RealTimeRiskEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(null!, CreateExecution(), CreateExitPlan(), CreateQuote()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenExecutionIsNull()
        {
            var engine = new RealTimeRiskEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(CreateAccount(), null!, CreateExitPlan(), CreateQuote()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenExitPlanIsNull()
        {
            var engine = new RealTimeRiskEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(CreateAccount(), CreateExecution(), null!, CreateQuote()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenQuoteIsNull()
        {
            var engine = new RealTimeRiskEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(CreateAccount(), CreateExecution(), CreateExitPlan(), null!));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenAccountBalanceIsInvalid()
        {
            var engine = new RealTimeRiskEngine();

            var account = CreateAccount();
            account.Balance = 0m;

            Assert.Throws<ArgumentException>(() =>
                engine.Evaluate(account, CreateExecution(), CreateExitPlan(), CreateQuote()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenQuoteIsInvalid()
        {
            var engine = new RealTimeRiskEngine();

            Assert.Throws<ArgumentException>(() =>
                engine.Evaluate(
                    CreateAccount(),
                    CreateExecution(),
                    CreateExitPlan(),
                    new MarketQuote { Bid = 1.2m, Ask = 1.1m }));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenDirectionIsInvalid()
        {
            var engine = new RealTimeRiskEngine();

            var execution = CreateExecution();
            execution.Direction = "BUY";

            Assert.Throws<ArgumentException>(() =>
                engine.Evaluate(CreateAccount(), execution, CreateExitPlan(), CreateQuote()));
        }

        [Fact]
        public void Evaluate_ShouldAllowTrade_WhenNoLimitsAreBreached()
        {
            var engine = new RealTimeRiskEngine();

            var result = engine.Evaluate(
                CreateAccount(),
                CreateExecution(),
                CreateExitPlan(),
                CreateQuote());

            Assert.True(result.IsAllowed);
            Assert.Equal(RiskAction.AllowTrade, result.Action);
            Assert.Empty(result.Violations);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenDrawdownLimitBreached()
        {
            var engine = new RealTimeRiskEngine(maxAccountDrawdownPercent: 5m);

            var account = CreateAccount();
            account.Equity = 90_000m;

            var result = engine.Evaluate(
                account,
                CreateExecution(),
                CreateExitPlan(),
                CreateQuote());

            Assert.False(result.IsAllowed);
            Assert.Equal(RiskAction.BlockTrade, result.Action);
            Assert.Contains(result.Violations, x => x.Contains("Account drawdown limit breached"));
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenDailyLossLimitBreached()
        {
            var engine = new RealTimeRiskEngine(maxDailyLossPercent: 2m);

            var account = CreateAccount();
            account.RealizedPnlToday = -3_000m;

            var result = engine.Evaluate(
                account,
                CreateExecution(),
                CreateExitPlan(),
                CreateQuote());

            Assert.False(result.IsAllowed);
            Assert.Contains(result.Violations, x => x.Contains("Daily loss limit breached"));
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenPositionRiskLimitBreached()
        {
            var engine = new RealTimeRiskEngine(maxPositionRiskPercent: 1m);

            var result = engine.Evaluate(
                CreateAccount(),
                CreateExecution(filledSize: 100_000m),
                CreateExitPlan(stopLoss: 1.0800m),
                CreateQuote());

            Assert.False(result.IsAllowed);
            Assert.Contains(result.Violations, x => x.Contains("Position risk limit breached"));
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenSpreadLimitBreached()
        {
            var engine = new RealTimeRiskEngine(maxSpreadPercent: 0.01m);

            var result = engine.Evaluate(
                CreateAccount(),
                CreateExecution(),
                CreateExitPlan(),
                new MarketQuote
                {
                    Bid = 1.1000m,
                    Ask = 1.1010m
                });

            Assert.False(result.IsAllowed);
            Assert.Contains(result.Violations, x => x.Contains("Spread too wide"));
        }

        private static AccountSnapshot CreateAccount()
        {
            return new AccountSnapshot
            {
                Balance = 100_000m,
                Equity = 100_000m,
                RealizedPnlToday = 0m
            };
        }

        private static ExecutionOrder CreateExecution(decimal filledSize = 10_000m)
        {
            return new ExecutionOrder
            {
                Pair = "eurusd",
                Direction = "long",
                ExecutedPrice = 1.1000m,
                FilledSize = filledSize
            };
        }

        private static ExitPlan CreateExitPlan(decimal stopLoss = 1.0950m)
        {
            return new ExitPlan
            {
                StopLoss = stopLoss
            };
        }

        private static MarketQuote CreateQuote()
        {
            return new MarketQuote
            {
                Bid = 1.0999m,
                Ask = 1.1001m
            };
        }
    }
}