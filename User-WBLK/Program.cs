var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

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
