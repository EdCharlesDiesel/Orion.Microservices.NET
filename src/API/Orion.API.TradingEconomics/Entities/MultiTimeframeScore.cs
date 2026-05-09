namespace Orion.API.TradingEconomics.Entities
{
    public partial class TechnicalAnalysisService
    {
        #region Private - Score Classes

        private class MultiTimeframeScore
        {
            public int LongScore { get; set; }
            public int ShortScore { get; set; }
            public List<string> Reasons { get; } = new();
        }

        #endregion
    }
}
