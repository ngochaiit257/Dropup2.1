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
        ImageMoreProvider image_more_provider = new ImageMoreProvider();
        // GET: Product
        public ActionResult ProductDetail(string seo_alias)
        {
            ViewBag.ListImageMore = image_more_provider.getByProductId(model.product_id);
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}