using System.Threading.Tasks;

namespace Orion.Domain.IRepositories
{
    public interface ICustomerCustomerDemoEvent:IRepository<ICustomerCustomerDemoEvent>
    {
        Task<ICustomerCustomerDemoEvent> Get(int id);
        ICustomerCustomerDemoEvent New();
    }
}
