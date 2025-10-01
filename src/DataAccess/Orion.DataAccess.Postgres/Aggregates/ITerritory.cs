using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface ITerritory : IEntity<int>, IBaseEntity
    {
        void FullUpdate(ITerritory o);

        string TerritoryDescription { get; }

        int RegionId { get; }

        //FIXME Need to investagate

        // IRegion Region { get; }

        // IEnumerable<IEmployeeTerritory> Employees { get; }

    }
}
