using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;


namespace BunnyLogistics.Controllers
{
    public class PageController : Controller
    {
        PageProvider page_provider = new PageProvider();
        // GET: Page
        public ActionResult PageDetail(string seo_alias)
        {
            return View(page_provider.getBySeoAlias(seo_alias));
        }
    }
}