using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropupManagement.Controllers
{
    public class PromotionEventController : Controller
    {
        LocalProvider local_provider = new LocalProvider();
        // GET: PromotionEvent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertPromotionEvent()
        {
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            return View();
        }
    }
}