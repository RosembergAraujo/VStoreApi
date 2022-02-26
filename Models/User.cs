using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VStoreAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Password { get; set; }
        // [Required] public byte[] Password { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Cpf { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Birth { get; set; }
        [Required] public string Phone { get; set; }
        public char Gender { get; set; }
        [Required] public string Role { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        
        public ICollection<Order> Orders { get; set; }

    }
}
