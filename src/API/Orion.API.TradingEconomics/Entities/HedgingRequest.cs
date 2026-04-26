namespace Orion.API.TradingEconomics.Entities
{
    public sealed class HedgingRequest
    {
        public string PortfolioBaseCurrency { get; set; } = "USD";
        public List<OpenPosition> OpenPositions { get; set; } = new();
        public List<HedgeCandidate> HedgeCandidates { get; set; } = new();
        public decimal MaxHedgePercent { get; set; } = 50m;
    }

    public sealed class HedgePosition
    {
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal NotionalUsd { get; set; }
        public decimal UnrealizedPnL { get; set; }
    }

    public sealed class HedgeCandidate
    {
        public string Pair { get; set; } = "";
        public decimal CorrelationToPortfolio { get; set; }
        public decimal VolatilityPercent { get; set; }
        public decimal LiquidityScore { get; set; }
    }

    public sealed class HedgingResult
    {
        public decimal NetLongExposureUsd { get; set; }
        public decimal NetShortExposureUsd { get; set; }
        public decimal NetExposureUsd { get; set; }
        public decimal GrossExposureUsd { get; set; }
        public decimal HedgeRequiredUsd { get; set; }
        public List<HedgeRecommendation> Recommendations { get; set; } = new();
        public string HedgeStatus { get; set; } = "";
        public string RiskSummary { get; set; } = "";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }

    public sealed class HedgeRecommendation
    {
        public string Pair { get; set; } = "";
        public string Direction { get; set; } = "";
        public decimal HedgeSizeUsd { get; set; }
        public decimal HedgeRatio { get; set; }
        public decimal CorrelationUsed { get; set; }
        public decimal Confidence { get; set; }
        public string Reason { get; set; } = "";
    }

}
