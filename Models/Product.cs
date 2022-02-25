using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VStoreAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName{ get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        
        // [ForeignKey("Order")]
        // public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
