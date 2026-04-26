namespace Orion.API.TradingEconomics.Entities;

public class DataSeries
{
    public string Symbol { get; set; }
    public List<decimal> Values { get; set; }
}