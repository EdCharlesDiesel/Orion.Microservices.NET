namespace Orion.API.TradingEconomics.Entities;

public sealed class AlphaResult
{
    public string Pair { get; set; } = "";
    public string Direction { get; set; } = "FLAT";
    public decimal AlphaScore { get; set; }
    public decimal Confidence { get; set; }

    public decimal TrendScore { get; set; }
    public decimal MomentumScore { get; set; }
    public decimal VolatilityPenalty { get; set; }
    public decimal SentimentScore { get; set; }
    public decimal MacroScore { get; set; }

    public int HighImpactMacroEvents { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}