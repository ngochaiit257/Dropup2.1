using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VinaFocus.Controllers
{
    public class SearchController : Controller
    {
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        // GET: Search
        public ActionResult Index(int? page_num, string keyword, string select_order)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            ViewBag.NumberAllResult = product_provider.searchOnClientSite(keyword).Count();
            ViewBag.page_num = page_num;
            ViewBag.SelectOrder = select_order;
            ViewBag.SearchString = keyword;
            return View();
        }

        public PartialViewResult GetProductBySearchString(int? page_num, string keyword, string select_order)
        {
            var list_return = new List<product>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (keyword == null)
            {
                keyword = "";
            }
            ViewBag.page_num = page_num;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.SelectOrder = select_order;
            ViewBag.SearchString = keyword;
            list_return = product_provider.searchOnClientSite(keyword);

            if (select_order == "ten-az")
            {
                list_return = list_return.OrderBy(p => p.product_name).ToList();
            }
            else if (select_order == "ten-za")
            {
                list_return = list_return.OrderByDescending(p => p.product_name).ToList();
            }
            else if (select_order == "gia-cao-thap")
            {
                list_return = list_return.OrderByDescending(p => p.promotion_price).ToList();
            }
            else if (select_order == "gia-thap-cao")
            {
                list_return = list_return.OrderBy(p => p.promotion_price).ToList();
            }
            else if (select_order == "moi-nhat")
            {
                list_return = list_return.OrderByDescending(p => p.create_date).ToList();
            }
            else if (select_order == "cu-nhat")
            {
                list_return = list_return.OrderBy(p => p.create_date).ToList();
            }
            else if (select_order == "pho-bien-nhat")
            {
                list_return = list_return.OrderBy(p => p.show_datetime).ToList();
            }

            ViewBag.NumberOfList = list_return.Count();
            return PartialView("_PartialViewSearchByKeyWord", list_return.ToPagedList(No_Of_Page, 20));
        }
    }
}