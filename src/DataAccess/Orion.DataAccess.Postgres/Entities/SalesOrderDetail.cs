using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesOrderDetail")]
    [Description("Individual products associated with a specific sales order. See SalesOrderHeader.")]
    public class SalesOrderDetail
    {
        [Key]
        [Column(name : "SalesOrderID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Sales Order ID is required")]
        [Display(Name = "Sales Order ID")]
        [Description("Primary key. Foreign key to SalesOrderHeader.SalesOrderID.")]
        public int? SalesOrderID { get; set; } // int
        [Key]
        [Column(name : "SalesOrderDetailID", TypeName = "int", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Sales Order Detail ID is required")]
        [Display(Name = "Sales Order Detail ID")]
        [Description("Primary key. One incremental unique number per product sold.")]
        public int? SalesOrderDetailID { get; set; } // int
        [Column(name : "CarrierTrackingNumber")]
        [MaxLength(25)]
        [StringLength(25)]
        [Display(Name = "Carrier Tracking Number")]
        [Description("Shipment tracking number supplied by the shipper.")]
        public string CarrierTrackingNumber { get; set; } // nvarchar(25)
        [Column(name : "OrderQty", TypeName = "smallint")]
        [Required(ErrorMessage = "Order Qty is required")]
        [Display(Name = "Order Qty")]
        [Description("Quantity ordered per product.")]
        public short? OrderQty { get; set; } // smallint
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product sold to customer. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "SpecialOfferID", TypeName = "int")]
        [Required(ErrorMessage = "Special Offer ID is required")]
        [Display(Name = "Special Offer ID")]
        [Description("Promotional code. Foreign key to SpecialOffer.SpecialOfferID.")]
        public int? SpecialOfferID { get; set; } // int
        [Column(name : "UnitPrice", TypeName = "money")]
        [Required(ErrorMessage = "Unit Price is required")]
        [Display(Name = "Unit Price")]
        [Description("Selling price of a single product.")]
        public decimal? UnitPrice { get; set; } // money
        [Column(name : "UnitPriceDiscount", TypeName = "money")]
        [Required(ErrorMessage = "Unit Price Discount is required")]
        [Display(Name = "Unit Price Discount")]
        [Description("Discount amount.")]
        public decimal? UnitPriceDiscount { get; set; } // money
        [Column(name : "LineTotal", TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required(ErrorMessage = "Line Total is required")]
        [Display(Name = "Line Total")]
        [Description("Per product subtotal. Computed as UnitPrice * (1 - UnitPriceDiscount) * OrderQty.")]
        public decimal? LineTotal { get; set; } // numeric(38,6)
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

        // Sales.SalesOrderDetail.SalesOrderID -> Sales.SalesOrderHeader.SalesOrderID (FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID)
        [ForeignKey("SalesOrderID")]
        public SalesOrderHeader SalesOrderHeader { get; set; }
        // Sales.SalesOrderDetail.ProductID -> Sales.SpecialOfferProduct.ProductID (FK_SalesOrderDetail_SpecialOfferProduct_SpecialOfferIDProductID)
        [ForeignKey("ProductID")]
        public SpecialOfferProduct SpecialOfferProduct { get; set; }
        // Sales.SalesOrderDetail.SpecialOfferID -> Sales.SpecialOfferProduct.SpecialOfferID (FK_SalesOrderDetail_SpecialOfferProduct_SpecialOfferIDProductID)

    }
}
