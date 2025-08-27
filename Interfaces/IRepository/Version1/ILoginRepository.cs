using static BACKEND_STORE.Models.Login;

namespace BACKEND_STORE.Interfaces.IRepository.Version1
{
    public interface ILoginRepository
    {
       Task<LoginResponse> Login(login dataUser);
    }
}
