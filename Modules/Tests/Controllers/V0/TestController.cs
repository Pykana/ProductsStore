using BACKEND_STORE.Modules.Tests.Interfaces;
using BACKEND_STORE.Modules.Tests.Models;
using BACKEND_STORE.Modules.User.Models;
using BACKEND_STORE.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Modules.Tests.Controllers.V0
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        private ITestService _TestService;

        public TestController(ITestService TestService)
        {
            _TestService = TestService;
        }


        [HttpGet("ProbarConexion")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ProducesResponseType(typeof(Test), 200)]
        public async Task<IActionResult> ProbarConexion()
        {
            try
            {
                Test resultado = await _TestService.ProbarConexion();

                switch (resultado.Value)
                {
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

        [HttpGet("VerVaribalesDeEntorno")]
        [ProducesResponseType(typeof(VariablesEntorno), 200)]
        public async Task<IActionResult> VerVaribalesDeEntorno()
        {
            try
            {
                VariablesEntorno resultado = await _TestService.VerVaribalesDeEntorno();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("VerificarEncriptamiento")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> VerificarEncriptamiento(
            [Required(ErrorMessage ="Ingrese una contraseña")]
            [MinLength(8,ErrorMessage ="La contraseña debe tener al menos 8 caracteres")]string contraseña)
        {
            try
            {
                if (string.IsNullOrEmpty(contraseña))
                    return BadRequest("La contraseña no puede estar vacía.");

                string resultado = await _TestService.VerificarEncriptamiento(contraseña);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("VerificarLogs")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> VerificarLogs(
            [Required(ErrorMessage ="Debe Ingresar un mensaje")]
            [MinLength(10,ErrorMessage ="El menasje debe tner al menos 10 caracteres")]string Mensaje)
        {
            try
            {
                if (string.IsNullOrEmpty(Mensaje))
                    return BadRequest("El mensaje no puede estar vacío.");
                string resultado = await _TestService.VerificarLogs(Mensaje);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPost("CreateToken")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> CreateToken()
        {
            try
            {
                try
                {
                    JWT_TokenRequest data = new JWT_TokenRequest
                    {
                        UserName = "Root",
                        UserId = 0,
                        Role = 0,
                        URI = string.Empty
                    };
                    var token = JWT.GenerateJwtToken(data);
                    return Ok(token);
                }
                catch (Exception ex)
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Unauthorized($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
