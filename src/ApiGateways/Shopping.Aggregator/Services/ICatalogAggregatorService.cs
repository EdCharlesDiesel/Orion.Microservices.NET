using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Shopping.Aggregator.Models;

namespace Orion.Shopping.Aggregator.Services
{
    public interface ICatalogAggregatorService
    {
        Task<IEnumerable<CatalogModel>> GetCatalog();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
        Task<CatalogModel> GetCatalog(string id);        
    }
}
