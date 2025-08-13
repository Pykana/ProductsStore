using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;
using static BACKEND_STORE.Models.Role;

namespace BACKEND_STORE.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService ) {
            _roleService = roleService;
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                IEnumerable<RolePost> roles = await _roleService.GetRoles();
                return Ok(roles);
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


        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestPost data)
        {
            try
            {
                Role.RolePost result = await _roleService.CreateRole(data);
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

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleRequestPut data)
        {
            try
            {
                GenericResponseDTO result = await _roleService.UpdateRole(data);
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

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] int id , string user)
        {
            try
            {
                GenericResponseDTO result = await _roleService.DeleteRole(id, user);
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
