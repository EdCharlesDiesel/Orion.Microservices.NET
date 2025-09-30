using Orion.Admin.Models.Products;
using Orion.Admin.Tools;

namespace Orion.Admin.Queries
{
    public interface IProductsListQuery: IQuery
    {
        Task<IEnumerable<ProductInfosViewModel>> GetAllProducts();
        
    }
}
