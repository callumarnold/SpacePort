using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SP.DataManager.Models;
using SP.DataManager.Data.DataAccess;

namespace SP.DataManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SPDataContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("SPData")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityDB")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //add data access services 
            services.AddScoped<IDockManagersDataAccess, DockManagersDataAccess>();
            services.AddScoped<IDocksDataAccess, DocksDataAccess>();
            services.AddScoped<ISpaceshipsDataAccess, SpaceshipsDataAccess>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRoles(services).Wait();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Space Port");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //get role manager and user manager from IServiceProvider
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //check if admin roles exists
            var checkRole = await RoleManager.RoleExistsAsync("Admin");
            if(checkRole == false)
            {
                await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            checkRole = await RoleManager.RoleExistsAsync("Manager");
            if (checkRole == false)
            {
                await RoleManager.CreateAsync(new IdentityRole("Manager"));
            }
            //gets user and assigns them to admin role 
            await UserManager.AddToRoleAsync(await UserManager.FindByEmailAsync("callumarnold@hotmail.com"), "Admin");
            await UserManager.AddToRoleAsync(await UserManager.FindByEmailAsync("manager@spaceport.com"), "Manager");
        }

    }
}
