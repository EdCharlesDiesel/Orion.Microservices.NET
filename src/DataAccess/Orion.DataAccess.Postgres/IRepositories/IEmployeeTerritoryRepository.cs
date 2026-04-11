using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IEmployeeTerritoryRepository:IRepository<IEmployeeTerritory>
    {
        Task<IEmployeeTerritory> Get(int id);
        IEmployeeTerritory New();
    }
}
