using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IBasketServices:IRepository<Basket>
{
    Task<List<Basket>?> BulkCreate(List<Basket>  baskets);
}

public class Basket
{
}