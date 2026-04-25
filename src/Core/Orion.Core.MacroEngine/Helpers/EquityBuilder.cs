using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public static class EquityBuilder
    {
        public static List<EquityPoint> Build(List<TradeResult> trades, double start)
        {
            var equity = start;
            var curve = new List<EquityPoint>();

            foreach (var t in trades.OrderBy(x => x.ExitTime))
            {
                equity += t.PnL;

                curve.Add(new EquityPoint
                {
                    Time = t.ExitTime,
                    Equity = equity
                });
            }

            return curve;
        }
    }
}
