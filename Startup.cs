using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using MwProject.Core.Services;
using MwProject.Data;
using MwProject.Persistence;
using MwProject.Persistence.Repositories;
using MwProject.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject
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

            // dla ka¿dego request'a jedna instancja tej klasy
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // repozytoria
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICalculationRepository, CalculationRepository>();
            services.AddScoped<IEstimatedSalesValueRepository, EstimatedSalesValueRepository>();
            services.AddScoped<IRequirementRepository, RequirementRepository>();
            services.AddScoped<IProjectRequirementRepository, ProjectRequirementRepository>();
            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<ITechnicalPropertyRepository, TechnicalPropertyRepository>();
            services.AddScoped<IProjectTechnicalPropertyRepository, ProjectTechnicalPropertyRepository>();
            services.AddScoped<ICategoryTechnicalPropertyRepository, CategoryTechnicalPropertyRepository>();
            services.AddScoped<ICategoryRequirementRepository, CategoryRequirementRepository>();
            services.AddScoped<IRankingCategoryRepository, RankingCategoryRepository>();
            services.AddScoped<IRankingElementRepository, RankingElementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectTeamMemberRepository, ProjectTeamMemberRepository>();
            // serwisy
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<IEstimatedSalesValueService, EstimatedSalesValueService>();
            services.AddScoped<IRequirementService, RequirementService>();
            services.AddScoped<IProjectRequirementService, ProjectRequirementService>();
            services.AddScoped<IProductGroupService, ProductGroupService>();
            services.AddScoped<ITechnicalPropertyService, TechnicalPropertyService>();
            services.AddScoped<IProjectTechnicalPropertyService, ProjectTechnicalPropertyService>();
            services.AddScoped<ICategoryTechnicalPropertyService, CategoryTechnicalPropertyService>();
            services.AddScoped<ICategoryRequirementService, CategoryRequirementService>();
            services.AddScoped<IRankingCategoryService, RankingCategoryService>();
            services.AddScoped<IRankingElementService, RankingElementService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectTeamMemberService, ProjectTeamMemberService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("LocalConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            // dodana obs³uga sesji
            services.AddDistributedMemoryCache();
            services.AddSession();

            /*
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // dodana obs³uga sesji
            // Kolejnoœæ oprogramowania poœrednicz¹cego jest wa¿na.
            // Wywo³aj UseSession wywo³anie po i przed UseRouting UseEndpoints. 
            app.UseSession();

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
        }
    }
}
