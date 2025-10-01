using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum LogEntryEventType {Deleted}
    public interface ILogEntryEvent: IEntity<long>, IBaseEntity
    {
        LogEntryEventType Type { get; }
        int LogEntryId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
