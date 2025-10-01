using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IPersonBusiness: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IPersonBusiness o);



       //  public Person Person { get; set; }
        string BusinessType { get; set; }
        string BusinessValue { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        
    }   
}