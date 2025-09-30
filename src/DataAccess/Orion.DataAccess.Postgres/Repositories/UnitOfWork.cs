using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;
using Serilog;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrionDbContext _context;

        public UnitOfWork(OrionDbContext context)
        {
            _context = context;
            BuildVersions = new BuildVersionRepository(_context);
            DatabaseLogs = new DatabaseLogRepository(_context);
            TransactionHistoryArchives = new TransactionHistoryArchivesRepository(_context);
            ErrorLogs = new ErrorLogsRepository(_context);
            Shifts = new ShiftsRepository(_context);
            Departments = new DepartmentsRepository(_context);
            JobCandidates = new  JobCandidatesRepository(_context);
            EmployeePayHistories = new EmployeePayHistoriesRepository(_context);
            EmployeeDepartmentHistories = new  EmployeeDepartmentHistoriesRepository(_context);
            Persons = new  PersonsRepository(_context);
            Addresses = new  AddressesRepository(_context); 
            AddressTypes = new  AddressTypesRepository(_context); 
            ContactTypes = new  ContactTypesRepository(_context); 
            CountryRegions = new  CountryRegionsRepository(_context);
            EmailAddresses = new  EmailAddressesRepository(_context);
            PersonPhones = new  PersonPhonesRepository(_context);
            PhoneNumberTypes = new  PhoneNumberTypesRepository(_context);
            StateProvinces = new  StateProvincesRepository(_context);
            BillOfMaterials = new  BillOfMaterialsRepository(_context);

            Products = new ProductsRepository(_context);
            Documents= new DocumentsRepository(_context); 
            Illustrations = new IllustrationsRepository(_context);
            Locations = new LocationsRepository(_context); 
            
            
            ProductCategories = new ProductCategoriesRepository(_context); 
            ProductCostHistorys = new ProductCostHistoriesRepository(_context); 
            ProductDescriptions = new ProductDescriptionsRepository(_context); 
            ProductDocuments = new ProductDocumentsRepository(_context); 
            ProductInventories = new ProductInventoriesRepository(_context); 
            ProductListPriceHistorys = new ProductListPriceHistorysRepository(_context); 
            ProductModels = new ProductModelsRepository(_context); 
            ProductModelIllustrations = new ProductModelIllustrationsRepository(_context); 
            ProductModelProductDescriptionCultures = new ProductModelProductDescriptionCulturesRepository(_context); 
            ProductPhotos = new ProductPhotosRepository(_context); 
            ProductReviews = new ProductReviewsRepository(_context); 
            ProductSubcategorys = new ProductSubcategorysRepository(_context); 
            ScrapReasons = new ScrapReasonsRepository(_context); 
            TransactionHistorys = new TransactionHistorysRepository(_context); 
            UnitMeasures = new UnitMeasuresRepository(_context); 
            WorkOrderRoutings = new WorkOrderRoutingsRepository(_context); 
            WorkOrders = new WorkOrdersRepository(_context); 
            
            Vendors = new VendorsRepository(_context); 
            PurchaseOrderHeaders = new PurchaseOrderHeadersRepository(_context); 
            PurchaseOrderDetails = new PurchaseOrderDetailsRepository(_context); 
            ShipMethods = new ShipMethodsRepository(_context); 
            ProductVendors = new ProductVendorsRepository(_context); 
            
             CountryRegionCurrencys  = new CountryRegionCurrencysRepository(_context); 
             CreditCards = new CreditCardsRepository(_context); 
             Currencys = new CurrencysRepository(_context); 
             CurrencyRates = new CurrencyRatesRepository(_context); 
             Customers = new CustomersRepository(_context); 
             PersonCreditCards = new PersonCreditCardsRepository(_context); 
             SalesOrderDetails = new SalesOrderDetailsRepository(_context); 
             SalesOrderHeaders = new SalesOrderHeadersRepository(_context); 
             SalesOrderHeaderSalesReasons = new SalesOrderHeaderSalesReasonsRepository(_context); 
             SalesPersons = new SalesPersonsRepository(_context); 
             SalesPersonQuotaHistorys = new SalesPersonQuotaHistorysRepository(_context); 
             SalesReasons = new SalesReasonsRepository(_context); 
             SalesTaxRates = new SalesTaxRatesRepository(_context); 
             SalesTerritorys = new SalesTerritorysRepository(_context); 
             SalesTerritoryHistorys = new SalesTerritoryHistorysRepository(_context); 
             ShoppingCartItems = new ShoppingCartItemsRepository(_context); 
             SpecialOffers = new SpecialOffersRepository(_context); 
             SpecialOfferProducts = new SpecialOfferProductsRepository(_context); 
             Stores = new StoresRepository(_context); 
        }

        public IBuildVersionRepository BuildVersions { get; set; }
        public IDatabaseLogRepository DatabaseLogs { get; set; }
        public ITransactionHistoryArchivesRepository TransactionHistoryArchives { get; set; }
        public IErrorLogsRepository ErrorLogs { get; set; }
        public IShiftsRepository Shifts { get; set; }
        public IDepartmentsRepository Departments { get; set; }
        public IJobCandidatesRepository JobCandidates { get; set; }
        public IEmployeePayHistoriesRepository EmployeePayHistories { get; set; }
        public IEmployeeDepartmentHistoriesRepository EmployeeDepartmentHistories { get; set; }
        public IPersonsRepository Persons { get; set; }
        public IAddressesRepository Addresses { get; set; }
        public IAddressTypesRepository AddressTypes { get; set; }
        public IContactTypesRepository ContactTypes { get; set; }
        public ICountryRegionsRepository CountryRegions { get; set; }
        public IEmailAddressesRepository EmailAddresses { get; set; }
        public IPersonPhonesRepository PersonPhones { get; set; }
        public IPhoneNumberTypesRepository PhoneNumberTypes { get; set; }
        public IStateProvincesRepository StateProvinces { get; set; }
        public IBillOfMaterialsRepository BillOfMaterials { get; set; }
        public IProductsRepository Products { get; set; }
        public IDocumentsRepository Documents { get; set; }
        public IIllustrationsRepository Illustrations { get; set; }
        public ILocationsRepository Locations { get; set; }
        public IProductCategoriesRepository ProductCategories { get; set; }
        public IProductCostHistorysRepository ProductCostHistorys { get; set; }
        public IProductDescriptionsRepository ProductDescriptions { get; set; }
        public IProductDocumentsRepository ProductDocuments { get; set; }
        public IProductInventoriesRepository ProductInventories { get; set; }
        public IProductListPriceHistorysRepository ProductListPriceHistorys { get; set; }
        public IProductModelsRepository ProductModels { get; set; }
        public IProductModelIllustrationsRepository ProductModelIllustrations { get; set; }
        public IProductModelProductDescriptionCulturesRepository ProductModelProductDescriptionCultures { get; set; }
        public IProductPhotosRepository ProductPhotos { get; set; }
        public IProductReviewsRepository ProductReviews { get; set; }
        public IProductSubcategorysRepository ProductSubcategorys { get; set; }
        public IScrapReasonsRepository ScrapReasons { get; set; }
        public ITransactionHistorysRepository TransactionHistorys { get; set; }
        public IUnitMeasuresRepository UnitMeasures { get; }
        public IWorkOrderRoutingsRepository WorkOrderRoutings { get; set; }
        public IWorkOrdersRepository WorkOrders { get; set; }
        public IVendorsRepository Vendors { get; set; }
        public IPurchaseOrderHeadersRepository PurchaseOrderHeaders { get; set; }
        public IPurchaseOrderDetailsRepository PurchaseOrderDetails { get; set; }
        public IShipMethodsRepository ShipMethods { get; set; }
        
        
        
        
        public IProductVendorsRepository ProductVendors { get; set; }
        public ICountryRegionCurrencysRepository CountryRegionCurrencys { get; set; }
        public ICreditCardsRepository CreditCards { get; set; }
        public ICurrencysRepository Currencys { get; set; }
        public ICurrencyRatesRepository CurrencyRates { get; set; }
        public ICustomersRepository Customers { get; set; }
        public IPersonCreditCardsRepository PersonCreditCards { get; set; }
        public ISalesOrderDetailsRepository SalesOrderDetails { get; set; }
        public ISalesOrderHeadersRepository SalesOrderHeaders { get; set; }
        public ISalesOrderHeaderSalesReasonsRepository SalesOrderHeaderSalesReasons { get; set; }
        public ISalesPersonsRepository SalesPersons { get; set; }
        public ISalesPersonQuotaHistorysRepository SalesPersonQuotaHistorys { get; set; }
        public ISalesReasonsRepository SalesReasons { get; set; }
        public ISalesTaxRatesRepository SalesTaxRates { get; set; }
        public ISalesTerritorysRepository SalesTerritorys { get; set; }
        public ISalesTerritoryHistorysRepository SalesTerritoryHistorys { get; set; }
        public IShoppingCartItemsRepository ShoppingCartItems { get; set; }
        public ISpecialOffersRepository SpecialOffers { get; set; }
        public ISpecialOfferProductsRepository SpecialOfferProducts { get; set; }
        public IStoresRepository Stores { get; set; }
        public IRevenueStreamsRepository RevenueStreams { get; set; }
        public Task StartAsync() => Task.CompletedTask;

        public Task CommitAsync() => Task.CompletedTask;

        public Task RollbackAsync() => Task.CompletedTask;
        
        public async Task<bool> SaveEntitiesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> SaveErrorsAsync(ErrorLog errorLog)
        {
            try
            {
                await _context.ErrorLogs.AddAsync(errorLog);
                var result = await _context.SaveChangesAsync();

                // Log with Serilog
                Log.Error("Database ErrorLog saved: {@ErrorLog}", errorLog);

                return result > 0;
            }
            catch (Exception ex)
            {
                // Log failure with Serilog
                Log.Error(ex, "Failed to save ErrorLog: {@ErrorLog}", errorLog);
                return false;
            }
        }


        public async Task<int> CompleteAsync() =>
            await _context.SaveChangesAsync();
        
        public void Dispose() => _context.Dispose();
    }
}