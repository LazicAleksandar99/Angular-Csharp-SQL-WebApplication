using System;

namespace Backend.Dtos
{
    public class DeliverDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Verification { get; set; }// pending, denied, verified
    }
}
