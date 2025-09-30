using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesTaxRate")]
    [Description("Tax rate lookup table.")]
    public class SalesTaxRate
    {
        [Key]
        [Column(name : "SalesTaxRateID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Sales Tax Rate ID is required")]
        [Display(Name = "Sales Tax Rate ID")]
        [Description("Primary key for SalesTaxRate records.")]
        public int? SalesTaxRateID { get; set; } // int
        [Column(name : "StateProvinceID", TypeName = "int")]
        [Required(ErrorMessage = "State Province ID is required")]
        [Display(Name = "State Province ID")]
        [Description("State, province, or country/region the sales tax applies to.")]
        public int? StateProvinceID { get; set; } // int
        [Column(name : "TaxType")]
        [Required(ErrorMessage = "Tax Type is required")]
        [Display(Name = "Tax Type")]
        [Description("1 = Tax applied to retail transactions, 2 = Tax applied to wholesale transactions, 3 = Tax applied to all sales (retail and wholesale) transactions.")]
        public byte? TaxType { get; set; } // tinyint
        [Column(name : "TaxRate")]
        [Required(ErrorMessage = "Tax Rate is required")]
        [Display(Name = "Tax Rate")]
        [Description("Tax rate amount.")]
        public decimal? TaxRate { get; set; } // smallmoney
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Tax rate description.")]
        public string Name { get; set; } // nvarchar(50)
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

        // Sales.SalesTaxRate.StateProvinceID -> Person.StateProvince.StateProvinceID (FK_SalesTaxRate_StateProvince_StateProvinceID)
        [ForeignKey("StateProvinceID")]
        public StateProvince StateProvince { get; set; }
    }
}
