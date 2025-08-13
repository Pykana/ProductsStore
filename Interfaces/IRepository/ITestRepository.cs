using BACKEND_STORE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface ITestRepository
    {
        Task<Test> ProbarConexion();
        Task<StoreConfig> VerVaribalesDeEntorno();
        Task<string> VerificarEncriptamiento(string contraseña);
        Task<string> VerificarLogs(string Mensaje);

    }
}
