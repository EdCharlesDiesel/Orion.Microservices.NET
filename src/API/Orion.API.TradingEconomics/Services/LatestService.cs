using Orion.API.TradingEconomics.Interfaces;
using Orion.DataAccess.Postgres.Repositories;

namespace Orion.API.TradingEconomics.Services
{
    public sealed class LatestService : ILatestService
    {
        public async Task<string> GetLatestUpdatesAsync()
        {
            return await HttpRequesterClass.HttpRequester("/updates");
        }

        public async Task<string> GetLatestUpdatesByDateAsync(DateTime startDate)
        {
            var date = startDate.ToString("yyyy-MM-dd");
            return await HttpRequesterClass.HttpRequester($"/updates/{date}");
        }
    }
}