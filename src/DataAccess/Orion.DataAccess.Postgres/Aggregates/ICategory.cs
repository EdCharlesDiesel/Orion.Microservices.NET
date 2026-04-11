using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface ICategory: IEntity<int>, IBaseEntity
    {
        void FullUpdate(ICategory o);
        string CategoryName { get; }
        string Description { get;}   
        byte[] Picture { get;}        
    }
}
