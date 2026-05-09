namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Trend bias analysis result
    /// </summary>
    public class TrendBiasResult
    {
        public string Bias { get; set; } = "Neutral";
        public int Strength { get; set; }
        public List<string> Reasons { get; set; } = new();
        public decimal ADX { get; set; }
        public bool IsTrending { get; set; }
    }
}