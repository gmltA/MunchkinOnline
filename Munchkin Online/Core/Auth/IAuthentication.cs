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

        /// <summary>
        /// Auth method for VK
        /// </summary>
        /// <param name="id">VKontakte unique user ID</param>
        /// <param name="email">User email adress from VK</param>
        /// <returns></returns>
        User Login(int id, string email);

        User Login(string login);

        void LogOut();

        IPrincipal CurrentUser { get; }
    }
}