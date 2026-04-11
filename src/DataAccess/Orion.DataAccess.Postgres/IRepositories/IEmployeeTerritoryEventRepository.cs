using System.Threading.Tasks;

namespace Orion.Domain.IRepositories
{
    public interface IEmployeeTerritoryEventRepository:IRepository<IEmployeeTerritoryEvent>
    {
        Task<IEmployeeTerritoryEvent> Get(int id);
        IEmployeeTerritoryEvent New();
    }

    public interface IEmployeeTerritoryEvent
    {
    }
}
