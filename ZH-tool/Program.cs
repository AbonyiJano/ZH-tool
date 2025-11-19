using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZH_tool.Configurations;
using ZH_tool.Data;
using ZH_tool.Mapping;
using ZH_tool.Models;
using ZH_tool.Repository;
using ZH_tool.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. PostgreSQL DbContext konfiguráció (EF Core)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // Az UseNpgsql a Npgsql.EntityFrameworkCore.PostgreSQL csomagból származik
    options.UseNpgsql(connectionString));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection(GeminiOptions.Gemini));
// 2. Repository réteg regisztrációja (Dependency Injection)
builder.Services.AddScoped<IRepository<Zh>, ZhRepository>();
builder.Services.AddScoped<IRepository<GeneraltZh>, GeneraltZhRepository>();
builder.Services.AddScoped<IRepository<Hallgato>, HallgatoRepository>();
builder.Services.AddScoped<IRepository<Megoldas>, MegoldasRepository>();
builder.Services.AddScoped<IRepository<Feladat>, FeladatRepository>();
builder.Services.AddScoped<IFeladatRepository, FeladatRepository>();
builder.Services.AddScoped<IRepository<Ertekeles>, ErtekelesRepository>();

// 3. Service réteg regisztrációja (Dependency Injection)
builder.Services.AddScoped<IZhService, ZhService>();
builder.Services.AddScoped<IHallgatoService, HallgatoService>();
builder.Services.AddScoped<IMegoldasService, MegoldasService>();
builder.Services.AddScoped<IFeladatService, FeladatService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<IErtekelesService, ErtekelesService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Add services to the container.
builder.Services.AddControllers();

// 4. Swagger/OpenAPI támogatás hozzáadása
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Opcionális: Segíti a Swagger UI-t, hogy megjelenítse a Controller metódusok leírását
    var filePath = Path.Combine(AppContext.BaseDirectory, "ZH-tool.xml");
    // Ahhoz, hogy ez mûködjön, be kell kapcsolni a "GenerateDocumentationFile"-t a .csproj fájlban:
    // <PropertyGroup><GenerateDocumentationFile>true</GenerateDocumentationFile></PropertyGroup>
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// 5. Swagger UI engedélyezése Fejlesztõi módban
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // A Swagger UI itt érhetõ el: https://localhost:{port}/swagger
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
