using MCBADataLibrary.Data;
using MCBAWebApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BankDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BankContext"),
        builder => builder.MigrationsAssembly("MCBAWebApp"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => 
{
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
    await WebDataAccess.InitalizeData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
