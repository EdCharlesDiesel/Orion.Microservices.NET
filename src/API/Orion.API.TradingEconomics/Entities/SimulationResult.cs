namespace Orion.API.TradingEconomics.Entities
{
    public class SimulationResult
    {
        public int SimulationId { get; set; }

        public decimal PortfolioReturn { get; set; }
        public decimal Risk { get; set; }

        public List<PortfolioPosition> Portfolio { get; set; }
    }

    public class ProbabilisticResult
    {
        public decimal MeanReturn { get; set; }
        public decimal StdDev { get; set; }

        public decimal ValueAtRisk95 { get; set; }
        public decimal   ExpectedShortfall { get; set; }

        public decimal ProbabilityOfLoss { get; set; }

        public List<decimal> Distribution { get; set; } = new();
    }
}
