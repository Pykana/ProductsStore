using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using static BACKEND_STORE.Models.Role;

namespace BACKEND_STORE.Interfaces.IService
{
    public interface IRoleService
    {
        Task<IEnumerable<RolePost>> GetRoles();
        Task<RolePost> CreateRole(RoleRequestPost data);
        Task<GenericResponseDTO> UpdateRole(RoleRequestPut data);

    }
}
