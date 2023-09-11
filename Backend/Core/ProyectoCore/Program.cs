using Microsoft.EntityFrameworkCore;
using ProyectoCore.Models;
using dotenv.net;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

// Add services to the container.
builder.Services.AddControllersWithViews();
var env = Environment.GetEnvironmentVariable("ConnectionStrings__cadenaSQL"); //esta usa el connectionstring de la variable de entorno
var appsettings = builder.Configuration.GetConnectionString("cadenaSQL"); // esta usa el connectionstring del que esta en el app setting


builder.Services.AddDbContext<TiendaPruebaContext>(options =>
    options.UseSqlServer(env)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}





app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
