using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities
{
    public class Inventory
    {
        [Key]
        public int ProductId { get; set; }
        public int InStockNumber { get; set; }
    }
}

