using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
  
    [Table("Person.StateProvince", Schema = "Person")]
    [Description("State and province lookup table.")]
    public class StateProvince
    {
        public StateProvince()
        {
            this.Addresses = new List<Address>();
            this.SalesTaxRates = new List<SalesTaxRate>();
        }

        [Key]
        [Column(name : "StateProvinceID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "State Province ID is required")]
        [Display(Name = "State Province ID")]
        [Description("Primary key for StateProvince records.")]
        public int? StateProvinceID { get; set; } // int
        [Column(name : "StateProvinceCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "State Province Code is required")]
        [Display(Name = "State Province Code")]
        [Description("ISO standard state or province code.")]
        public string StateProvinceCode { get; set; } // nchar(3)
        [Column(name : "CountryRegionCode")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Country Region Code is required")]
        [Display(Name = "Country Region Code")]
        [Description("ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode. ")]
        public string CountryRegionCode { get; set; } // nvarchar(3)
        [Column(name : "IsOnlyStateProvinceFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Is Only State Province Flag is required")]
        [Display(Name = "Is Only State Province Flag")]
        [Description("0 = StateProvinceCode exists. 1 = StateProvinceCode unavailable, using CountryRegionCode.")]
        [NotMapped]
        public bool? IsOnlyStateProvinceFlag { get; set; } // bit
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("State or province description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "TerritoryID", TypeName = "int")]
        [Required(ErrorMessage = "Territory ID is required")]
        [Display(Name = "Territory ID")]
        [Description("ID of the territory in which the state or province is located. Foreign key to SalesTerritory.SalesTerritoryID.")]
        public int? TerritoryID { get; set; } // int
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

        // Person.StateProvince.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_StateProvince_CountryRegion_CountryRegionCode)
        [ForeignKey("CountryRegionCode")]
        public CountryRegion CountryRegion { get; set; }
        // Person.StateProvince.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_StateProvince_SalesTerritory_TerritoryID)
        [ForeignKey("TerritoryID")]
        public SalesTerritory SalesTerritory { get; set; }
        // Person.Address.StateProvinceID -> Person.StateProvince.StateProvinceID (FK_Address_StateProvince_StateProvinceID)
        public IEnumerable<Address> Addresses { get; set; }
        // Sales.SalesTaxRate.StateProvinceID -> Person.StateProvince.StateProvinceID (FK_SalesTaxRate_StateProvince_StateProvinceID)
        public IEnumerable<SalesTaxRate> SalesTaxRates { get; set; }
    }
}
