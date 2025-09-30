using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.CountryRegionCurrency")]
    [Description("Cross-reference table mapping ISO currency codes to a country or region.")]
    public class CountryRegionCurrency
    {
        [Key]
        [Column(name : "CountryRegionCode", Order = 1)]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Country Region Code is required")]
        [Display(Name = "Country Region Code")]
        [Description("ISO code for countries and regions. Foreign key to CountryRegion.CountryRegionCode.")]
        public string CountryRegionCode { get; set; } // nvarchar(3)
        [Key]
        [Column(name : "CurrencyCode", TypeName = "nchar", Order = 2)]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Currency Code is required")]
        [Display(Name = "Currency Code")]
        [Description("ISO standard currency code. Foreign key to Currency.CurrencyCode.")]
        public string CurrencyCode { get; set; } // nchar(3)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.CountryRegionCurrency.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_CountryRegionCurrency_CountryRegion_CountryRegionCode)
        [ForeignKey("CountryRegionCode")]
        public CountryRegion CountryRegion { get; set; }
        // Sales.CountryRegionCurrency.CurrencyCode -> Sales.Currency.CurrencyCode (FK_CountryRegionCurrency_Currency_CurrencyCode)
        [ForeignKey("CurrencyCode")]
        public Currency Currency { get; set; }
    }
}
