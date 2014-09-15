using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Auth
{
    public class CurrentUser
    {
        public static User Get
        {
            get
            {
                return ((new CustomAuthentication()).CurrentUser.Identity as UserIndentity).CurrentUser;
            }
        }
    }
}