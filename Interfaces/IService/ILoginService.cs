using static BACKEND_STORE.Models.DTO.LoginDTO;
using static BACKEND_STORE.Models.POST.Login;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface ILoginService
    {
        Task<LoginResponse> Register(registerPOST dataUser);


    }
}
