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


        public User Login(string email, string pass)
        {
            return DB.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0 && string.Compare(p.Password, pass, false) == 0);
        }


        public User GetUser(string email)
        {
            return DB.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0);
        }
    }
}