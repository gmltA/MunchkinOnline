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

        public Sexes Sex { get; set; }

        public Roles Role { get; set; }

        public DateTime LastActivity { get; set; }

        public virtual ICollection<User> Friends { get; set; }
     }

    enum Roles
    {
        Admin,
        Player
    }

    enum Sexes
    {
        Male,
        Female
    }
}