using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Entities
{
    public class SimpleExecutionCostModel : IExecutionCostModel
    {
        public decimal EstimateSpread(string pair, decimal bid, decimal ask)
        {
            return Math.Abs(ask - bid);
        }

        public decimal EstimateSlippage(string pair, decimal size)
        {
            // Size-based slippage model
            var baseSlippage = 0.00005M;

            var liquidityFactor = pair.Contains("ZAR") ? 2.5M : 1.0M;

            return baseSlippage * liquidityFactor * (decimal)Math.Log((double)(1 + (size / 10000)));
        }
    }
}
