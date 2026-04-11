using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesPersonQuotaHistory")]
    [Description("Sales performance tracking.")]
    public class SalesPersonQuotaHistory
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Sales person identification number. Foreign key to SalesPerson.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "QuotaDate", Order = 2)]
        [Required(ErrorMessage = "Quota Date is required")]
        [Display(Name = "Quota Date")]
        [Description("Sales quota date.")]
        public DateTime? QuotaDate { get; set; } // datetime
        [Column(name : "SalesQuota", TypeName = "money")]
        [Required(ErrorMessage = "Sales Quota is required")]
        [Display(Name = "Sales Quota")]
        [Description("Sales quota amount.")]
        public decimal? SalesQuota { get; set; } // money
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

        // Sales.SalesPersonQuotaHistory.BusinessEntityID -> Sales.SalesPerson.BusinessEntityID (FK_SalesPersonQuotaHistory_SalesPerson_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public SalesPerson SalesPerson { get; set; }
    }
}
