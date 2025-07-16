using BACKEND_STORE.Config;
using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Repositories;
using BACKEND_STORE.Services;
using BACKEND_STORE.Shared;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
/*VARIABLES DE ENTORNO*/
/*SETEAR VARIABLES COMO GLOBALES*/
void SetConfig(string key, string? value) => builder.Configuration[key] = value ?? "";

// Base de datos
SetConfig("STORE_DATABASE_IP", Environment.GetEnvironmentVariable("STORE_DATABASE_IP"));
SetConfig("STORE_DATABASE_PORT", Environment.GetEnvironmentVariable("STORE_DATABASE_PORT"));
SetConfig("STORE_DATABASE_NAMEDB_STORE", Environment.GetEnvironmentVariable("STORE_DATABASE_NAMEDB_STORE"));
SetConfig("STORE_DATABASE_USER", Environment.GetEnvironmentVariable("STORE_DATABASE_USER"));
SetConfig("STORE_DATABASE_PASS", Environment.GetEnvironmentVariable("STORE_DATABASE_PASS"));
SetConfig("STORE_DATABASE_TRUST_CERT", Environment.GetEnvironmentVariable("STORE_DATABASE_TRUST_CERT"));

// Email
SetConfig("STORE_KEY_EMAIL_HOST", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_HOST"));
SetConfig("STORE_KEY_EMAIL_BASE", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_BASE"));
SetConfig("STORE_KEY_EMAIL_PORT", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PORT"));
SetConfig("STORE_KEY_EMAIL_USER", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_USER"));
SetConfig("STORE_KEY_EMAIL_PASSW", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PASSW"));
SetConfig("STORE_KEY_EMAIL_SSL", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_SSL"));

// Paths
SetConfig("STORE_PATH_LOGS", Environment.GetEnvironmentVariable("STORE_PATH_LOGS"));
SetConfig("STORE_PATH_EMAIL_TEMPLATE", Environment.GetEnvironmentVariable("STORE_PATH_EMAIL_TEMPLATE"));

// Config
SetConfig("STORE_CONFIG_TIMEZONE", Environment.GetEnvironmentVariable("STORE_CONFIG_TIMEZONE"));
SetConfig("STORE_CONFIG_SAVEDB", Environment.GetEnvironmentVariable("STORE_CONFIG_SAVEDB"));

// URLs externas
SetConfig("STORE_EXTERNAL_URL", Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL"));
SetConfig("STORE_EXTERNAL_URL2", Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL2"));

bool swaggerFlag = bool.TryParse(Environment.GetEnvironmentVariable("STORE_CONFIG_SWAGGER"), out var flag) && flag;  

var corsOriginsRaw = Environment.GetEnvironmentVariable("STORE_CONFIG_CORS_ORIGINS") ?? "";
var corsOrigins = corsOriginsRaw
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddEndpointsApiExplorer();

if (swaggerFlag) {
    builder.Services.AddSwaggerGen();
}
// Inyeccion de logs y configuracion de timezone
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<TimeZoneService>();
builder.Services.AddSingleton<Encryption>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("StoreCorsPolicy", policy =>
    {
        policy
            .WithOrigins(corsOrigins) 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Registro correcto de repositorio e interfaz
builder.Services.AddScoped<ITestRepository, TestRepository>();

// Registro correcto del servicio e interfaz
builder.Services.AddScoped<ITestService, TestService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

string ConnectionString = Connection.SQLServerConnection(builder.Configuration);

// Configuracion de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConnectionString));

var app = builder.Build();

//// Creacion de la base de datos si no existe
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();
//}


if (swaggerFlag) {
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.UseHttpsRedirection();
app.UseCors("StoreCorsPolicy");
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();
app.Run();

