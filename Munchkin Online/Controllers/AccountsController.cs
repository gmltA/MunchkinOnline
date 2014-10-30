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
using Munchkin_Online.Core.Notifications;

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
            if (CurrentUser == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel reg)
        {
            if (CurrentUser == null)
            {
                if (ModelState.IsValid)
                {
                    var user = new User();
                    user.Email = reg.Email;
                    user.Nickname = reg.Nickname;
                    user.PasswordHash = reg.Password;
                    user.LastActivity = DateTime.Now;
                    if (Users.Add(user) == false)
                        return View(reg);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    return View(reg);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
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
                int uniqueUserID = int.Parse(result.ProviderUserId);
                if (Users.GetUserByVkId(uniqueUserID) == null)
                {
                    User newUser = new User();
                    newUser.PasswordHash = null;
                    newUser.Email = result.ExtraData["email"];
                    newUser.LastActivity = DateTime.Now;
                    newUser.Nickname = result.UserName;
                    newUser.VkId = uniqueUserID;
                    newUser.VkAccessToken = result.ExtraData["accessToken"];
                    if (Users.Add(newUser) == false)
                    {
                        NotificationManager.Instance.Add("Can't add user data to DB, perhaps, we already have user with this e-mail", NotificationType.Error);
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (Auth.Login(uniqueUserID, result.ExtraData["email"]) == null)
                {
                    NotificationManager.Instance.Add("Can't authenticate user with provided data", NotificationType.Error);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            NotificationManager.Instance.Add("Can't complete VK authentication", NotificationType.Error);
            return RedirectToAction("Index", "Home");
        }
    }
}
