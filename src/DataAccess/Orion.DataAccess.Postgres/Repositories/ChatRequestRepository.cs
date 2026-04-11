using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.DataAccess.Postgres.Repositories;
public class ChatRequestRepository(OrionDbContext context) : IChatRequestServices
{
    public async Task<string> GetChatRequests()
    {
        return await HttpRequesterClass.HttpRequester("/chatRequest");
    }

    public async Task<string> GetChatRequestsByDate(DateTime startDate, DateTime endDate)
    {
        if (startDate == default || endDate == default)
            return "Invalid date range";

        string textStartDate = startDate.ToString("yyyy-MM-dd");
        string textEndDate = endDate.ToString("yyyy-MM-dd");

        return await HttpRequesterClass.HttpRequester($"/chatRequest/country/all/{textStartDate}/{textEndDate}");
    }

    public async Task<string> GetChatRequestsByCountries(params string[] countries)
    {
        if (countries.Any(string.IsNullOrWhiteSpace))
            return "Invalid country names";

        return await HttpRequesterClass.HttpRequester($"/chatRequest/country/{string.Join(",", countries)}");
    }

    public async Task<string> GetChatRequestsByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries)
    {
        if (startDate == default || endDate == default || countries.Any(string.IsNullOrWhiteSpace))
            return "Invalid input";

        string textStartDate = startDate.ToString("yyyy-MM-dd");
        string textEndDate = endDate.ToString("yyyy-MM-dd");

        return await HttpRequesterClass.HttpRequester($"/chatRequest/country/{string.Join(",", countries)}/{textStartDate}/{textEndDate}");
    }

    public async Task<string> GetChatRequestsByIndicator(params string[] indicators)
    {
        if (indicators.Any(string.IsNullOrWhiteSpace))
            return "Invalid indicator names";

        return await HttpRequesterClass.HttpRequester($"/chatRequest/indicator/{string.Join(",", indicators)}");
    }


    public async Task<ChatRequest> Create(List<ChatRequest> chatRequestEvents)
    {
        if (chatRequestEvents == null || !chatRequestEvents.Any())
            throw new ArgumentException("Event list cannot be null or empty.");

        await context.ChatRequests.AddRangeAsync(chatRequestEvents);
        await context.SaveChangesAsync();

        // Return the first created event (or you can change this to return the list)
        return chatRequestEvents.First();
    }


}

public interface IChatRequestServices
{
}