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
    options.UseNpgsql(connectionString));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection(GeminiOptions.Gemini));

// 2. Repository réteg regisztrációja
builder.Services.AddScoped<IRepository<Zh>, ZhRepository>();
builder.Services.AddScoped<IRepository<GeneraltZh>, GeneraltZhRepository>();
builder.Services.AddScoped<IRepository<Hallgato>, HallgatoRepository>();
builder.Services.AddScoped<IRepository<Megoldas>, MegoldasRepository>();
builder.Services.AddScoped<IRepository<Feladat>, FeladatRepository>();
builder.Services.AddScoped<IFeladatRepository, FeladatRepository>();
builder.Services.AddScoped<IRepository<Ertekeles>, ErtekelesRepository>();

// 3. Service réteg regisztrációja
builder.Services.AddScoped<IZhService, ZhService>();
builder.Services.AddScoped<IHallgatoService, HallgatoService>();
builder.Services.AddScoped<IMegoldasService, MegoldasService>();
builder.Services.AddScoped<IFeladatService, FeladatService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<IErtekelesService, ErtekelesService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddControllers();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// 4. Swagger/OpenAPI támogatás
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "ZH-tool.xml");
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

// 5. Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
