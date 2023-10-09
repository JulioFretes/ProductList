
using Microsoft.EntityFrameworkCore;
using ProductList.DataBase;
using Microsoft.AspNetCore.Identity;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configurar o serviço de injeção de dependência do DbContext
builder.Services.AddDbContext<ListContext>(op => op.UseSqlServer(config["conn"]));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ListContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
