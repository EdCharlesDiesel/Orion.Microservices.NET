using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ISupplierEventRepository:IRepository<ISupplierEvent>
    {
        Task<ISupplierEvent> Get(int id);
        ISupplierEvent New();
    }
}
