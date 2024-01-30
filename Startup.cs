using Main_Plaid.Models;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Main_Plaid.Models;

namespace Main_Plaid
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Other configurations...

            // Bind PlaidApi section from appsettings.json to a strongly typed class
            services.Configure<AccessTokenRequest>(Configuration.GetSection("PlaidApi"));

            // Add MVC services
            services.AddControllersWithViews();
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44300; // Set the HTTPS port
            });


            // Add other services as needed
            // services.AddScoped<IMyService, MyService>();
            // ...

            // Configure other options, authentication, authorization, etc.
            // ...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure for production, error handling, etc.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Plaid}/{action=Index}");
            });
        }
    }
}
