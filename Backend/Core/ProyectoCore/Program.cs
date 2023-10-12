using ProyectoCore.ControllersApi; 
using Microsoft.EntityFrameworkCore;
using ProyectoCore.Models;
using ProyectoCore.Repository;
using ProyectoCore.Interface;
using dotenv.net;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProyectoCore.GraphQL;
using HotChocolate;



var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();


// para el api

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IReseñaRepository, ReseñaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); // agregue esto
builder.Services.AddScoped<ICarritoProductoRepository, CarritoProductoRepository>(); // agregue esto
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<IListaRepository, ListaRepository>();
builder.Services.AddScoped <IListaProductoRepository, ListaProductoRepository>();
builder.Services.AddScoped<IReciboRepository, ReciboRepository>();
builder.Services.AddScoped<IMetodoRepository, MetodoRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") 
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Add services to the container.
builder.Services.AddControllersWithViews();
var env = Environment.GetEnvironmentVariable("ConnectionStrings__cadenaSQL"); //esta usa el connectionstring de la variable de entorno
var appsettings = builder.Configuration.GetConnectionString("cadenaSQL"); // esta usa el connectionstring del que esta en el app setting
var jwtSecret = "AguacateMiClaveMuyLargaQueCumpleConLosRequisitosDelAlgoritmoHMACSHA256123";
builder.Services.AddSingleton(jwtSecret);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5230/", 
            ValidAudience = "http://localhost:5230/", 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
/*builder.Services.AddAuthorization(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt: Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration("Jwt:Key")))
    }
}*/
builder.Services.AddDbContext<TiendaPruebaContext>(options =>
    options.UseSqlServer(env) // en este caso usamos la varaibles de entorno
);




builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddProjections()
    .AddSorting()
    .AddFiltering()
    .AddType<UsuarioType>()
    ;


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

app.UseCors("AllowSpecificOrigin");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapGraphQL("/graphQL");

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


api.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

api.UseCors("AllowSpecificOrigin");


api.Run();