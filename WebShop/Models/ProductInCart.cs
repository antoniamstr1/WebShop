using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class ProductInCart
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}

