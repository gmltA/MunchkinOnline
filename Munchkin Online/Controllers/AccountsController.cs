using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Models;

namespace Munchkin_Online.Controllers
{
    public class AccountsController : Controller
    {

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
            if(CurrentUser.Get == null) return View();
                else
            return RedirectPermanent("/");
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (CurrentUser.Get == null)
            {
                if (ModelState.IsValid)
                {
                    user.LastActivity = DateTime.Now;
                    if (Users.Add(user) == null)
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

        
    }
}
