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
            return View();
        }

    }
}
