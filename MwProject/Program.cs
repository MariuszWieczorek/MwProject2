using EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MwProject.Core;
using MwProject.Core.Models.Domains;
using MwProject.Core.Repositories;
using MwProject.Core.Services;
using MwProject.Data;
using MwProject.Persistence;
using MwProject.Persistence.Repositories;
using MwProject.Persistence.Services;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// logowanie do pliku
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddNLogWeb();

// Add services to the container.
#region DependencyInjection

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IUrlHelper, UrlHelper>();

// repozytoria
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();
builder.Services.AddScoped<IEstimatedSalesValueRepository, EstimatedSalesValueRepository>();
builder.Services.AddScoped<IRequirementRepository, RequirementRepository>();
builder.Services.AddScoped<IProjectRequirementRepository, ProjectRequirementRepository>();
builder.Services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
builder.Services.AddScoped<ITechnicalPropertyRepository, TechnicalPropertyRepository>();
builder.Services.AddScoped<IProjectTechnicalPropertyRepository, ProjectTechnicalPropertyRepository>();
builder.Services.AddScoped<ICategoryTechnicalPropertyRepository, CategoryTechnicalPropertyRepository>();
builder.Services.AddScoped<ICategoryRequirementRepository, CategoryRequirementRepository>();
builder.Services.AddScoped<IRankingCategoryRepository, RankingCategoryRepository>();
builder.Services.AddScoped<IRankingElementRepository, RankingElementRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectTeamMemberRepository, ProjectTeamMemberRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IProjectStatusRepository, ProjectStatusRepository>();
builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();
builder.Services.AddScoped<IProjectClientRepository, ProjectClientRepository>();
builder.Services.AddScoped<IProjectRiskRepository, ProjectRiskRepository>();

// serwisy
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<IEstimatedSalesValueService, EstimatedSalesValueService>();
builder.Services.AddScoped<IRequirementService, RequirementService>();
builder.Services.AddScoped<IProjectRequirementService, ProjectRequirementService>();
builder.Services.AddScoped<IProductGroupService, ProductGroupService>();
builder.Services.AddScoped<ITechnicalPropertyService, TechnicalPropertyService>();
builder.Services.AddScoped<IProjectTechnicalPropertyService, ProjectTechnicalPropertyService>();
builder.Services.AddScoped<ICategoryTechnicalPropertyService, CategoryTechnicalPropertyService>();
builder.Services.AddScoped<ICategoryRequirementService, CategoryRequirementService>();
builder.Services.AddScoped<IRankingCategoryService, RankingCategoryService>();
builder.Services.AddScoped<IRankingElementService, RankingElementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectTeamMemberService, ProjectTeamMemberService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProjectStatusService, ProjectStatusService>();
builder.Services.AddScoped<IProjectGroupService, ProjectGroupService>();
builder.Services.AddScoped<IProjectClientService, ProjectClientService>();
builder.Services.AddScoped<IProjectRiskService, ProjectRiskService>();

#endregion

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// zmieniamy IdentityUser na ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// dodana obs³uga sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


#region obs³uga wysy³ania wiadomoœci e-mail
// odczyt parametrów z appsettings.json
var emailConfig = builder.Configuration
.GetSection("EmailConfiguration2")
.Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
// klasa obs³uguj¹ca wysy³anie wiadomoœci e-mail
builder.Services.AddScoped<IEmailSender, EmailSender>();
#endregion

#region polityka hase³
builder.Services.Configure<IdentityOptions>(opt =>
    {
        opt.Password.RequiredLength = 5;
        opt.Password.RequireDigit = false;
        opt.Password.RequireUppercase = false;
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
    });
#endregion



var app = builder.Build();

// Logger
var logger = app.Services.GetService<ILogger<Program>>();

// Test trybu Developerskiego
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    logger.LogInformation("DEVELOPMENT Mode");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    logger.LogInformation("PRODUCTION Mode");
}


// dodany mechanizm obs³ugi sesji  
// w .Net 5 by³o w klasie Startup.cs w metodzie Configure()
// Kolejnoœæ oprogramowania poœrednicz¹cego jest wa¿na.
// Wywo³aj UseSession wywo³anie po i przed UseRouting UseEndpoints. 
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
