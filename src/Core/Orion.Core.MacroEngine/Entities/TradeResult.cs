using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class TradeResult
    {
        public string Pair { get; set; } = default!;
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }

        public double EntryPrice { get; set; }
        public double ExitPrice { get; set; }

        public double PositionSize { get; set; }
        public double PnL { get; set; }

        public double ReturnPct => PnL / PositionSize;
    }

    public class EquityPoint
    {
        public DateTime Time { get; set; }
        public double Equity { get; set; }
    }
}
