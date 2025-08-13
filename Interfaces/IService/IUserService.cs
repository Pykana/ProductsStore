using BACKEND_STORE.Models.ENTITIES;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsers();

    }
}
