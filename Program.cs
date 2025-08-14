using BACKEND_STORE.Config;
using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Repositories;
using BACKEND_STORE.Services;
using BACKEND_STORE.Shared;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using static BACKEND_STORE.Config.JWT;

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

//JWT
SetConfig("STORE_JWT_SECRET_KEY", Environment.GetEnvironmentVariable("STORE_JWT_SECRET_KEY"));
SetConfig("STORE_JWT_ISSUER", Environment.GetEnvironmentVariable("STORE_JWT_ISSUER"));
SetConfig("STORE_JWT_AUDIENCE", Environment.GetEnvironmentVariable("STORE_JWT_AUDIENCE"));
SetConfig("STORE_JWT_EXPIRATION_MINUTES", Environment.GetEnvironmentVariable("STORE_JWT_EXPIRATION_MINUTES"));

bool swaggerFlag = bool.TryParse(Environment.GetEnvironmentVariable("STORE_CONFIG_SWAGGER"), out var flag) && flag;  

var corsOriginsRaw = Environment.GetEnvironmentVariable("STORE_CONFIG_CORS_ORIGINS") ?? "";
var corsOrigins = corsOriginsRaw
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddEndpointsApiExplorer();

if (swaggerFlag) {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Configuraci?n para que Swagger acepte JWTS
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Introduce el token en el formato: Bearer {tu token}"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Registro correcto del servicio e interfaz
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar autenticaci?n JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("STORE_JWT_ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("STORE_JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("STORE_JWT_SECRET_KEY")))
        };
    });

string ConnectionString = Connection.SQLServerConnection(builder.Configuration);

// Configuracion de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConnectionString));

var app = builder.Build();

// Creacion de la base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


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

