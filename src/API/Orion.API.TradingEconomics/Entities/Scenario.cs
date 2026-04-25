namespace Orion.API.TradingEconomics.Entities
{
    public class Scenario
    {
        public string Name { get; set; } = default!;
        public List<ScenarioShock> Shocks { get; set; } = new();
    }

    public class ScenarioShock
    {
        public string Country { get; set; } = default!;
        public string Indicator { get; set; } = default!;

        public decimal ShockValue { get; set; } // absolute or %
        public ShockType Type { get; set; }
    }

    public enum ShockType
    {
        Absolute,   // +2%
        Relative    // +10%
    }

    public class ScenarioResult
    {
        public string ScenarioName { get; set; } = default!;

        public List<CurrencyFactorScore> Factors { get; set; }
        public List<FxSignal> Signals { get; set; }
        public List<PortfolioPosition> Portfolio { get; set; }

        public ScenarioImpact Impact { get; set; }
        public string Name { get; internal set; }
        public string Direction { get; internal set; }
    }

    public class ScenarioImpact
    {
        public decimal ExpectedReturnChange { get; set; }
        public decimal RiskChange { get; set; }

        public List<string> KeyDrivers { get; set; } = new();
    }
}
