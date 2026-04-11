using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductModel")]
    [Description("Product model classification.")]
    public class ProductModel
    {
        public ProductModel()
        {
            this.Products = new List<Product>();
            this.ProductModelIllustrations = new List<ProductModelIllustration>();
            this.ProductModelProductDescriptionCultures = new List<ProductModelProductDescriptionCulture>();
        }

        [Key]
        [Column(name : "ProductModelID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product Model ID is required")]
        [Display(Name = "Product Model ID")]
        [Description("Primary key for ProductModel records.")]
        public int? ProductModelID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Product model description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "CatalogDescription", TypeName = "xml")]
        [Display(Name = "Catalog Description")]
        [Description("Detailed product catalog information in xml format.")]
        public string CatalogDescription { get; set; } // XML(.)
        [Column(name : "Instructions", TypeName = "xml")]
        [Display(Name = "Instructions")]
        [Description("Manufacturing instructions in xml format.")]
        public string Instructions { get; set; } // XML(.)
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

        // Production.Product.ProductModelID -> Production.ProductModel.ProductModelID (FK_Product_ProductModel_ProductModelID)
        public IEnumerable<Product> Products { get; set; }
        // Production.ProductModelIllustration.ProductModelID -> Production.ProductModel.ProductModelID (FK_ProductModelIllustration_ProductModel_ProductModelID)
        public IEnumerable<ProductModelIllustration> ProductModelIllustrations { get; set; }
        // Production.ProductModelProductDescriptionCulture.ProductModelID -> Production.ProductModel.ProductModelID (FK_ProductModelProductDescriptionCulture_ProductModel_ProductModelID)
        public IEnumerable<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
    }
}
