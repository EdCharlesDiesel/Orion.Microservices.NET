using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    //FIXME MAke sure you investigate EmployeeID
    public interface IOrder: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IOrderFullEditDto o);

        int CustomerId { get; set; }

        int? EmployeeId { get; set; }

        DateTime? OrderDate { get; set; }

        DateTime? RequiredDate { get; set; }

        DateTime? ShippedDate { get; set; }

        int? ShipVia { get; set; }

        decimal? Freight { get; set; }

        string ShipName { get; set; }

        string ShipAddress { get; set; }

        string ShipCity { get; set; }

        string ShipRegion { get; set; }

        string ShipPostalCode { get; set; }

        string ShipCountry { get; set; }

        // Orion.DataAccess.Models.Customer Customer { get; set; }

        // Employee Employee { get; set; }

        // Shipper ShippedBy { get; set; }

        // IEnumerable<OrderDetail> OrderDetails { get; set; }     
    }
}
