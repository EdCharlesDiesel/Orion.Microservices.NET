using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SpecialOfferProduct")]
    [Description("Cross-reference table mapping products to special offer discounts.")]
    public class SpecialOfferProduct
    {
        [Key]
        [Column(name : "SpecialOfferID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Special Offer ID is required")]
        [Display(Name = "Special Offer ID")]
        [Description("Primary key for SpecialOfferProduct records.")]
        public int? SpecialOfferID { get; set; } // int
        [Key]
        [Column(name : "ProductID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
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

        // Sales.SpecialOfferProduct.SpecialOfferID -> Sales.SpecialOffer.SpecialOfferID (FK_SpecialOfferProduct_SpecialOffer_SpecialOfferID)
        [ForeignKey("SpecialOfferID")]
        public SpecialOffer SpecialOffer { get; set; }
        // Sales.SpecialOfferProduct.ProductID -> Production.Product.ProductID (FK_SpecialOfferProduct_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        // Sales.SalesOrderDetail.SpecialOfferID -> Sales.SpecialOfferProduct.SpecialOfferID (FK_SalesOrderDetail_SpecialOfferProduct_SpecialOfferIDProductID)
        [InverseProperty("SpecialOfferProduct")]
        public IEnumerable<SalesOrderDetail> SalesOrderDetails { get; set; }
        // Sales.SalesOrderDetail.ProductID -> Sales.SpecialOfferProduct.ProductID (FK_SalesOrderDetail_SpecialOfferProduct_SpecialOfferIDProductID)

    }
}
