using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM_Media.Controllers
{
    public class CategoryController : Controller
    {
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        StoreMessageProvider sm_provider = new StoreMessageProvider();
        // GET: Category
        public ActionResult Index(int? page_num, long? category_id, string select_order)
        {
            ViewBag.NumberAllResult = product_provider.getByCategoryIdOnSite((long)category_id).Count();
            ViewBag.CategoryId = category_id;
            ViewBag.page_num = page_num;
            ViewBag.SelectOrder = select_order;
            return View();
        }

        public PartialViewResult GetProductByCategory(int? page_num, long? category_id, string select_order)
        {
            var list_return = new List<product>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if()
            ViewBag.CategoryId = category_id;
            ViewBag.page_num = page_num;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.SelectOrder = select_order;
            list_return = product_provider.getByCategoryIdOnSite((long)category_id);

            if (blog_category_id > 0)
            {
                list_return = blog_provider.getListShowOnClientSite((long)blog_category_id);
                ViewBag.NumberOfList = list_return.Count();
            }
            else
            {
                list_return = blog_provider.getListShowOnClientSite();
                ViewBag.NumberOfList = list_return.Count();
            }

            ViewBag.NumberOfList = list_return.Count();
            return PartialView("_PartialViewListProductInCategory", list_return.ToPagedList(No_Of_Page, 12));
        }

        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult ServiceDetail(string seo_alias)
        {
            return View(category_provider.getBySeoAlias(seo_alias));
        }
    }
}