using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Purchasing.PurchaseOrderHeader")]
    [Description("General purchase order information. See PurchaseOrderDetail.")]
    public class PurchaseOrderHeader
    {
        public PurchaseOrderHeader()
        {
            this.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
        }

        [Key]
        [Column(name : "PurchaseOrderID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Purchase Order ID is required")]
        [Display(Name = "Purchase Order ID")]
        [Description("Primary key.")]
        public int? PurchaseOrderID { get; set; } // int
        [Column(name : "RevisionNumber")]
        [Required(ErrorMessage = "Revision Number is required")]
        [Display(Name = "Revision Number")]
        [Description("Incremental number to track changes to the purchase order over time.")]
        public byte? RevisionNumber { get; set; } // tinyint
        [Column(name : "Status")]
        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [Description("Order current status. 1 = Pending; 2 = Approved; 3 = Rejected; 4 = Complete")]
        public byte? Status { get; set; } // tinyint
        [Column(name : "EmployeeID", TypeName = "int")]
        [Required(ErrorMessage = "Employee ID is required")]
        [Display(Name = "Employee ID")]
        [Description("Employee who created the purchase order. Foreign key to Employee.BusinessEntityID.")]
        public int? EmployeeID { get; set; } // int
        [Column(name : "VendorID", TypeName = "int")]
        [Required(ErrorMessage = "Vendor ID is required")]
        [Display(Name = "Vendor ID")]
        [Description("Vendor with whom the purchase order is placed. Foreign key to Vendor.BusinessEntityID.")]
        public int? VendorID { get; set; } // int
        [Column(name : "ShipMethodID", TypeName = "int")]
        [Required(ErrorMessage = "Ship Method ID is required")]
        [Display(Name = "Ship Method ID")]
        [Description("Shipping method. Foreign key to ShipMethod.ShipMethodID.")]
        public int? ShipMethodID { get; set; } // int
        [Column(name : "OrderDate")]
        [Required(ErrorMessage = "Order Date is required")]
        [Display(Name = "Order Date")]
        [Description("Purchase order creation date.")]
        public DateTime? OrderDate { get; set; } // datetime
        [Column(name : "ShipDate")]
        [Display(Name = "Ship Date")]
        [Description("Estimated shipment date from the vendor.")]
        public DateTime? ShipDate { get; set; } // datetime
        [Column(name : "SubTotal", TypeName = "money")]
        [Required(ErrorMessage = "Sub Total is required")]
        [Display(Name = "Sub Total")]
        [Description("Purchase order subtotal. Computed as SUM(PurchaseOrderDetail.LineTotal)for the appropriate PurchaseOrderID.")]
        public decimal? SubTotal { get; set; } // money
        [Column(name : "TaxAmt", TypeName = "money")]
        [Required(ErrorMessage = "Tax Amt is required")]
        [Display(Name = "Tax Amt")]
        [Description("Tax amount.")]
        public decimal? TaxAmt { get; set; } // money
        [Column(name : "Freight", TypeName = "money")]
        [Required(ErrorMessage = "Freight is required")]
        [Display(Name = "Freight")]
        [Description("Shipping cost.")]
        public decimal? Freight { get; set; } // money
        [Column(name : "TotalDue", TypeName = "money")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required(ErrorMessage = "Total Due is required")]
        [Display(Name = "Total Due")]
        [Description("Total due to vendor. Computed as Subtotal + TaxAmt + Freight.")]
        public decimal? TotalDue { get; set; } // money
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Purchasing.PurchaseOrderHeader.EmployeeID -> HumanResources.Employee.BusinessEntityID (FK_PurchaseOrderHeader_Employee_EmployeeID)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
        // Purchasing.PurchaseOrderHeader.VendorID -> Purchasing.Vendor.BusinessEntityID (FK_PurchaseOrderHeader_Vendor_VendorID)
        [ForeignKey("BusinessEntityID")]
        public Vendor Vendor { get; set; }
        // Purchasing.PurchaseOrderHeader.ShipMethodID -> Purchasing.ShipMethod.ShipMethodID (FK_PurchaseOrderHeader_ShipMethod_ShipMethodID)
        [ForeignKey("ShipMethodID")]
        public ShipMethod ShippedBy { get; set; }
        // Purchasing.PurchaseOrderDetail.PurchaseOrderID -> Purchasing.PurchaseOrderHeader.PurchaseOrderID (FK_PurchaseOrderDetail_PurchaseOrderHeader_PurchaseOrderID)
        public IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
