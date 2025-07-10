using BACKEND_STORE.Models.DB;

namespace BACKEND_STORE.Interfaces
{
    public interface ITestService
    {
        Task<Test> ProbarConexion();
    }
}
