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
    [Authorize]
    public class FriendsController : Controller
    {
        public ActionResult List()
        {
            return PartialView(CurrentUser.Instance.Current.Friends);
        }

        [HttpPost]
        public ActionResult Search()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FriendPlateList(string name)
        {
            using (UserRepository repo = new UserRepository())
            {
                return PartialView(repo.GetPotentialFriendListByNickname(name, CurrentUser.Instance.Current));
            }
        }

        public string ManageFriend(Guid? id)
        {
            using (UserRepository repo = new UserRepository())
            {
                User curUser = repo.GetUser(CurrentUser.Instance.Current.Id);
                if (Request.HttpMethod == "PUT")
                    curUser.Friends.Add(repo.GetUser(id.Value));
                else if (Request.HttpMethod == "DELETE")
                    curUser.Friends.Remove(repo.GetUser(id.Value));
                else
                {
                    Response.StatusCode = 404;
                    return "";
                }
                repo.ForceSaveChanges();
                return "success";
            }
        }
    }
}
