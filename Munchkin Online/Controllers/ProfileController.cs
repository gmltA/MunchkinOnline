using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Munchkin_Online.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index(Guid? profileId)
        {
            if (profileId.HasValue)
            {
                UserRepository repo = new UserRepository();
                User user = repo.GetUser(profileId.Value);

                if (user != null)
                    return View(user);
            }

            //todo: add proper error message (Profile not found)
            Response.StatusCode = 404;
            return new EmptyResult();
        }

        public ActionResult TopPlayers()
        {
            UserRepository repo = new UserRepository();
            return View(repo.GetTopPlayers());
        }
    }

    
}
