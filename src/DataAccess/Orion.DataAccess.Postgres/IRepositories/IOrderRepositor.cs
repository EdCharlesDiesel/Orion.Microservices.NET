using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IOrderRepository:IRepository<IOrder>
    {
        Task<IOrder> Get(int id);
        IOrder New();
        Task Delete(int orderId);
        object UnitOfWork { get; set; }
    }
}
