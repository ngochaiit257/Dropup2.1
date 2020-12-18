using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicBachGiai.Controllers
{
    public class BlogController : Controller
    {
        BlogProvider blog_provider = new BlogProvider();
        BlogCategoryProvider blog_category_provider = new BlogCategoryProvider();
        StorePartnerProvider sp_provider = new StorePartnerProvider();
        // GET: Blog
        public ActionResult Index(int? page_num, string seo_alias)
        {
            ViewBag.page_num = page_num;
            ViewBag.CategoryBlogSeoAlias = seo_alias;
            return View();
        }

        public PartialViewResult GetBlog(int? page_num, string seo_alias)
        {
            var list_return = new List<blog>();
            var category_blog = getBySeoAlias()
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            ViewBag.page_num = page_num;
            ViewBag.CategoryBlogSeoAlias = seo_alias;
            int No_Of_Page = (page_num ?? 1);
            if()
            list_return = blog_provider.getListShowOnClientSite();
            ViewBag.NumberOfList = list_return.Count();
            return PartialView("_PartialViewListBlog", list_return.ToPagedList(No_Of_Page, 12));
        }

        public ActionResult BlogDetail(string seo_alias)
        {
            return View(blog_provider.getBySeoAlias(seo_alias));
        }
    }
}