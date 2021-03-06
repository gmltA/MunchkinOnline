﻿using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Matchmaking;
using Munchkin_Online.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Munchkin_Online.Core.Longpool;

namespace Munchkin_Online.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            Match match = MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (match == null || match.State != State.InGame)
            {
                NotificationManager.Instance.Add("You can't access game field while not in game", NotificationType.Error);
                return RedirectToAction("Index", "Home");
            }
            Longpool.Instance.PushMessageToUserDelayed(CurrentUser.Instance.Current.Id, LongpoolBuilder.GetInitMessage(match));
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

        [HttpPost]
        public string ProcessAction(ActionInfo actionInfo)
        {
            Match m = MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            actionInfo.Invoker = m.Players.Where(p => p.UserId == CurrentUser.Instance.Current.Id).FirstOrDefault();

            if (m != null)
            {
                return new GameDirector(m).ProcessAction(actionInfo);
            }
            return "Match has not been found";
        }

        public ActionResult Leave()
        {
            Match m = MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (m != null)
            {
                MatchManager.Instance.UserLeaveFromMatch(CurrentUser.Instance.Current.Id);
                return RedirectToAction("Index", "Home");
            }

            return new EmptyResult();
        }
    }
}
