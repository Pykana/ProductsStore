using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("ProbarConexion")]
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

        [HttpGet("VerVaribalesDeEntorno")]
        public async Task<IActionResult> VerVaribalesDeEntorno( )
        {
            try
            {
                StoreConfig resultado = await _TestService.VerVaribalesDeEntorno();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("VerificarEncriptamiento")]
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

    }
}
