using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.ProductPhoto")]
    [Description("Product images.")]
    public class ProductPhoto
    {
        public ProductPhoto()
        {
            this.ProductProductPhotos = new List<ProductProductPhoto>();
        }

        [Key]
        [Column(name : "ProductPhotoID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product Photo ID is required")]
        [Display(Name = "Product Photo ID")]
        [Description("Primary key for ProductPhoto records.")]
        public int? ProductPhotoID { get; set; } // int
        [Column(name : "ThumbNailPhoto")]
        [MaxLength]
        [Display(Name = "Thumb Nail Photo")]
        [Description("Small image of the product.")]
        public byte[] ThumbNailPhoto { get; set; } // varbinary(max)
        [Column(name : "ThumbnailPhotoFileName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Thumbnail Photo File Name")]
        [Description("Small image file name.")]
        public string ThumbnailPhotoFileName { get; set; } // nvarchar(50)
        [Column(name : "LargePhoto")]
        [MaxLength]
        [Display(Name = "Large Photo")]
        [Description("Large image of the product.")]
        public byte[] LargePhoto { get; set; } // varbinary(max)
        [Column(name : "LargePhotoFileName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Large Photo File Name")]
        [Description("Large image file name.")]
        public string LargePhotoFileName { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductProductPhoto.ProductPhotoID -> Production.ProductPhoto.ProductPhotoID (FK_ProductProductPhoto_ProductPhoto_ProductPhotoID)
        public IEnumerable<ProductProductPhoto> ProductProductPhotos { get; set; }
    }
}
