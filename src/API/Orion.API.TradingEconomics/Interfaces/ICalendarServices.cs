using Orion.DataAccess.Postgres.Entities.TradingEconomics;

namespace Orion.API.TradingEconomics.Interfaces;

public interface ICalendarServices
{
    Task Create(List<TradingEconomicsCalendar> calendarEvents);
    Task<object?> GetCalendarEventsByIndicators(string[] names);
    Task<object?> GetCalendarEventsByCountriesAndDates(DateTime startDate, DateTime endDate, string[] names);
    Task<object?> GetCalendarEventsByDate(DateTime startDate, DateTime endDate);
    Task<object?> GetCalendarEventsByCountries(string[] names);
    Task<string> GetCalendarEvents();
    void GetCalendarEventsByIndicator(string[] indicators);
}