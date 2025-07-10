using BACKEND_STORE.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_STORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("conexion")]
        public async Task<IActionResult> ProbarConexion()
        {
            try
            {
                var puedeConectar = await _context.Database.CanConnectAsync();

                if (puedeConectar)
                    return Ok("Conexión exitosa con la base de datos.");
                else
                    return StatusCode(500, "No se pudo conectar a la base de datos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error de conexión: {ex.Message}");
            }
        }
    }
}
