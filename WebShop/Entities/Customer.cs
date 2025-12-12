using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class Customer 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string? PasswordHash { get; set; } = string.Empty;
        public string? Role { get; set; } = "customer";
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public Address? Address { get; set; }
    }
}

