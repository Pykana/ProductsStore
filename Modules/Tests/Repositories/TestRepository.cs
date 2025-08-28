using BACKEND_STORE.Modules.Tests.Interfaces;
using BACKEND_STORE.Modules.Tests.Models;
using BACKEND_STORE.Shared;
using BACKEND_STORE.Shared.DB;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Modules.Tests.Repositories
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

        public async Task<VariablesEntorno> VerVaribalesDeEntorno()
        {
            try
            {
                var config = new VariablesEntorno
                {
                    CONFIG_DEBUG = Variables.CONFIG_DEBUG,
                    CONFIG_CLIENT = Variables.CONFIG_CLIENT,
                    STORE_CONFIG_TIMEZONE = Variables.STORE_CONFIG_TIMEZONE,
                    STORE_CONFIG_CORS_ORIGINS = Variables.STORE_CONFIG_CORS_ORIGINS,
                    STORE_CONFIG_SWAGGER = Variables.STORE_CONFIG_SWAGGER,
                    STORE_CONFIG_SWAGGER_NAME = Variables.STORE_CONFIG_SWAGGER_NAME,
                    STORE_CONFIG_SAVEDB = Variables.STORE_CONFIG_SAVEDB,
                    STORE_PATH_LOGS = Variables.STORE_PATH_LOGS,
                    STORE_DATABASE_IP = Variables.STORE_DATABASE_IP,
                    STORE_DATABASE_PORT = Variables.STORE_DATABASE_PORT,
                    STORE_DATABASE_NAMEDB_STORE = Variables.STORE_DATABASE_NAMEDB_STORE,
                    STORE_DATABASE_USER = Variables.STORE_DATABASE_USER,
                    STORE_DATABASE_PASS = Variables.STORE_DATABASE_PASS,
                    STORE_DATABASE_ENCRYPT = Variables.STORE_DATABASE_ENCRYPT,
                    STORE_DATABASE_TRUST_CERT = Variables.STORE_DATABASE_TRUST_CERT,
                    STORE_DATABASE_CONNECTION = Variables.STORE_DATABASE_CONNECTION,
                    STORE_KEY_EMAIL_HOST = Variables.STORE_KEY_EMAIL_HOST,
                    STORE_KEY_EMAIL_BASE = Variables.STORE_KEY_EMAIL_BASE,
                    STORE_KEY_EMAIL_PORT = Variables.STORE_KEY_EMAIL_PORT,
                    STORE_KEY_EMAIL_EMAIL_NAME = Variables.STORE_KEY_EMAIL_EMAIL_NAME,
                    STORE_KEY_EMAIL_USER = Variables.STORE_KEY_EMAIL_USER,
                    STORE_KEY_EMAIL_PASSW = Variables.STORE_KEY_EMAIL_PASSW,
                    STORE_KEY_EMAIL_SSL = Variables.STORE_KEY_EMAIL_SSL,
                    STORE_PATH_EMAIL_TEMPLATE = Variables.STORE_PATH_EMAIL_TEMPLATE,
                    STORE_JWT_SECRET_KEY = Variables.STORE_JWT_SECRET_KEY,
                    STORE_JWT_ISSUER = Variables.STORE_JWT_ISSUER,
                    STORE_JWT_AUDIENCE = Variables.STORE_JWT_AUDIENCE,
                    STORE_JWT_EXPIRATION_MINUTES = Variables.STORE_JWT_EXPIRATION_MINUTES,
                    STORE_EXTERNAL_URL = Variables.STORE_EXTERNAL_URL,
                    STORE_EXTERNAL_URL2 = Variables.STORE_EXTERNAL_URL2,

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
