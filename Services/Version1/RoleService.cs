using BACKEND_STORE.Interfaces.IRepository.Version1;
using BACKEND_STORE.Interfaces.IService.Version1;
using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using static BACKEND_STORE.Models.Role;

namespace BACKEND_STORE.Services.Version1
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
