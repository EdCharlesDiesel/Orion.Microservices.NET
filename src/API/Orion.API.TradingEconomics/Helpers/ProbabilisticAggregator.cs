using Orion.API.TradingEconomics.Entities;
using System.Linq;

namespace Orion.API.TradingEconomics.Helpers
{
    public static class ProbabilisticAggregator
    {
        public static ProbabilisticResult Aggregate(List<SimulationResult> sims)
        {
            var returns = sims.Select(x => x.PortfolioReturn).ToList();

            var mean = returns.Average();
            var std = Math.Sqrt(returns.Sum(r => Math.Pow((double)(r - mean), 2)) / returns.Count);

            var sorted = returns.OrderBy(x => x).ToList();

            var var95 = sorted[(int)(0.05 * sorted.Count)];
            var es = sorted.Take((int)(0.05 * sorted.Count)).Average();

            var probLoss = returns.Count(r => r < 0) / (double)returns.Count;

            return new ProbabilisticResult
            {
                MeanReturn = mean,
                StdDev = (decimal)std,
                ValueAtRisk95 = var95,
                ExpectedShortfall = es,
                ProbabilityOfLoss = (decimal)probLoss,
                Distribution = returns
            };
        }
    }
}
