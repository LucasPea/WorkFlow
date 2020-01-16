using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NextCowry.Controllers
{
    public class CowryHomeController : Controller
    {
        // GET: CowryHome
        public ActionResult CowryHome()
        {
            return View();
        }
    }
}