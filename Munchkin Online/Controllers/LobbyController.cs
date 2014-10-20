using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Matchmaking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Munchkin_Online.Controllers
{
    [Authorize]
    public class LobbyController : Controller
    {
        public ActionResult Index()
        {
            return View(MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id));
        }

        public ActionResult Create()
        {
            return View(MatchManager.Instance.GetOrCreateNewMatchForUser(CurrentUser.Instance.Current));
        }

        public ActionResult Join(Guid? lobbyGuid)
        {
            try
            {
                MatchManager.Instance.UserJoinLobby(CurrentUser.Instance.Current, lobbyGuid);
            }
            catch (LobbyJoinException)
            {
                //todo: handle exceptions properly
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Leave()
        {
            MatchManager.Instance.UserLeaveFromMatch(CurrentUser.Instance.Current.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}
