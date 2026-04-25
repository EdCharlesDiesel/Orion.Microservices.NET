using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Helpers
{
    public static class EquityBuilder
    {
        public static List<EquityPoint> Build(List<TradeResult> trades, decimal start)
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
