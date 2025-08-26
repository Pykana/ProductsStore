using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models;
using BACKEND_STORE.Models.ENTITIES;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

        //[Authorize(Roles = "1,2")]
        [HttpGet("GetRoles")]
        [ProducesResponseType(typeof(IEnumerable<RolePost>), 200)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                IEnumerable<RolePost> roles = await _roleService.GetRoles();
                if (roles == null || !roles.Any())
                {
                    return NotFound("No se encontraron roles.");
                }
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

        //[Authorize(Roles = "1,2")]
        [HttpGet("GetRoleByID")]
        [ProducesResponseType(typeof(RolePost), 200)]
        public async Task<IActionResult> GetRolesById([Required(ErrorMessage ="El ID es obligatorio")] int id)
        {
            try
            {
                RolePost roles = await _roleService.GetRolesById(id);
                if (roles == null)
                {
                    return NotFound($"Rol con ID {id} no encontrado.");
                }
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


        [Authorize(Roles = "1,2")]
        [HttpPost("CreateRole")]
        [ProducesResponseType(typeof(Role.RolePost), 200)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestPost data)
        {
            try
            {
                Role.RolePost result = await _roleService.CreateRole(data);
                if (result == null)
                {
                    return BadRequest("Error al crear el rol. Verifique los datos proporcionados.");
                }
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
        [Authorize(Roles = "1,2")]
        [HttpPut("UpdateRole")]
        [ProducesResponseType(typeof(GenericResponseDTO), 200)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleRequestPut data)
        {
            try
            {
                GenericResponseDTO result = await _roleService.UpdateRole(data);
                if (result == null)
                {
                    return BadRequest("Error al actualizar el rol. Verifique los datos proporcionados.");
                }
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

        [Authorize(Roles = "1")]
        [HttpDelete("DeleteRole")]
        [ProducesResponseType(typeof(GenericResponseDTO), 200)]
        public async Task<IActionResult> DeleteRole([FromQuery] int id , string user)
        {
            try
            {
                GenericResponseDTO result = await _roleService.DeleteRole(id, user);
                if (result == null)
                {
                    return BadRequest("Error al eliminar el rol. Verifique los datos proporcionados.");
                }
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
