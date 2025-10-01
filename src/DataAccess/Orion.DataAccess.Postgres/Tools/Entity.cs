using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Services;

namespace Orion.DataAccess.Postgres.Tools
{
    public abstract class Entity<TK>: IEntity<TK>
        where TK: IEquatable<TK>
    {
        //FIXME I need to fix the Id I am using a hack to make sure all my test cases pass.
        
       // public virtual K Id { get; protected set; }
        public virtual TK Id { get;  set; }
        public bool IsTransient()
        {
            return Equals(Id, default(TK));
            
        }
        public override bool Equals(object obj)
        {
            return obj is Entity<TK> entity &&
              Equals(entity); 
        }

        public bool Equals(IEntity<TK> other)
        {
            if (other == null || 
                other.IsTransient() || IsTransient())
                return false;

            return Equals(Id, other.Id);
        }

        int? _requestedHashCode;
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = HashCode.Combine(Id);
                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }
        public static bool operator ==(Entity<TK> left, Entity<TK> right)
        {
            if (Equals(left, null))
                return (Equals(right, null));
            return left.Equals(right);
        }
        public static bool operator !=(Entity<TK> left, Entity<TK> right)
        {
            return !(left == right);
        }
        [NotMapped]
        public List<IEventNotification> DomainEvents { get; private set; }
        public void AddDomainEvent(IEventNotification evt)
        {
            DomainEvents ??= new List<IEventNotification>();
            DomainEvents.Add(evt);
        }
        public void RemoveDomainEvent(IEventNotification evt)
        {
            DomainEvents?.Remove(evt);
        }
    }
}
