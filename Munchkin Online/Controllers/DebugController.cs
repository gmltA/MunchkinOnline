using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Munchkin_Online.Core.Longpool;

namespace Munchkin_Online.Controllers
{
    [Authorize]
    public class DebugController : Controller
    {
        //
        // GET: /Debug/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LongPool()
        {
            return View(Longpool.Instance.Clients);
        }

        public void NotifyAll()
        {
            AsyncMessage m = new AsyncMessage(MessageType.Notification);
            m.Data = "Hello!";
            Longpool.Instance.PushMessage(m);
        }

        public void DisconnectAll()
        {
            Longpool.Instance.PushMessage(new AsyncMessage(MessageType.StopPooling));
        }

    }
}
