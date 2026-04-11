using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IRegion: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IRegion o);

        string RegionDescription { get;}
     
    }  
}
