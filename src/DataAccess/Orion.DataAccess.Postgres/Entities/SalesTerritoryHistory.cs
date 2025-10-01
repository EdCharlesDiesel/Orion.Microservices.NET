using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{

    [Table("Sales.SalesTerritoryHistory", Schema = "Sales")]
    [Description("Sales representative transfers to other sales territories.")]
    public class SalesTerritoryHistory
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key. The sales rep.  Foreign key to SalesPerson.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "TerritoryID", TypeName = "int", Order = 3)]
        [Required(ErrorMessage = "Territory ID is required")]
        [Display(Name = "Territory ID")]
        [Description("Primary key. Territory identification number. Foreign key to SalesTerritory.SalesTerritoryID.")]
        public int? TerritoryID { get; set; } // int
        [Key]
        [Column(name : "StartDate", Order = 2)]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [Description("Primary key. Date the sales representive started work in the territory.")]
        public DateTime? StartDate { get; set; } // datetime
        [Column(name : "EndDate")]
        [Display(Name = "End Date")]
        [Description("Date the sales representative left work in the territory.")]
        public DateTime? EndDate { get; set; } // datetime
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

        // Sales.SalesTerritoryHistory.BusinessEntityID -> Sales.SalesPerson.BusinessEntityID (FK_SalesTerritoryHistory_SalesPerson_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public SalesPerson SalesPerson { get; set; }
        // Sales.SalesTerritoryHistory.TerritoryID -> Sales.SalesTerritory.TerritoryID (FK_SalesTerritoryHistory_SalesTerritory_TerritoryID)
        [ForeignKey("TerritoryID")]
        public SalesTerritory SalesTerritory { get; set; }
    }
}
