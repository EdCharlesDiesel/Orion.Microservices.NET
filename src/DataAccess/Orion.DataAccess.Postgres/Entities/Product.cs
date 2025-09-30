using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.Product")]
    [Description("Products sold or used in the manfacturing of sold products.")]
    public class Product
    {
        public Product()
        {
            this.BillOfMaterials = new List<BillOfMaterials>();
            this.BillOfMaterials1 = new List<BillOfMaterials>();
            this.ProductCostHistories = new List<ProductCostHistory>();
            this.ProductDocuments = new List<ProductDocument>();
            this.ProductInventories = new List<ProductInventory>();
            this.ProductListPriceHistories = new List<ProductListPriceHistory>();
            this.ProductProductPhotos = new List<ProductProductPhoto>();
            this.ProductReviews = new List<ProductReview>();
            this.TransactionHistories = new List<TransactionHistory>();
            this.WorkOrders = new List<WorkOrder>();
            this.ProductVendors = new List<ProductVendor>();
            this.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            this.ShoppingCartItems = new List<ShoppingCartItem>();
            this.SpecialOfferProducts = new List<SpecialOfferProduct>();
        }

        [Key]
        [Column(name : "ProductID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Primary key for Product records.")]
        public int? ProductID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Name of the product.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ProductNumber")]
        [MaxLength(25)]
        [StringLength(25)]
        [Required(ErrorMessage = "Product Number is required")]
        [Display(Name = "Product Number")]
        [Description("Unique product identification number.")]
        public string ProductNumber { get; set; } // nvarchar(25)
        [Column(name : "MakeFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Make Flag is required")]
        [Display(Name = "Make Flag")]
        [Description("0 = Product is purchased, 1 = Product is manufactured in-house.")]
        [NotMapped]
        public bool? MakeFlag { get; set; } // bit
        [Column(name : "FinishedGoodsFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Finished Goods Flag is required")]
        [Display(Name = "Finished Goods Flag")]
        [Description("0 = Product is not a salable item. 1 = Product is salable.")]
        [NotMapped]
        public bool? FinishedGoodsFlag { get; set; } // bit
        [Column(name : "Color")]
        [MaxLength(15)]
        [StringLength(15)]
        [Display(Name = "Color")]
        [Description("Product color.")]
        public string Color { get; set; } // nvarchar(15)
        [Column(name : "SafetyStockLevel", TypeName = "smallint")]
        [Required(ErrorMessage = "Safety Stock Level is required")]
        [Display(Name = "Safety Stock Level")]
        [Description("Minimum inventory quantity. ")]
        public short? SafetyStockLevel { get; set; } // smallint
        [Column(name : "ReorderPoint", TypeName = "smallint")]
        [Required(ErrorMessage = "Reorder Point is required")]
        [Display(Name = "Reorder Point")]
        [Description("Inventory level that triggers a purchase order or work order. ")]
        public short? ReorderPoint { get; set; } // smallint
        [Column(name : "StandardCost", TypeName = "money")]
        [Required(ErrorMessage = "Standard Cost is required")]
        [Display(Name = "Standard Cost")]
        [Description("Standard cost of the product.")]
        public decimal? StandardCost { get; set; } // money
        [Column(name : "ListPrice", TypeName = "money")]
        [Required(ErrorMessage = "List Price is required")]
        [Display(Name = "List Price")]
        [Description("Selling price.")]
        public decimal? ListPrice { get; set; } // money
        [Column(name : "Size")]
        [MaxLength(5)]
        [StringLength(5)]
        [Display(Name = "Size")]
        [Description("Product size.")]
        public string Size { get; set; } // nvarchar(5)
        [Column(name : "SizeUnitMeasureCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Display(Name = "Size Unit Measure Code")]
        [Description("Unit of measure for Size column.")]
        public string SizeUnitMeasureCode { get; set; } // nchar(3)
        [Column(name : "WeightUnitMeasureCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Display(Name = "Weight Unit Measure Code")]
        [Description("Unit of measure for Weight column.")]
        public string WeightUnitMeasureCode { get; set; } // nchar(3)
        [Column(name : "Weight", TypeName = "decimal")]
        [Display(Name = "Weight")]
        [Description("Product weight.")]
        public decimal? Weight { get; set; } // decimal(8,2)
        [Column(name : "DaysToManufacture", TypeName = "int")]
        [Required(ErrorMessage = "Days To Manufacture is required")]
        [Display(Name = "Days To Manufacture")]
        [Description("Number of days required to manufacture the product.")]
        public int? DaysToManufacture { get; set; } // int
        [Column(name : "ProductLine", TypeName = "nchar")]
        [MaxLength(2)]
        [StringLength(2)]
        [Display(Name = "Product Line")]
        [Description("R = Road, M = Mountain, T = Touring, S = Standard")]
        public string ProductLine { get; set; } // nchar(2)
        [Column(name : "Class", TypeName = "nchar")]
        [MaxLength(2)]
        [StringLength(2)]
        [Display(Name = "Class")]
        [Description("H = High, M = Medium, L = Low")]
        public string Class { get; set; } // nchar(2)
        [Column(name : "Style", TypeName = "nchar")]
        [MaxLength(2)]
        [StringLength(2)]
        [Display(Name = "Style")]
        [Description("W = Womens, M = Mens, U = Universal")]
        public string Style { get; set; } // nchar(2)
        [Column(name : "ProductSubcategoryID", TypeName = "int")]
        [Display(Name = "Product Subcategory ID")]
        [Description("Product is a member of this product subcategory. Foreign key to ProductSubCategory.ProductSubCategoryID. ")]
        public int? ProductSubcategoryID { get; set; } // int
        [Column(name : "ProductModelID", TypeName = "int")]
        [Display(Name = "Product Model ID")]
        [Description("Product is a member of this product model. Foreign key to ProductModel.ProductModelID.")]
        public int? ProductModelID { get; set; } // int
        [Column(name : "SellStartDate")]
        [Required(ErrorMessage = "Sell Start Date is required")]
        [Display(Name = "Sell Start Date")]
        [Description("Date the product was available for sale.")]
        public DateTime? SellStartDate { get; set; } // datetime
        [Column(name : "SellEndDate")]
        [Display(Name = "Sell End Date")]
        [Description("Date the product was no longer available for sale.")]
        public DateTime? SellEndDate { get; set; } // datetime
        [Column(name : "DiscontinuedDate")]
        [Display(Name = "Discontinued Date")]
        [Description("Date the product was discontinued.")]
        public DateTime? DiscontinuedDate { get; set; } // datetime
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

        // Production.Product.SizeUnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_Product_UnitMeasure_SizeUnitMeasureCode)
        [ForeignKey("SizeUnitMeasureCode")]
        public UnitMeasure UnitMeasure { get; set; }
        // Production.Product.WeightUnitMeasureCode -> Production.UnitMeasure.UnitMeasureCode (FK_Product_UnitMeasure_WeightUnitMeasureCode)
        [ForeignKey("WeightUnitMeasureCode")]
        public UnitMeasure UnitMeasure1 { get; set; }
        // Production.Product.ProductSubcategoryID -> Production.ProductSubcategory.ProductSubcategoryID (FK_Product_ProductSubcategory_ProductSubcategoryID)
        [ForeignKey("ProductSubcategoryID")]
        public ProductSubcategory ProductSubcategory { get; set; }
        // Production.Product.ProductModelID -> Production.ProductModel.ProductModelID (FK_Product_ProductModel_ProductModelID)
        [ForeignKey("ProductModelID")]
        public ProductModel ProductModel { get; set; }
        // Production.BillOfMaterials.ProductAssemblyID -> Production.Product.ProductID (FK_BillOfMaterials_Product_ProductAssemblyID)
        [InverseProperty("Product")]
        public IEnumerable<BillOfMaterials> BillOfMaterials { get; set; }
        // Production.BillOfMaterials.ComponentID -> Production.Product.ProductID (FK_BillOfMaterials_Product_ComponentID)
        [InverseProperty("Product1")]
        public IEnumerable<BillOfMaterials> BillOfMaterials1 { get; set; }
        // Production.ProductCostHistory.ProductID -> Production.Product.ProductID (FK_ProductCostHistory_Product_ProductID)
        public IEnumerable<ProductCostHistory> ProductCostHistories { get; set; }
        // Production.ProductDocument.ProductID -> Production.Product.ProductID (FK_ProductDocument_Product_ProductID)
        public IEnumerable<ProductDocument> ProductDocuments { get; set; }
        // Production.ProductInventory.ProductID -> Production.Product.ProductID (FK_ProductInventory_Product_ProductID)
        public IEnumerable<ProductInventory> ProductInventories { get; set; }
        // Production.ProductListPriceHistory.ProductID -> Production.Product.ProductID (FK_ProductListPriceHistory_Product_ProductID)
        public IEnumerable<ProductListPriceHistory> ProductListPriceHistories { get; set; }
        // Production.ProductProductPhoto.ProductID -> Production.Product.ProductID (FK_ProductProductPhoto_Product_ProductID)
        public IEnumerable<ProductProductPhoto> ProductProductPhotos { get; set; }
        // Production.ProductReview.ProductID -> Production.Product.ProductID (FK_ProductReview_Product_ProductID)
        public IEnumerable<ProductReview> ProductReviews { get; set; }
        // Production.TransactionHistory.ProductID -> Production.Product.ProductID (FK_TransactionHistory_Product_ProductID)
        public IEnumerable<TransactionHistory> TransactionHistories { get; set; }
        // Production.WorkOrder.ProductID -> Production.Product.ProductID (FK_WorkOrder_Product_ProductID)
        public IEnumerable<WorkOrder> WorkOrders { get; set; }
        // Purchasing.ProductVendor.ProductID -> Production.Product.ProductID (FK_ProductVendor_Product_ProductID)
        public IEnumerable<ProductVendor> ProductVendors { get; set; }
        // Purchasing.PurchaseOrderDetail.ProductID -> Production.Product.ProductID (FK_PurchaseOrderDetail_Product_ProductID)
        public IEnumerable<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        // Sales.ShoppingCartItem.ProductID -> Production.Product.ProductID (FK_ShoppingCartItem_Product_ProductID)
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
        // Sales.SpecialOfferProduct.ProductID -> Production.Product.ProductID (FK_SpecialOfferProduct_Product_ProductID)
        public IEnumerable<SpecialOfferProduct> SpecialOfferProducts { get; set; }
    }
}
