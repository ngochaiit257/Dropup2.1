using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElysienVietNam.Controllers
{
    public class BlogController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        BlogProvider blog_provider = new BlogProvider();
        StorePartnerProvider sp_provider = new StorePartnerProvider();
        // GET: Blog
        public ActionResult Index(int? page_num, long? blog_category_id)
        {
            ViewBag.page_num = page_num;
            ViewBag.BlogCategoryId = blog_category_id;
            return View();
        }

        public PartialViewResult GetBlog(int? page_num, long? blog_category_id)
        {
            var list_return = new List<blog>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            ViewBag.page_num = page_num;
            ViewBag.BlogCategoryId = blog_category_id;
            int No_Of_Page = (page_num ?? 1);
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

            return PartialView("_PartialViewListBlog", list_return.ToPagedList(No_Of_Page, 12));
        }

        public ActionResult BlogDetail(string seo_alias)
        {
            ViewData["ListMyPham"] = product_provider.getByCategoryIdOnSite(55);
            ViewData["ListMayMoc"] = product_provider.getByCategoryIdOnSite(56);
            ViewData["ListDacBiet"] = product_provider.getByCategoryIdOnSite(57);
            return View(blog_provider.getBySeoAlias(seo_alias));
        }
    }
}