using BACKEND_STORE.Modules.Tests.Models;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Modules.Tests.Interfaces
{
    public interface ITestService
    {
        Task<Test> ProbarConexion();
        Task<VariablesEntorno> VerVaribalesDeEntorno();
        Task<string> VerificarEncriptamiento(string contraseña);
        Task<string> VerificarLogs(string Mensaje);

    }
}
