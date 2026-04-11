using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductCategory")]
    [Description("High-level product categorization.")]
    public class ProductCategory
    {
        public ProductCategory()
        {
            this.ProductSubcategories = new List<ProductSubcategory>();
        }

        [Key]
        [Column(name : "ProductCategoryID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product Category ID is required")]
        [Display(Name = "Product Category ID")]
        [Description("Primary key for ProductCategory records.")]
        public int? ProductCategoryID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Category description.")]
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

        // Production.ProductSubcategory.ProductCategoryID -> Production.ProductCategory.ProductCategoryID (FK_ProductSubcategory_ProductCategory_ProductCategoryID)
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
    }
}
