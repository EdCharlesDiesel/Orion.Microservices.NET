using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IFeature: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IFeature o);
  
        string FeatureName { get; set; }

        bool IsEnabled { get; set; }

        string Username { get; set; }       
    }
}
