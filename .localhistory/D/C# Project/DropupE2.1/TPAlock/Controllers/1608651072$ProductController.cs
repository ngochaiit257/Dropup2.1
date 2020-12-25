using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TPAlock.Controllers
{
    public class ProductController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Product
        public ActionResult ProductDetail(string seo_alias)
        {
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}