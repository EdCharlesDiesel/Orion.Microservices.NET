using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities.Common;

/// <summary>
/// Trading Economics Calendar of the database. 
/// </summary>
[Table("TradingEconomicsCalendar")]
public class TradingEconomicsCalendar: IBaseEntity
{
    public object SourceURL;
    public string CalendarId { get; set; }
    public int Importance { get; set; }
    public string Country { get; set; }
    public string Category { get; set; }
    public string Event { get; set; }
    public string Source { get; set; }
    public string SourceUrl { get; set; }
    public string Actual { get; set; }
    public string Previous { get; set; }
    public string Forecast { get; set; }
    public string TeForecast { get; set; }
    public string Url { get; set; }
    public string DateSpan { get; set; }
    public string Revised { get; set; }
    public string Currency { get; set; }
    public string Unit { get; set; }
    public string Ticker { get; set; }
    public string Symbol { get; set; }
    public string Reference { get; set; }
    public DateTime? LastUpdate { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public Status Status { get; set; }
}

