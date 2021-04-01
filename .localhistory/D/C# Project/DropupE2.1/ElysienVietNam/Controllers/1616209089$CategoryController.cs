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
    public class CategoryController : Controller
    {
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        StoreMessageProvider sm_provider = new StoreMessageProvider();
        // GET: Category
        public ActionResult Index(int? page_num, string seo_alias, string select_order, string search_string, string tab_filter, string choose_grid_view,
                                    string color, decimal? min_price, decimal? max_price)
        {
            ViewData["ListDacBiet"] = product_provider.getByCategoryIdOnSite(57);
            ViewBag.NumberAllResult = product_provider.getByCategorySeoAliasOnSite(seo_alias).Count();
            var category = category_provider.getBySeoAlias(seo_alias);

            var category_parent = category_provider.getCategoryParentByCategoryId(category.category_id);
            if (category_parent != null)
            {
                var category_grand = category_provider.getCategoryParentByCategoryId(category_parent.category_id);
                if (category_grand != null)
                {
                    ViewBag.CategoryParent = category_grand;
                }
                else
                {
                    ViewBag.CategoryParent = category_parent;
                }
            }
            else
            {
                ViewBag.CategoryParent = category;
            }

            ViewBag.Category = category;
            ViewBag.CategoryID = category.category_id;
            ViewBag.CategorySeoAlias = seo_alias;
            ViewBag.page_num = page_num;
            ViewBag.SelectOrder = select_order;
            ViewBag.FilterValue = search_string;
            ViewBag.TabFilter = tab_filter;
            ViewBag.ChooseGridView = choose_grid_view;
            ViewBag.Color = color;
            ViewBag.MinPrice = min_price;
            ViewBag.MaxPrice = max_price;
            return View();
        }

        public PartialViewResult GetProductByCategory(int? page_num, string seo_alias, string select_order, string search_string, string tab_filter, string choose_grid_view,
                                    string color, decimal? min_price, decimal? max_price)
        {
            var list_return = new List<product>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (search_string == null)
            {
                search_string = "";
            }
            if (color == null)
            {
                color = "";
            }
            if (min_price == null)
            {
                min_price = 0;
            }
            if (max_price == null)
            {
                max_price = 1000000000;
            }

            var category = category_provider.getBySeoAlias(seo_alias);
            ViewBag.Category = category;
            ViewBag.CategoryID = category.category_id;
            ViewBag.CategorySeoAlias = seo_alias;
            ViewBag.page_num = page_num;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.SelectOrder = select_order;
            ViewBag.FilterValue = search_string;
            ViewBag.TabFilter = tab_filter;
            ViewBag.ChooseGridView = choose_grid_view;
            ViewBag.Color = color;
            ViewBag.MinPrice = min_price;
            ViewBag.MaxPrice = max_price;
            list_return = product_provider.filterOnClientSite(seo_alias, search_string, tab_filter, color, (decimal)min_price, (decimal)max_price);
            //list_return = product_provider.filterOnClientSite(seo_alias);

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
            return PartialView("_PartialViewListProductInCategory", list_return.ToPagedList(No_Of_Page, 12));
        }
    }
}