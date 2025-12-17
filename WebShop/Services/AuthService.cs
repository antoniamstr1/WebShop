using WebShop.Data;
using WebShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;



namespace WebShop.Services
{

    public interface IAuthService
    {
        Task<Customer?> RegisterAsync(CustomerDto request);
        Task<TokenResponseDto?> LoginAsync(CustomerDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
        TokenResponseDto AnonymousLogin();

    }
    public class AuthService(ApplicationDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(CustomerDto request)
        {
            var user = await context.Customers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<Customer>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return await CreateTokenResponse(user);
        }

        public TokenResponseDto AnonymousLogin()
        {
            var guestId = Guid.NewGuid();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, guestId.ToString()),
                    new Claim(ClaimTypes.Role, "guest"),
                    new Claim("isGuest", "true")
                };

            var accessToken = CreateToken(claims, DateTime.UtcNow.AddHours(6));

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = null
            };
        }


        private async Task<TokenResponseDto> CreateTokenResponse(Customer? user)
        {

            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        // registracija customera -> spremanje u bazu + hash password
        public async Task<Customer?> RegisterAsync(CustomerDto request)
        {

            var user = new Customer();
            var hashedPassword = new PasswordHasher<Customer>()
                .HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = hashedPassword;

            context.Customers.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<Customer?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Customers.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(Customer user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            Console.WriteLine("refresh" + refreshToken);
            return refreshToken;
        }

        private string CreateToken(List<Claim> claims, DateTime expiresAt)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string CreateToken(Customer user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, "customer")
    };

            return CreateToken(claims, DateTime.UtcNow.AddDays(1));
        }

    }
}
