namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Result of stop loss calculation
    /// </summary>
    public class StopLossResult
    {
        public decimal Stop { get; set; }
        public string Method { get; set; } = string.Empty;
        public decimal DistancePips { get; set; }
        public decimal DistancePrice { get; set; }
        public bool IsValid { get; set; } = true;
    }
}