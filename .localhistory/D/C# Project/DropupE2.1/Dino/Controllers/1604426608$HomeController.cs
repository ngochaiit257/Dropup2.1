using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dino.Controllers
{
    public class HomeController : Controller
    {
        //[outputcache(duration = 2592000, varybyparam = "none", sqldependency = "sitecache:product;sitecache:category;sitecache:category_product;" +
        //   "sitecache:cart_of_user;sitecache:cart_product_variation;sitecache:blog;sitecache:system_log;" +
        //   "sitecache:store_logo;sitecache:store_banner;sitecache:store_slide;sitecache:store_membership;sitecache:store_customer_say;" +
        //   "sitecache:store_partner;sitecache:store_contact")]
        public ActionResult Index()
        {
            return View();
        }
    }
}