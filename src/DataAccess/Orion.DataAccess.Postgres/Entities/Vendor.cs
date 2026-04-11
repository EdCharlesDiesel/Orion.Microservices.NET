using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Purchasing.Vendor")]
    [Description("Companies from whom Adventure Works Cycles purchases parts or other goods.")]
    public class Vendor
    {
        public Vendor()
        {
            this.ProductVendors = new List<ProductVendor>();
            this.PurchaseOrderHeaders = new List<PurchaseOrderHeader>();
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for Vendor records.  Foreign key to BusinessEntity.BusinessEntityID")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "AccountNumber")]
        [MaxLength(15)]
        [StringLength(15)]
        [Required(ErrorMessage = "Account Number is required")]
        [Display(Name = "Account Number")]
        [Description("Vendor account (identification) number.")]
        public string AccountNumber { get; set; } // nvarchar(15)
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Company name.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "CreditRating")]
        [Required(ErrorMessage = "Credit Rating is required")]
        [Display(Name = "Credit Rating")]
        [Description("1 = Superior, 2 = Excellent, 3 = Above average, 4 = Average, 5 = Below average")]
        public byte? CreditRating { get; set; } // tinyint
        [Column(name : "PreferredVendorStatus", TypeName = "bit")]
        [Required(ErrorMessage = "Preferred Vendor Status is required")]
        [Display(Name = "Preferred Vendor Status")]
        [Description("0 = Do not use if another vendor is available. 1 = Preferred over other vendors supplying the same product.")]
        [NotMapped]
        public bool? PreferredVendorStatus { get; set; } // bit
        [Column(name : "ActiveFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Active Flag is required")]
        [Display(Name = "Active Flag")]
        [Description("0 = Vendor no longer used. 1 = Vendor is actively used.")]
        [NotMapped]
        public bool? ActiveFlag { get; set; } // bit
        [Column(name : "PurchasingWebServiceURL")]
        [MaxLength(1024)]
        [StringLength(1024)]
        [Display(Name = "Purchasing Web Service URL")]
        [Description("Vendor URL.")]
        public string PurchasingWebServiceURL { get; set; } // nvarchar(1024)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Purchasing.Vendor.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_Vendor_BusinessEntity_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public BusinessEntity BusinessEntity { get; set; }
        // Purchasing.ProductVendor.BusinessEntityID -> Purchasing.Vendor.BusinessEntityID (FK_ProductVendor_Vendor_BusinessEntityID)
        public IEnumerable<ProductVendor> ProductVendors { get; set; }
        // Purchasing.PurchaseOrderHeader.VendorID -> Purchasing.Vendor.BusinessEntityID (FK_PurchaseOrderHeader_Vendor_VendorID)
        public IEnumerable<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
    }
}
