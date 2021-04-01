using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace ElysienVietNam.Controllers
{
    public class HomeController : Controller
    {
        ProductProvider product_provider = new ProductProvider();

        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult Index()
        {
            //ViewData["ListMyPham"] = product_provider.getByCategoryIdOnSite(55);
            //ViewData["ListMayMoc"] = product_provider.getByCategoryIdOnSite(56);
            //ViewData["ListDacBiet"] = product_provider.getByCategoryIdOnSite(57);
            return View();
        }

    }
}