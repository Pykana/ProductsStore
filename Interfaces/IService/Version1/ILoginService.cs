using static BACKEND_STORE.Models.Login;

namespace BACKEND_STORE.Interfaces.IService.Version1
{
    public interface ILoginService
    {
        Task<LoginResponse> Login(login dataUser);
    }
}
