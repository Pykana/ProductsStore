using BACKEND_STORE.Interfaces.IService;
using static BACKEND_STORE.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_STORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _LoginService;
        public LoginController(ILoginService LoginService)
        {
            _LoginService = LoginService;
        }

        [HttpPost("RegisterNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] registerPOST dataUser)
        {
            try
            {
                LoginResponse resultado = await _LoginService.Register(dataUser);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


    }
}
