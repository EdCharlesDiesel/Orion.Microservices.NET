using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Orion.External.PaymentGateway
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     //TODO: Uncomment this
            //     // c.SwaggerDoc("v1", new OpenApiInfo
            //     // {
            //     //     Title = "External Payment Gateway API",
            //     //     Summary = null,
            //     //     Description = null,
            //     //     Version = "v1",
            //     //     TermsOfService = null,
            //     //     Contact = null,
            //     //     License = null,
            //     //     Extensions = null
            //     // });
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // app.UseSwagger();
            //
            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "External Payment Gateway API V1");
            // });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
