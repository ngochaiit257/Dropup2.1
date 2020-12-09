using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tokaido.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult PageContact()
        {
            return View();
        }

        public ActionResult PageDetail(string seo_alias)
        {
            return View(page_provider.getBySeoAlias(seo_alias));
        }
    }
}