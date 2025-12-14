using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}


