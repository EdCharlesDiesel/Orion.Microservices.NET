using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.IRepositories;
public interface ICatalogServices:IRepository<Product>
{
    Task BulkCreate(List<Product> entity); 
}

public class Product
{
    public Guid Id { get; set; }
}