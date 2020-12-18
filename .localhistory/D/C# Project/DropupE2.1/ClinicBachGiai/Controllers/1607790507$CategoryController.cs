using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicBachGiai.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index(int? page_num, string seo_alias)
        {
            var category_blog = blog_category_provider.getBySeoAlias(seo_alias);
            ViewBag.page_num = page_num;
            ViewBag.CategoryBlogSeoAlias = seo_alias;
            ViewBag.CategoryBlog = category_blog;
            return View();
        }

        public PartialViewResult GetBlog(int? page_num, string seo_alias)
        {
            var list_return = new List<blog>();
            var category_blog = blog_category_provider.getBySeoAlias(seo_alias);
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            ViewBag.page_num = page_num;
            ViewBag.CategoryBlogSeoAlias = seo_alias;
            ViewBag.CategoryBlog = category_blog;
            int No_Of_Page = (page_num ?? 1);
            if (category_blog != null)
            {
                list_return = blog_provider.getByCategoryIdShowing(category_blog.blog_category_id);
            }
            else
            {
                list_return = blog_provider.getListShowOnClientSite();
            }

            ViewBag.NumberOfList = list_return.Count();
            return PartialView("_PartialViewListBlog", list_return.ToPagedList(No_Of_Page, 12));
        }
    }
}