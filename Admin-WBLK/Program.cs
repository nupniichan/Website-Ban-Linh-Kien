using Admin_WBLK.Models;
using Admin_WBLK.Models.Observers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thm DbContext vo DI container
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31))
    )
);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AccountManagement/Login"; // Đường dẫn đến trang đăng nhập
        options.LogoutPath = "/AccountManagement/Logout"; // Đường dẫn để đăng xuất
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Thời gian hết hạn cookie
    });

// Đăng ký các dịch vụ Builder Pattern
builder.Services.AddScoped<Admin_WBLK.Models.Builders.IDashboardBuilder, Admin_WBLK.Models.Builders.DashboardBuilder>();
builder.Services.AddScoped<Admin_WBLK.Models.Builders.DashboardDirector>();

// Đăng ký các dịch vụ Observer Pattern
builder.Services.AddSingleton<IRevenueSubject, RevenueManager>();
builder.Services.AddSingleton<IRevenueObserver, RevenueLogger>();

var app = builder.Build();

// Cấu hình RevenueLogger với RevenueManager
var revenueSubject = app.Services.GetRequiredService<IRevenueSubject>();
var revenueObserver = app.Services.GetRequiredService<IRevenueObserver>();
revenueSubject.Attach(revenueObserver);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "accountManagement",
    pattern: "/account-management/{action=Index}/{id?}",
    defaults: new { controller = "AccountManagement" });

app.MapControllerRoute(
    name: "requestManagement",
    pattern: "/request-management/{action=Index}/{id?}",
    defaults: new { controller = "RequestManagement" });
app.Run();
