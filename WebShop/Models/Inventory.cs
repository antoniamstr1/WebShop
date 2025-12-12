using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Inventory
    {
        [Key]
        public int ProductId { get; set; }
        public int InStockNumber { get; set; }
    }
}

