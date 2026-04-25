namespace Orion.API.TradingEconomics.Helpers;

public class MarketPipelineException : Exception
{
    public MarketPipelineException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}