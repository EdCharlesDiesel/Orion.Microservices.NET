using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Aggregator.Models;

namespace Orion.Aggregator.Services;

public interface IRevenueStreamAggregatorService
{
    Task<IEnumerable<CatalogModel>> GetCatalog();
    Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
    Task<CatalogModel> GetCatalog(string id);       
    
}