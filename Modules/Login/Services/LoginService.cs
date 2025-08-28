using static BACKEND_STORE.Modules.Login.Models.Login;
using BACKEND_STORE.Modules.Login.Interfaces;

namespace BACKEND_STORE.Modules.Login.Services
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
