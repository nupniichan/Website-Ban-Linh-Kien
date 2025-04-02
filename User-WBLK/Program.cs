using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient factory
builder.Services.AddHttpClient();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add DbContext to DI container
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31))
    )
);

// Configure Authentication with both local and external login
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Shared/AccessDenied";
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Shared/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
})
// Temporary cookie for external authentication data.
.AddCookie("External")
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    googleOptions.ReturnUrlParameter = builder.Configuration["Authentication:Google:RedirectUri"];
    googleOptions.SignInScheme = "External";
})

.AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    facebookOptions.ReturnUrlParameter = builder.Configuration["Authentication:Facebook:RedirectUri"];
    facebookOptions.SignInScheme = "External"; 
});

// Configure cookie settings if needed
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax; 
});

// Connect MomoAPI 
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<CheckCookiesSession>();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use Session before Authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "account",
    pattern: "/account/{action=Profile}/{id?}",
    defaults: new { controller = "ProfileManagement" });

app.MapControllerRoute(
    name: "orderHistory",
    pattern: "/orderHistory/{action=OrderHistory}/{id?}",
    defaults: new { controller = "ProfileManagement" });

// Test route (remove after testing)
app.MapControllerRoute(
    name: "productsList",
    pattern: "/productsList/{action=Index}/{id?}",
    defaults: new { controller = "ProductsList" });

app.Run();
