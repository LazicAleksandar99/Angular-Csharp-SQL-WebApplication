using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public enum Userrole {Admin = 0,Deliverer = 1,NormalUser=2 }
    public abstract class Person
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Adress { get; set; }
        public Userrole Role { get; set; }
        public string Picture { get; set; }

    }
}
