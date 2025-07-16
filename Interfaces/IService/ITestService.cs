using BACKEND_STORE.Models.GET;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface ITestService
    {
        Task<Test> ProbarConexion();
        Task<StoreConfig> VerVaribalesDeEntorno();
        Task<string> VerificarEncriptamiento(string contraseña);
        Task<string> VerificarLogs(string Mensaje);

    }
}
