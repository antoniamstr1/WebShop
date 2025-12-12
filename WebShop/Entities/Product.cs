using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
        public Category Category { get; set; }
        public ICollection<Image> Images { get; set; }
        public Inventory Inventory { get; set; }
    }
}

