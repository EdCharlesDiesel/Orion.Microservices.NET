using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Handlers
{
    public class CustomerDeleteEvent : IEventNotification
    {
        public object CustomerId { get; set; }
        public object OldVersion { get; set; }
    }
}