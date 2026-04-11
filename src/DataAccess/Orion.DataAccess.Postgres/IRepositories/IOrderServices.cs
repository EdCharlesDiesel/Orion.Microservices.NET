using System.Threading.Tasks;

namespace Orion.Domain.IRepositories;

public interface IOrderServices
{
    // Task <List<Core.Orders.Domain.Order>> _task(List<Core.Orders.Domain.Order> entity);
    //
    // Task CreateOrders(List<Core.Orders.Domain.Order> entity);
    Task<object?> GetAllAsync();

}