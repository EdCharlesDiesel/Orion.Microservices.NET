using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductReview")]
    [Description("Customer reviews of products they have purchased.")]
    public class ProductReview
    {
        [Key]
        [Column(name : "ProductReviewID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product Review ID is required")]
        [Display(Name = "Product Review ID")]
        [Description("Primary key for ProductReview records.")]
        public int? ProductReviewID { get; set; } // int
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "ReviewerName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Reviewer Name is required")]
        [Display(Name = "Reviewer Name")]
        [Description("Name of the reviewer.")]
        public string ReviewerName { get; set; } // nvarchar(50)
        [Column(name : "ReviewDate")]
        [Required(ErrorMessage = "Review Date is required")]
        [Display(Name = "Review Date")]
        [Description("Date review was submitted.")]
        public DateTime? ReviewDate { get; set; } // datetime
        [Column(name : "EmailAddress")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        [Description("Reviewer's e-mail address.")]
        public string EmailAddress { get; set; } // nvarchar(50)
        [Column(name : "Rating", TypeName = "int")]
        [Required(ErrorMessage = "Rating is required")]
        [Display(Name = "Rating")]
        [Description("Product rating given by the reviewer. Scale is 1 to 5 with 5 as the highest rating.")]
        public int? Rating { get; set; } // int
        [Column(name : "Comments")]
        [MaxLength(3850)]
        [StringLength(3850)]
        [Display(Name = "Comments")]
        [Description("Reviewer's comments")]
        public string Comments { get; set; } // nvarchar(3850)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductReview.ProductID -> Production.Product.ProductID (FK_ProductReview_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
