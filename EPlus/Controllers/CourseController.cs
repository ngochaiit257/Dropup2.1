using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace EPlus.Controllers
{
    public class CourseController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Course
        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult CourseDetail(string seo_alias)
        {
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}