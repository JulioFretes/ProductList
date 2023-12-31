using ProductList.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Recuperar a string de conexao do arquivo appsettings.json
var conn = builder.Configuration.GetConnectionString("conexao");

//Configurar o servi�o de inje��o de depend�ncia do DbContext
builder.Services.AddDbContext<ListContext>(op => op.UseSqlServer(conn));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ListContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
