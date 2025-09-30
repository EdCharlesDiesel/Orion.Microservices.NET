using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Purchasing.ShipMethod")]
    [Description("Shipping company lookup table.")]
    public class ShipMethod
    {
        public ShipMethod()
        {
            this.PurchaseOrderHeaders = new List<PurchaseOrderHeader>();
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
        }

        [Key]
        [Column(name : "ShipMethodID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Ship Method ID is required")]
        [Display(Name = "Ship Method ID")]
        [Description("Primary key for ShipMethod records.")]
        public int? ShipMethodID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Shipping company name.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ShipBase", TypeName = "money")]
        [Required(ErrorMessage = "Ship Base is required")]
        [Display(Name = "Ship Base")]
        [Description("Minimum shipping charge.")]
        public decimal? ShipBase { get; set; } // money
        [Column(name : "ShipRate", TypeName = "money")]
        [Required(ErrorMessage = "Ship Rate is required")]
        [Display(Name = "Ship Rate")]
        [Description("Shipping charge per pound.")]
        public decimal? ShipRate { get; set; } // money
        [Column(name : "rowguid")]
        [Required(ErrorMessage = "rowguid is required")]
        [Display(Name = "rowguid")]
        [Description("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")]
        public Guid? rowguid { get; set; } // uniqueidentifier
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Purchasing.PurchaseOrderHeader.ShipMethodID -> Purchasing.ShipMethod.ShipMethodID (FK_PurchaseOrderHeader_ShipMethod_ShipMethodID)
        public IEnumerable<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        // Sales.SalesOrderHeader.ShipMethodID -> Purchasing.ShipMethod.ShipMethodID (FK_SalesOrderHeader_ShipMethod_ShipMethodID)
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
