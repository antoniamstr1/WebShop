using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
    }
}


