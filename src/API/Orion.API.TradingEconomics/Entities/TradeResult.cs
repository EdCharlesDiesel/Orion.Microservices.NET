namespace Orion.API.TradingEconomics.Entities
{
    public class TradeResult
    {
        public string Pair { get; set; } = default!;
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }

        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }

        public decimal PositionSize { get; set; }
        public decimal PnL { get; set; }
        public decimal ReturnPct => PnL / PositionSize;
    }

    public class EquityPoint
    {
        public DateTime Time { get; set; }
        public decimal Equity { get; set; }
    }
}
