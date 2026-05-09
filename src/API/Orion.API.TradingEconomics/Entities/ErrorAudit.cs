namespace Orion.API.TradingEconomics.Entities;

public class ErrorAudit
{
    public string Stage { get; set; }
    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public Dictionary<string, object> Context { get; set; }
}