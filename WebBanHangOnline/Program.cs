using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using WebBanHangOnline.Data;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
// Thêm dịch vụ session vào
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thiết lập thời gian timeout cho session
options.Cookie.HttpOnly = true; // Sử dụng http-only cookie khi gửi session ID
options.Cookie.IsEssential = true; // Đảm bảo cookie này là bắt buộc để ứng dụng hoạt động
});


var app = builder.Build();

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

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Contact",
    pattern: "lien-he",
    defaults: new { controller = "Contact", action = "Index" });
app.MapControllerRoute(
    name: "ShoppingCart",
    pattern: "gio-hang",
    defaults: new { controller = "ShoppingCart", action = "Index" });
//app.MapControllerRoute(
//    name: "DetailProducts",
//    pattern: "chi-tiet/{alias}-p{id}.html",
//    defaults: new { controller = "Product", action = "DetailProduct" });
app.MapControllerRoute(
    name: "DetailProducts",
    pattern: "{controller=Product}/{action=DetailProduct}/{alias}-p{id}");

app.MapControllerRoute(
    name: "CategoryProduct",
    pattern: "danh-muc-san-pham/{alias}-{id}",
    defaults: new { controller = "Product", action = "ProductCategory" });
app.MapControllerRoute(
    name: "Products",
    pattern: "san-pham",
    defaults: new { controller = "Product", action = "Index" });

app.MapRazorPages();

app.Run();
