using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;;

namespace ClinicBachGiai.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult ArticaleDetail(string seo_alias)
        {
            return View();
        }
    }
}