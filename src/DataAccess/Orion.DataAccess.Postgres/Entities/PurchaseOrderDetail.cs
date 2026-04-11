using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Purchasing.PurchaseOrderDetail")]
    [Description("Individual products associated with a specific purchase order. See PurchaseOrderHeader.")]
    public class PurchaseOrderDetail
    {
        [Key]
        [Column(name : "PurchaseOrderID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Purchase Order ID is required")]
        [Display(Name = "Purchase Order ID")]
        [Description("Primary key. Foreign key to PurchaseOrderHeader.PurchaseOrderID.")]
        public int? PurchaseOrderID { get; set; } // int
        [Key]
        [Column(name : "PurchaseOrderDetailID", TypeName = "int", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Purchase Order Detail ID is required")]
        [Display(Name = "Purchase Order Detail ID")]
        [Description("Primary key. One line number per purchased product.")]
        public int? PurchaseOrderDetailID { get; set; } // int
        [Column(name : "DueDate")]
        [Required(ErrorMessage = "Due Date is required")]
        [Display(Name = "Due Date")]
        [Description("Date the product is expected to be received.")]
        public DateTime? DueDate { get; set; } // datetime
        [Column(name : "OrderQty", TypeName = "smallint")]
        [Required(ErrorMessage = "Order Qty is required")]
        [Display(Name = "Order Qty")]
        [Description("Quantity ordered.")]
        public short? OrderQty { get; set; } // smallint
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "UnitPrice", TypeName = "money")]
        [Required(ErrorMessage = "Unit Price is required")]
        [Display(Name = "Unit Price")]
        [Description("Vendor's selling price of a single product.")]
        public decimal? UnitPrice { get; set; } // money
        [Column(name : "LineTotal", TypeName = "money")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required(ErrorMessage = "Line Total is required")]
        [Display(Name = "Line Total")]
        [Description("Per product subtotal. Computed as OrderQty * UnitPrice.")]
        public decimal? LineTotal { get; set; } // money
        [Column(name : "ReceivedQty", TypeName = "decimal")]
        [Required(ErrorMessage = "Received Qty is required")]
        [Display(Name = "Received Qty")]
        [Description("Quantity actually received from the vendor.")]
        public decimal? ReceivedQty { get; set; } // decimal(8,2)
        [Column(name : "RejectedQty", TypeName = "decimal")]
        [Required(ErrorMessage = "Rejected Qty is required")]
        [Display(Name = "Rejected Qty")]
        [Description("Quantity rejected during inspection.")]
        public decimal? RejectedQty { get; set; } // decimal(8,2)
        [Column(name : "StockedQty", TypeName = "decimal")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required(ErrorMessage = "Stocked Qty is required")]
        [Display(Name = "Stocked Qty")]
        [Description("Quantity accepted into inventory. Computed as ReceivedQty - RejectedQty.")]
        public decimal? StockedQty { get; set; } // decimal(9,2)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Purchasing.PurchaseOrderDetail.PurchaseOrderID -> Purchasing.PurchaseOrderHeader.PurchaseOrderID (FK_PurchaseOrderDetail_PurchaseOrderHeader_PurchaseOrderID)
        [ForeignKey("PurchaseOrderID")]
        public PurchaseOrderHeader PurchaseOrderHeader { get; set; }
        // Purchasing.PurchaseOrderDetail.ProductID -> Production.Product.ProductID (FK_PurchaseOrderDetail_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
