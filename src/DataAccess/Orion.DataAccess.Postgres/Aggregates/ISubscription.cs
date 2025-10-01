using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface ISubscription: IEntity<int>, IBaseEntity
    {
        void FullUpdate(ISubscription o);
        string Username { get; }
        
        string SubscriptionLevel { get; }
    }
}
