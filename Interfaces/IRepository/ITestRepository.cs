using BACKEND_STORE.Models.DB;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface ITestRepository
    {
        Task<Test> ProbarConexion();
    }
}
