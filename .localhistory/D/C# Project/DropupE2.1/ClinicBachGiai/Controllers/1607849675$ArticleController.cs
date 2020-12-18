using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace ClinicBachGiai.Controllers
{
    public class ArticleController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Article
        public ActionResult ArticaleDetail(string seo_alias)
        {
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}