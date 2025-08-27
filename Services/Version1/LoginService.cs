using static BACKEND_STORE.Models.Login;
using BACKEND_STORE.Interfaces.IRepository.Version1;
using BACKEND_STORE.Interfaces.IService.Version1;

namespace BACKEND_STORE.Services.Version1
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<LoginResponse> Login(login dataUser)
        {
            return await _loginRepository.Login(dataUser);
        }

    }
}
