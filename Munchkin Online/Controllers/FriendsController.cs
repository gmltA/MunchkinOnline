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
        UserRepository Repository = new UserRepository();

        [HttpPost]
        public ActionResult Search()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FriendPlateList(string name)
        {    
            return PartialView(Repository.GetPotentialFriendListByNickname(name, CurrentUser.Instance.Current));
        }

        public string ManageFriend(Guid? id)
        {
            User curUser = Repository.GetUser(CurrentUser.Instance.Current.Id);
            if (Request.HttpMethod == "PUT")
                curUser.Friends.Add(Repository.GetUser(id.Value));
            else if (Request.HttpMethod == "DELETE")
                curUser.Friends.Remove(Repository.GetUser(id.Value));
            else
            {
                Response.StatusCode = 404;
                return "";
            }
            Repository.ForceSaveChanges();
            return "success";
        }
    }
}
