using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Models;
using Ninject;

namespace Munchkin_Online.Controllers
{
    public class AccountsController : Controller
    {
        [Inject]
        public IAuthentication Auth { get; set; }

        public User CurrentUser
        {
            get
            {
                return ((UserIndentity)(Auth.CurrentUser.Identity)).User;
            }
        }

        UserRepository Users = new UserRepository();

        //
        // GET: /Accounts/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            if(CurrentUser == null) return View();
                else
            return RedirectPermanent("/");
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (CurrentUser == null)
            {
                if (ModelState.IsValid)
                {
                    user.LastActivity = DateTime.Now;
                    if (Users.Add(user) == false)
                        return View(user);
                    else
                        return RedirectPermanent("/");
                }
                else
                    return View(user);
            }
            else
                return RedirectPermanent("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel m)
        {
            if (ModelState.IsValid)
            {
                if (Auth.Login(m.Email, m.Password) != null)
                    return RedirectPermanent("/");
                else
                    return View();
            }
            else
                return View();

        }

        
    }
}
