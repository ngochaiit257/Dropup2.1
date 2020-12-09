using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BunnyLogistics.Controllers
{
    public class BlogController : Controller
    {
        BlogProvider blog_provider = new BlogProvider();
        StorePartnerProvider sp_provider = new StorePartnerProvider();
        // GET: Blog
        public ActionResult Index(int? page_num)
        {
            ViewBag.page_num = page_num;
            return View();
        }

        public PartialViewResult GetBlog(int? page_num)
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

        public ActionResult BlogDetail(string seo_alias)
        {
            ViewData["ListPartner"] = sp_provider.getAll();
            return View(blog_provider.getBySeoAlias(seo_alias));
        }
    }
}