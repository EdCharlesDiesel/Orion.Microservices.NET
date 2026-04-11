using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ITerritoryRepository:IRepository<ITerritory>
    {
        Task<ITerritory> Get(int id);
        ITerritory New();
    }
}
