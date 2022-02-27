using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VStoreAPI.Models
{
    public class Product
    {
        [Key] public int Id { get; set; }
        [Required] public string ProductNa { get; set; }
        [Required] public float Price { get; set; }
        public float Discount { get; set; } = 0;
        public string Category { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Manufacturer { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
