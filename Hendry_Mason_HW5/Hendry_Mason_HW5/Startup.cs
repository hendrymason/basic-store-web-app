using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

//Referencing the project
using Hendry_Mason_HW5.Models;
using Hendry_Mason_HW5.DAL;

namespace Hendry_Mason_HW5
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var connectionString = "Server=tcp:sp21masonhendryhw5.database.windows.net,1433;Initial Catalog=sp21MasonHendryHW5;Persist Security Info=False;" +
                "User ID=MISAdmin;Password=Password123;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddMvc();

            //Uncomment after you add identity
            services.AddIdentity<AppUser, IdentityRole>(opts => {                
             opts.User.RequireUniqueEmail = true;                
             opts.Password.RequiredLength = 6;                
             opts.Password.RequireNonAlphanumeric = false;                
             opts.Password.RequireLowercase = false;                
             opts.Password.RequireUppercase = false;                
             opts.Password.RequireDigit = false;            })            
             .AddEntityFrameworkStores<AppDbContext>()            
             .AddDefaultTokenProviders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                CultureInfo.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

                await next.Invoke();
            });

            //Uncomment after you add identity
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=Index}/{id?}");
            });
        }
    }
}
