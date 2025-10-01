using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface ITerm: IEntity<int>, IBaseEntity
    {
        void FullUpdate(ITermFullEditDto o);   
     
        bool IsDeleted { get;}

        string Role { get; }

        DateTime StartOfTerm { get;  }
        
        DateTime EndOfTerm { get; set; }
        
        int NumberOfTerms { get; set; }

        int BusinessOwnerId { get; }
   
    }
}
