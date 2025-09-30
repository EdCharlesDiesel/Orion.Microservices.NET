using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities.Common
{
    public class CategoryEvent: Entity<long>, ICategoryEvent
    {
        public CategoryEventType Type { get; set; }

        public int CategoryId { get; set; }

        public long? OldVersion { get; set; }

        public long? NewVersion { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? DeleteDate { get; set; }
        
        public Status Status { get; set; }

      //  int IEntity<int>.Id;
    }

    public class CategoryEventType
    {
    }

    public interface ICategoryEvent
    {
    }
}

