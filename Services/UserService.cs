using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models.ENTITIES;

namespace BACKEND_STORE.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<Users>> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

    }
}
