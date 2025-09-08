using dotenv.net;

namespace BACKEND_STORE.Config
{
    public static class EnvironmentVariableConfig
    {
        public static VariablesEntorno Variables { get; private set; } = new VariablesEntorno();
        private static string GetEnv(IDictionary<string, string> env, string key, string defaultValue = "")
        {
            return env.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void Load()
        {
            var envVars = DotEnv.Read();

            Variables = new VariablesEntorno
            {
                // Debug / Cliente
                CONFIG_DEBUG = GetEnv(envVars, "CONFIG_DEBUG", "false").ToLower() == "true",
                CONFIG_CLIENT = GetEnv(envVars, "CONFIG_CLIENT") ?? "CLIENT",
                STORE_CONFIG_TIMEZONE = GetEnv(envVars, "STORE_CONFIG_TIMEZONE") ?? "UTC",
                STORE_CONFIG_CORS_ORIGINS = GetEnv(envVars, "STORE_CONFIG_CORS_ORIGINS")
                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .ToList(),

                // Swagger
                STORE_CONFIG_SWAGGER = GetEnv(envVars, "STORE_CONFIG_SWAGGER", "false").ToLower() == "true",
                STORE_CONFIG_SWAGGER_NAME = GetEnv(envVars, "STORE_CONFIG_SWAGGER_NAME") ?? "docs",

                // Logs
                STORE_CONFIG_SAVEDB = GetEnv(envVars, "STORE_CONFIG_SAVEDB", "false").ToLower() == "true",
                STORE_PATH_LOGS = GetEnv(envVars, "STORE_PATH_LOGS") ?? "Logs",

                // DB
                STORE_DATABASE_IP = GetEnv(envVars, "STORE_DATABASE_IP"),
                STORE_DATABASE_PORT = int.TryParse(GetEnv(envVars, "STORE_DATABASE_PORT", "1433"), out var port) ? port : 1433,
                STORE_DATABASE_NAMEDB_STORE = GetEnv(envVars, "STORE_DATABASE_NAMEDB_STORE"),
                STORE_DATABASE_USER = GetEnv(envVars, "STORE_DATABASE_USER"),
                STORE_DATABASE_PASS = GetEnv(envVars, "STORE_DATABASE_PASS"),
                STORE_DATABASE_ENCRYPT = GetEnv(envVars, "STORE_DATABASE_ENCRYPT", "true").ToLower() == "true",
                STORE_DATABASE_TRUST_CERT = GetEnv(envVars, "STORE_DATABASE_TRUST_CERT", "true").ToLower() == "true",

                // Email
                STORE_KEY_EMAIL_HOST = GetEnv(envVars, "STORE_KEY_EMAIL_HOST"),
                STORE_KEY_EMAIL_BASE = GetEnv(envVars, "STORE_KEY_EMAIL_BASE"),
                STORE_KEY_EMAIL_PORT = int.TryParse(GetEnv(envVars, "STORE_KEY_EMAIL_PORT", "587"), out var emailPort) ? emailPort : 587,
                STORE_KEY_EMAIL_EMAIL_NAME = GetEnv(envVars, "STORE_KEY_EMAIL_EMAIL_NAME"),
                STORE_KEY_EMAIL_USER = GetEnv(envVars, "STORE_KEY_EMAIL_USER"),
                STORE_KEY_EMAIL_PASSW = GetEnv(envVars, "STORE_KEY_EMAIL_PASSW"),
                STORE_KEY_EMAIL_SSL = GetEnv(envVars, "STORE_KEY_EMAIL_SSL", "true").ToLower() == "true",

                // Email templates
                STORE_PATH_EMAIL_TEMPLATE = GetEnv(envVars, "STORE_PATH_EMAIL_TEMPLATE") ?? "Templates",

                // JWT
                STORE_JWT_SECRET_KEY = GetEnv(envVars, "STORE_JWT_SECRET_KEY"),
                STORE_JWT_ISSUER = GetEnv(envVars, "STORE_JWT_ISSUER"),
                STORE_JWT_AUDIENCE = GetEnv(envVars, "STORE_JWT_AUDIENCE"),
                STORE_JWT_EXPIRATION_MINUTES = int.TryParse(GetEnv(envVars, "STORE_JWT_EXPIRATION_MINUTES", "60"), out var jwtExp) ? jwtExp : 60,

                // External APIs
                STORE_EXTERNAL_URL = GetEnv(envVars, "STORE_EXTERNAL_URL"),
                STORE_EXTERNAL_URL2 = GetEnv(envVars, "STORE_EXTERNAL_URL2")
            };

            var connectionString =
                    $"Server={Variables.STORE_DATABASE_IP},{Variables.STORE_DATABASE_PORT};" +
                    $"Database={Variables.STORE_DATABASE_NAMEDB_STORE};" +
                    $"User Id={Variables.STORE_DATABASE_USER};" +
                    $"Password={Variables.STORE_DATABASE_PASS};" +
                    $"Encrypt={Variables.STORE_DATABASE_ENCRYPT};" +
                    $"TrustServerCertificate={Variables.STORE_DATABASE_TRUST_CERT};";

            Variables.STORE_DATABASE_CONNECTION = connectionString;
        }

        public class VariablesEntorno
        {
            // Debug
            public bool CONFIG_DEBUG { get; set; } = false;
            public string CONFIG_CLIENT { get; set; } = string.Empty;
            public string STORE_CONFIG_TIMEZONE { get; set; } = string.Empty;
            public List<string>? STORE_CONFIG_CORS_ORIGINS { get; set; } = new List<string>();

            // Swagger
            public bool STORE_CONFIG_SWAGGER { get; set; } = false;
            public string STORE_CONFIG_SWAGGER_NAME { get; set; } = string.Empty;

            // Logs
            public bool STORE_CONFIG_SAVEDB { get; set; } = false;
            public string STORE_PATH_LOGS { get; set; } = string.Empty;

            // Database
            public string STORE_DATABASE_IP { get; set; } = string.Empty;
            public int STORE_DATABASE_PORT { get; set; } = 0;
            public string STORE_DATABASE_NAMEDB_STORE { get; set; } = string.Empty;
            public string STORE_DATABASE_USER { get; set; } = string.Empty;
            public string STORE_DATABASE_PASS { get; set; } = string.Empty;
            public bool STORE_DATABASE_ENCRYPT { get; set; } = true;
            public bool STORE_DATABASE_TRUST_CERT { get; set; } = true;
            public string STORE_DATABASE_CONNECTION { get; set; } = string.Empty;

            // Email
            public string STORE_KEY_EMAIL_HOST { get; set; } = string.Empty;
            public string STORE_KEY_EMAIL_BASE { get; set; } = string.Empty;
            public int STORE_KEY_EMAIL_PORT { get; set; } = 0;
            public string STORE_KEY_EMAIL_EMAIL_NAME { get; set; } = string.Empty;
            public string STORE_KEY_EMAIL_USER { get; set; } = string.Empty;
            public string STORE_KEY_EMAIL_PASSW { get; set; } = string.Empty;
            public bool STORE_KEY_EMAIL_SSL { get; set; } = true;

            // Email Templates
            public string STORE_PATH_EMAIL_TEMPLATE { get; set; } = string.Empty;

            // JWT
            public string STORE_JWT_SECRET_KEY { get; set; } = string.Empty;
            public string STORE_JWT_ISSUER { get; set; } = string.Empty;
            public string STORE_JWT_AUDIENCE { get; set; } = string.Empty;
            public int STORE_JWT_EXPIRATION_MINUTES { get; set; } = 30;

            // External APIs
            public string STORE_EXTERNAL_URL { get; set; } = string.Empty;
            public string STORE_EXTERNAL_URL2 { get; set; } = string.Empty;
        }

        //Exponer a Controlador
        public static Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
             {
                #region Variables
                 //Cliente
                 { "DEBUG", Variables.CONFIG_DEBUG.ToString() },
                #endregion
                #region Secretos
                 //{ "Secrets:RUTA_PLANTILLAS", Variables.RUTA_PLANTILLAS },
                 #endregion
             };
        }
    }
}
