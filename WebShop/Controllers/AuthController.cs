using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Services;
using System.Security.Claims;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(CustomerDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null) return BadRequest("Username already exists.");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(CustomerDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null) return BadRequest("Invalid username or password.");
            return Ok(result);
        }

        [HttpPost("anonymouslogin")]
        public async Task<ActionResult> AnonymousLogin()
        {
            var result = _authService.AnonymousLogin();
            Response.Cookies.Append("jwt_token", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
            //return Ok(result);
            return Ok(new { message = "ok" });
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);
            if (result?.AccessToken == null || result?.RefreshToken == null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            return Ok(new { UserId = userId, Role = User.FindFirstValue(ClaimTypes.Role) });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }
    }

}