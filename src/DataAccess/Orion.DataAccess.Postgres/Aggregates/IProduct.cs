using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IProduct: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IProductFullEditDto o);

        string ProductName { get;}

        string Description { get;}

        decimal UnitPrice { get;} 

        string Image { get;} 


        byte[] CoverImage {get;}

        int DurationInDays { get; }

        DateTime? StartValidityDate { get;}

        DateTime? EndValidityDate { get; }

        int CategoryId { get; }
        
    }   
}

 