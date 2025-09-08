using BACKEND_STORE.Shared;
using BACKEND_STORE.Shared.ENTITIES;
using static BACKEND_STORE.Modules.User.Models.User;

namespace BACKEND_STORE.Modules.User.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<GenericResponseDTO> CreateUser(UserRequestPost data);
        Task<userDTO> GetUserById(int id);
        Task<GenericResponseDTO> UpdateUser(UserRequestPut data);
        Task<GenericResponseDTO> DeleteUser(int id, string pass, string Actual_User);

    }
}
