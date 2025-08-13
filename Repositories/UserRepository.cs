using BACKEND_STORE.Config;
using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Models.ENTITIES;
using BACKEND_STORE.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BACKEND_STORE.Repositories
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



    }
}
