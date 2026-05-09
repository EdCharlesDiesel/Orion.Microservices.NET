namespace Orion.API.TradingEconomics.Entities;

public sealed class MacroEvent
{
    public string Name { get; set; } = "";
    public string Currency { get; set; } = "";
    public string Impact { get; set; } = "";
    public DateTime EventTimeUtc { get; set; }
    public string Country { get; set; }
    public string EventName { get; set; }
    public DateTime Date { get; set; }
    public decimal? Actual { get; set; }
    public decimal? Forecast { get; set; }
    public decimal? Previous { get; set; }
}