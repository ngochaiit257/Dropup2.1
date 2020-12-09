using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tokaido.Controllers
{
    public class ProductController : Controller
    {
        ImageMoreProvider image_more_provider = new ImageMoreProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        ElementVariationProvider ev_provider = new ElementVariationProvider();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductDetail(string seo_alias)
        {
            var model = product_provider.getBySEOalias(seo_alias);
            ViewBag.ListImageMore = image_more_provider.getByProductId(model.product_id);
            ViewBag.AllInstock = pv_provider.getByProductId(model.product_id).Sum(p => p.product_variation_in_stock);
            return View(model);
        }
    }
}