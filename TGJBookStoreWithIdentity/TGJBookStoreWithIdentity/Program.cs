using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stripe;
using TGJBookStoreWithIdentity.Data;
using TGJBookStoreWithIdentity.Models;


var builder = WebApplication.CreateBuilder(args);

//Config stripe settings - calls strip keys - TKS
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//adding ApplicationDbContext Service to hold identity tables -- JS 9NOV
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Add new context
builder.Services.AddDbContext<TGJShopContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});


//Authentication builder // !clashing with custom identity! - JS 19NOV
var externalAuthServices = builder.Services;
var configuration = builder.Configuration;
externalAuthServices.AddAuthentication()

    .AddGoogle(googleOptions => 
    {
        googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    })
    
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
    });


//changed default identity to AppUser class - JS 9NOV
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //added roles// JS 10 NOV

    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()              //added
    .AddDefaultTokenProviders(); //added
    
builder.Services.AddControllersWithViews();

var app = builder.Build();

//add sessions - TKS
app.UseSession();

//Secret Key for stripe account - TKS
StripeConfiguration.ApiKey = "";



/////////////////////
//THIS BLOCK IS CHECKING THE DB HAS ROLES ON APP STARTUP - JS 9 NOV
//https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/
//tried to implement this block in ApplicationDbContext as a OnModelCreating method but couldnt make it work..

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var errorLog = services.GetRequiredService<ILoggerFactory>(); //exception handler

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await UserContext.SeedRolesAsync(userManager, roleManager); //call seed roles method from our usercontext seed file-JS
        await UserContext.SeedAdminUserAsync(userManager, roleManager); //call seed admin user from our usercontext seed file-JS
    }
    catch (Exception ex)
    {
        var log = errorLog.CreateLogger<Program>();
        log.LogError(ex, "Seeding Failed");
    }
}
/////////////////////////////// JS 9NOV


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
