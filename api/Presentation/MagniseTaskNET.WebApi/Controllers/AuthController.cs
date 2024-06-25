using MagniseTaskNET.Application.Interfaces;
using MagniseTaskNET.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MagniseTaskNET.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var tokenResponse = await _authService.GetJwtTokenAsync(loginRequest.Email, loginRequest.Password);
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return Unauthorized("Unable to obtain JWT token.");
            }

            return Ok(tokenResponse);
        }
    }
}
