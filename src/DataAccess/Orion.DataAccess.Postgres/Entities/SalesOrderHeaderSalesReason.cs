using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesOrderHeaderSalesReason")]
    [Description("Cross-reference table mapping sales orders to sales reason codes.")]
    public class SalesOrderHeaderSalesReason
    {
        [Key]
        [Column(name : "SalesOrderID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Sales Order ID is required")]
        [Display(Name = "Sales Order ID")]
        [Description("Primary key. Foreign key to SalesOrderHeader.SalesOrderID.")]
        public int? SalesOrderID { get; set; } // int
        [Key]
        [Column(name : "SalesReasonID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Sales Reason ID is required")]
        [Display(Name = "Sales Reason ID")]
        [Description("Primary key. Foreign key to SalesReason.SalesReasonID.")]
        public int? SalesReasonID { get; set; } // int
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.SalesOrderHeaderSalesReason.SalesOrderID -> Sales.SalesOrderHeader.SalesOrderID (FK_SalesOrderHeaderSalesReason_SalesOrderHeader_SalesOrderID)
        [ForeignKey("SalesOrderID")]
        public SalesOrderHeader SalesOrderHeader { get; set; }
        // Sales.SalesOrderHeaderSalesReason.SalesReasonID -> Sales.SalesReason.SalesReasonID (FK_SalesOrderHeaderSalesReason_SalesReason_SalesReasonID)
        [ForeignKey("SalesReasonID")]
        public SalesReason SalesReason { get; set; }
    }
}
