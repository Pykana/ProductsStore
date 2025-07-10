using BACKEND_STORE.Models.DB;

namespace BACKEND_STORE.Interfaces
{
    public interface ITestRepository
    {
        Task<Test> ProbarConexion();
    }
}
