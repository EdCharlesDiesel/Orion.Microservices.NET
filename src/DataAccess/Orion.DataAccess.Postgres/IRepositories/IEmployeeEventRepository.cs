using System.Threading.Tasks;

namespace Orion.Domain.IRepositories
{
    public interface IEmployeeEventRepository:IRepository<IEmployeeEvent>
    {
        Task<IEmployeeEvent> Get(int id);
        IEmployeeEvent New();
    }

    public interface IEmployeeEvent
    {
    }
}
