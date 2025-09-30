using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IBaseEntity
    {
        DateTime CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
        DateTime? DeleteDate { get; set; }
        Status Status { get; set; }
    }
}
