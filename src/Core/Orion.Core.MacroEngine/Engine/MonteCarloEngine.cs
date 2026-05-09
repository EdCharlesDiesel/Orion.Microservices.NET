using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.BacktestEngine
{
    public class MonteCarloEngine
    {
        public List<double> Run(List<TradeResult> trades, int simulations = 1000)
        {
            var results = new List<double>();
            var rand = new Random();

            for (int i = 0; i < simulations; i++)
            {
                var equity = 100000.0;

                var shuffled = trades.OrderBy(x => rand.Next()).ToList();

                foreach (var t in shuffled)
                {
                    equity += t.PnL;
                }

                results.Add(equity);
            }

            return results;
        }
    }
}
