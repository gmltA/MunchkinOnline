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
    public class LobbyController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            Match match = Matchmaking.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (match != null)
                return View("Create", match);
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            Match match = Matchmaking.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (match == null)
            {
                match = new Match();
                match.Id = Guid.NewGuid();
                match.Creator = new Player(CurrentUser.Instance.Current);
                match.Players = new List<Player>();
                match.Players.Add(new Player(CurrentUser.Instance.Current));
                Matchmaking.Instance.Matches.Add(match);
            }
            return View(match);
        }

        [Authorize]
        public ActionResult Join(Guid? lobbyGuid)
        {
            Match match = Matchmaking.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (match != null)
            {
                return RedirectToAction("Index");
            }

            match = Matchmaking.Instance.Matches.Where(m => m.Id == lobbyGuid).FirstOrDefault();
            if (match == null)
            {
                //no lobby with provided guid
                return RedirectToAction("Index", "Home");
            }

            if (match.Players.Count > 3)
            {
                return RedirectToAction("Index");
            }

            match.Players.Add(new Player(CurrentUser.Instance.Current));

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Leave()
        {
            Match match = Matchmaking.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (match != null)
            {
                if (match.Creator.UserId == CurrentUser.Instance.Current.Id)
                    Matchmaking.Instance.Matches.Remove(match);
                else
                    match.Players.RemoveAll(p => p.UserId == CurrentUser.Instance.Current.Id);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
