using BACKEND_STORE.Config;
using BACKEND_STORE.Modules.Login.Interfaces;
using BACKEND_STORE.Modules.Login.Repositories;
using BACKEND_STORE.Modules.Login.Services;
using BACKEND_STORE.Modules.Rol.Interfaces;
using BACKEND_STORE.Modules.Rol.Repositories;
using BACKEND_STORE.Modules.Rol.Services;
using BACKEND_STORE.Modules.Tests.Interfaces;
using BACKEND_STORE.Modules.Tests.Repositories;
using BACKEND_STORE.Modules.Tests.Services;
using BACKEND_STORE.Modules.User.Interfaces;
using BACKEND_STORE.Modules.User.Repositories;
using BACKEND_STORE.Modules.User.Services;
using BACKEND_STORE.Shared;
using BACKEND_STORE.Shared.DB;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

var builder = WebApplication.CreateBuilder(args);

//Cargar Variables
Load();

foreach (var kvp in ToDictionary())
{
    builder.Configuration[kvp.Key] = kvp.Value;
}


#region Builder

//swagger
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var provider = builder.Services.BuildServiceProvider()
        .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(
            description.GroupName,
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = $"{Variables.CONFIG_CLIENT} API",
                Version = description.ApiVersion.ToString(),
                Description = "Documentación generada automáticamente"
            }
        );
    }
});

// --- API Versioning ---
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Api Explorer para Swagger
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Inyeccion de logs y configuracion de timezone
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<TimeZoneService>();
builder.Services.AddSingleton<Encryption>();

//Cors
var corsHosts = Variables.STORE_CONFIG_CORS_ORIGINS ?? new List<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (corsHosts.Contains("*"))
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            var origins = corsHosts
                .Select(h => h.StartsWith("http://") || h.StartsWith("https://") ? h : "http://" + h)
                .ToArray();

            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

//compresión de respuestas HTTP
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});


builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestService, TestService>();

// Registro correcto de repositorio e interfaz
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
 
// Registro correcto del servicio e interfaz
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddControllers();

builder.Services.AddAuthorization();

// Configuracion de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Variables.STORE_DATABASE_CONNECTION));

var app = builder.Build();
#endregion

#region App
// Creacion de la base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (Variables.STORE_CONFIG_SWAGGER) {
        app.UseSwagger();
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"{Variables.CONFIG_CLIENT} {description.GroupName.ToUpper()}"
                );
            }

            // Configuración global (fuera del foreach)
            options.RoutePrefix = Variables.STORE_CONFIG_SWAGGER_NAME;
            options.DocumentTitle = $"{Variables.CONFIG_CLIENT} API Docs";
            options.DefaultModelsExpandDepth(-1);
        });
}

//app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("StoreCorsPolicy");
app.UseResponseCompression();
app.MapControllers();
app.Run();
#endregion
