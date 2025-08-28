using BACKEND_STORE.Modules.User.Interfaces;
using BACKEND_STORE.Shared;
using BACKEND_STORE.Shared.DB;
using BACKEND_STORE.Shared.ENTITIES;
using System.Text.Json;
using static BACKEND_STORE.Modules.User.Models.User;

namespace BACKEND_STORE.Modules.User.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;  // DbContext for database operations
        private readonly Encryption _encryption; // Encryption service for password hashings
        private readonly Logs _logs; // Logging service for logging messages

        public UserRepository(AppDbContext context, Encryption encryption, Logs logs)
        {
            _context = context;
            _encryption = encryption;
            _logs = logs;
        }

        public Task<IEnumerable<Users>> GetAllUsers()
        {
            try
            {
                _logs.SaveLog("Iniciando GetAllUsers");

                IEnumerable<Users> users = _context.Users
                    .Where(u => u.is_active) 
                    .ToList();

                if (!users.Any())
                {
                    _logs.SaveLog("Sin usuerios en la tabla");
                }
                else
                {
                    _logs.SaveLog($"Found {users.Count()} active users.");
                }
                _logs.SaveLog("Result: " + JsonSerializer.Serialize(users.ToList()));

                return Task.FromResult(users);
            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error in GetAllUsers: {ex.Message}");
                return Task.FromResult<IEnumerable<Users>>(new List<Users>());
            }
        }
        public Task<userDTO> GetUserById(int id)
        {
            try
            {
                _logs.SaveLog($"Iniciando GetUserById con ID: {id}");



                var user = _context.Users.Where(u => u.Id_User == id && u.is_active).FirstOrDefault();
                var role = _context.Roles.Where(r => r.Id_Role == user.RoleId).FirstOrDefault();

                if(user == null)
                {
                    _logs.SaveLog($"User with ID {id} not found.");
                    return Task.FromResult<userDTO>(null);
                }
                if(role == null)
                {
                    _logs.SaveLog($"Role for user ID {id} not found.");
                    return Task.FromResult<userDTO>(null);
                }

                userDTO userDto = new userDTO
                {
                    user_name = user.username,
                    user_email = user.email,
                    role = role.role_name
                };
                return Task.FromResult(userDto);
            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error in GetUserById: {ex.Message}");
                return Task.FromResult<userDTO>(null);
            }
        }

        public Task<GenericResponseDTO> CreateUser(UserRequestPost data)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.email == data.user_email);
                if (existingUser != null)
                {
                    _logs.SaveLog($"User with email {data.user_email} already exists.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "El correo ya esta en uso."
                    });
                }

                existingUser = _context.Users.FirstOrDefault(u => u.username == data.user_name);
                if (existingUser != null)
                {
                    _logs.SaveLog($"User {data.user_name} already exists.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "El nombre de usuario ya esta en uso."
                    });
                }

                Users newUser = new Users
                {
                    username = data.user_name,
                    name = data.name,
                    lastname = data.lastname,
                    email = data.user_email,
                    password = _encryption.HashPassword(data.user_password),
                    is_active = true,
                    RoleId = data.Id_Role,
                    created_at = DateTime.UtcNow,
                    created_by = data.userEdit
                };

                var role = _context.Roles.FirstOrDefault(r => r.Id_Role == data.Id_Role);
                if (role != null) {
                    newUser.Role = role;
                }
                else
                {
                    _logs.SaveLog($"Role with ID {data.Id_Role} not found.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Rol no encontrado."
                    });
                }

                _context.Users.Add(newUser);
                if (_context.SaveChanges() > 0)
                {
                    _logs.SaveLog($"User {data.user_name} created successfully.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = true,
                        Message = "Usuario creado correctamente."
                    });
                }
                else
                {
                    _logs.SaveLog("Failed to create user.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Error al crear el usuario."
                    });
                }
            }
            catch (Exception ex)
            {
                _logs.SaveLog($"Error in CreateUser: {ex.Message}");
                return Task.FromResult(new GenericResponseDTO
                {
                    Success = false,
                    Message = "Error interno del servidor."
                });
            }
        }

        public Task<GenericResponseDTO> UpdateUser(UserRequestPut data)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id_User == data.Id_User && u.is_active);
                if(user == null)
                {
                    _logs.SaveLog($"User with ID {data.Id_User} not found.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Usuario no encontrado."
                    });
                }
                var role = _context.Roles.FirstOrDefault(r => r.Id_Role == data.Id_Role);
                if (role == null)
                {
                    _logs.SaveLog($"Role with ID {data.Id_Role} not found.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Rol no encontrado."
                    });
                }

                var passwordHash = _encryption.HashPassword(data.user_password);

                var VerifiPassword = _encryption.VerifyPassword(data.user_password, user.password);
                if(!VerifiPassword)
                {
                    _logs.SaveLog($"Password verification failed for user ID {data.Id_User}.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Contraseña incorrecta."
                    });
                }
                user.username = data.user_name;
                user.email = data.user_email;
                user.password = passwordHash;
                user.RoleId = data.Id_Role;
                user.is_active = data.IsActive;
                user.updated_at = DateTime.UtcNow;

                _context.Users.Update(user);
                if (_context.SaveChanges() > 0)
                {
                    _logs.SaveLog($"User {data.Id_User} updated successfully.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = true,
                        Message = "Usuario actualizado correctamente."
                    });
                }
                else
                {
                    _logs.SaveLog("Failed to update user.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Error al actualizar el usuario."
                    });
                }

            }
            catch(Exception ex)
            {
                _logs.SaveLog($"Error in UpdateUser: {ex.Message}");
                return Task.FromResult(new GenericResponseDTO
                {
                    Success = false,
                    Message = "Error interno del servidor."
                });
            }
        }


        public Task<GenericResponseDTO> DeleteUser(int id, string pass, string Actual_User)
        {
            try
            {
                    var user = _context.Users.FirstOrDefault(u => u.Id_User == id && u.is_active);
                if (user == null)
                {
                    _logs.SaveLog($"User with ID {id} not found.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Usuario no encontrado."
                    });
                }
                var VerifiPassword = _encryption.VerifyPassword(pass, user.password);
                if (!VerifiPassword)
                {
                    _logs.SaveLog($"Password verification failed for user ID {id}.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Contraseña incorrecta."
                    });
                }
                if(user.username == Actual_User)
                {
                    _logs.SaveLog($"User {Actual_User} cannot delete themselves.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "No puedes eliminar tu propio usuario."
                    });
                }
                if(user.RoleId == 1)
                {
                    _logs.SaveLog($"User {user.username} with ID {id} is an admin and cannot be deleted.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "No puedes eliminar un usuario administrador."
                    });
                }
                if(user.is_active == false)
                {
                    _logs.SaveLog($"User {user.username} with ID {id} is already inactive.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "El usuario ya esta inactivo."
                    });
                }
                user.is_active = false;
                user.deleted_at = DateTime.UtcNow;
                user.deleted_by = Actual_User;

                _context.Users.Update(user);
                if (_context.SaveChanges() <= 0)
                {
                    _logs.SaveLog("Failed to delete user.");
                    return Task.FromResult(new GenericResponseDTO
                    {
                        Success = false,
                        Message = "Error al eliminar el usuario."
                    });
                }
                return Task.FromResult(new GenericResponseDTO
                {
                    Success = true,
                    Message = "Usuario eliminado correctamente."
                });
            }
            catch(Exception ex)
            {
                _logs.SaveLog($"Error in DeleteUser: {ex.Message}");
                return Task.FromResult(new GenericResponseDTO
                {
                    Success = false,
                    Message = "Error interno del servidor."
                });
            }
        }


    }
}
