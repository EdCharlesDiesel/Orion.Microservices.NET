using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Tools
{
    public class EventMediator(IServiceProvider services) : IEventMediator
    {
        public async Task TriggerEvents(IEnumerable<IEventNotification> events)
        {
            if (events == null) return;
            foreach(var ev in events)
            {
                var triggerType = typeof(EventTrigger<>).MakeGenericType(ev.GetType());
                var trigger = services.GetService(triggerType);
                var task = (Task)triggerType.GetMethod(nameof(EventTrigger<IEventNotification>.Trigger))
                    .Invoke(trigger, new object[] { ev });
                await task.ConfigureAwait(false);

            }
        }
    }
}
