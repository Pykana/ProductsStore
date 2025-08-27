using BACKEND_STORE.Models;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Interfaces.IService.Version0
{
    public interface ITestService
    {
        Task<Test> ProbarConexion();
        Task<VariablesEntorno> VerVaribalesDeEntorno();
        Task<string> VerificarEncriptamiento(string contraseña);
        Task<string> VerificarLogs(string Mensaje);

    }
}
