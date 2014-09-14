using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Database
{
    public class UserRepository
    {
        MainContext DB = new MainContext();

        public IQueryable<User> Accounts
        {
            get
            {
                return DB.Users;
            }
        }

        public bool Add(User instance)
        {
            try
            {
                DB.Users.Add(instance);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}