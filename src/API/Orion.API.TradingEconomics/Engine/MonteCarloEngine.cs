using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public class MonteCarloEngine
    {
        public List<decimal> Run(List<TradeResult> trades, int simulations = 1000)
        {
            var results = new List<decimal>();
            var rand = new Random();

            for (int i = 0; i < simulations; i++)
            {
                var equity = 100000.0M;

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
