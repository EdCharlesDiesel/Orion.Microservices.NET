using Orion.Admin.Models.Customers;
using Orion.Admin.Tools;

namespace Orion.Admin.Queries
{
    public interface ICustomersListQuery: IQuery
    {
        Task<IEnumerable<CustomerInfosViewModel>> GetAllCustomers();
        
    }
}
