using System.ComponentModel.DataAnnotations;

namespace VStoreAPI.ViewModels
{
    public class CreateProductViewModel
    {
        [Key] public int Id { get; set; }
        [Required] public string ProductName { get; set; }
        [Required] public float Price { get; set; }
        public float Discount { get; set; } = 0;
        public string Category { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Manufacturer { get; set; }
    }
}
