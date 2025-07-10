using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Models.DB;
using BACKEND_STORE.Models.GET;

namespace BACKEND_STORE.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;

        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Test> ProbarConexion()
        {
            try
            {
                var puedeConectar = await _context.Database.CanConnectAsync();

                if (puedeConectar)
                    return new Test
                    {
                        Value = 1,
                        Message = "Conexión exitosa a la base de datos."
                    };
                else
                    return new Test
                    {
                        Value = 0,
                        Message = "No se pudo establecer conexión con la base de datos."
                    };
            }
            catch (Exception ex)
            {
                return new Test
                {
                    Value = ex.HResult,
                    Message = $"Error al intentar conectar a la base de datos: {ex.Message}"
                };
            }
        }
    }
}
