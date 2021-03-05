using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPlus.Controllers
{
    public class BlogController : Controller
    {
        BlogProvider blog_provider = new BlogProvider();
        StorePartnerProvider sp_provider = new StorePartnerProvider();
        // GET: Blog
        public ActionResult Index(int? page_num, string seo_alias)
        {
            ViewBag.page_num = page_num;
            return View();
        }

        public PartialViewResult GetBlog(int? page_num, string seo_alias)
        {
            var list_return = new List<blog>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            ViewBag.page_num = page_num;
            int No_Of_Page = (page_num ?? 1);
            list_return = blog_provider.getListShowOnClientSite();
            ViewBag.NumberOfList = list_return.Count();
            return PartialView("_PartialViewListBlog", list_return.ToPagedList(No_Of_Page, 12));
        }

        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
                "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
                "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
                "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult BlogDetail(string seo_alias)
        {
            return View(blog_provider.getBySeoAlias(seo_alias));
        }
    }
}