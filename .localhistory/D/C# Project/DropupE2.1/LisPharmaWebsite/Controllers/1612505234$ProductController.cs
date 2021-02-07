using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LisPharmaWebsite.Controllers
{
    public class ProductController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        ImageMoreProvider image_more_provider = new ImageMoreProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        ElementVariationProvider ev_provider = new ElementVariationProvider();
        // GET: Product
        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //"SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //"SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //"SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult ProductDetail(string seo_alias)
        {
            ViewBag.ListImageMore = image_more_provider.getByProductId(model.product_id);
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}