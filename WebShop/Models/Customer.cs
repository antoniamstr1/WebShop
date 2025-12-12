using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class Customer : IdentityUser
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}

