using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_STORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUserInfo")]
        public ActionResult Index() {
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

    }
}
