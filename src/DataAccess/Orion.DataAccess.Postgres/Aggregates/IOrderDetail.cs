using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IOrderDetail: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IOrderDetailFullEditDto o);
      
        int OrderId { get; set; }

        int ProductId { get; set; }

        decimal UnitPrice { get; set; }

        short Quantity { get; set; }

        Single Discount { get; set; }       
    }
}
