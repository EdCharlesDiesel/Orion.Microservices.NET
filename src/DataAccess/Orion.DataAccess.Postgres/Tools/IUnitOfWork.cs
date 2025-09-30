using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Tools
{
    public interface IUnitOfWork
    {
        IBuildVersionRepository BuildVersions { get; }
        IDatabaseLogRepository DatabaseLogs { get; }
        ITransactionHistoryArchivesRepository TransactionHistoryArchives { get; set; }
        IErrorLogsRepository ErrorLogs { get; set; }
        IShiftsRepository Shifts { get; set; }
        IDepartmentsRepository Departments { get; set; }
        IJobCandidatesRepository JobCandidates { get; set; }
        IEmployeePayHistoriesRepository EmployeePayHistories { get; set; }
        IEmployeeDepartmentHistoriesRepository EmployeeDepartmentHistories { get; set; }
        IPersonsRepository Persons { get; set; }
        IAddressesRepository Addresses { get; set; }
        IAddressTypesRepository AddressTypes { get; set; }
        IContactTypesRepository ContactTypes { get; set; }
        ICountryRegionsRepository CountryRegions { get; set; }
        IEmailAddressesRepository EmailAddresses { get; set; }
        IPersonPhonesRepository PersonPhones { get; set; }
        IPhoneNumberTypesRepository PhoneNumberTypes { get; set; }
        IStateProvincesRepository StateProvinces { get; set; }
        IBillOfMaterialsRepository BillOfMaterials { get; set; }
        IProductsRepository Products { get; set; }
        IDocumentsRepository Documents { get; set; }
        IIllustrationsRepository Illustrations { get; set; }
        ILocationsRepository Locations { get; set; }
        IProductCategoriesRepository ProductCategories { get; set; }
        IProductCostHistorysRepository ProductCostHistorys { get; set; }
        IProductDescriptionsRepository ProductDescriptions { get; set; }
        IProductDocumentsRepository ProductDocuments { get; set; }
        IProductInventoriesRepository ProductInventories { get; set; }
        IProductListPriceHistorysRepository ProductListPriceHistorys { get; set; }
        IProductModelsRepository ProductModels { get; set; }
        IProductModelIllustrationsRepository ProductModelIllustrations { get; set; }
        IProductModelProductDescriptionCulturesRepository ProductModelProductDescriptionCultures { get; set; }
        IProductPhotosRepository ProductPhotos { get; set; }
        IProductReviewsRepository ProductReviews { get; set; }
        IProductSubcategorysRepository ProductSubcategorys { get; set; }
        IScrapReasonsRepository ScrapReasons { get; set; }
        ITransactionHistorysRepository TransactionHistorys { get; set; }
        IUnitMeasuresRepository UnitMeasures { get; }
        IWorkOrderRoutingsRepository WorkOrderRoutings { get; set; }
        IWorkOrdersRepository WorkOrders { get; set; }
        IVendorsRepository Vendors { get; set; }
        IPurchaseOrderHeadersRepository PurchaseOrderHeaders { get; set; }
        IPurchaseOrderDetailsRepository PurchaseOrderDetails { get; set; }
        IShipMethodsRepository ShipMethods { get; set; }
        IProductVendorsRepository ProductVendors { get; set; }
        ICountryRegionCurrencysRepository CountryRegionCurrencys { get; set; }
        ICreditCardsRepository CreditCards { get; set; }
        ICurrencysRepository Currencys { get; set; }
        ICurrencyRatesRepository CurrencyRates { get; set; }
        ICustomersRepository Customers { get; set; }
        IPersonCreditCardsRepository PersonCreditCards { get; set; }
        ISalesOrderDetailsRepository SalesOrderDetails { get; set; }
        ISalesOrderHeadersRepository SalesOrderHeaders { get; set; }
        ISalesOrderHeaderSalesReasonsRepository SalesOrderHeaderSalesReasons { get; set; }
        ISalesPersonsRepository SalesPersons { get; set; }
        ISalesPersonQuotaHistorysRepository SalesPersonQuotaHistorys { get; set; }
        ISalesReasonsRepository SalesReasons { get; set; }
        ISalesTaxRatesRepository SalesTaxRates { get; set; }
        ISalesTerritorysRepository SalesTerritorys { get; set; }
        ISalesTerritoryHistorysRepository SalesTerritoryHistorys { get; set; }
        IShoppingCartItemsRepository ShoppingCartItems { get; set; }
        ISpecialOffersRepository SpecialOffers { get; set; }
        ISpecialOfferProductsRepository SpecialOfferProducts { get; set; }
        IStoresRepository Stores { get; set; }
        IRevenueStreamsRepository RevenueStreams { get; set; }


        Task<bool> SaveEntitiesAsync();
        Task<bool> SaveErrorsAsync(ErrorLog errorLogDto);
        Task StartAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> CompleteAsync();
    }

    public interface IRevenueStreamsRepository
    {
        Task<IEnumerable<RevenueStream>> GetAllAsync();
    }

    [NotMapped]
    public class RevenueStream
    {
        private decimal UnitPrice { get; set; }
        private DateTime ModifiedDate { get; set; }
    }
}

