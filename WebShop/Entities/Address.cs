using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Zipcode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? AdditionalInfo { get; set; }

    }
}
