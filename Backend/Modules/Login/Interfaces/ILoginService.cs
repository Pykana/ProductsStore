using static BACKEND_STORE.Modules.Login.Models.Login;

namespace BACKEND_STORE.Modules.Login.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponse> Login(login dataUser);
    }
}
