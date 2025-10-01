using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Services
{
    public interface IEventMediator
    {
        Task TriggerEvents(IEnumerable<IEventNotification> events);
    }
}
