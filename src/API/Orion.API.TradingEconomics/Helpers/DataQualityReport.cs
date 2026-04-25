using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Helpers;

public class DataQualityReport
{
    public string Pair { get; set; }
    public DateTime Timestamp { get; set; }
    public DataQualityResult Quality { get; set; }
    public int DataPoints { get; set; }
    public DateRange DateRange { get; set; }
}