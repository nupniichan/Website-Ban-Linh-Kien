using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Website_Ban_Linh_Kien.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm DbContext vào DI container
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31))
    )
);

// Cấu hình Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Shared/AccessDenied";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Shared/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie tồn tại 7 ngày
        options.SlidingExpiration = true; // Gia hạn cookie mỗi lần request
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax; // hoặc None nếu cần
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<CheckCookiesSession>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

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
    
// cái này tui đang test nào xong xoá sau
app.MapControllerRoute(
    name: "producstList",
    pattern: "/productsList/{action=Index}/{id?}",
    defaults: new { controller = "ProductsList" });

app.Run();
