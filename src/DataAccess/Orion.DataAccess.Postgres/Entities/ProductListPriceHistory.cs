using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductListPriceHistory")]
    [Description("Changes in the list price of a product over time.")]
    public class ProductListPriceHistory
    {
        [Key]
        [Column(name : "ProductID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID")]
        public int? ProductID { get; set; } // int
        [Key]
        [Column(name : "StartDate", Order = 2)]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [Description("List price start date.")]
        public DateTime? StartDate { get; set; } // datetime
        [Column(name : "EndDate")]
        [Display(Name = "End Date")]
        [Description("List price end date")]
        public DateTime? EndDate { get; set; } // datetime
        [Column(name : "ListPrice", TypeName = "money")]
        [Required(ErrorMessage = "List Price is required")]
        [Display(Name = "List Price")]
        [Description("Product list price.")]
        public decimal? ListPrice { get; set; } // money
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductListPriceHistory.ProductID -> Production.Product.ProductID (FK_ProductListPriceHistory_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
