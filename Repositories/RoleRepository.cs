using BACKEND_STORE.Config;
using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Models.ENTITIES;
using BACKEND_STORE.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static BACKEND_STORE.Models.Role;

namespace BACKEND_STORE.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;  // DbContext for database operations
        private readonly Encryption _encryption; // Encryption service for password hashings
        private readonly Logs _logs; // Logging service for logging messages

        public RoleRepository(AppDbContext context, Encryption encryption, Logs logs)
        {
            _context = context;
            _encryption = encryption;
            _logs = logs;
        }
        public async Task<IEnumerable<RolePost>> GetRoles()
        {
            try
            {
                    var roles = await _context.Roles
                        .Where(r => r.is_active)
                        .Select(r => new RolePost
                        {
                            Id_Role = r.Id_Role,
                            role_name = r.role_name,
                            role_description = r.role_description
                        })
                        .ToListAsync();


                if (!roles.Any())
                {
                    _logs.SaveLog("Sin Roles en la tabla");
                }
                else
                {
                    _logs.SaveLog($"Found {roles.Count} roles users.");
                }

                _logs.SaveLog("Result: " + JsonSerializer.Serialize(roles));

                return roles;
            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error retrieving roles: {ex.Message}");
                throw;
            }
        }


        public async Task<RolePost> CreateRole(RoleRequestPost data)
        {
            try
            {
                _logs.SaveLog($"Iniciando Crear Role con Data : " + JsonSerializer.Serialize(data));

                Roles RoleData = new Roles
                {
                    role_name = data.role_name,
                    role_description = data.role_description,
                    created_at = DateTime.UtcNow,
                    is_active = true,
                    created_by = data.create_by
                };

                _context.Roles.Add(RoleData);

                if(await _context.SaveChangesAsync() > 0)
                {
                    _logs.SaveLog($"Role {data.role_name} created successfully.");

                    return new RolePost
                    {
                        Id_Role = RoleData.Id_Role,
                        role_name = RoleData.role_name,
                        role_description = RoleData.role_description,
                    };
                }
                else
                {
                    _logs.SaveLog("Failed to create role.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error creating role: {ex.Message}");
                throw;
            }
        }


        public async Task<GenericResponseDTO> UpdateRole(RoleRequestPut data)
        {
            try
            {
                _logs.SaveLog($"Iniciando Actualizar Role con Data : " + JsonSerializer.Serialize(data));
                var role = await _context.Roles.FindAsync(data.Id_Role);
                if (role == null)
                {
                    _logs.SaveLog($"Role with ID {data.Id_Role} not found.");
                    return new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }
                role.role_name = data.role_name;
                role.role_description = data.role_description;
                role.is_active = data.IsActive;
                role.updated_at = DateTime.UtcNow;
                role.updated_by = data.update_by;
                _context.Roles.Update(role);
                if (await _context.SaveChangesAsync() > 0)
                {
                    _logs.SaveLog($"Role {data.Id_Role} updated successfully.");
                    return new GenericResponseDTO
                    {
                        Success = true,
                        Message = "Role updated successfully."
                    };
                }
                else
                {
                    _logs.SaveLog("Failed to update role.");
                    return new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Failed to update role."
                    };
                }

            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error updating role: {ex.Message}");
                throw;
            }

        }
    }
}
