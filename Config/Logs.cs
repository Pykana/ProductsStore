namespace BACKEND_STORE.Config
{
    public class Logs
    {
        private readonly string _logsPath;
        private readonly bool _flagSaveDB;

        public Logs(IConfiguration configuration)
        {
            _logsPath = configuration["Secrets:STORE_PATH_LOGS"] ?? AppContext.BaseDirectory + "Logs";
            _flagSaveDB = bool.TryParse(configuration["Secrets:STORE_CONFIG_SAVEDB"], out var flag) && flag;
        }

        public bool SaveLog(string message)
        {
            try
            {
                switch (_flagSaveDB) {
                    case true: //  
                        return false;
                    case false:
                        string path;
                        path = string.IsNullOrEmpty(_logsPath) ? AppContext.BaseDirectory + "Logs" : _logsPath;

                        string fullPath = Path.Combine(path, $"log_{DateTime.Now:yyyyMMdd}.txt");
                        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";

                        File.AppendAllText(fullPath, logMessage);
                        return true;
                    default:
                        throw new InvalidOperationException("La configuración de guardado de logs no es válida.");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var crashDir = Path.Combine(AppContext.BaseDirectory, "Logs");
                    Directory.CreateDirectory(crashDir);
                    File.AppendAllText(
                        Path.Combine(crashDir, $"crash_{DateTime.Now:yyyyMMdd}.txt"),
                        $"[{DateTime.Now}] Error al guardar log: {ex.Message}{Environment.NewLine}"
                    );
                    return false;
                }
                catch
                {
                    return false;
                    // Pray for your life
                }
            }
        }
    }
}
