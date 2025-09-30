using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.CountryRegion")]
    [Description("Lookup table containing the ISO standard codes for countries and regions.")]
    public class CountryRegion
    {
        public CountryRegion()
        {
            this.StateProvinces = new List<StateProvince>();
            this.CountryRegionCurrencies = new List<CountryRegionCurrency>();
            this.SalesTerritories = new List<SalesTerritory>();
        }

        
        
        [Key]
        [Column(name : "CountryRegionCode")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Country Region Code is required")]
        [Display(Name = "Country Region Code")]
        [Description("ISO standard code for countries and regions.")]
        public string CountryRegionCode { get; set; } // nvarchar(3)
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Country or region name.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Person.StateProvince.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_StateProvince_CountryRegion_CountryRegionCode)
        public IEnumerable<StateProvince> StateProvinces { get; set; }
        // Sales.CountryRegionCurrency.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_CountryRegionCurrency_CountryRegion_CountryRegionCode)
        public IEnumerable<CountryRegionCurrency> CountryRegionCurrencies { get; set; }
        // Sales.SalesTerritory.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_SalesTerritory_CountryRegion_CountryRegionCode)
        public IEnumerable<SalesTerritory> SalesTerritories { get; set; }
    }
}
