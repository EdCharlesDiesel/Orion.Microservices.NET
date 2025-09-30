using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Orion.Admin.Areas.API;
using Orion.Admin.Controllers;
using Orion.Admin.Security;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.AllFeatures;
using Orion.DataAccess.Postgres.Data;
using Orion.Domain.IRepositories;
using Orion.Domain.Utility;


namespace Orion.Admin
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            services.AddIdentity<MasterUser, IdentityRole<int>>(options =>
                {
                    options.Password.RequiredLength = 2;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoleManager<RoleManager<IdentityRole<int>>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<OrionDbContext>();

            RegisterTypes(services);

            services.AddMvc()
                .AddXmlSerializerFormatters();
            
 
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SecurityConstants.PolicyNameEditBusinessOwner,
                                  policy => policy.Requirements.Add(
                                      new EditBusinessOwnerRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, EditBusinessOwnerHandler>(); 


            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAllQueries(GetType().Assembly);
            services.AddAllCommandHandlers(GetType().Assembly);
            services.AddAllEventHandlers(GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

                     

          //  CheckDatabaseHasBeenDeployed(app);

            var ci = new CultureInfo("en-za");

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UsePopulateSubscriptionClaimsMiddleware();

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=BusinessOwner}/{action=Index}/{id?}");
            // });
        }

        private void CheckDatabaseHasBeenDeployed(IApplicationBuilder app)
        {
            using (var scope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<OrionDbContext>())
                {
                    context?.Database.Migrate();
                }

                using (var context = scope.ServiceProvider.GetService<OrionDbContext>())
                {
                    context?.Database.Migrate();
                }
            }
        }

        void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IFeatureManager, FeatureManager>();
            // services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();


            // services.AddTransient<ILogger, Logger>();
            //
            // services.AddDbContext<OrionDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("Orion_Default_Windows_ConnectionString")));

           //services.AddTransient<IOrionDbContext, OrionDbContext>();

            //services.AddTransient<IRepository<Person>, SqlEntityFrameworkPersonRepository>();

            // services.AddTransient<IValidatorStrategy<BusinessOwner>, DefaultValidatorStrategy<BusinessOwner>>();
            //
            // services.AddTransient<IDaysInOfficeStrategy, DefaultDaysInOfficeStrategy>();
            //
            // services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();
            //
            // services.AddTransient<IBusinessOwnerService, BusinessOwnerService>();
            //
            // services.AddTransient<ISubscriptionService, SubscriptionService>();

            services.AddTransient<ITestDataUtility, TestDataUtility>();

            services.AddTransient<PopulateSubscriptionClaimsMiddleware>();

            services.AddTransient<IUserAuthorizationStrategy, DefaultUserAuthorizationStrategy>();

            services.AddTransient<IUserClaimsPrincipalProvider, 
                HttpContextUserClaimsPrincipalProvider>();            
        }
    }

    internal class FeatureManager : IFeatureManager
    {
        public bool CustomerSatisfaction { get; set; }
        public bool Search { get; set; }
        public bool SearchByBirthBusinessProvince { get; set; }
    }

    public class BusinessOwner
    {
        public string LastName { get; set; }
    }

    internal class SqlEntityFrameworkFeatureRepository
    {
    }

    internal class SubscriptionService
    {
    }

    internal class BusinessOwnerService
    {
    }
}


