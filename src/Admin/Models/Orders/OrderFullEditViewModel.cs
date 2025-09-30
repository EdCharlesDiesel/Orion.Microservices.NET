using System.ComponentModel.DataAnnotations;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.DTOs;

namespace Orion.Admin.Models.Orders
{
    public class OrderFullEditViewModel(IOrder o) : IOrderFullEditDto
    {
        public int Id { get; set; } = o.Id;

        [MaxLength(5)]
        public int CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? OrderDate { get; set; } = o.OrderDate;

        public DateTime? RequiredDate { get; set; } = o.RequiredDate;

        public DateTime? ShippedDate { get; set; } = o.ShippedDate;

        public int? ShipVia { get; set; } = o.ShipVia;

        public decimal? Freight { get; set; } = o.Freight;

        [MaxLength(40)]
        public string ShipName { get; set; } = o.ShipName;

        [MaxLength(60)]
        public string ShipAddress { get; set; } = o.ShipAddress;

        [MaxLength(15)]
        public string ShipCity { get; set; } = o.ShipCity;

        [MaxLength(15)]
        public string ShipRegion { get; set; } = o.ShipRegion;

        [MaxLength(10)]
        public string ShipPostalCode { get; set; } = o.ShipPostalCode;

        [MaxLength(15)]
        public string ShipCountry { get; set; } = o.ShipCountry;

        //[Display(Name = "Category")]
        //public int CategoryId { get; set; }

       // public string Status { get; set; }
        
    }
}
