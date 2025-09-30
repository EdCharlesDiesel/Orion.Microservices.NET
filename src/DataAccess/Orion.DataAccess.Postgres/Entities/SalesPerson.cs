using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesPerson")]
    [Description("Sales representative current information.")]
    public class SalesPerson
    {
        public SalesPerson()
        {
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
            this.SalesPersonQuotaHistories = new List<SalesPersonQuotaHistory>();
            this.SalesTerritoryHistories = new List<SalesTerritoryHistory>();
            this.Stores = new List<Store>();
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for SalesPerson records. Foreign key to Employee.BusinessEntityID")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "TerritoryID", TypeName = "int")]
        [Display(Name = "Territory ID")]
        [Description("Territory currently assigned to. Foreign key to SalesTerritory.SalesTerritoryID.")]
        public int? TerritoryID { get; set; } // int
        [Column(name : "SalesQuota", TypeName = "money")]
        [Display(Name = "Sales Quota")]
        [Description("Projected yearly sales.")]
        public decimal? SalesQuota { get; set; } // money
        [Column(name : "Bonus", TypeName = "money")]
        [Required(ErrorMessage = "Bonus is required")]
        [Display(Name = "Bonus")]
        [Description("Bonus due if quota is met.")]
        public decimal? Bonus { get; set; } // money
        [Column(name : "CommissionPct")]
        [Required(ErrorMessage = "Commission Pct is required")]
        [Display(Name = "Commission Pct")]
        [Description("Commision percent received per sale.")]
        public decimal? CommissionPct { get; set; } // smallmoney
        [Column(name : "SalesYTD", TypeName = "money")]
        [Required(ErrorMessage = "Sales YTD is required")]
        [Display(Name = "Sales YTD")]
        [Description("Sales total year to date.")]
        public decimal? SalesYTD { get; set; } // money
        [Column(name : "SalesLastYear", TypeName = "money")]
        [Required(ErrorMessage = "Sales Last Year is required")]
        [Display(Name = "Sales Last Year")]
        [Description("Sales total of previous year.")]
        public decimal? SalesLastYear { get; set; } // money
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

        // Sales.SalesPerson.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_SalesPerson_Employee_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
        // Sales.SalesPerson.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_SalesPerson_SalesTerritory_TerritoryID)
        [ForeignKey("TerritoryID")]
        public SalesTerritory SalesTerritory { get; set; }
        // Sales.SalesOrderHeader.SalesPersonID -> Sales.SalesPerson.BusinessEntityID (FK_SalesOrderHeader_SalesPerson_SalesPersonID)
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
        // Sales.SalesPersonQuotaHistory.BusinessEntityID -> Sales.SalesPerson.BusinessEntityID (FK_SalesPersonQuotaHistory_SalesPerson_BusinessEntityID)
        public IEnumerable<SalesPersonQuotaHistory> SalesPersonQuotaHistories { get; set; }
        // Sales.SalesTerritoryHistory.BusinessEntityID -> Sales.SalesPerson.BusinessEntityID (FK_SalesTerritoryHistory_SalesPerson_BusinessEntityID)
        public IEnumerable<SalesTerritoryHistory> SalesTerritoryHistories { get; set; }
        // Sales.Store.SalesPersonID -> Sales.SalesPerson.BusinessEntityID (FK_Store_SalesPerson_SalesPersonID)
        public IEnumerable<Store> Stores { get; set; }
    }
}
