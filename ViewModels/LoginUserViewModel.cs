using System.ComponentModel.DataAnnotations;

namespace VStoreAPI.ViewModels
{
    public class LoginUserViewModel
    {
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }
    }
}