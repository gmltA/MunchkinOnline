using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Matchmaking;
using Munchkin_Online.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Munchkin_Online.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find()
        {
            if (MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id) != null)
            {
                NotificationManager.Instance.Add("You can't join matchmaking queue while in lobby", NotificationType.Error);
                return RedirectToAction("Index", "Lobby");
            }
            return View();
        }

    }
}
