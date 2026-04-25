namespace Orion.API.TradingEconomics.Entities;

public class MacroData
{
    public Dictionary<string, CurrencyMacroData> Data { get; set; } = new();
    public bool IsLive { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Warning { get; set; }
    public string DataSource { get; set; }
}