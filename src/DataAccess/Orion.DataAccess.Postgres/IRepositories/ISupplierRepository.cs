using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ISupplierRepository:IRepository<ISupplier>
    {
        Task<ISupplier> Get(int id);
        ISupplier New();
    }
}
