namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ILatestService
    {
        Task<string> GetLatestUpdatesAsync();
        Task<string> GetLatestUpdatesByDateAsync(DateTime startDate);
    }
}