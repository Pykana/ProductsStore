using BACKEND_STORE.Interfaces.IService;
using static BACKEND_STORE.Models.Login;
using BACKEND_STORE.Interfaces.IRepository;

namespace BACKEND_STORE.Services
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
