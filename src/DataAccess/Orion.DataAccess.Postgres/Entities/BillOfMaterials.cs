using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    
    [Table("Production.BillOfMaterials")]
    [Description("Items required to make bicycles and bicycle subassemblies. It identifies the heirarchical relationship between a parent product and its components.")]
    public class BillOfMaterials
    {
        [Key]
        [Column(name : "BillOfMaterialsID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Bill Of Materials ID is required")]
        [Display(Name = "Bill Of Materials ID")]
        [Description("Primary key for BillOfMaterials records.")]
        public int? BillOfMaterialsId { get; set; } // int
        [Column(name : "ProductAssemblyID", TypeName = "int")]
        [Display(Name = "Product Assembly ID")]
        [Description("Parent product identification number. Foreign key to Product.ProductID.")]
        public int? ProductAssemblyId { get; set; } // int
        [Column(name : "ComponentID", TypeName = "int")]
        [Required(ErrorMessage = "Component ID is required")]
        [Display(Name = "Component ID")]
        [Description("Component identification number. Foreign key to Product.ProductID.")]
        public int? ComponentId { get; set; } // int
        [Column(name : "StartDate")]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [Description("Date the component started being used in the assembly item.")]
        public DateTime? StartDate { get; set; } // datetime
        [Column(name : "EndDate")]
        [Display(Name = "End Date")]
        [Description("Date the component stopped being used in the assembly item.")]
        public DateTime? EndDate { get; set; } // datetime
        [Column(name : "UnitMeasureCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Unit Measure Code is required")]
        [Display(Name = "Unit Measure Code")]
        [Description("Standard code identifying the unit of measure for the quantity.")]
        public string UnitMeasureCode { get; set; } // nchar(3)
        [Column(name : "BOMLevel", TypeName = "smallint")]
        [Required(ErrorMessage = "BOM Level is required")]
        [Display(Name = "BOM Level")]
        [Description("Indicates the depth the component is from its parent (AssemblyID).")]
        public short? BomLevel { get; set; } // smallint
        [Column(name : "PerAssemblyQty", TypeName = "decimal")]
        [Required(ErrorMessage = "Per Assembly Qty is required")]
        [Display(Name = "Per Assembly Qty")]
        [Description("Quantity of the component needed to create the assembly.")]
        public decimal? PerAssemblyQty { get; set; } // decimal(8,2)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.BillOfMaterials.ProductAssemblyID -> Production.Product.ProductID (FK_BillOfMaterials_Product_ProductAssemblyID)
        [ForeignKey("ProductAssemblyID")]
        public Product Product { get; set; }
        // Production.BillOfMaterials.ComponentID -> Production.Product.ProductID (FK_BillOfMaterials_Product_ComponentID)
        [ForeignKey("ComponentID")]
        public Product Product1 { get; set; }
        // Production.BillOfMaterials.UnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_BillOfMaterials_UnitMeasure_UnitMeasureCode)
        [ForeignKey("UnitMeasureCode")]
        public UnitMeasure UnitMeasure { get; set; }
    }
}
