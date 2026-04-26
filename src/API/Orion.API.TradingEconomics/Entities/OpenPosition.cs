namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenPosition
    {
        public string Pair { get; set; } = default!;
        public string Direction { get; set; } = default!;

        public decimal EntryPrice { get; set; }
        public decimal Size { get; set; }

        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; } 
        public bool IsClosed { get; set; }
        public decimal NotionalUsd { get; set; }
    }
}
