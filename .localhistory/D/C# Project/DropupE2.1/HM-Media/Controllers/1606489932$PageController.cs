using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM_Media.Controllers
{
    public class PageController : Controller
    {
        PageProvider page_provider = new PageProvider();
        StoreMessageProvider sm_provider = new StoreMessageProvider();


        // GET: Page
        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult PageDetail(string seo_alias)
        {
            return View(page_provider.getBySeoAlias(seo_alias));
        }

        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult PageContact()
        {
            return View();
        }


        public ActionResult SendMessage(string sender_name, string sender_email, string sender_phone_number, string sender_subject, string sender_message)
        {
            store_message sm = new store_message();
            sm.sender_name = sender_name;
            sm.sender_email = sender_email;
            sm.sender_phone_number = sender_phone_number;
            sm.sender_subject = sender_subject;
            sm.sender_message = sender_message;
            sm.reading_status = false;
            sm.create_datetime = DateTime.Now;
            sm_provider.insertStoreMessage(sm);

            return Json("Success");
        }
    }
}