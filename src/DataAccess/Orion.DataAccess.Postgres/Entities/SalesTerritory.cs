using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesTerritory")]
    [Table("Person.AddressType", Schema = "Person")]
    [Description("Sales territory lookup table.")]
    public class SalesTerritory
    {
        public SalesTerritory()
        {
            this.StateProvinces = new List<StateProvince>();
            this.Customers = new List<Customer>();
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
            this.SalesPeople = new List<SalesPerson>();
            this.SalesTerritoryHistories = new List<SalesTerritoryHistory>();
        }

        [Key]
        [Column(name : "TerritoryID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Territory ID is required")]
        [Display(Name = "Territory ID")]
        [Description("Primary key for SalesTerritory records.")]
        public int? TerritoryID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Sales territory description")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "CountryRegionCode")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Country Region Code is required")]
        [Display(Name = "Country Region Code")]
        [Description("ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode. ")]
        public string CountryRegionCode { get; set; } // nvarchar(3)
        [Column(name : "Group")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Group is required")]
        [Display(Name = "Group")]
        [Description("Geographic area to which the sales territory belong.")]
        public string Group { get; set; } // nvarchar(50)
        [Column(name : "SalesYTD", TypeName = "money")]
        [Required(ErrorMessage = "Sales YTD is required")]
        [Display(Name = "Sales YTD")]
        [Description("Sales in the territory year to date.")]
        public decimal? SalesYTD { get; set; } // money
        [Column(name : "SalesLastYear", TypeName = "money")]
        [Required(ErrorMessage = "Sales Last Year is required")]
        [Display(Name = "Sales Last Year")]
        [Description("Sales in the territory the previous year.")]
        public decimal? SalesLastYear { get; set; } // money
        [Column(name : "CostYTD", TypeName = "money")]
        [Required(ErrorMessage = "Cost YTD is required")]
        [Display(Name = "Cost YTD")]
        [Description("Business costs in the territory year to date.")]
        public decimal? CostYTD { get; set; } // money
        [Column(name : "CostLastYear", TypeName = "money")]
        [Required(ErrorMessage = "Cost Last Year is required")]
        [Display(Name = "Cost Last Year")]
        [Description("Business costs in the territory the previous year.")]
        public decimal? CostLastYear { get; set; } // money
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

        // Sales.SalesTerritory.CountryRegionCode -> Person.CountryRegion.CountryRegionCode (FK_SalesTerritory_CountryRegion_CountryRegionCode)
        [ForeignKey("CountryRegionCode")]
        public CountryRegion CountryRegion { get; set; }
        // Person.StateProvince.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_StateProvince_SalesTerritory_TerritoryID)
        public IEnumerable<StateProvince> StateProvinces { get; set; }
        // Sales.Customer.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_Customer_SalesTerritory_TerritoryID)
        public IEnumerable<Customer> Customers { get; set; }
        // Sales.SalesOrderHeader.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_SalesOrderHeader_SalesTerritory_TerritoryID)
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
        // Sales.SalesPerson.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_SalesPerson_SalesTerritory_TerritoryID)
        public IEnumerable<SalesPerson> SalesPeople { get; set; }
        // Sales.SalesTerritoryHistory.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_SalesTerritoryHistory_SalesTerritory_TerritoryID)
        public IEnumerable<SalesTerritoryHistory> SalesTerritoryHistories { get; set; }
    }
}
