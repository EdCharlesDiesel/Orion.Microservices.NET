namespace Orion.API.TradingEconomics.DTO
{
    /// <summary>
    /// FRED API status response
    /// </summary>
    public class FredStatusResponse
    {
        public bool IsConfigured { get; set; }
        public bool IsConnected { get; set; }
        public string? ApiKeyProvided { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CheckedAt { get; set; }
        public Dictionary<string, bool> SeriesAvailability { get; set; } = new();
    }
}