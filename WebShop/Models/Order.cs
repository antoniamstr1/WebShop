using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        public Guid? UserId { get; set; }

        public bool Shipped { get; set; } = false;
        public bool Arrived { get; set; } = false;
        public bool Paid { get; set; } = false;

        public Address Address { get; set; }
        public string? email {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
