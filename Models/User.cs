using System;

using System.Collections.Generic;

namespace VStoreAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string UserName { get; set; }
        public string Birth { get; set; }
        public string Phone { get; set; }
        public char Gender { get; set; }
        public string Role { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; }

    }
}
