namespace Orion.API.TradingEconomics.Entities
{
    public sealed class SentimentRequest
    {
        public string Pair { get; set; } = "";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public List<SentimentItem> Items { get; set; } = new();
    }

    public sealed class SentimentItem
    {
        public string Source { get; set; } = "";
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public decimal Weight { get; set; } = 1m;
    }

    public sealed class SentimentResult
    {
        public string Pair { get; set; } = "";

        public decimal Score { get; set; }
        public string Bias { get; set; } = "NEUTRAL";

        public decimal Confidence { get; set; }
        public List<string> Reasons { get; set; } = new();

        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
