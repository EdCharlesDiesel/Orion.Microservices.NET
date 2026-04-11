using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductInventory")]
    [Description("Product inventory information.")]
    public class ProductInventory
    {
        [Key]
        [Column(name : "ProductID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Key]
        [Column(name : "LocationID", TypeName = "smallint", Order = 2)]
        [Required(ErrorMessage = "Location ID is required")]
        [Display(Name = "Location ID")]
        [Description("Inventory location identification number. Foreign key to Location.LocationID. ")]
        public short? LocationID { get; set; } // smallint
        [Column(name : "Shelf")]
        [MaxLength(10)]
        [StringLength(10)]
        [Required(ErrorMessage = "Shelf is required")]
        [Display(Name = "Shelf")]
        [Description("Storage compartment within an inventory location.")]
        public string Shelf { get; set; } // nvarchar(10)
        [Column(name : "Bin")]
        [Required(ErrorMessage = "Bin is required")]
        [Display(Name = "Bin")]
        [Description("Storage container on a shelf in an inventory location.")]
        public byte? Bin { get; set; } // tinyint
        [Column(name : "Quantity", TypeName = "smallint")]
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Description("Quantity of products in the inventory location.")]
        public short? Quantity { get; set; } // smallint
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

        // Production.ProductInventory.ProductID -> Production.Product.ProductID (FK_ProductInventory_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        // Production.ProductInventory.LocationID -> Production.Location.LocationID (FK_ProductInventory_Location_LocationID)
        [ForeignKey("LocationID")]
        public Location Location { get; set; }
    }
}
