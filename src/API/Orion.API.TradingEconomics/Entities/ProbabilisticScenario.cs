namespace Orion.API.TradingEconomics.Entities
{
    public class ProbabilisticScenario
    {
        public int SimulationId { get; set; }
        public List<ScenarioShock> Shocks { get; set; } = new();
    }
}
