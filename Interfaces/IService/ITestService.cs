using BACKEND_STORE.Models.GET;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface ITestService
    {
        Task<Test> ProbarConexion();
    }
}
