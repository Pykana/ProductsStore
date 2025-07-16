namespace BACKEND_STORE.Models.GET
{
    public class Test
    {
        public int Value { get; set; }
        public string Message { get; set; }
    }

    public class StoreConfig
    {
        public DatabaseConfig Database { get; set; } = new();
        public EmailConfig Email { get; set; } = new();
        public LogConfig Logs { get; set; } = new();
        public ApiConfig Api { get; set; } = new();
        public ExternalUrlsConfig ExternalUrls { get; set; } = new();
    }

    public class DatabaseConfig
    {
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Name { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool TrustCert { get; set; }
    }

    public class EmailConfig
    {
        public string Host { get; set; } = string.Empty;
        public string Base { get; set; } = string.Empty;
        public int Port { get; set; }
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }

    public class LogConfig
    {
        public string LogsPath { get; set; } = string.Empty;
        public string EmailTemplatePath { get; set; } = string.Empty;
    }

    public class ApiConfig
    {
        public string Timezone { get; set; } = string.Empty;
        public bool SwaggerEnabled { get; set; }
        public bool SaveToDatabase { get; set; }
        public string CorsOrigins { get; set; } = string.Empty;
    }

    public class ExternalUrlsConfig
    {
        public string Url1 { get; set; } = string.Empty;
        public string Url2 { get; set; } = string.Empty;
    }

}
