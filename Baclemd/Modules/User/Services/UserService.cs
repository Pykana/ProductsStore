using BACKEND_STORE.Shared.ENTITIES;
using BACKEND_STORE.Modules.User.Interfaces;
using BACKEND_STORE.Shared;
using static BACKEND_STORE.Modules.User.Models.User;

namespace BACKEND_STORE.Modules.User.Services
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
        public Task<userDTO> GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }
        public Task<GenericResponseDTO> CreateUser(UserRequestPost data)
        {
            return _userRepository.CreateUser(data);
        }
        public Task<GenericResponseDTO> UpdateUser(UserRequestPut data)
        {
            return _userRepository.UpdateUser(data);
        }

        public Task<GenericResponseDTO> DeleteUser(int id, string pass, string Actual_User)
        {
            return _userRepository.DeleteUser(id, pass, Actual_User);
        }

    }
}
