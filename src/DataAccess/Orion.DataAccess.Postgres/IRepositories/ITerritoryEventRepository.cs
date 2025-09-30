using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ITerritoryEventRepository:IRepository<ITerritoryEvent>
    {
        Task<ITerritoryEvent> Get(int id);
        ITerritoryEvent New();
    }
}
