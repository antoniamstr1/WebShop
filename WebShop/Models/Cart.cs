using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public Customer? Customer { get; set; }

        public decimal Price { get; set;}
        public bool PaymentConfirmed { get; set; }
        public bool Terminated { get; set; }

    }
}

