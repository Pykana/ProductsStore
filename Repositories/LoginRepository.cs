using BACKEND_STORE.Config;
using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Shared;
using static BACKEND_STORE.Models.Login;

namespace BACKEND_STORE.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;  // DbContext for database operations
        private readonly Encryption _encryption; // Encryption service for password hashings
        private readonly Logs _logs; // Logging service for logging messages
        private readonly Token _jwtService; // JWT service for generating tokens


        public LoginRepository(AppDbContext context, Encryption encryption, Logs logs, Token jwtService)
        {
            _context = context;
            _encryption = encryption;
            _logs = logs;
            _jwtService = jwtService;
         }
     
        public async Task<LoginResponse> Login(login dataUser)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.username == dataUser.username && u.is_active);

                if (user == null)
                {
                    _logs.SaveLog($"User {dataUser.username} not found or inactive.");
                    return new LoginResponse
                    {
                        idUser = "",
                        user = "",
                        roleid = "",
                        success = false
                    };
                }

                var hasshpass = _encryption.VerifyPassword(dataUser.password, user.password);
                if (!hasshpass)
                {
                    _logs.SaveLog($"User Passowrd {dataUser.password} is not correct");

                    return new LoginResponse
                    {
                        idUser = user.Id_User.ToString(),
                        user = user.username,
                        roleid = user.RoleId.ToString(),
                        token = "",
                        success = false
                    };
                }
                _logs.SaveLog($"User {dataUser.username} logged in successfully.");

                var token = _jwtService.GenerateToken(user.Id_User.ToString(), user.username, user.RoleId.ToString());

                return new LoginResponse
                {
                    idUser = user.Id_User.ToString(),
                    user = user.username,
                    roleid = user.RoleId.ToString(),
                    token = token, 
                    success = true
                };

            }
            catch(Exception ex)
            {
                _logs.SaveLog($"Error in LoginUser: {ex.Message}");
                return new LoginResponse
                {
                    idUser = "",
                    user = "",
                    roleid = "",
                    token = "",
                    success= false
                };
            }
        }
    }
}
