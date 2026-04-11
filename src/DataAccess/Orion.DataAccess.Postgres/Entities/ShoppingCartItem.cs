using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.ShoppingCartItem")]
    [Description("Contains online customer orders until the order is submitted or cancelled.")]
    public class ShoppingCartItem
    {
        [Key]
        [Column(name : "ShoppingCartItemID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Shopping Cart Item ID is required")]
        [Display(Name = "Shopping Cart Item ID")]
        [Description("Primary key for ShoppingCartItem records.")]
        public int? ShoppingCartItemID { get; set; } // int
        [Column(name : "ShoppingCartID")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Shopping Cart ID is required")]
        [Display(Name = "Shopping Cart ID")]
        [Description("Shopping cart identification number.")]
        public string ShoppingCartID { get; set; } // nvarchar(50)
        [Column(name : "Quantity", TypeName = "int")]
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Description("Product quantity ordered.")]
        public int? Quantity { get; set; } // int
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product ordered. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "DateCreated")]
        [Required(ErrorMessage = "Date Created is required")]
        [Display(Name = "Date Created")]
        [Description("Date the time the record was created.")]
        public DateTime? DateCreated { get; set; } // datetime
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.ShoppingCartItem.ProductID -> Production.Product.ProductID (FK_ShoppingCartItem_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
