using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IOrderDetailRepository:IRepository<IOrderDetail>
    {
        Task<IOrderDetail> Get(int id);
        IOrderDetail New();
    }
}
