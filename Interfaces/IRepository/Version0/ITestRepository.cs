using BACKEND_STORE.Models;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Interfaces.IRepository.Version0
{
    public interface ITestRepository
    {
        Task<Test> ProbarConexion();
        Task<VariablesEntorno> VerVaribalesDeEntorno();
        Task<string> VerificarEncriptamiento(string contraseña);
        Task<string> VerificarLogs(string Mensaje);
    }
}
