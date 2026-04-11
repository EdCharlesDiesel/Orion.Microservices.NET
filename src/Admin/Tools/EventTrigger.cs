using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Tools
{
    public class EventTrigger<T>(IEnumerable<IEventHandler<T>> handlers)
        where T : IEventNotification
    {
        public async Task Trigger(T ev)
        {
            foreach (var handler in handlers)
                await handler.HandleAsync(ev);
        }
    }
}
