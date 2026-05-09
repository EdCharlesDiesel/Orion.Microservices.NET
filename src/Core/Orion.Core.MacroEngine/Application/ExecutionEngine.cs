using Orion.Core.MacroEngine.Entities;
using Orion.Core.MacroEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Application
{
    public class ExecutionEngine
    {
        private readonly IMarketDataFeed _market;
        private readonly IExecutionCostModel _cost;

        public ExecutionEngine(IMarketDataFeed market, IExecutionCostModel cost)
        {
            _market = market;
            _cost = cost;
        }

        public async Task<ExecutionOrder> ExecuteAsync(
            string pair,
            string direction,
            double size)
        {
            var tick = await _market.GetLatestTickAsync(pair);

            var mid = (tick.Bid + tick.Ask) / 2.0;

            var spread = _cost.EstimateSpread(pair, tick.Bid, tick.Ask);
            var slippage = _cost.EstimateSlippage(pair, size);

            double executedPrice;

            if (direction == "LONG")
            {
                executedPrice = tick.Ask + slippage;
            }
            else
            {
                executedPrice = tick.Bid - slippage;
            }

            return new ExecutionOrder
            {
                Pair = pair,
                Direction = direction,
                RequestedSize = size,
                FilledSize = size, // assume full fill for now
                RequestedPrice = mid,
                ExecutedPrice = executedPrice,
                SpreadCost = spread,
                SlippageCost = slippage,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
