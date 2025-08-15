using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models;
using BACKEND_STORE.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BACKEND_STORE.Models.Login;
using static BACKEND_STORE.Models.User;

namespace BACKEND_STORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
