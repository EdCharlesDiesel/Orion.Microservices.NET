namespace Orion.API.TradingEconomics.Engine;

public sealed class TradingAlert
{
    public string Severity { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static TradingAlert Info(string message)
    {
        return new TradingAlert
        {
            Severity = "INFO",
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }

    public static TradingAlert Warning(string message)
    {
        return new TradingAlert
        {
            Severity = "WARNING",
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }

    public static TradingAlert Critical(string message)
    {
        return new TradingAlert
        {
            Severity = "CRITICAL",
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }
}