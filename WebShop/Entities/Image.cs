using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
    }
}


