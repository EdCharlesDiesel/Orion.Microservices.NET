using Orion.DataAccess.Postgres.Entities.Shared;

namespace Orion.DataAccess.Postgres.Services;

public interface IRevenueStreamsRepository
{
    Task<IEnumerable<RevenueStream>> GetAllAsync();
}