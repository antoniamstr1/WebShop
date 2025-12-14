using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models;
using Microsoft.AspNetCore.Authorization;
using WebShop.Services;
using System.Security.Claims;
using WebShop.Entities;

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

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);
            if (result?.AccessToken == null || result?.RefreshToken == null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            return Ok(new
            {
                UserId = userIdClaim,
                Email = emailClaim
            });
        }
    }

}