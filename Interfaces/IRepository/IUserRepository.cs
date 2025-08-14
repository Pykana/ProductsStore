using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using static BACKEND_STORE.Models.User;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<GenericResponseDTO> CreateUser(UserRequestPost data);
        Task<userDTO> GetUserById(int id);
        Task<GenericResponseDTO> UpdateUser(UserRequestPut data);
        Task<GenericResponseDTO> DeleteUser(int id, string pass, string Actual_User);
    }
}
