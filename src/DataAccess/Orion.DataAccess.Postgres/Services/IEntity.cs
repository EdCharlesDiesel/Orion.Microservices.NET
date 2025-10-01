using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Services
{
    public interface IEntity<out TK>
        where TK : IEquatable<TK>
    {
        TK Id { get; }
        bool IsTransient();
        List<IEventNotification> DomainEvents { get; }
        void AddDomainEvent(IEventNotification evt);
        void RemoveDomainEvent(IEventNotification evt);
        
    }
}
