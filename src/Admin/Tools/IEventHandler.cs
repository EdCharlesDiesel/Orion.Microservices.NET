using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Tools
{
    public interface IEventHandler
    {
    }
    public interface IEventHandler<T>: IEventHandler
    where T : IEventNotification
    {
        Task HandleAsync(T ev);
    }
}
