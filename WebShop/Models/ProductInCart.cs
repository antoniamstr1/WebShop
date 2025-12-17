using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class ProductInCart
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }

        public Product Product { get; set; }
        public int Amount { get; set; }    }
}

