namespace Orion.DataAccess.Postgres.Tools
{
    public interface IEventMediator
    {
        Task TriggerEvents(IEnumerable<IEventNotification> events);
    }
}
