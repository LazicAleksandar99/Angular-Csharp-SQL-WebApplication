using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public enum Userrole { Admin = 0, Deliverer = 1, NormalUser = 2 }

    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public Userrole Role { get; set; }
        public string Picture { get; set; }
        public bool Registrated { get; set; }
        public string Verification { get; set; }// pending, denied, verified
        public List<Order> Orders { get; set; }

    }
}
