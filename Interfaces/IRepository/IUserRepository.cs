using BACKEND_STORE.Models.ENTITIES;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
    }
}
