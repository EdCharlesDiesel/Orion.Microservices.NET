using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Common;

public class Forecast:Entity<Guid>
{
    public string Country { get; set; }                // “Mexico”
    public string Category { get; set; }               // “Auto Exports”
    public string Title { get; set; }                  // “Mexico Auto Exports”
    public double? LatestValue { get; set; }           // 272.69
    public DateTime? LatestValueDate { get; set; }     // “2023-03-31T00:00:00”

    public double? ForecastValue1Q { get; set; }       // 257.00
    public double? ForecastValue2Q { get; set; }       // 250.00
    public double? ForecastValue3Q { get; set; }       // 239.00
    public double? ForecastValue4Q { get; set; }       // 274.00

    public double? ForecastValue1 { get; set; }        // 239.00
    public double? ForecastValue2 { get; set; }        // 242.00
    public double? ForecastValue3 { get; set; }        // 232.00

    public DateTime? Q1Date { get; set; }             // “2023-06-30T00:00:00”
    public DateTime? Q2Date { get; set; }             // “2023-09-30T00:00:00”
    public DateTime? Q3Date { get; set; }             // “2023-12-31T00:00:00”
    public DateTime? Q4Date { get; set; }             // “2024-03-31T00:00:00”

    public DateTime? ForecastLastUpdate { get; set; }  // “2024-03-31T00:00:00”

    public string Frequency { get; set; }              // “Monthly”
    public string Unit { get; set; }                   // “Thousand Units”
    public string HistoricalDataSymbol { get; set; }  
}