using Application.Interfaces.UserInterface;
using Application.Models.AuthModels.Login;
using Application.Models.AuthModels.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var result = await _userService.RegisterUser(request);
            return new JsonResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var result = await _userService.LoginUser(request);
            return new JsonResult(result);
        }

        [HttpGet("users")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return new JsonResult(result);
        }
    }
}
