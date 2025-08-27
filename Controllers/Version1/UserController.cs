using BACKEND_STORE.Interfaces.IService.Version1;
using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static BACKEND_STORE.Models.User;

namespace BACKEND_STORE.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<Users>), 200)]
        public ActionResult GetAllUsers() {
            try
            {
                IEnumerable<Users> AllUsers = _userService.GetAllUsers().Result;
                return Ok(AllUsers);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Error: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("UserById")]
        [ProducesResponseType(typeof(userDTO), 200)]
        public async Task<IActionResult> GetUserById([Required(ErrorMessage ="Debe ingresar un ID")]int id)
        {
            try
            {
                userDTO user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Error de argumento: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("CreateUser")]
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

        [HttpPut("UpdateUser")]
        [ProducesResponseType(typeof(GenericResponseDTO), 200)]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequestPut data)
        {
            try
            {
                GenericResponseDTO result = await _userService.UpdateUser(data);
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

        [HttpDelete("DeleteUser")]
        [ProducesResponseType(typeof(GenericResponseDTO), 200)]
        public async Task<IActionResult> DeleteUser(int id, string pass, string Actual_User)
        {
            try
            {
                GenericResponseDTO result = await _userService.DeleteUser(id, pass, Actual_User);
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
    }
}
