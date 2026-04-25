using Orion.Core.MacroEngine.Application;
using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Service
{
    public class PortfolioExecutor
    {
        private readonly ExecutionEngine _engine;

        public PortfolioExecutor(ExecutionEngine engine)
        {
            _engine = engine;
        }

        public async Task<List<ExecutionOrder>> ExecutePortfolio(
            List<PortfolioPosition> positions)
        {
            var orders = new List<ExecutionOrder>();

            foreach (var p in positions)
            {
                var order = await _engine.ExecuteAsync(
                    p.Pair,
                    p.Direction,
                    p.PositionSize);

                orders.Add(order);
            }

            return orders;
        }
    }
}
