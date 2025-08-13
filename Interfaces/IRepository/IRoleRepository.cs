using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using static BACKEND_STORE.Models.Role;

namespace BACKEND_STORE.Interfaces.IRepository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RolePost>> GetRoles();
        Task<RolePost> CreateRole(RoleRequestPost data);
        Task<GenericResponseDTO> UpdateRole(RoleRequestPut data);
        Task<GenericResponseDTO> DeleteRole(int id, string user);
    }
}
