using BACKEND_STORE.Modules.Login.Interfaces;
using BACKEND_STORE.Modules.User.Interfaces;
using BACKEND_STORE.Shared;
using Microsoft.AspNetCore.Mvc;
using static BACKEND_STORE.Modules.Login.Models.Login;
using static BACKEND_STORE.Modules.User.Models.User;

namespace BACKEND_STORE.Modules.Login.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private ILoginService _LoginService;
        private IUserService _userService;

        public LoginController(ILoginService LoginService, IUserService userService)
        {
            _LoginService = LoginService;
            _userService = userService;
        }

        [HttpPost("RegisterNewUser")]
        [ProducesResponseType(typeof(GenericResponseDTO), 200)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestPost data)
        {
            try
            {
                GenericResponseDTO result = await _userService.CreateUser(data);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Error de argumento: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"Operación inválida: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] login dataUser)
        {
            try
            {
                LoginResponse resultado = await _LoginService.Login(dataUser);
                
                if(resultado == null || !resultado.success)
                {
                    return Unauthorized("Credenciales inválidas o usuario no encontrado.");
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
