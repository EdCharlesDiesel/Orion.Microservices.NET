using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IProductRepository: IRepository<IProduct>
    {
        Task<IProduct> Get(int id);
        IProduct New();
        Task<IProduct> Delete(int id);
    }
}
