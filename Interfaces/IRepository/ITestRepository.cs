using BACKEND_STORE.Models.GET;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface ITestRepository
    {
        Task<Test> ProbarConexion();
    }
}
