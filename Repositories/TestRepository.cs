using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Config;
using BACKEND_STORE.Shared;
using BACKEND_STORE.Models;

namespace BACKEND_STORE.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;  // DbContext for database operations
        private readonly Encryption _encryption; // Encryption service for password hashings
        private readonly Logs _logs; // Logging service for logging messages

        public TestRepository(AppDbContext context, Encryption encryption, Logs logs)
        {
            _context = context;
            _encryption = encryption;
            _logs = logs;
        }

        public async Task<Test> ProbarConexion()
        {
            try
            {
                var puedeConectar = await _context.Database.CanConnectAsync();

                if (puedeConectar)
                    return new Test
                    {
                        Value = 1,
                        Message = "Conexión exitosa a la base de datos."
                    };
                else
                    return new Test
                    {
                        Value = 0,
                        Message = "No se pudo establecer conexión con la base de datos."
                    };
            }
            catch (Exception ex)
            {
                return new Test
                {
                    Value = ex.HResult,
                    Message = $"Error al intentar conectar a la base de datos: {ex.Message}"
                };
            }
        }


        public async Task<StoreConfig> VerVaribalesDeEntorno()
        {
            try
            {
                var config = new StoreConfig
                {
                    Database = new DatabaseConfig
                    {
                        Ip = Environment.GetEnvironmentVariable("STORE_DATABASE_IP") ?? "",
                        Port = int.TryParse(Environment.GetEnvironmentVariable("STORE_DATABASE_PORT"), out var dbPort) ? dbPort : 0,
                        Name = Environment.GetEnvironmentVariable("STORE_DATABASE_NAMEDB_STORE") ?? "",
                        User = Environment.GetEnvironmentVariable("STORE_DATABASE_USER") ?? "",
                        Password = Environment.GetEnvironmentVariable("STORE_DATABASE_PASS") ?? "",
                        TrustCert = bool.TryParse(Environment.GetEnvironmentVariable("STORE_DATABASE_TRUST_CERT"), out var trustCert) && trustCert
                    },
                    Email = new EmailConfig
                    {
                        Host = Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_HOST") ?? "",
                        Base = Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_BASE") ?? "",
                        Port = int.TryParse(Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PORT"), out var emailPort) ? emailPort : 0,
                        User = Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_USER") ?? "",
                        Password = Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_PASSW") ?? "",
                        UseSSL = bool.TryParse(Environment.GetEnvironmentVariable("STORE_KEY_EMAIL_SSL"), out var useSSL) && useSSL
                    },
                    Logs = new LogConfig
                    {
                        LogsPath = Environment.GetEnvironmentVariable("STORE_PATH_LOGS") ?? "",
                        EmailTemplatePath = Environment.GetEnvironmentVariable("STORE_PATH_EMAIL_TEMPLATE") ?? ""
                    },
                    Api = new ApiConfig
                    {
                        Timezone = Environment.GetEnvironmentVariable("STORE_CONFIG_TIMEZONE") ?? "",
                        SwaggerEnabled = bool.TryParse(Environment.GetEnvironmentVariable("STORE_CONFIG_SWAGGER"), out var swagger) && swagger,
                        SaveToDatabase = bool.TryParse(Environment.GetEnvironmentVariable("STORE_CONFIG_SAVEDB"), out var saveDb) && saveDb,
                        CorsOrigins = Environment.GetEnvironmentVariable("STORE_CONFIG_CORS_ORIGINS") ?? ""
                    },
                    JWT = new JWTConfig
                    {
                        STORE_JWT_SECRET_KEY = Environment.GetEnvironmentVariable("STORE_JWT_SECRET_KEY") ?? "",
                        STORE_JWT_ISSUER = Environment.GetEnvironmentVariable("STORE_JWT_ISSUER") ?? "",
                        STORE_JWT_AUDIENCE = Environment.GetEnvironmentVariable("STORE_JWT_AUDIENCE") ?? "",
                        STORE_JWT_EXPIRATION_MINUTES = Environment.GetEnvironmentVariable("STORE_JWT_EXPIRATION_MINUTES") ?? ""
                    },
                    ExternalUrls = new ExternalUrlsConfig
                    {
                        Url1 = Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL") ?? "",
                        Url2 = Environment.GetEnvironmentVariable("STORE_EXTERNAL_URL2") ?? ""
                    }
                };

                return config;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las variables de entorno: {ex.Message}");
            }
        }
        public async Task<string> VerificarEncriptamiento(string contraseña)
        {
            try
            {
                string contraseñaEncriptada = _encryption.HashPassword(contraseña);

                return contraseñaEncriptada; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar el encriptamiento: {ex.Message}");
            }
        }

        public async Task<string> VerificarLogs(string Mensaje)
        {
            try
            {
                bool flag = _logs.SaveLog(Mensaje);
                if (flag) 
                    return "Mensaje guardado en el log exitosamente.";
                else
                    return "No se pudo guardar el mensaje en el log.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el archivo log: {ex.Message}");
            }
        }



    }
}
