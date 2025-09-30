using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Purchasing.ProductVendor")]
    [Description("Cross-reference table mapping vendors with the products they supply.")]
    public class ProductVendor
    {
        [Key]
        [Column(name : "ProductID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Primary key. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key. Foreign key to Vendor.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "AverageLeadTime", TypeName = "int")]
        [Required(ErrorMessage = "Average Lead Time is required")]
        [Display(Name = "Average Lead Time")]
        [Description("The average span of time (in days) between placing an order with the vendor and receiving the purchased product.")]
        public int? AverageLeadTime { get; set; } // int
        [Column(name : "StandardPrice", TypeName = "money")]
        [Required(ErrorMessage = "Standard Price is required")]
        [Display(Name = "Standard Price")]
        [Description("The vendor's usual selling price.")]
        public decimal? StandardPrice { get; set; } // money
        [Column(name : "LastReceiptCost", TypeName = "money")]
        [Display(Name = "Last Receipt Cost")]
        [Description("The selling price when last purchased.")]
        public decimal? LastReceiptCost { get; set; } // money
        [Column(name : "LastReceiptDate")]
        [Display(Name = "Last Receipt Date")]
        [Description("Date the product was last received by the vendor.")]
        public DateTime? LastReceiptDate { get; set; } // datetime
        [Column(name : "MinOrderQty", TypeName = "int")]
        [Required(ErrorMessage = "Min Order Qty is required")]
        [Display(Name = "Min Order Qty")]
        [Description("The maximum quantity that should be ordered.")]
        public int? MinOrderQty { get; set; } // int
        [Column(name : "MaxOrderQty", TypeName = "int")]
        [Required(ErrorMessage = "Max Order Qty is required")]
        [Display(Name = "Max Order Qty")]
        [Description("The minimum quantity that should be ordered.")]
        public int? MaxOrderQty { get; set; } // int
        [Column(name : "OnOrderQty", TypeName = "int")]
        [Display(Name = "On Order Qty")]
        [Description("The quantity currently on order.")]
        public int? OnOrderQty { get; set; } // int
        [Column(name : "UnitMeasureCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Unit Measure Code is required")]
        [Display(Name = "Unit Measure Code")]
        [Description("The product's unit of measure.")]
        public string UnitMeasureCode { get; set; } // nchar(3)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Purchasing.ProductVendor.ProductID -> Production.Product.ProductID (FK_ProductVendor_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        // Purchasing.ProductVendor.BusinessEntityID -> Purchasing.Vendor.BusinessEntityID (FK_ProductVendor_Vendor_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Vendor Vendor { get; set; }
        // Purchasing.ProductVendor.UnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_ProductVendor_UnitMeasure_UnitMeasureCode)
        [ForeignKey("UnitMeasureCode")]
        public UnitMeasure UnitMeasure { get; set; }
    }
}
