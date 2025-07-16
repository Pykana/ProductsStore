using static BACKEND_STORE.Models.DTO.LoginDTO;
using static BACKEND_STORE.Models.POST.Login;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface ILoginRepository
    {
       Task<LoginResponse> Register(registerPOST dataUser);

    }
}
