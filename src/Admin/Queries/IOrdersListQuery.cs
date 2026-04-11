using Orion.Admin.Models.Orders;
using Orion.Admin.Tools;

namespace Orion.Admin.Queries
{
    public interface IOrdersListQuery: IQuery
    {
        Task<IEnumerable<OrderInfosViewModel>> GetAllOrders();        
    }
}
