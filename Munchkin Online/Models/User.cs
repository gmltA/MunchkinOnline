﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }

        public int Games { get; set; }

        public int Wins { get; set; }

        public Gender Gender { get; set; }

        public Role Role { get; set; }

        public DateTime LastActivity { get; set; }

        public virtual ICollection<User> Friends { get; set; }
     }

    public enum Role
    {
        Admin,
        Player
    }

    public enum Gender
    {
        Male,
        Female
    }
}