using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.BacktestEngine
{
    public class MarketReplayEngine
    {
        public async IAsyncEnumerable<Candle> ReplayAsync(
            IEnumerable<Candle> candles)
        {
            foreach (var candle in candles.OrderBy(x => x.Time))
            {
                yield return candle;

                // simulate real-time flow
                await Task.Delay(1);
            }
        }
    }
}
