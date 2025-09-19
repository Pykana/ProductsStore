using BACKEND_STORE.Shared.ENTITIES;
using BACKEND_STORE.Modules.Rol.Interfaces;
using BACKEND_STORE.Shared;
using static BACKEND_STORE.Modules.Rol.Models.Role;

namespace BACKEND_STORE.Modules.Rol.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RolePost>> GetRoles()
        {
            return await _roleRepository.GetRoles();
        }
        public async Task<RolePost> GetRolesById(int id)
        {
            return await _roleRepository.GetRolesById(id);
        }
        
        public async Task<RolePost> CreateRole(RoleRequestPost data)
        {
            return await _roleRepository.CreateRole(data);
        }

        public async Task<GenericResponseDTO> UpdateRole(RoleRequestPut data)
        {
            return await _roleRepository.UpdateRole(data);
        }
       
        public async Task<GenericResponseDTO> DeleteRole(int id, string user)
        {
            return await _roleRepository.DeleteRole(id,  user);
        }

    }
}
