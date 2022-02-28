using System.ComponentModel.DataAnnotations;

namespace VStoreAPI.ViewModels
{
    public class AddProductToOrderViewModel
    {
        [Required] public int ProductId { get; set; }
        [Required] public int OrderId { get; set; }
    }
}
