using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using WebBanHangOnline.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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
    name: "Products",
    pattern: "san-pham",
    defaults: new { controller = "Product", action = "Index" });

app.MapControllerRoute(
    name: "CategoryProduct",
    pattern: "danh-muc-san-pham/{alias}-{id}",
    defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional });
app.MapRazorPages();

app.Run();
