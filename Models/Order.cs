using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VStoreAPI.Models
{
    public class Order
    {
        [Key] public int Id { get; set; }
        [Required] public string Status { get; set; }
        
        // [ForeignKey("Products")]
        // public int ProductsId { get; set; }
        public ICollection<Product> Products { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
