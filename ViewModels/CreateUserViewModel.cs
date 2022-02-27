using System.ComponentModel.DataAnnotations;


namespace VStoreAPI.ViewModels
{
    public class CreateUserViewModel
    {
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Cpf { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Birth { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public char Gender { get; set; }
        public string Role { get; set; }
        
    }
}