using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities.Shared;
using Orion.DataAccess.Postgres.Services;

namespace Orion.DataAccess.Postgres.Repositories;

public class RevenueStreamsRepository : IRevenueStreamsRepository
{
    public RevenueStreamsRepository(OrionDbContext context)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RevenueStream>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}