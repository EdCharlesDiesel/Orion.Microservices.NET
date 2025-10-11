using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orion.DataAccess.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Person");

            migrationBuilder.EnsureSchema(
                name: "Shared");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.EnsureSchema(
                name: "TradingEconomics");

            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.EnsureSchema(
                name: "HumanResources");

            migrationBuilder.EnsureSchema(
                name: "Production");

            migrationBuilder.EnsureSchema(
                name: "Purchasing");

            migrationBuilder.CreateTable(
                name: "AddressType",
                schema: "Person",
                columns: table => new
                {
                    AddressTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressType", x => x.AddressTypeID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Basket",
                schema: "Shared",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    IsCheckedOut = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildVersion",
                schema: "public",
                columns: table => new
                {
                    SystemInformationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatabaseVersion = table.Column<string>(name: "Database Version", type: "character varying(25)", maxLength: 25, nullable: true),
                    VersionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildVersion", x => x.SystemInformationID);
                });

            migrationBuilder.CreateTable(
                name: "BusinessEntity",
                schema: "Person",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessEntity", x => x.BusinessEntityID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTwo = table.Column<string>(type: "text", nullable: false),
                    LeagueCode = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionMatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComtradeCategories",
                schema: "TradingEconomics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: false),
                    PrettyName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComtradeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactCreditCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCreditCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "Sales",
                columns: table => new
                {
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyCode);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseLog",
                columns: table => new
                {
                    DatabaseLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatabaseUser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Event = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Schema = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Object = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TSQL = table.Column<string>(type: "text", nullable: false),
                    XmlEvent = table.Column<string>(type: "xml", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseLog", x => x.DatabaseLogID);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "HumanResources",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GroupName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "HumanResources",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NationalIDNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    LoginID = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    OrganizationLevel = table.Column<short>(type: "smallint", nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nchar", maxLength: 1, nullable: false),
                    Gender = table.Column<string>(type: "nchar", maxLength: 1, nullable: false),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false),
                    VacationHours = table.Column<short>(type: "smallint", nullable: false),
                    SickLeaveHours = table.Column<short>(type: "smallint", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    JobLevel = table.Column<int>(type: "integer", nullable: false),
                    YearsInService = table.Column<int>(type: "integer", nullable: false),
                    SuggestedBonus = table.Column<int>(type: "integer", nullable: false),
                    Salary = table.Column<int>(type: "integer", nullable: false),
                    MinimumRaiseGiven = table.Column<bool>(type: "boolean", nullable: false),
                    EntityVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.BusinessEntityID);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    ErrorLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ErrorTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ErrorNumber = table.Column<int>(type: "int", nullable: false),
                    ErrorSeverity = table.Column<int>(type: "int", nullable: true),
                    ErrorState = table.Column<int>(type: "int", nullable: true),
                    ErrorProcedure = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: true),
                    ErrorLine = table.Column<int>(type: "int", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.ErrorLogID);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                schema: "Shared",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FeatureName = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forecast",
                schema: "TradingEconomics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    LatestValue = table.Column<double>(type: "double precision", nullable: true),
                    LatestValueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ForecastValue1Q = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue2Q = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue3Q = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue4Q = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue1 = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue2 = table.Column<double>(type: "double precision", nullable: true),
                    ForecastValue3 = table.Column<double>(type: "double precision", nullable: true),
                    Q1Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Q2Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Q3Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Q4Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ForecastLastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    HistoricalDataSymbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<bool>(type: "boolean", nullable: false),
                    Push = table.Column<bool>(type: "boolean", nullable: false),
                    Sms = table.Column<bool>(type: "boolean", nullable: false),
                    Marketing = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrionCalendarEvent",
                columns: table => new
                {
                    OrionCalendarEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<int>(name: "Employee ID", type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    JobLevel = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Salary = table.Column<decimal>(type: "numeric", nullable: false),
                    SuggestedBonus = table.Column<decimal>(type: "numeric", nullable: false),
                    YearsInService = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrionCalendarEvent", x => x.OrionCalendarEventID);
                });

            migrationBuilder.CreateTable(
                name: "Person.ContactType",
                columns: table => new
                {
                    ContactTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.ContactType", x => x.ContactTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Person.CountryRegion",
                columns: table => new
                {
                    CountryRegionCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.CountryRegion", x => x.CountryRegionCode);
                });

            migrationBuilder.CreateTable(
                name: "Person.PhoneNumberType",
                columns: table => new
                {
                    PhoneNumberTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.PhoneNumberType", x => x.PhoneNumberTypeID);
                });

            migrationBuilder.CreateTable(
                name: "PrivacySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProfileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    ShowEmail = table.Column<bool>(type: "boolean", nullable: false),
                    ShowPhone = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacySettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Production.Culture",
                columns: table => new
                {
                    CultureID = table.Column<string>(type: "nchar", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.Culture", x => x.CultureID);
                });

            migrationBuilder.CreateTable(
                name: "Production.Illustration",
                columns: table => new
                {
                    IllustrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Diagram = table.Column<string>(type: "xml", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.Illustration", x => x.IllustrationID);
                });

            migrationBuilder.CreateTable(
                name: "Production.Location",
                columns: table => new
                {
                    LocationID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CostRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Availability = table.Column<decimal>(type: "decimal", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.Location", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductCategory",
                columns: table => new
                {
                    ProductCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductCategory", x => x.ProductCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductDescription",
                columns: table => new
                {
                    ProductDescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductDescription", x => x.ProductDescriptionID);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductModel",
                columns: table => new
                {
                    ProductModelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CatalogDescription = table.Column<string>(type: "xml", nullable: false),
                    Instructions = table.Column<string>(type: "xml", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductModel", x => x.ProductModelID);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductPhoto",
                columns: table => new
                {
                    ProductPhotoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ThumbNailPhoto = table.Column<byte[]>(type: "bytea", nullable: false),
                    ThumbnailPhotoFileName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LargePhoto = table.Column<byte[]>(type: "bytea", nullable: false),
                    LargePhotoFileName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductPhoto", x => x.ProductPhotoID);
                });

            migrationBuilder.CreateTable(
                name: "Production.ScrapReason",
                schema: "Production",
                columns: table => new
                {
                    ScrapReasonID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ScrapReason", x => x.ScrapReasonID);
                });

            migrationBuilder.CreateTable(
                name: "Production.TransactionHistoryArchive",
                schema: "Production",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ReferenceOrderID = table.Column<int>(type: "int", nullable: false),
                    ReferenceOrderLineID = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionType = table.Column<string>(type: "nchar", maxLength: 1, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ActualCost = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.TransactionHistoryArchive", x => x.TransactionID);
                });

            migrationBuilder.CreateTable(
                name: "Production.UnitMeasure",
                schema: "Production",
                columns: table => new
                {
                    UnitMeasureCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.UnitMeasure", x => x.UnitMeasureCode);
                });

            migrationBuilder.CreateTable(
                name: "Purchasing.ShipMethod",
                schema: "Purchasing",
                columns: table => new
                {
                    ShipMethodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShipBase = table.Column<decimal>(type: "money", nullable: false),
                    ShipRate = table.Column<decimal>(type: "money", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchasing.ShipMethod", x => x.ShipMethodID);
                });

            migrationBuilder.CreateTable(
                name: "Sales.CreditCard",
                columns: table => new
                {
                    CreditCardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CardNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ExpMonth = table.Column<byte>(type: "smallint", nullable: false),
                    ExpYear = table.Column<short>(type: "smallint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.CreditCard", x => x.CreditCardID);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesReason",
                columns: table => new
                {
                    SalesReasonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReasonType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesReason", x => x.SalesReasonID);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SpecialOffer",
                schema: "Sales",
                columns: table => new
                {
                    SpecialOfferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DiscountPct = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MinQty = table.Column<int>(type: "int", nullable: false),
                    MaxQty = table.Column<int>(type: "int", nullable: true),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SpecialOffer", x => x.SpecialOfferID);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                schema: "HumanResources",
                columns: table => new
                {
                    ShiftID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.ShiftID);
                });

            migrationBuilder.CreateTable(
                name: "StoreContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradingEconomicsCalendar",
                schema: "TradingEconomics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CalendarId = table.Column<string>(type: "text", nullable: false),
                    Importance = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Event = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    SourceUrl = table.Column<string>(type: "text", nullable: false),
                    Actual = table.Column<string>(type: "text", nullable: false),
                    Previous = table.Column<string>(type: "text", nullable: false),
                    Forecast = table.Column<string>(type: "text", nullable: false),
                    TeForecast = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    DateSpan = table.Column<string>(type: "text", nullable: false),
                    Revised = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Ticker = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingEconomicsCalendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                schema: "Shared",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BasketId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_Basket_BasketId",
                        column: x => x.BasketId,
                        principalSchema: "Shared",
                        principalTable: "Basket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreditRating = table.Column<byte>(type: "smallint", nullable: false),
                    PurchasingWebServiceURL = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Vendor_BusinessEntity_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "Person",
                        principalTable: "BusinessEntity",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.CurrencyRate",
                columns: table => new
                {
                    CurrencyRateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrencyRateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FromCurrencyCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    ToCurrencyCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    AverageRate = table.Column<decimal>(type: "money", nullable: false),
                    EndOfDayRate = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.CurrencyRate", x => x.CurrencyRateID);
                    table.ForeignKey(
                        name: "FK_Sales.CurrencyRate_Currency_FromCurrencyCode",
                        column: x => x.FromCurrencyCode,
                        principalSchema: "Sales",
                        principalTable: "Currency",
                        principalColumn: "CurrencyCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.CurrencyRate_Currency_ToCurrencyCode",
                        column: x => x.ToCurrencyCode,
                        principalSchema: "Sales",
                        principalTable: "Currency",
                        principalColumn: "CurrencyCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DurationInMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeBusinessEntityID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Employee_EmployeeBusinessEntityID",
                        column: x => x.EmployeeBusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayHistory",
                schema: "HumanResources",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    RateChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rate = table.Column<decimal>(type: "money", nullable: false),
                    PayFrequency = table.Column<byte>(type: "smallint", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayHistory", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_EmployeePayHistory_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobCandidate",
                schema: "HumanResources",
                columns: table => new
                {
                    JobCandidateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessEntityID = table.Column<int>(type: "int", nullable: true),
                    Resume = table.Column<string>(type: "text", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCandidate", x => x.JobCandidateID);
                    table.ForeignKey(
                        name: "FK_JobCandidate_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID");
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonType = table.Column<string>(type: "nchar", maxLength: 2, nullable: false),
                    Title = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Suffix = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    EmailPromotion = table.Column<int>(type: "int", nullable: false),
                    AdditionalContactInfo = table.Column<string>(type: "xml", nullable: false),
                    Demographics = table.Column<string>(type: "xml", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BusinessEntityID1 = table.Column<int>(type: "int", nullable: false),
                    EmployeeBusinessEntityID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Person_BusinessEntity_BusinessEntityID1",
                        column: x => x.BusinessEntityID1,
                        principalSchema: "Person",
                        principalTable: "BusinessEntity",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Employee_EmployeeBusinessEntityID",
                        column: x => x.EmployeeBusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID");
                });

            migrationBuilder.CreateTable(
                name: "Production.Document",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    DocumentLevel = table.Column<short>(type: "smallint", nullable: true),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    FileExtension = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Revision = table.Column<string>(type: "nchar", maxLength: 5, nullable: false),
                    ChangeNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DocumentSummary = table.Column<string>(type: "text", nullable: false),
                    Document = table.Column<byte[]>(type: "bytea", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.Document", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Production.Document_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.CountryRegionCurrency",
                columns: table => new
                {
                    CountryRegionCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.CountryRegionCurrency", x => new { x.CurrencyCode, x.CountryRegionCode });
                    table.ForeignKey(
                        name: "FK_Sales.CountryRegionCurrency_Currency_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalSchema: "Sales",
                        principalTable: "Currency",
                        principalColumn: "CurrencyCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.CountryRegionCurrency_Person.CountryRegion_CountryReg~",
                        column: x => x.CountryRegionCode,
                        principalTable: "Person.CountryRegion",
                        principalColumn: "CountryRegionCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesTerritory",
                schema: "Sales",
                columns: table => new
                {
                    TerritoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryRegionCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Group = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SalesYTD = table.Column<decimal>(type: "money", nullable: false),
                    SalesLastYear = table.Column<decimal>(type: "money", nullable: false),
                    CostYTD = table.Column<decimal>(type: "money", nullable: false),
                    CostLastYear = table.Column<decimal>(type: "money", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesTerritory", x => x.TerritoryID);
                    table.ForeignKey(
                        name: "FK_Sales.SalesTerritory_Person.CountryRegion_CountryRegionCode",
                        column: x => x.CountryRegionCode,
                        principalTable: "Person.CountryRegion",
                        principalColumn: "CountryRegionCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                schema: "Shared",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Company = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    Timezone = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LoginCount = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Subscription = table.Column<string>(type: "text", nullable: true),
                    UserTypeId = table.Column<string>(type: "text", nullable: true),
                    IsLoggedIn = table.Column<string>(type: "text", nullable: true),
                    NotificationSettingsId = table.Column<int>(type: "integer", nullable: false),
                    PrivacySettingsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfile_NotificationSettings_NotificationSettingsId",
                        column: x => x.NotificationSettingsId,
                        principalTable: "NotificationSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfile_PrivacySettings_PrivacySettingsId",
                        column: x => x.PrivacySettingsId,
                        principalTable: "PrivacySettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductSubcategory",
                columns: table => new
                {
                    ProductSubcategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCategoryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductSubcategory", x => x.ProductSubcategoryID);
                    table.ForeignKey(
                        name: "FK_Production.ProductSubcategory_Production.ProductCategory_Pr~",
                        column: x => x.ProductCategoryID,
                        principalTable: "Production.ProductCategory",
                        principalColumn: "ProductCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductModelIllustration",
                columns: table => new
                {
                    ProductModelID = table.Column<int>(type: "int", nullable: false),
                    IllustrationID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductModelIllustration", x => new { x.ProductModelID, x.IllustrationID });
                    table.ForeignKey(
                        name: "FK_Production.ProductModelIllustration_Production.Illustration~",
                        column: x => x.IllustrationID,
                        principalTable: "Production.Illustration",
                        principalColumn: "IllustrationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductModelIllustration_Production.ProductModel~",
                        column: x => x.ProductModelID,
                        principalTable: "Production.ProductModel",
                        principalColumn: "ProductModelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductModelProductDescriptionCulture",
                columns: table => new
                {
                    ProductModelID = table.Column<int>(type: "int", nullable: false),
                    ProductDescriptionID = table.Column<int>(type: "int", nullable: false),
                    CultureID = table.Column<string>(type: "nchar", maxLength: 6, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductModelProductDescriptionCulture", x => new { x.ProductModelID, x.ProductDescriptionID, x.CultureID });
                    table.ForeignKey(
                        name: "FK_Production.ProductModelProductDescriptionCulture_Production~",
                        column: x => x.CultureID,
                        principalTable: "Production.Culture",
                        principalColumn: "CultureID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductModelProductDescriptionCulture_Productio~1",
                        column: x => x.ProductDescriptionID,
                        principalTable: "Production.ProductDescription",
                        principalColumn: "ProductDescriptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductModelProductDescriptionCulture_Productio~2",
                        column: x => x.ProductModelID,
                        principalTable: "Production.ProductModel",
                        principalColumn: "ProductModelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDepartmentHistory",
                schema: "HumanResources",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    DepartmentID = table.Column<short>(type: "smallint", nullable: false),
                    ShiftID = table.Column<int>(type: "integer", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDepartmentHistory", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartmentHistory_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalSchema: "HumanResources",
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartmentHistory_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartmentHistory_Shift_ShiftID",
                        column: x => x.ShiftID,
                        principalSchema: "HumanResources",
                        principalTable: "Shift",
                        principalColumn: "ShiftID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchasing.PurchaseOrderHeader",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisionNumber = table.Column<byte>(type: "smallint", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false),
                    ShipMethodID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShipDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubTotal = table.Column<decimal>(type: "money", nullable: false),
                    TaxAmt = table.Column<decimal>(type: "money", nullable: false),
                    Freight = table.Column<decimal>(type: "money", nullable: false),
                    TotalDue = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchasing.PurchaseOrderHeader", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_Purchasing.PurchaseOrderHeader_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchasing.PurchaseOrderHeader_Purchasing.ShipMethod_ShipMe~",
                        column: x => x.ShipMethodID,
                        principalSchema: "Purchasing",
                        principalTable: "Purchasing.ShipMethod",
                        principalColumn: "ShipMethodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchasing.PurchaseOrderHeader_Vendor_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Vendor",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.BusinessEntityContact",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    ContactTypeID = table.Column<int>(type: "int", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.BusinessEntityContact", x => new { x.BusinessEntityID, x.PersonID, x.ContactTypeID });
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityContact_BusinessEntity_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "Person",
                        principalTable: "BusinessEntity",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityContact_Person.ContactType_ContactType~",
                        column: x => x.ContactTypeID,
                        principalTable: "Person.ContactType",
                        principalColumn: "ContactTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityContact_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.EmailAddress",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    EmailAddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.EmailAddress", x => new { x.BusinessEntityID, x.EmailAddressID });
                    table.ForeignKey(
                        name: "FK_Person.EmailAddress_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.Password",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar", maxLength: 128, nullable: false),
                    PasswordSalt = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.Password", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Person.Password_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.PersonPhone",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PhoneNumberTypeID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.PersonPhone", x => new { x.BusinessEntityID, x.PhoneNumber });
                    table.ForeignKey(
                        name: "FK_Person.PersonPhone_Person.PhoneNumberType_PhoneNumberTypeID",
                        column: x => x.PhoneNumberTypeID,
                        principalTable: "Person.PhoneNumberType",
                        principalColumn: "PhoneNumberTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.PersonPhone_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.PersonCreditCard",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    CreditCardID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.PersonCreditCard", x => new { x.BusinessEntityID, x.CreditCardID });
                    table.ForeignKey(
                        name: "FK_Sales.PersonCreditCard_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.PersonCreditCard_Sales.CreditCard_CreditCardID",
                        column: x => x.CreditCardID,
                        principalTable: "Sales.CreditCard",
                        principalColumn: "CreditCardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.StateProvince",
                schema: "Person",
                columns: table => new
                {
                    StateProvinceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateProvinceCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    CountryRegionCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TerritoryID = table.Column<int>(type: "int", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.StateProvince", x => x.StateProvinceID);
                    table.ForeignKey(
                        name: "FK_Person.StateProvince_Person.CountryRegion_CountryRegionCode",
                        column: x => x.CountryRegionCode,
                        principalTable: "Person.CountryRegion",
                        principalColumn: "CountryRegionCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.StateProvince_Sales.SalesTerritory_TerritoryID",
                        column: x => x.TerritoryID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SalesTerritory",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesPerson",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    TerritoryID = table.Column<int>(type: "int", nullable: true),
                    SalesQuota = table.Column<decimal>(type: "money", nullable: true),
                    Bonus = table.Column<decimal>(type: "money", nullable: false),
                    CommissionPct = table.Column<decimal>(type: "numeric", nullable: false),
                    SalesYTD = table.Column<decimal>(type: "money", nullable: false),
                    SalesLastYear = table.Column<decimal>(type: "money", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesPerson", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Sales.SalesPerson_Employee_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesPerson_Sales.SalesTerritory_TerritoryID",
                        column: x => x.TerritoryID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SalesTerritory",
                        principalColumn: "TerritoryID");
                });

            migrationBuilder.CreateTable(
                name: "Production.Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Color = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    SafetyStockLevel = table.Column<short>(type: "smallint", nullable: false),
                    ReorderPoint = table.Column<short>(type: "smallint", nullable: false),
                    StandardCost = table.Column<decimal>(type: "money", nullable: false),
                    ListPrice = table.Column<decimal>(type: "money", nullable: false),
                    Size = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SizeUnitMeasureCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    WeightUnitMeasureCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal", nullable: true),
                    DaysToManufacture = table.Column<int>(type: "int", nullable: false),
                    ProductLine = table.Column<string>(type: "nchar", maxLength: 2, nullable: false),
                    Class = table.Column<string>(type: "nchar", maxLength: 2, nullable: false),
                    Style = table.Column<string>(type: "nchar", maxLength: 2, nullable: false),
                    ProductSubcategoryID = table.Column<int>(type: "int", nullable: true),
                    ProductModelID = table.Column<int>(type: "int", nullable: true),
                    SellStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DiscontinuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Production.Product_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Production.Product_Production.ProductModel_ProductModelID",
                        column: x => x.ProductModelID,
                        principalTable: "Production.ProductModel",
                        principalColumn: "ProductModelID");
                    table.ForeignKey(
                        name: "FK_Production.Product_Production.ProductSubcategory_ProductSub~",
                        column: x => x.ProductSubcategoryID,
                        principalTable: "Production.ProductSubcategory",
                        principalColumn: "ProductSubcategoryID");
                    table.ForeignKey(
                        name: "FK_Production.Product_Production.UnitMeasure_SizeUnitMeasureCo~",
                        column: x => x.SizeUnitMeasureCode,
                        principalSchema: "Production",
                        principalTable: "Production.UnitMeasure",
                        principalColumn: "UnitMeasureCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.Product_Production.UnitMeasure_WeightUnitMeasure~",
                        column: x => x.WeightUnitMeasureCode,
                        principalSchema: "Production",
                        principalTable: "Production.UnitMeasure",
                        principalColumn: "UnitMeasureCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Person",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressLine1 = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    AddressLine2 = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    City = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    StateProvinceID = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<int>(type: "integer", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Person.StateProvince_StateProvinceID",
                        column: x => x.StateProvinceID,
                        principalSchema: "Person",
                        principalTable: "Person.StateProvince",
                        principalColumn: "StateProvinceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesTaxRate",
                schema: "Sales",
                columns: table => new
                {
                    SalesTaxRateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateProvinceID = table.Column<int>(type: "int", nullable: false),
                    TaxType = table.Column<byte>(type: "smallint", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesTaxRate", x => x.SalesTaxRateID);
                    table.ForeignKey(
                        name: "FK_Sales.SalesTaxRate_Person.StateProvince_StateProvinceID",
                        column: x => x.StateProvinceID,
                        principalSchema: "Person",
                        principalTable: "Person.StateProvince",
                        principalColumn: "StateProvinceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesPersonQuotaHistory",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    QuotaDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SalesQuota = table.Column<decimal>(type: "money", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesPersonQuotaHistory", x => new { x.BusinessEntityID, x.QuotaDate });
                    table.ForeignKey(
                        name: "FK_Sales.SalesPersonQuotaHistory_Sales.SalesPerson_BusinessEnt~",
                        column: x => x.BusinessEntityID,
                        principalTable: "Sales.SalesPerson",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesTerritoryHistory",
                schema: "Sales",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TerritoryID = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesTerritoryHistory", x => new { x.BusinessEntityID, x.TerritoryID });
                    table.ForeignKey(
                        name: "FK_Sales.SalesTerritoryHistory_Sales.SalesPerson_BusinessEntit~",
                        column: x => x.BusinessEntityID,
                        principalTable: "Sales.SalesPerson",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesTerritoryHistory_Sales.SalesTerritory_TerritoryID",
                        column: x => x.TerritoryID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SalesTerritory",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: true),
                    Demographics = table.Column<string>(type: "xml", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.BusinessEntityID);
                    table.ForeignKey(
                        name: "FK_Store_BusinessEntity_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "Person",
                        principalTable: "BusinessEntity",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Store_Sales.SalesPerson_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Sales.SalesPerson",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.SpecialOfferProduct",
                schema: "Person",
                columns: table => new
                {
                    SpecialOfferID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.SpecialOfferProduct", x => new { x.SpecialOfferID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_Person.SpecialOfferProduct_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.SpecialOfferProduct_Sales.SpecialOffer_SpecialOfferID",
                        column: x => x.SpecialOfferID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SpecialOffer",
                        principalColumn: "SpecialOfferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.BillOfMaterials",
                columns: table => new
                {
                    BillOfMaterialsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductAssemblyID = table.Column<int>(type: "int", nullable: true),
                    ComponentID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UnitMeasureCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    BOMLevel = table.Column<short>(type: "smallint", nullable: false),
                    PerAssemblyQty = table.Column<decimal>(type: "decimal", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProductAssemblyID1 = table.Column<int>(type: "int", nullable: false),
                    ComponentID1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.BillOfMaterials", x => x.BillOfMaterialsID);
                    table.ForeignKey(
                        name: "FK_Production.BillOfMaterials_Production.Product_ComponentID1",
                        column: x => x.ComponentID1,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.BillOfMaterials_Production.Product_ProductAssemb~",
                        column: x => x.ProductAssemblyID1,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.BillOfMaterials_Production.UnitMeasure_UnitMeasu~",
                        column: x => x.UnitMeasureCode,
                        principalSchema: "Production",
                        principalTable: "Production.UnitMeasure",
                        principalColumn: "UnitMeasureCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductCostHistory",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StandardCost = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductCostHistory", x => new { x.ProductID, x.StartDate });
                    table.ForeignKey(
                        name: "FK_Production.ProductCostHistory_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductDocument",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    DocumentNode = table.Column<string>(type: "text", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocumentBusinessEntityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductDocument", x => new { x.ProductID, x.DocumentNode });
                    table.ForeignKey(
                        name: "FK_Production.ProductDocument_Production.Document_DocumentBusi~",
                        column: x => x.DocumentBusinessEntityID,
                        principalTable: "Production.Document",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductDocument_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductInventory",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<short>(type: "smallint", nullable: false),
                    Shelf = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Bin = table.Column<byte>(type: "smallint", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductInventory", x => new { x.ProductID, x.LocationID });
                    table.ForeignKey(
                        name: "FK_Production.ProductInventory_Production.Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Production.Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductInventory_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductListPriceHistory",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ListPrice = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductListPriceHistory", x => new { x.ProductID, x.StartDate });
                    table.ForeignKey(
                        name: "FK_Production.ProductListPriceHistory_Production.Product_Produ~",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductProductPhoto",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductPhotoID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductProductPhoto", x => new { x.ProductID, x.ProductPhotoID });
                    table.ForeignKey(
                        name: "FK_Production.ProductProductPhoto_Production.ProductPhoto_Prod~",
                        column: x => x.ProductPhotoID,
                        principalTable: "Production.ProductPhoto",
                        principalColumn: "ProductPhotoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.ProductProductPhoto_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.ProductReview",
                columns: table => new
                {
                    ProductReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ReviewerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "character varying(3850)", maxLength: 3850, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.ProductReview", x => x.ProductReviewID);
                    table.ForeignKey(
                        name: "FK_Production.ProductReview_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.TransactionHistory",
                schema: "Production",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ReferenceOrderID = table.Column<int>(type: "int", nullable: false),
                    ReferenceOrderLineID = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionType = table.Column<string>(type: "nchar", maxLength: 1, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ActualCost = table.Column<decimal>(type: "money", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.TransactionHistory", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Production.TransactionHistory_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.WorkOrder",
                schema: "Production",
                columns: table => new
                {
                    WorkOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    OrderQty = table.Column<int>(type: "int", nullable: false),
                    StockedQty = table.Column<int>(type: "int", nullable: false),
                    ScrappedQty = table.Column<short>(type: "smallint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScrapReasonID = table.Column<short>(type: "smallint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.WorkOrder", x => x.WorkOrderID);
                    table.ForeignKey(
                        name: "FK_Production.WorkOrder_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.WorkOrder_Production.ScrapReason_ScrapReasonID",
                        column: x => x.ScrapReasonID,
                        principalSchema: "Production",
                        principalTable: "Production.ScrapReason",
                        principalColumn: "ScrapReasonID");
                });

            migrationBuilder.CreateTable(
                name: "Purchasing.ProductVendor",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    AverageLeadTime = table.Column<int>(type: "int", nullable: false),
                    StandardPrice = table.Column<decimal>(type: "money", nullable: false),
                    LastReceiptCost = table.Column<decimal>(type: "money", nullable: true),
                    LastReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MinOrderQty = table.Column<int>(type: "int", nullable: false),
                    MaxOrderQty = table.Column<int>(type: "int", nullable: false),
                    OnOrderQty = table.Column<int>(type: "int", nullable: true),
                    UnitMeasureCode = table.Column<string>(type: "nchar", maxLength: 3, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchasing.ProductVendor", x => new { x.ProductID, x.BusinessEntityID });
                    table.ForeignKey(
                        name: "FK_Purchasing.ProductVendor_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchasing.ProductVendor_Production.UnitMeasure_UnitMeasure~",
                        column: x => x.UnitMeasureCode,
                        principalSchema: "Production",
                        principalTable: "Production.UnitMeasure",
                        principalColumn: "UnitMeasureCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchasing.ProductVendor_Vendor_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Vendor",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchasing.PurchaseOrderDetail",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderQty = table.Column<short>(type: "smallint", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    LineTotal = table.Column<decimal>(type: "money", nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "decimal", nullable: false),
                    RejectedQty = table.Column<decimal>(type: "decimal", nullable: false),
                    StockedQty = table.Column<decimal>(type: "decimal", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchasing.PurchaseOrderDetail", x => new { x.ProductID, x.PurchaseOrderDetailID });
                    table.ForeignKey(
                        name: "FK_Purchasing.PurchaseOrderDetail_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchasing.PurchaseOrderDetail_Purchasing.PurchaseOrderHead~",
                        column: x => x.PurchaseOrderID,
                        principalTable: "Purchasing.PurchaseOrderHeader",
                        principalColumn: "PurchaseOrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.ShoppingCartItem",
                schema: "Sales",
                columns: table => new
                {
                    ShoppingCartItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingCartID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.ShoppingCartItem", x => x.ShoppingCartItemID);
                    table.ForeignKey(
                        name: "FK_Sales.ShoppingCartItem_Production.Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Production.Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Person",
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HumanResources",
                        principalTable: "Employee",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person.BusinessEntityAddress",
                columns: table => new
                {
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    AddressTypeID = table.Column<int>(type: "int", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person.BusinessEntityAddress", x => new { x.BusinessEntityID, x.AddressID });
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityAddress_AddressType_AddressTypeID",
                        column: x => x.AddressTypeID,
                        principalSchema: "Person",
                        principalTable: "AddressType",
                        principalColumn: "AddressTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityAddress_Address_AddressID",
                        column: x => x.AddressID,
                        principalSchema: "Person",
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person.BusinessEntityAddress_BusinessEntity_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalSchema: "Person",
                        principalTable: "BusinessEntity",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonID = table.Column<int>(type: "int", nullable: true),
                    StoreID = table.Column<int>(type: "int", nullable: true),
                    TerritoryID = table.Column<int>(type: "int", nullable: true),
                    AccountNumber = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.Customer", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Sales.Customer_Person_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Person",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.Customer_Sales.SalesTerritory_TerritoryID",
                        column: x => x.TerritoryID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SalesTerritory",
                        principalColumn: "TerritoryID");
                    table.ForeignKey(
                        name: "FK_Sales.Customer_Store_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Store",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Production.WorkOrderRouting",
                schema: "Production",
                columns: table => new
                {
                    WorkOrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    OperationSequence = table.Column<short>(type: "smallint", nullable: true),
                    LocationID = table.Column<short>(type: "smallint", nullable: false),
                    ScheduledStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualResourceHrs = table.Column<decimal>(type: "decimal", nullable: true),
                    PlannedCost = table.Column<decimal>(type: "money", nullable: false),
                    ActualCost = table.Column<decimal>(type: "money", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production.WorkOrderRouting", x => new { x.WorkOrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_Production.WorkOrderRouting_Production.Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Production.Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Production.WorkOrderRouting_Production.WorkOrder_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalSchema: "Production",
                        principalTable: "Production.WorkOrder",
                        principalColumn: "WorkOrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesOrderHeader",
                columns: table => new
                {
                    SalesOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisionNumber = table.Column<byte>(type: "smallint", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShipDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    SalesOrderNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalesPersonID = table.Column<int>(type: "int", nullable: true),
                    TerritoryID = table.Column<int>(type: "int", nullable: true),
                    BillToAddressID = table.Column<int>(type: "int", nullable: false),
                    ShipToAddressID = table.Column<int>(type: "int", nullable: false),
                    ShipMethodID = table.Column<int>(type: "int", nullable: false),
                    CreditCardID = table.Column<int>(type: "int", nullable: true),
                    CreditCardApprovalCode = table.Column<string>(type: "varchar", maxLength: 15, nullable: false),
                    CurrencyRateID = table.Column<int>(type: "int", nullable: true),
                    SubTotal = table.Column<decimal>(type: "money", nullable: false),
                    TaxAmt = table.Column<decimal>(type: "money", nullable: false),
                    Freight = table.Column<decimal>(type: "money", nullable: false),
                    TotalDue = table.Column<decimal>(type: "money", nullable: false),
                    Comment = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BusinessEntityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesOrderHeader", x => x.SalesOrderID);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Address_BillToAddressID",
                        column: x => x.BillToAddressID,
                        principalSchema: "Person",
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Address_ShipToAddressID",
                        column: x => x.ShipToAddressID,
                        principalSchema: "Person",
                        principalTable: "Address",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Purchasing.ShipMethod_ShipMethodID",
                        column: x => x.ShipMethodID,
                        principalSchema: "Purchasing",
                        principalTable: "Purchasing.ShipMethod",
                        principalColumn: "ShipMethodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Sales.CreditCard_CreditCardID",
                        column: x => x.CreditCardID,
                        principalTable: "Sales.CreditCard",
                        principalColumn: "CreditCardID");
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Sales.CurrencyRate_CurrencyRateID",
                        column: x => x.CurrencyRateID,
                        principalTable: "Sales.CurrencyRate",
                        principalColumn: "CurrencyRateID");
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Sales.Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Sales.Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Sales.SalesPerson_BusinessEntityID",
                        column: x => x.BusinessEntityID,
                        principalTable: "Sales.SalesPerson",
                        principalColumn: "BusinessEntityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeader_Sales.SalesTerritory_TerritoryID",
                        column: x => x.TerritoryID,
                        principalSchema: "Sales",
                        principalTable: "Sales.SalesTerritory",
                        principalColumn: "TerritoryID");
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesOrderDetail",
                columns: table => new
                {
                    SalesOrderID = table.Column<int>(type: "int", nullable: false),
                    SalesOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarrierTrackingNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    OrderQty = table.Column<short>(type: "smallint", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    SpecialOfferID = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    UnitPriceDiscount = table.Column<decimal>(type: "money", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    rowguid = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesOrderDetail", x => new { x.SalesOrderID, x.SalesOrderDetailID });
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderDetail_Person.SpecialOfferProduct_SpecialOf~",
                        columns: x => new { x.SpecialOfferID, x.ProductID },
                        principalSchema: "Person",
                        principalTable: "Person.SpecialOfferProduct",
                        principalColumns: new[] { "SpecialOfferID", "ProductID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderDetail_Sales.SalesOrderHeader_SalesOrderID",
                        column: x => x.SalesOrderID,
                        principalTable: "Sales.SalesOrderHeader",
                        principalColumn: "SalesOrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales.SalesOrderHeaderSalesReason",
                columns: table => new
                {
                    SalesOrderID = table.Column<int>(type: "int", nullable: false),
                    SalesReasonID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales.SalesOrderHeaderSalesReason", x => new { x.SalesOrderID, x.SalesReasonID });
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeaderSalesReason_Sales.SalesOrderHeader_Sa~",
                        column: x => x.SalesOrderID,
                        principalTable: "Sales.SalesOrderHeader",
                        principalColumn: "SalesOrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales.SalesOrderHeaderSalesReason_Sales.SalesReason_SalesRe~",
                        column: x => x.SalesReasonID,
                        principalTable: "Sales.SalesReason",
                        principalColumn: "SalesReasonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Person",
                table: "AddressType",
                columns: new[] { "AddressTypeID", "ModifiedDate", "Name", "rowguid" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Billing", new Guid("ea71b71c-4e46-4815-9ba4-6ab3eb5d0aef") },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Home", new Guid("3fb98e4f-6e9b-4709-9f83-167a2022261d") },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main Office", new Guid("a5cecf60-7dea-4f44-87ed-54d8f7ce1c38") },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Primary", new Guid("20e92d96-73cf-4c51-825e-b6d77c90601a") },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shipping", new Guid("ba7c5704-e938-4694-85a9-99534cb1d4c2") },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Archive", new Guid("b12f97d1-aa1f-4043-ac86-1b83525b0228") }
                });

            migrationBuilder.InsertData(
                schema: "Person",
                table: "BusinessEntity",
                columns: new[] { "BusinessEntityID", "ModifiedDate", "rowguid" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("679ee679-e0aa-4469-aa42-b993b47bcf75") });

            migrationBuilder.InsertData(
                schema: "Sales",
                table: "Currency",
                columns: new[] { "CurrencyCode", "ModifiedDate", "Name" },
                values: new object[] { "ZAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rand" });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "Department",
                columns: new[] { "DepartmentID", "GroupName", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "Research and Development", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Engineering" },
                    { 2, "Research and Development", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tool Design" },
                    { 3, "Sales and Marketing", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sales" },
                    { 4, "Sales and Marketing", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Marketing" },
                    { 5, "Inventory Management", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Purchasing" },
                    { 6, "Research and Development", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Research and Development" },
                    { 7, "Manufacturing", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Production" },
                    { 8, "Manufacturing", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Production Control" },
                    { 9, "Executive General and Administration", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Human Resources" },
                    { 10, "Executive General and Administration", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finance" },
                    { 11, "Executive General and Administration", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Information Services" },
                    { 12, "Quality Assurance", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sales" },
                    { 13, "Quality Assurance", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quality Assurance" },
                    { 14, "Executive General and Administration", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Facilities and Maintenance" },
                    { 15, "Sales and Inventory Management", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shipping and Receiving" },
                    { 16, "Executive General and Administration", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Executive" }
                });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "Employee",
                columns: new[] { "BusinessEntityID", "BirthDate", "EntityVersion", "Gender", "HireDate", "JobLevel", "JobTitle", "LoginID", "MaritalStatus", "MinimumRaiseGiven", "ModifiedDate", "NationalIDNumber", "rowguid", "Salary", "SickLeaveHours", "SuggestedBonus", "VacationHours", "YearsInService" },
                values: new object[] { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "M", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chief Executive Officer", "adventure-works\\ken0", "S", false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "295847284", new Guid("956ce7f1-c0b5-41b6-9cf1-150966904c9c"), 5000, (short)3, 200, (short)0, 10 });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "JobCandidate",
                columns: new[] { "JobCandidateID", "BusinessEntityID", "ModifiedDate", "Resume" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LinkedIn" },
                    { 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LinkedIn" }
                });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "Shift",
                columns: new[] { "ShiftID", "EndTime", "ModifiedDate", "Name", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 15, 0, 0, 0), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Day", new TimeSpan(0, 7, 0, 0, 0) },
                    { 2, new TimeSpan(0, 23, 0, 0, 0), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Evening", new TimeSpan(0, 15, 0, 0, 0) },
                    { 3, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Night", new TimeSpan(0, 23, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "EmployeeDepartmentHistory",
                columns: new[] { "BusinessEntityID", "DepartmentID", "EndDate", "ModifiedDate", "ShiftID", "StartDate" },
                values: new object[] { 1, (short)16, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "HumanResources",
                table: "EmployeePayHistory",
                columns: new[] { "BusinessEntityID", "ModifiedDate", "PayFrequency", "Rate", "RateChangeDate" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), (byte)2, 125.5m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_Address_StateProvinceID",
                schema: "Person",
                table: "Address",
                column: "StateProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketId",
                schema: "Shared",
                table: "BasketItem",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EmployeeBusinessEntityID",
                table: "Courses",
                column: "EmployeeBusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_AddressId",
                table: "EmployeeAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartmentHistory_DepartmentID",
                schema: "HumanResources",
                table: "EmployeeDepartmentHistory",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartmentHistory_ShiftID",
                schema: "HumanResources",
                table: "EmployeeDepartmentHistory",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_JobCandidate_BusinessEntityID",
                schema: "HumanResources",
                table: "JobCandidate",
                column: "BusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_BusinessEntityID1",
                table: "Person",
                column: "BusinessEntityID1");

            migrationBuilder.CreateIndex(
                name: "IX_Person_EmployeeBusinessEntityID",
                table: "Person",
                column: "EmployeeBusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.BusinessEntityAddress_AddressID",
                table: "Person.BusinessEntityAddress",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.BusinessEntityAddress_AddressTypeID",
                table: "Person.BusinessEntityAddress",
                column: "AddressTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.BusinessEntityContact_ContactTypeID",
                table: "Person.BusinessEntityContact",
                column: "ContactTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.PersonPhone_PhoneNumberTypeID",
                table: "Person.PersonPhone",
                column: "PhoneNumberTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.SpecialOfferProduct_ProductID",
                schema: "Person",
                table: "Person.SpecialOfferProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Person.StateProvince_CountryRegionCode",
                schema: "Person",
                table: "Person.StateProvince",
                column: "CountryRegionCode");

            migrationBuilder.CreateIndex(
                name: "IX_Person.StateProvince_TerritoryID",
                schema: "Person",
                table: "Person.StateProvince",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.BillOfMaterials_ComponentID1",
                table: "Production.BillOfMaterials",
                column: "ComponentID1");

            migrationBuilder.CreateIndex(
                name: "IX_Production.BillOfMaterials_ProductAssemblyID1",
                table: "Production.BillOfMaterials",
                column: "ProductAssemblyID1");

            migrationBuilder.CreateIndex(
                name: "IX_Production.BillOfMaterials_UnitMeasureCode",
                table: "Production.BillOfMaterials",
                column: "UnitMeasureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Production.Product_CategoryId",
                table: "Production.Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Production.Product_ProductModelID",
                table: "Production.Product",
                column: "ProductModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.Product_ProductSubcategoryID",
                table: "Production.Product",
                column: "ProductSubcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.Product_SizeUnitMeasureCode",
                table: "Production.Product",
                column: "SizeUnitMeasureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Production.Product_WeightUnitMeasureCode",
                table: "Production.Product",
                column: "WeightUnitMeasureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductDocument_DocumentBusinessEntityID",
                table: "Production.ProductDocument",
                column: "DocumentBusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductInventory_LocationID",
                table: "Production.ProductInventory",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductModelIllustration_IllustrationID",
                table: "Production.ProductModelIllustration",
                column: "IllustrationID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductModelProductDescriptionCulture_CultureID",
                table: "Production.ProductModelProductDescriptionCulture",
                column: "CultureID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductModelProductDescriptionCulture_ProductDes~",
                table: "Production.ProductModelProductDescriptionCulture",
                column: "ProductDescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductProductPhoto_ProductPhotoID",
                table: "Production.ProductProductPhoto",
                column: "ProductPhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductReview_ProductID",
                table: "Production.ProductReview",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.ProductSubcategory_ProductCategoryID",
                table: "Production.ProductSubcategory",
                column: "ProductCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.TransactionHistory_ProductID",
                schema: "Production",
                table: "Production.TransactionHistory",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.WorkOrder_ProductID",
                schema: "Production",
                table: "Production.WorkOrder",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.WorkOrder_ScrapReasonID",
                schema: "Production",
                table: "Production.WorkOrder",
                column: "ScrapReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Production.WorkOrderRouting_LocationID",
                schema: "Production",
                table: "Production.WorkOrderRouting",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchasing.ProductVendor_BusinessEntityID",
                table: "Purchasing.ProductVendor",
                column: "BusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchasing.ProductVendor_UnitMeasureCode",
                table: "Purchasing.ProductVendor",
                column: "UnitMeasureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Purchasing.PurchaseOrderDetail_PurchaseOrderID",
                table: "Purchasing.PurchaseOrderDetail",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchasing.PurchaseOrderHeader_BusinessEntityID",
                table: "Purchasing.PurchaseOrderHeader",
                column: "BusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchasing.PurchaseOrderHeader_ShipMethodID",
                table: "Purchasing.PurchaseOrderHeader",
                column: "ShipMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.CountryRegionCurrency_CountryRegionCode",
                table: "Sales.CountryRegionCurrency",
                column: "CountryRegionCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.CurrencyRate_FromCurrencyCode",
                table: "Sales.CurrencyRate",
                column: "FromCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.CurrencyRate_ToCurrencyCode",
                table: "Sales.CurrencyRate",
                column: "ToCurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.Customer_BusinessEntityID",
                table: "Sales.Customer",
                column: "BusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.Customer_TerritoryID",
                table: "Sales.Customer",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.PersonCreditCard_CreditCardID",
                table: "Sales.PersonCreditCard",
                column: "CreditCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderDetail_SpecialOfferID_ProductID",
                table: "Sales.SalesOrderDetail",
                columns: new[] { "SpecialOfferID", "ProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_BillToAddressID",
                table: "Sales.SalesOrderHeader",
                column: "BillToAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_BusinessEntityID",
                table: "Sales.SalesOrderHeader",
                column: "BusinessEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_CreditCardID",
                table: "Sales.SalesOrderHeader",
                column: "CreditCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_CurrencyRateID",
                table: "Sales.SalesOrderHeader",
                column: "CurrencyRateID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_CustomerID",
                table: "Sales.SalesOrderHeader",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_ShipMethodID",
                table: "Sales.SalesOrderHeader",
                column: "ShipMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_ShipToAddressID",
                table: "Sales.SalesOrderHeader",
                column: "ShipToAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeader_TerritoryID",
                table: "Sales.SalesOrderHeader",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesOrderHeaderSalesReason_SalesReasonID",
                table: "Sales.SalesOrderHeaderSalesReason",
                column: "SalesReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesPerson_TerritoryID",
                table: "Sales.SalesPerson",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesTaxRate_StateProvinceID",
                schema: "Sales",
                table: "Sales.SalesTaxRate",
                column: "StateProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesTerritory_CountryRegionCode",
                schema: "Sales",
                table: "Sales.SalesTerritory",
                column: "CountryRegionCode");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.SalesTerritoryHistory_TerritoryID",
                schema: "Sales",
                table: "Sales.SalesTerritoryHistory",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Sales.ShoppingCartItem_ProductID",
                schema: "Sales",
                table: "Sales.ShoppingCartItem",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_NotificationSettingsId",
                schema: "Shared",
                table: "UserProfile",
                column: "NotificationSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_PrivacySettingsId",
                schema: "Shared",
                table: "UserProfile",
                column: "PrivacySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UserId",
                schema: "Shared",
                table: "UserProfile",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BasketItem",
                schema: "Shared");

            migrationBuilder.DropTable(
                name: "BuildVersion",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ChatRequests");

            migrationBuilder.DropTable(
                name: "CompetitionMatches");

            migrationBuilder.DropTable(
                name: "ComtradeCategories",
                schema: "TradingEconomics");

            migrationBuilder.DropTable(
                name: "ContactCreditCards");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.DropTable(
                name: "DatabaseLog");

            migrationBuilder.DropTable(
                name: "EmployeeAddresses");

            migrationBuilder.DropTable(
                name: "EmployeeDepartmentHistory",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "EmployeePayHistory",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "Feature",
                schema: "Shared");

            migrationBuilder.DropTable(
                name: "Forecast",
                schema: "TradingEconomics");

            migrationBuilder.DropTable(
                name: "Individuals");

            migrationBuilder.DropTable(
                name: "JobCandidate",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrionCalendarEvent");

            migrationBuilder.DropTable(
                name: "Person.BusinessEntityAddress");

            migrationBuilder.DropTable(
                name: "Person.BusinessEntityContact");

            migrationBuilder.DropTable(
                name: "Person.EmailAddress");

            migrationBuilder.DropTable(
                name: "Person.Password");

            migrationBuilder.DropTable(
                name: "Person.PersonPhone");

            migrationBuilder.DropTable(
                name: "Production.BillOfMaterials");

            migrationBuilder.DropTable(
                name: "Production.ProductCostHistory");

            migrationBuilder.DropTable(
                name: "Production.ProductDocument");

            migrationBuilder.DropTable(
                name: "Production.ProductInventory");

            migrationBuilder.DropTable(
                name: "Production.ProductListPriceHistory");

            migrationBuilder.DropTable(
                name: "Production.ProductModelIllustration");

            migrationBuilder.DropTable(
                name: "Production.ProductModelProductDescriptionCulture");

            migrationBuilder.DropTable(
                name: "Production.ProductProductPhoto");

            migrationBuilder.DropTable(
                name: "Production.ProductReview");

            migrationBuilder.DropTable(
                name: "Production.TransactionHistory",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Production.TransactionHistoryArchive",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Production.WorkOrderRouting",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Purchasing.ProductVendor");

            migrationBuilder.DropTable(
                name: "Purchasing.PurchaseOrderDetail");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Sales.CountryRegionCurrency");

            migrationBuilder.DropTable(
                name: "Sales.PersonCreditCard");

            migrationBuilder.DropTable(
                name: "Sales.SalesOrderDetail");

            migrationBuilder.DropTable(
                name: "Sales.SalesOrderHeaderSalesReason");

            migrationBuilder.DropTable(
                name: "Sales.SalesPersonQuotaHistory");

            migrationBuilder.DropTable(
                name: "Sales.SalesTaxRate",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Sales.SalesTerritoryHistory",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Sales.ShoppingCartItem",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "StoreContacts");

            migrationBuilder.DropTable(
                name: "TradingEconomicsCalendar",
                schema: "TradingEconomics");

            migrationBuilder.DropTable(
                name: "UserProfile",
                schema: "Shared");

            migrationBuilder.DropTable(
                name: "VendorAddresses");

            migrationBuilder.DropTable(
                name: "VendorContacts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Basket",
                schema: "Shared");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "Shift",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "AddressType",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Person.ContactType");

            migrationBuilder.DropTable(
                name: "Person.PhoneNumberType");

            migrationBuilder.DropTable(
                name: "Production.Document");

            migrationBuilder.DropTable(
                name: "Production.Illustration");

            migrationBuilder.DropTable(
                name: "Production.Culture");

            migrationBuilder.DropTable(
                name: "Production.ProductDescription");

            migrationBuilder.DropTable(
                name: "Production.ProductPhoto");

            migrationBuilder.DropTable(
                name: "Production.Location");

            migrationBuilder.DropTable(
                name: "Production.WorkOrder",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Purchasing.PurchaseOrderHeader");

            migrationBuilder.DropTable(
                name: "Person.SpecialOfferProduct",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Sales.SalesOrderHeader");

            migrationBuilder.DropTable(
                name: "Sales.SalesReason");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "NotificationSettings");

            migrationBuilder.DropTable(
                name: "PrivacySettings");

            migrationBuilder.DropTable(
                name: "Production.ScrapReason",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropTable(
                name: "Production.Product");

            migrationBuilder.DropTable(
                name: "Sales.SpecialOffer",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Purchasing.ShipMethod",
                schema: "Purchasing");

            migrationBuilder.DropTable(
                name: "Sales.CreditCard");

            migrationBuilder.DropTable(
                name: "Sales.CurrencyRate");

            migrationBuilder.DropTable(
                name: "Sales.Customer");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Production.ProductModel");

            migrationBuilder.DropTable(
                name: "Production.ProductSubcategory");

            migrationBuilder.DropTable(
                name: "Production.UnitMeasure",
                schema: "Production");

            migrationBuilder.DropTable(
                name: "Person.StateProvince",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Production.ProductCategory");

            migrationBuilder.DropTable(
                name: "BusinessEntity",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Sales.SalesPerson");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "HumanResources");

            migrationBuilder.DropTable(
                name: "Sales.SalesTerritory",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "Person.CountryRegion");
        }
    }
}
