using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.DataAccess.Postgres.Data
{
    // Inherit from IdentityDbContext instead of DbContext
    public sealed class OrionDbContext(
        DbContextOptions<OrionDbContext> options)
        : IdentityDbContext<IdentityUser, IdentityRole, string>(options), IOrionDbContext
    {
        // Your DbSets
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<BuildVersion> BuildVersions { get; set; }
        public DbSet<BillOfMaterials> BillOfMaterials { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCreditCard> ContactCreditCards { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<CountryRegion> CountryRegions { get; set; }
        public DbSet<CountryRegionCurrency> CountryRegionCurrencies { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<DatabaseLog> DatabaseLogs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
        public DbSet<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
        public DbSet<EmployeePayHistory> EmployeePayHistories { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Illustration> Illustrations { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<JobCandidate> JobCandidates { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductCostHistory> ProductCostHistories { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
        public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<ProductListPriceHistory> ProductListPriceHistories { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductModelIllustration> ProductModelIllustrations { get; set; }
        public DbSet<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductProductPhoto> ProductProductPhotos { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<ProductVendor> ProductVendors { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public DbSet<SalesOrderHeaderSalesReason> SalesOrderHeaderSalesReasons { get; set; }
        public DbSet<SalesPerson> SalesPersons { get; set; }
        public DbSet<SalesPersonQuotaHistory> SalesPersonQuotaHistories { get; set; }
        public DbSet<SalesReason> SalesReasons { get; set; }
        public DbSet<SalesTaxRate> SalesTaxRates { get; set; }
        public DbSet<SalesTerritory> SalesTerritories { get; set; }
        public DbSet<SalesTerritoryHistory> SalesTerritoryHistories { get; set; }
        public DbSet<ScrapReason> ScrapReasons { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShipMethod> ShipMethods { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<SpecialOfferProduct> SpecialOfferProducts { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreContact> StoreContacts { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<TransactionHistoryArchive> TransactionHistoryArchives { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorAddress> VendorAddresses { get; set; }
        public DbSet<VendorContact> VendorContacts { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderRouting> WorkOrderRoutings { get; set; }
        public DbSet<OrionCalendarEvent> OrionCalendarEvents { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Person> Persons { get; set; }
        
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<ComtradeCategories> ComtradeCategories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<ChatRequest> ChatRequests { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CompetitionMatch> CompetitionMatches { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<OrderDetail> Orders { get; set; }
        public DbSet<TradingEconomicsCalendar> TradingEconomicsCalendars { get; set; }
        public DbSet<PersonPhone>  PersonPhones { get; set; }
        public DbSet<PersonCreditCard>?  PersonCreditCards => null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 👈 keep Identity configuration
            
            modelBuilder.Entity<BuildVersion>()
                .ToTable("BuildVersion", "public")
                .HasKey(x => x.SystemInformationID);

            modelBuilder.Entity<BuildVersion>()
                .Property(x => x.DatabaseVersion)
                .HasColumnName("Database Version");

            modelBuilder.Entity<BusinessEntityAddress>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.AddressID });

            modelBuilder.Entity<BusinessEntityContact>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.PersonID, bea.ContactTypeID });

            modelBuilder.Entity<CountryRegionCurrency>()
                .HasKey(bea => new { bea.CurrencyCode, bea.CountryRegionCode });

            modelBuilder.Entity<EmailAddress>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.EmailAddressID });

            modelBuilder.Entity<EmployeeDepartmentHistory>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.DepartmentID, bea.StartDate });

            modelBuilder.Entity<EmployeePayHistory>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.RateChangeDate });

            modelBuilder.Entity<PersonCreditCard>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.CreditCardID });

            modelBuilder.Entity<PersonPhone>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.PhoneNumber });

            modelBuilder.Entity<ProductCostHistory>()
                .HasKey(bea => new { bea.ProductID, bea.StartDate });

            modelBuilder.Entity<ProductDocument>()
                .HasKey(bea => new { bea.ProductID, bea.DocumentNode });

            modelBuilder.Entity<ProductInventory>()
                .HasKey(bea => new { bea.ProductID, bea.LocationID });

            modelBuilder.Entity<ProductListPriceHistory>()
                .HasKey(bea => new { bea.ProductID, bea.StartDate });

            modelBuilder.Entity<ProductModelIllustration>()
                .HasKey(bea => new { bea.ProductModelID, bea.IllustrationID });

            modelBuilder.Entity<ProductModelProductDescriptionCulture>()
                .HasKey(bea => new { bea.ProductModelID, bea.ProductDescriptionID, bea.CultureID });
        
            modelBuilder.Entity<ProductProductPhoto>()
                .HasKey(bea => new { bea.ProductID, bea.ProductPhotoID });

            modelBuilder.Entity<ProductVendor>()
                .HasKey(bea => new { bea.ProductID, bea.BusinessEntityID });

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasKey(bea => new { bea.ProductID, bea.PurchaseOrderDetailID });
            
            modelBuilder.Entity<SalesOrderDetail>()
                .HasKey(d => new { d.SalesOrderID, d.SalesOrderDetailID });

            modelBuilder.Entity<SalesOrderHeaderSalesReason>()
                .HasKey(bea => new { bea.SalesOrderID, bea.SalesReasonID });

            modelBuilder.Entity<SalesPersonQuotaHistory>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.QuotaDate });

            modelBuilder.Entity<SalesTerritoryHistory>()
                .HasKey(bea => new { bea.BusinessEntityID, bea.TerritoryID });

            modelBuilder.Entity<SpecialOfferProduct>()
                .HasKey(sop => new { sop.SpecialOfferID, sop.ProductID });

            modelBuilder.Entity<SalesOrderDetail>()
                .HasOne(sod => sod.SpecialOfferProduct)
                .WithMany(sop => sop.SalesOrderDetails)
                .HasForeignKey(sod => new { sod.SpecialOfferID, sod.ProductID }); 

            modelBuilder.Entity<WorkOrderRouting>()
                .HasKey(bea => new { bea.WorkOrderID, bea.ProductID });

            modelBuilder.Entity<ProductDocument>()
                .Property(p => p.DocumentNode)
                .HasConversion(
                    v => v.ToString(),
                    v => SqlHierarchyId.Parse(v)
                )
                .HasColumnType("text");
            
            modelBuilder.Entity<Person>()
                .ToTable("Person");

            modelBuilder.Entity<Store>()
                .ToTable("Store");

            modelBuilder.Entity<Vendor>()
                .ToTable("Vendor");

            modelBuilder.Entity<BusinessEntity>()
                .ToTable("BusinessEntity");
            
            // ✅ Seed data
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department", "HumanResources");

                entity.HasKey(e => e.DepartmentID);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .IsRequired();

                // ✅ Seed data
                entity.HasData(
                    new Department
                    {
                        DepartmentID = 1,
                        Name = "Engineering",
                        GroupName = "Research and Development",
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 2,
                        Name = "Tool Design",
                        GroupName = "Research and Development",
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 3,
                        Name = "Sales",
                        GroupName = "Sales and Marketing",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 4,
                        Name = "Marketing",
                        GroupName = "Sales and Marketing",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 5,
                        Name = "Purchasing",
                        GroupName = "Inventory Management",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 6,
                        Name = "Research and Development",
                        GroupName = "Research and Development",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 7,
                        Name = "Production",
                        GroupName = "Manufacturing",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 8,
                        Name = "Production Control",
                        GroupName = "Manufacturing",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 9,
                        Name = "Human Resources",
                        GroupName = "Executive General and Administration",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 10,
                        Name = "Finance",
                        GroupName = "Executive General and Administration",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 11,
                        Name = "Information Services",
                        GroupName = "Executive General and Administration",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 12,
                        Name = "Sales",
                        GroupName = "Quality Assurance",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 13,
                        Name = "Quality Assurance",
                        GroupName = "Quality Assurance",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 14,
                        Name = "Facilities and Maintenance",
                        GroupName = "Executive General and Administration",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 15,
                        Name = "Shipping and Receiving",
                        GroupName = "Sales and Inventory Management",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Department
                    {
                        DepartmentID = 16,
                        Name = "Executive",
                        GroupName = "Executive General and Administration",
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    }
                    
                );
            }); 
            
            modelBuilder.Entity<ProductDocument>()
                .Property(p => p.DocumentNode)
                .HasConversion(
                    v => v.ToString(),
                    v => SqlHierarchyId.Parse(v)
                )
                .HasColumnType("text");
            
                modelBuilder.Entity<Person>()
                     .ToTable("Person");

                modelBuilder.Entity<Store>()
                    .ToTable("Store");

                modelBuilder.Entity<Vendor>()
                    .ToTable("Vendor");

                modelBuilder.Entity<BusinessEntity>()
                    .ToTable("BusinessEntity");
                
                
            // ✅ Seed default BusinessEntity data 
            modelBuilder.Entity<BusinessEntity>(entity =>
            {
                entity.ToTable("BusinessEntity", "Person");
        
                entity.HasKey(e => e.BusinessEntityID);
        
                // ✅ Seed data
                entity.HasData(
                    new BusinessEntity( )
                    {
                        BusinessEntityID = 1,
                        Rowguid = Guid.NewGuid(),
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                        BusinessEntityContact = new List<BusinessEntityContact> {},
                        BusinessEntityAddress = new List<BusinessEntityAddress>{}
                    }
                );
            });
            
            // ✅ Seed AddressType data
            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("AddressType", "Person");

                entity.HasKey(e => e.AddressTypeId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Rowguid)
                    .IsRequired();

                entity.Property(e => e.ModifiedDate)
                    .IsRequired();

                // ✅ Seed data
                entity.HasData(
                    new AddressType("Billing")
                    {
                        AddressTypeId = 1,
                        Name = "Billing",
                        Rowguid = Guid.NewGuid(), // <-- add this
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new AddressType("Home")
                    {
                        AddressTypeId = 2,
                        Name = "Home",
                        Rowguid = Guid.NewGuid(), // <-- add this
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new AddressType("Main Office")
                    {
                        AddressTypeId = 3,
                        Name = "Main Office",
                        Rowguid = Guid.NewGuid(), // <-- add this
                         ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new AddressType("Primary")
                    {
                        AddressTypeId = 4,
                        Name = "Primary",
                        Rowguid = Guid.NewGuid(), // <-- add this
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new AddressType("Shipping")
                    {
                        AddressTypeId = 5,
                        Name = "Shipping",
                        Rowguid = Guid.NewGuid(), // <-- add this
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new AddressType("Archive")
                    {
                        AddressTypeId = 6,
                        Name = "Archive",
                        Rowguid = Guid.NewGuid(), // <-- add this
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    }
                );
            });
            
            // ✅ Seed Shift data 
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift", "HumanResources");

                entity.HasKey(e => e.ShiftID);

                entity.Property(e => e.StartTime)
                    .IsRequired();

                entity.Property(e => e.EndTime)
                    .IsRequired();

                entity.Property(e => e.ModifiedDate)
                    .IsRequired();

                // ✅ Seed data
                entity.HasData(
                    new Shift()
                    {
                        ShiftID = 1,
                        Name = "Day",
                        StartTime = TimeSpan.Parse("07:00:00.0000000"),
                        EndTime = TimeSpan.Parse("15:00:00.0000000"),
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Shift()
                    {
                        ShiftID = 2,
                        Name = "Evening",
                        StartTime = TimeSpan.Parse("15:00:00.0000000"),
                        EndTime = TimeSpan.Parse("23:00:00.0000000"),
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Shift()
                    {
                        ShiftID = 3,
                        Name = "Night",
                        StartTime = TimeSpan.Parse("23:00:00.0000000"),
                        EndTime = TimeSpan.Parse("07:00:00.0000000"),
                        ModifiedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                    }
                );
            });
            
            // Configure RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired();
                entity.HasOne(rt => rt.User)
                    .WithMany()
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}