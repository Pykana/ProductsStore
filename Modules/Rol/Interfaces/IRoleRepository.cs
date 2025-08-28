using BACKEND_STORE.Shared;
using static BACKEND_STORE.Modules.Rol.Models.Role;

namespace BACKEND_STORE.Modules.Rol.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RolePost>> GetRoles();
        Task<RolePost> CreateRole(RoleRequestPost data);
        Task<RolePost> GetRolesById(int id);
        Task<GenericResponseDTO> UpdateRole(RoleRequestPut data);
        Task<GenericResponseDTO> DeleteRole(int id, string user);
    }
}
