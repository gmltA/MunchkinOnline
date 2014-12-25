using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Longpool;
using Munchkin_Online.Core.Matchmaking;
using Munchkin_Online.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
            MatchManager.Instance.UserLeaveFromLobby(CurrentUser.Instance.Current.Id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ContentResult InvitePlayer(Guid? playerGuid)
        {
            if (!playerGuid.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong user Guid", MediaTypeNames.Text.Plain);
            }

            MatchInvite invite = MatchManager.Instance.InviteUserToLobby(CurrentUser.Instance.Current, playerGuid.Value);
            if (invite == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Can't invite player to lobby", MediaTypeNames.Text.Plain);
            }
            else
            {
                Longpool.Instance.PushMessageToUser(playerGuid.Value, new AsyncMessage(invite));
                return Content("success");
            }
        }

        [HttpPost]
        public ContentResult KickPlayer(Guid? playerGuid)
        {
            if (!playerGuid.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong user Guid", MediaTypeNames.Text.Plain);
            }

            if (!MatchManager.Instance.KickUserFromLobby(CurrentUser.Instance.Current, playerGuid.Value))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong user Guid", MediaTypeNames.Text.Plain);
            }

            return Content("success");
        }

        [HttpPost]
        public ActionResult RenderInvite(Guid? inviteGuid)
        {
            if (!inviteGuid.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new EmptyResult();
            }

            MatchInvite invite = MatchManager.Instance.MatchInvites.Where(i => i.Id == inviteGuid.Value).FirstOrDefault();
            return PartialView(invite);
        }

        public ActionResult SlotContextMenu(int? emptySlot)
        {
            ViewData["EmptySlot"] = false;
            if (emptySlot.HasValue)
            {
                if (emptySlot.Value == 1)
                    ViewData["EmptySlot"] = true;
            }
            else
                ViewData["EmptySlot"] = true;

            Match lobby = MatchManager.Instance.FindMatchByParticipantID(CurrentUser.Instance.Current.Id);
            if (lobby.Creator.UserId != CurrentUser.Instance.Current.Id)
            {
                ViewData["ForCreator"] = false;
                return PartialView(new List<Munchkin_Online.Models.User>());
            }
            else
            {
                ViewData["ForCreator"] = true;
                var excludeFromInviteList = MatchManager.Instance.GetExcludeFromInviteUserIdList(CurrentUser.Instance.Current.Id);
                return PartialView(CurrentUser.Instance.Current.Friends.Where(f => f.State != Munchkin_Online.Models.State.Offline && !excludeFromInviteList.Contains(f.Id)).ToList());
            }
        }

        [HttpPost]
        public ContentResult AcceptInvite(Guid? inviteGuid)
        {
            if (!inviteGuid.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong invite Guid", MediaTypeNames.Text.Plain);
            }

            MatchInvite invite = MatchManager.Instance.MatchInvites.Where(i => i.Id == inviteGuid.Value).FirstOrDefault();
            if (invite == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong invite Guid", MediaTypeNames.Text.Plain);
            }

            try
            {
                MatchManager.Instance.UserJoinLobby(CurrentUser.Instance.Current, invite.MatchId);
            }
            catch (LobbyJoinException)
            {
                //todo: handle exceptions properly
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Can't join lobby", MediaTypeNames.Text.Plain);
            }
            finally
            {
                MatchManager.Instance.MatchInvites.Remove(invite);
            }
            return Content("success");
        }

        [HttpPost]
        public ContentResult DenyInvite(Guid? inviteGuid)
        {
            if (!inviteGuid.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong invite Guid", MediaTypeNames.Text.Plain);
            }

            MatchInvite invite = MatchManager.Instance.MatchInvites.Where(i => i.Id == inviteGuid.Value).FirstOrDefault();
            if (invite == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Wrong invite Guid", MediaTypeNames.Text.Plain);
            }

            MatchManager.Instance.MatchInvites.Remove(invite);

            return Content("success");
        }

        public ActionResult Start()
        {
            try
            {
                try
                {
                    if (MatchManager.Instance.UserStartsMatch(CurrentUser.Instance.Current.Id))
                        return RedirectToAction("Index", "Game");
                    else
                    {
                        if (Request.UrlReferrer != null)
                            return Redirect(Request.UrlReferrer.AbsolutePath);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch (NotEnoughPlayersException)
                {
                    NotificationManager.Instance.Add("Can't start match: not enough players!", NotificationType.Error);
                    throw;
                }
                catch (WrongMatchStateException)
                {
                    NotificationManager.Instance.Add("Can't start match: wrong match state (already started, etc)!", NotificationType.Error);
                    throw;
                }
            }
            catch (MatchStartException)
            {
                if (Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.AbsolutePath);
                else
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}
