using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models.DB;
using static BACKEND_STORE.Models.DTO.LoginDTO;
using static BACKEND_STORE.Models.POST.Login;

namespace BACKEND_STORE.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> Register(registerPOST dataUser)
        {
            return null;
        }
    }
}
