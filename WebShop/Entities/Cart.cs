using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public required Customer Customer { get; set; }
        public int Price { get; set;}
        public bool PaymentConfirmed { get; set; }
        public bool Terminated { get; set; }
    }
}

