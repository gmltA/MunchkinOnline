using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
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

        [Authorize]
        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectPermanent("/");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
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

        [AllowAnonymous]
        public void VKLogin()
        {
            OAuthWebSecurity.RequestAuthentication("VK", Url.Action("AuthenticationCallback"));
        }

        [AllowAnonymous]
        public ActionResult AuthenticationCallback()
        {
            var result = OAuthWebSecurity.VerifyAuthentication();
            if (result.IsSuccessful)
            {
                var uniqueUserID = uint.Parse(result.ProviderUserId);
                if (Users.GetUserByVkId(uniqueUserID) == null)
                {
                    User newUser = new User();
                    newUser.LastActivity = DateTime.Now;
                    newUser.Nickname = result.UserName;
                    newUser.VkId = uniqueUserID;
                    newUser.VkAccessToken = result.ExtraData["accessToken"];
                    newUser.Role = Role.Player;
                    if (Users.Add(newUser) == false)
                    {
                        return RedirectPermanent("/rules/");
                    }
                    else
                        return RedirectPermanent("/");
                }
            }
            return RedirectPermanent("/");
        }
    }
}
