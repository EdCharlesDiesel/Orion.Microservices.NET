namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Result of take profit calculation
    /// </summary>
    public class TakeProfitResult
    {
        public decimal TP1 { get; set; }
        public decimal TP2 { get; set; }
        public string MethodTP1 { get; set; } = string.Empty;
        public string MethodTP2 { get; set; } = string.Empty;
        public decimal RR1 { get; set; }
        public decimal RR2 { get; set; }
        public bool TP1Valid { get; set; }
        public bool TP2Valid { get; set; }
    }
}