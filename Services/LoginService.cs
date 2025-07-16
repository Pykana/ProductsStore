using BACKEND_STORE.Interfaces.IService;
using static BACKEND_STORE.Models.DTO.LoginDTO;
using static BACKEND_STORE.Models.POST.Login;
using static BACKEND_STORE.Models.GET.Login;

namespace BACKEND_STORE.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginService _LoginService;
        public LoginService(ILoginService LoginService)
        {
            _LoginService = LoginService;
        }


        public async Task<LoginResponse> Register(registerPOST dataUser)
        {
            return await _LoginService.Register(dataUser);
        }



    }
}
