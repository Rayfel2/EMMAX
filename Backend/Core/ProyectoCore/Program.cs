using Microsoft.EntityFrameworkCore;
using ProyectoCore.Models;
using ProyectoCore.Repository;
using ProyectoCore.Interface;
using dotenv.net;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();


// para el api

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Reemplaza con la URL de tu aplicación Angular
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Add services to the container.
builder.Services.AddControllersWithViews();
var env = Environment.GetEnvironmentVariable("ConnectionStrings__cadenaSQL"); //esta usa el connectionstring de la variable de entorno
var appsettings = builder.Configuration.GetConnectionString("cadenaSQL"); // esta usa el connectionstring del que esta en el app setting


builder.Services.AddDbContext<TiendaPruebaContext>(options =>
    options.UseSqlServer(env) // en este caso usamos la varaibles de entorno
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

app.UseCors("AllowSpecificOrigin");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


//para el api

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var api = builder.Build();

// Configure the HTTP request pipeline.
if (api.Environment.IsDevelopment())
{
    api.UseSwagger();
    api.UseSwaggerUI();
}

api.UseAuthorization();

api.MapControllers();

api.UseCors("AllowSpecificOrigin");


api.Run();