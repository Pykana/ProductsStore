namespace BACKEND_STORE.Config
{
    public static class Connection
    {
        public static string SQLServerConnection(IConfiguration config)
        {
            return $"Server={config["STORE_DATABASE_IP"]},{config["STORE_DATABASE_PORT"]};" +
                   $"Database={config["STORE_DATABASE_NAMEDB_STORE"]};" + 
                   $"User Id={config["STORE_DATABASE_USER"]};" +
                   $"Password={config["STORE_DATABASE_PASS"]};" +
                   $"TrustServerCertificate={config["STORE_DATABASE_TRUST_CERT"]};Encrypt=True;";
        }
    }
}

