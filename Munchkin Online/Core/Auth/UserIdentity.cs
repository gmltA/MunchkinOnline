using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Auth
{
    public class UserIndentity : IIdentity
    {
        public User User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }
                //иначе аноним
                return "anonym";
            }
        }

        public void Init(string email, UserRepository repository)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User = repository.GetUser(email);
                User.LastActivity = DateTime.Now;
                repository.ForceSaveChanges();
            }
        }

        public User CurrentUser
        {
            get
            {
                return User;
            }
        }
    }
}