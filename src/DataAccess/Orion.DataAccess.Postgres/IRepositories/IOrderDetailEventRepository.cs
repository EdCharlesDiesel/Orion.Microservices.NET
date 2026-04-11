using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IOrderDetailEventRepository:IRepository<IOrderDetailEvent>
    {
        Task<IOrderDetailEvent> Get(int id);
        IOrderDetailEvent New();
    }
}
