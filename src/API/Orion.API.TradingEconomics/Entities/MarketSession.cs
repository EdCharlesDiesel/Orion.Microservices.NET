namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Market session information
    /// </summary>
    public class MarketSession
    {
        public string Session { get; set; } = "Unknown";
        public bool IsOpen { get; set; }
        public DateTime NextSessionChange { get; set; }
        public string[] ActiveSessions { get; set; } = Array.Empty<string>();
    }
}
