using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    
    [Table("Production.UnitMeasure", Schema = "Production")]
    [Description("Unit of measure lookup table.")]
    public class UnitMeasure
    {
        public UnitMeasure()
        {
            this.BillOfMaterials = new List<BillOfMaterials>();
            this.Products = new List<Product>();
            this.Products1 = new List<Product>();
            this.ProductVendors = new List<ProductVendor>();
        }

        [Key]
        [Column(name : "UnitMeasureCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "Unit Measure Code is required")]
        [Display(Name = "Unit Measure Code")]
        [Description("Primary key.")]
        public string UnitMeasureCode { get; set; } // nchar(3)
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Unit of measure description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.BillOfMaterials.UnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_BillOfMaterials_UnitMeasure_UnitMeasureCode)
        public IEnumerable<BillOfMaterials> BillOfMaterials { get; set; }
        // Production.Product.SizeUnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_Product_UnitMeasure_SizeUnitMeasureCode)
        [InverseProperty("UnitMeasure")]
        public IEnumerable<Product> Products { get; set; }
        // Production.Product.WeightUnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_Product_UnitMeasure_WeightUnitMeasureCode)
        [InverseProperty("UnitMeasure1")]
        public IEnumerable<Product> Products1 { get; set; }
        // Purchasing.ProductVendor.UnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_ProductVendor_UnitMeasure_UnitMeasureCode)
        public IEnumerable<ProductVendor> ProductVendors { get; set; }
    }
}
