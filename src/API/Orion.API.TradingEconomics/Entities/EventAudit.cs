namespace Orion.API.TradingEconomics.Engine;

public class EventAudit
{
    public string EventName { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
}