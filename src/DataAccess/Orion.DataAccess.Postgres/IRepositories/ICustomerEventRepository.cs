namespace Orion.Domain.IRepositories
{
    public interface ICustomerEventRepository
    {
        void New(CustomerEventType deleted, object evCustomerId, object evOldVersion);
    }

    public class CustomerEventType
    {
        public static CustomerEventType Deleted { get; set; }
    }
}