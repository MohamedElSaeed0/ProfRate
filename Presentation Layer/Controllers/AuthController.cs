using Microsoft.AspNetCore.Mvc;
using ProfRate.DTOs;
using ProfRate.Services;

namespace ProfRate.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        // تسجيل الدخول
        [HttpPost]
        [Route("login")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }
    }
}
