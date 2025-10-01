

using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.Shared;
using Orion.DataAccess.Postgres.Entities.TradingEconomics;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Data
{
    public interface IOrionDbContext
    {
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<AddressType> AddressTypes { get; set; }
        DbSet<BuildVersion> BuildVersions { get; set; }
        DbSet<BillOfMaterials> BillOfMaterials { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<ContactCreditCard> ContactCreditCards { get; set; }
        DbSet<ContactType> ContactTypes { get; set; }
        DbSet<CountryRegion> CountryRegions { get; set; }
        DbSet<CountryRegionCurrency> CountryRegionCurrencies { get; set; }
        DbSet<CreditCard> CreditCards { get; set; }
        DbSet<Culture> Cultures { get; set; }
        DbSet<Currency> Currencies{ get; set; }
        DbSet<CurrencyRate> CurrencyRates { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<DatabaseLog> DatabaseLogs { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
        DbSet<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
        DbSet<EmployeePayHistory> EmployeePayHistories { get; set; }
        DbSet<ErrorLog> ErrorLogs { get; set; }
        DbSet<Illustration> Illustrations { get; set; }
        DbSet<Individual> Individuals { get; set; }
        DbSet<JobCandidate> JobCandidates { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<ProductCostHistory> ProductCostHistories{ get; set; }
        DbSet<ProductDescription> ProductDescriptions { get; set; }
        DbSet<ProductDocument> ProductDocuments { get; set; }
        DbSet<ProductInventory> ProductInventories { get; set; }
        DbSet<ProductListPriceHistory> ProductListPriceHistories { get; set; }
        DbSet<ProductModel> ProductModels { get; set; }
        DbSet<ProductModelIllustration> ProductModelIllustrations { get; set; }
        DbSet<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
        DbSet<ProductPhoto> ProductPhotos { get; set; }
        DbSet<ProductProductPhoto> ProductProductPhotos { get; set; }
        DbSet<ProductReview> ProductReviews { get; set; }
        DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        DbSet<ProductVendor> ProductVendors { get; set; }
        DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        DbSet<SalesOrderHeaderSalesReason> SalesOrderHeaderSalesReasons { get; set; }
        DbSet<SalesPerson> SalesPersons { get; set; }
        DbSet<SalesPersonQuotaHistory> SalesPersonQuotaHistories { get; set; }
        DbSet<SalesReason> SalesReasons { get; set; }
        DbSet<SalesTaxRate> SalesTaxRates { get; set; }
        DbSet<SalesTerritory> SalesTerritories { get; set; }
        DbSet<SalesTerritoryHistory> SalesTerritoryHistories { get; set; }
        DbSet<ScrapReason> ScrapReasons { get; set; }
        DbSet<Shift> Shifts { get; set; }
        DbSet<ShipMethod> ShipMethods { get; set; }
        DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        DbSet<SpecialOffer> SpecialOffers { get; set; }
        DbSet<SpecialOfferProduct> SpecialOfferProducts { get; set; }
        DbSet<StateProvince> StateProvinces { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<StoreContact> StoreContacts { get; set; }
        DbSet<TransactionHistory> TransactionHistories { get; set; }
        DbSet<TransactionHistoryArchive> TransactionHistoryArchives { get; set; }
        DbSet<UnitMeasure> UnitMeasures { get; set; }
        DbSet<Vendor> Vendors { get; set; }
        DbSet<VendorAddress> VendorAddresses { get; set; }
        DbSet<VendorContact> VendorContacts { get; set; }
        DbSet<WorkOrder> WorkOrders { get; set; }
        DbSet<WorkOrderRouting> WorkOrderRoutings{ get; set; }
        DbSet<TradingEconomicsCalendar> TradingEconomicsCalendars { get; set; }
        DbSet<OrionCalendarEvent> OrionCalendarEvents { get; set; }
        DbSet<OrderDetail> Orders { get; set; }
        DbSet<Basket> Baskets  { get; set; }
        DbSet<ChatRequest> ChatRequests  { get; set; }
        DbSet<Coupon> Coupons  { get; set; }
        DbSet<CompetitionMatch> CompetitionMatches  { get; set; }
        public DbSet<PersonPhone>  PersonPhones { get; set; }
        
        DbSet<Course> Courses { get; set; }
   
        //Trading Economics
        DbSet<ComtradeCategories> ComtradeCategories  { get; set; }
        
        DbSet<Forecast> Forecasts  { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class OrderDetail: Entity<int>
    {
    }

    public class ContactCreditCard: Entity<int>
    {
    }

    public class CustomerAddress: Entity<int>
    {
    }

    public class StoreContact: Entity<int>
    {
    }

    public class VendorContact: Entity<int>
    {
    }

    public class VendorAddress: Entity<int>
    {
    }

    public class Individual: Entity<int>
    {
    }

    public class Contact: Entity<int>
    {
    }
}
