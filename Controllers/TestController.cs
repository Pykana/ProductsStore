using BACKEND_STORE.Interfaces;
using BACKEND_STORE.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_STORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ITestService _TestService;
        public TestController( ITestService TestService)
        {
            _TestService = TestService;
        }

        [HttpGet("conexion")]
        public async Task<IActionResult> ProbarConexion()
        {
            try { 
                Test resultado = await _TestService.ProbarConexion();

                switch (resultado.Value) {
                    case 0:
                        return BadRequest(resultado);
                    case 1:
                        return Ok(resultado);
                    default:
                        return StatusCode(500, resultado);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
