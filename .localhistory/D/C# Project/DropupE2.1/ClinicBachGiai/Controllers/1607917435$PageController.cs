using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicBachGiai.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult CoSoVatChat()
        {
            return View();
        }

        public ActionResult DoiNguBacSi()
        {
            return View();
        }

    }
}