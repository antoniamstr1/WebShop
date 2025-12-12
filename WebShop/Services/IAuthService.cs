
using WebShop.Models;

namespace WebShop.Services
{
    public interface IAuthService
    {
        Task<Customer?> RegisterAsync(CustomerDto request);
        Task<TokenResponseDto?> LoginAsync(CustomerDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
