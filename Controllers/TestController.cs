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

                if (resultado.Value == 1)
                {
                    return Ok(new { message = resultado.Message });
                }
                else
                {
                    return StatusCode(500, new { message = resultado.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
