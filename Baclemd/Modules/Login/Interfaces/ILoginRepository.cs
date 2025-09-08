using static BACKEND_STORE.Modules.Login.Models.Login;

namespace BACKEND_STORE.Modules.Login.Interfaces
{
    public interface ILoginRepository
    {
       Task<LoginResponse> Login(login dataUser);
    }
}
