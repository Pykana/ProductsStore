using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BACKEND_STORE.Config
{
    public class Logs
    {
        private readonly string _logsPath;
        private readonly bool _flagSaveDB;

        public Logs(IConfiguration configuration)
        {
            _logsPath = configuration["STORE_PATH_LOGS"] ?? AppContext.BaseDirectory + "Logs";
            _flagSaveDB = bool.TryParse(configuration["STORE_CONFIG_SAVEDB"], out var flag) && flag;
        }

        public void SaveLog(string message)
        {
            try
            {
                if (!_flagSaveDB || string.IsNullOrEmpty(_logsPath))
                    return;

                Directory.CreateDirectory(_logsPath);

                string fullPath = Path.Combine(_logsPath, $"log_{DateTime.Now:yyyyMMdd}.txt");
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";

                File.AppendAllText(fullPath, logMessage);
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
                }
                catch
                {
                    // Pray for your life
                }
            }
        }
    }
}
