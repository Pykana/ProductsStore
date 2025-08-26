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
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using static BACKEND_STORE.Config.JWT;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
/*VARIABLES DE ENTORNO*/
/*SETEAR VARIABLES COMO GLOBALES*/
void SetConfig(string key, string? value) => builder.Configuration[key] = value ?? "";

// Base de datos
SetConfig("Secrets:STORE_DATABASE_IP", Environment.GetEnvironmentVariable("STORE_DATABASE_IP"));
SetConfig("Secrets:STORE_DATABASE_PORT", Environment.GetEnvironmentVariable("STORE_DATABASE_PORT"));
SetConfig("Secrets:STORE_DATABASE_NAMEDB_STORE", Environment.GetEnvironmentVariable("STORE_DATABASE_NAMEDB_STORE"));
SetConfig("Secrets:STORE_DATABASE_USER", Environment.GetEnvironmentVariable("STORE_DATABASE_USER"));
SetConfig("Secrets:STORE_DATABASE_PASS", Environment.GetEnvironmentVariable("STORE_DATABASE_PASS"));
SetConfig("Secrets:STORE_DATABASE_TRUST_CERT", Environment.GetEnvironmentVariable("STORE_DATABASE_TRUST_CERT"));

// Email
SetConfig("Secrets:STORE_KEY_EMAIL_HOST", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_HOST"));
SetConfig("Secrets:STORE_KEY_EMAIL_BASE", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_BASE"));
SetConfig("Secrets:STORE_KEY_EMAIL_PORT", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PORT"));
SetConfig("Secrets:STORE_KEY_EMAIL_USER", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_USER"));
SetConfig("Secrets:STORE_KEY_EMAIL_PASSW", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PASSW"));
SetConfig("Secrets:STORE_KEY_EMAIL_SSL", Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_SSL"));

// Paths
SetConfig("Secrets:STORE_PATH_LOGS", Environment.GetEnvironmentVariable("STORE_PATH_LOGS"));
SetConfig("Secrets:STORE_PATH_EMAIL_TEMPLATE", Environment.GetEnvironmentVariable("STORE_PATH_EMAIL_TEMPLATE"));

// Config
SetConfig("Secrets:STORE_CONFIG_TIMEZONE", Environment.GetEnvironmentVariable("STORE_CONFIG_TIMEZONE"));
SetConfig("Secrets:STORE_CONFIG_SAVEDB", Environment.GetEnvironmentVariable("STORE_CONFIG_SAVEDB"));

// URLs externas
SetConfig("Secrets:STORE_EXTERNAL_URL", Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL"));
SetConfig("Secrets:STORE_EXTERNAL_URL2", Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL2"));

//JWT

var jwtSettings = new JWTSettings
{
    SecretKey = Environment.GetEnvironmentVariable("STORE_JWT_SECRET_KEY") ?? "",
    Issuer = Environment.GetEnvironmentVariable("STORE_JWT_ISSUER") ?? "",
    Audience = Environment.GetEnvironmentVariable("STORE_JWT_AUDIENCE") ?? "",
    ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("STORE_JWT_EXPIRATION_MINUTES"), out var exp) ? exp : 60
};

bool swaggerFlag = bool.TryParse(Environment.GetEnvironmentVariable("STORE_CONFIG_SWAGGER"), out var flag) && flag;  

var corsOriginsRaw = Environment.GetEnvironmentVariable("STORE_CONFIG_CORS_ORIGINS") ?? "";
var corsOrigins = corsOriginsRaw
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddEndpointsApiExplorer();

if (swaggerFlag) {
    builder.Services.AddEndpointsApiExplorer();
  
}

// Inyeccion de logs y configuracion de timezone
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<TimeZoneService>();
builder.Services.AddSingleton<Encryption>();
builder.Services.AddSingleton(jwtSettings);

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
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
 
// Registro correcto del servicio e interfaz
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

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


// 1️⃣ Vincular sección "JWT" del appsettings.json (o variables de entorno) con la clase JWTSettings
builder.Services.Configure<JWTSettings>(
    builder.Configuration.GetSection("JWT")
);

// 2️⃣ Registrar Token como servicio
builder.Services.AddTransient<Token>();

// Configuración de Swagger con autenticación JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT generado al hacer login"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Evita que se ejecute la respuesta por defecto
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\":\"No estás autorizado\"}");
        },
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            if (context.Exception is SecurityTokenExpiredException)
            {
                return context.Response.WriteAsync("{\"message\":\"El token ha expirado\"}");
            }
            else
            {
                return context.Response.WriteAsync("{\"message\":\"Token inválido\"}");
            }
        }


    };
});


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
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("StoreCorsPolicy");
app.UseResponseCompression();
app.MapControllers();
app.Run();

