using Orion.Core.MacroEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class SimpleExecutionCostModel : IExecutionCostModel
    {
        public double EstimateSpread(string pair, double bid, double ask)
        {
            return Math.Abs(ask - bid);
        }

        public double EstimateSlippage(string pair, double size)
        {
            // Size-based slippage model
            var baseSlippage = 0.00005;

            var liquidityFactor = pair.Contains("ZAR") ? 2.5 : 1.0;

            return baseSlippage * liquidityFactor * Math.Log(1 + size / 10000);
        }
    }
}
