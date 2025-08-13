using static BACKEND_STORE.Models.Login;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface ILoginService
    {
        Task<LoginResponse> Register(registerPOST dataUser);


    }
}
