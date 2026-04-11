using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductDescription")]
    [Description("Product descriptions in several languages.")]
    public class ProductDescription
    {
        public ProductDescription()
        {
            this.ProductModelProductDescriptionCultures = new List<ProductModelProductDescriptionCulture>();
        }

        [Key]
        [Column(name : "ProductDescriptionID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product Description ID is required")]
        [Display(Name = "Product Description ID")]
        [Description("Primary key for ProductDescription records.")]
        public int? ProductDescriptionID { get; set; } // int
        [Column(name : "Description")]
        [MaxLength(400)]
        [StringLength(400)]
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [Description("Description of the product.")]
        public string Description { get; set; } // nvarchar(400)
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

        // Production.ProductModelProductDescriptionCulture.ProductDescriptionID -> Production.ProductDescription.ProductDescriptionID (FK_ProductModelProductDescriptionCulture_ProductDescription_ProductDescriptionID)
        public IEnumerable<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
    }
}
