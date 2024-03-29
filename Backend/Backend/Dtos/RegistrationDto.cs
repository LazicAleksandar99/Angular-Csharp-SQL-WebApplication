﻿using Backend.Models;
using System;

namespace Backend.Dtos
{
    public class RegistrationDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public Userrole Role { get; set; }
        public string Picture { get; set; }
    }
}
