using Application.Interfaces.UserInterfaces;
using Application.Models.UserModels;
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

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequestDTO user)
        {
            var result = await _userService.RegisterUser(user);
            return new JsonResult(result);
        }

        // Get: api/user/login
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO login)
        {
            var result = await _userService.Login(login);
            return new JsonResult(result);
        }

        //Get: api/user/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return new JsonResult(result);
        }
    }
}

