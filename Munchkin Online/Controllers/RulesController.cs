using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Munchkin_Online.Controllers
{
    public class RulesController : Controller
    {
        //
        // GET: /Rules/

        public ActionResult Index()
        {
            ViewBag.pageID = Core.PageID.PAGE_RULES;
            return View();
        }

    }
}
