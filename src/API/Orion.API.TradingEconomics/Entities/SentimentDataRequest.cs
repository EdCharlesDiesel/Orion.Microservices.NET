namespace Orion.API.TradingEconomics.Entities
{
    public sealed class SentimentDataRequest
    {
        public string Pair { get; set; } = "";
        public DateTime FromUtc { get; set; } = DateTime.UtcNow.AddDays(-3);
        public DateTime ToUtc { get; set; } = DateTime.UtcNow;
        public int MaxItems { get; set; } = 25;
    }
}
