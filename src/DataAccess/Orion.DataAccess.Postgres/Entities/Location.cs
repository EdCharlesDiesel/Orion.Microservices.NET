using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.Location")]
    [Description("Product inventory and manufacturing locations.")]
    public class Location
    {
        public Location()
        {
            this.ProductInventories = new List<ProductInventory>();
            this.WorkOrderRoutings = new List<WorkOrderRouting>();
        }

        [Key]
        [Column(name : "LocationID", TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Location ID is required")]
        [Display(Name = "Location ID")]
        [Description("Primary key for Location records.")]
        public short? LocationID { get; set; } // smallint
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Location description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "CostRate")]
        [Required(ErrorMessage = "Cost Rate is required")]
        [Display(Name = "Cost Rate")]
        [Description("Standard hourly cost of the manufacturing location.")]
        public decimal? CostRate { get; set; } // smallmoney
        [Column(name : "Availability", TypeName = "decimal")]
        [Required(ErrorMessage = "Availability is required")]
        [Display(Name = "Availability")]
        [Description("Work capacity (in hours) of the manufacturing location.")]
        public decimal? Availability { get; set; } // decimal(8,2)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductInventory.LocationID -> Production.Location.LocationID (FK_ProductInventory_Location_LocationID)
        public IEnumerable<ProductInventory> ProductInventories { get; set; }
        // Production.WorkOrderRouting.LocationID -> Production.Location.LocationID (FK_WorkOrderRouting_Location_LocationID)
        public IEnumerable<WorkOrderRouting> WorkOrderRoutings { get; set; }
    }
}
