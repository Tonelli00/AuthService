using Application.Features.User.Command;
using Application.Features.User.Query;
using Application.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _mediator.Send(new RegisterCommand(request));
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginQuery(request));
            return Ok(result);
        }


        [HttpPatch("change-password")]
        [Authorize(Roles = "Current,Admin,SuperAdmin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null)
                return Unauthorized(new { message = "UserId claim not found" });

            request.UserId = Guid.Parse(userId);

            var response = await _mediator.Send(new ChangePasswordCommand(request));

            return Ok(response);
        }


        [HttpPatch("change-role/{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeUserRole([FromRoute] Guid userId, [FromBody] ChangeUserRoleRequest request)
        {
            var response = await _mediator.Send(new ChangeUserRoleCommand(userId, request));
            return Ok(response);
        }

        [HttpGet("users")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }
    }
}
