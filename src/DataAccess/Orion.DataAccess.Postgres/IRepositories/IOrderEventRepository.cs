using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IOrderEventRepository:IRepository<IOrderEvent>
    {
        Task<IOrderEvent> Get(int id);
        IOrderEvent New(OrderEventType deleted);
    }
}
