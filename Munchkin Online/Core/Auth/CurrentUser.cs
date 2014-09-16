using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;
using Ninject;

namespace Munchkin_Online.Core.Auth
{
    public class CurrentUser
    {

        public static readonly CurrentUser Instance = new CurrentUser();

        public User Current { get; set; }

    }
}