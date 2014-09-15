using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Database
{
    public class MainContext : DbContext
    {
        public MainContext()
            : base("DefaultConnection")
        { }

        public DbSet<User> Users { get; set; }
    }
}