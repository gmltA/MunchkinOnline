using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Auth
{
    public interface IAuthentication
    {
        /// <summary>
        /// Конекст (тут мы получаем доступ к запросу и кукисам)
        /// </summary>
        HttpContext HttpContext { get; set; }

        User Login(string login, string password);

        User Login(string login);

        void LogOut();

        IPrincipal CurrentUser { get; }
    }
}