using BACKEND_STORE.Shared;
using BACKEND_STORE.Modules.Tests.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;
using BACKEND_STORE.Modules.Tests.Models;

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
                 CheckAccess();

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
                CheckAccess();

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
                CheckAccess();

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
                CheckAccess();
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


        private IActionResult CheckAccess()
        {
            try
            {
                if (!Variables.CONFIG_DEBUG)
                    return BadRequest("Este endpoint está deshabilitado por configuración.");
                return null!;
            }
            catch (Exception)
            {
                return Conflict("Configuracion erronea");
            }
        }


    }
}
