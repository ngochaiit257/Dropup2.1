using Data.Providers;
using QuynhBoki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuynhBoki.Controllers
{
    public class ProductController : Controller
    {
        ImageMoreProvider image_more_provider = new ImageMoreProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        ElementVariationProvider ev_provider = new ElementVariationProvider();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult BaiGiangDetail(string seo_alias)
        {
            var model = product_provider.getBySEOalias(seo_alias);
            ViewBag.AllInstock = pv_provider.getByProductId(model.product_id).Sum(p => p.product_variation_in_stock);
            return View(model);
        }

        public ActionResult TaiLieuDetail(string seo_alias)
        {
            var model = product_provider.getBySEOalias(seo_alias);
            ViewBag.AllInstock = pv_provider.getByProductId(model.product_id).Sum(p => p.product_variation_in_stock);
            return View(model);
        }

        public ActionResult GetProductVariationElement(long product_id)
        {
            var list_pv = pv_provider.getByProductId(product_id);
            var list_pe = new List<ElementVariationModel>();
            foreach (var pv in list_pv)
            {
                var list_ev = ev_provider.getByProductVariationId(pv.product_variation_id);
                foreach (var ev in list_ev)
                {
                    ElementVariationModel obj = new ElementVariationModel();
                    obj.product_variation_id = pv.product_variation_id;
                    obj.prop_id = (int)ev.product_variation_property_id;
                    obj.ev_name = ev.element_variation_name;
                    obj.pv_image_url = pv.product_variation_image;
                    if (pv.product_variation_price != null)
                    {
                        obj.pv_price = (decimal)pv.product_variation_price;
                    }
                    if (pv.product_variation_price_comparison != null)
                    {
                        obj.pv_price_comparison = (decimal)pv.product_variation_price_comparison;
                    }
                    if (obj.pv_price > 0 && obj.pv_price_comparison > 0)
                    {
                        obj.pv_promotion_percent = (int)Math.Round((double)((obj.pv_price_comparison - obj.pv_price) / obj.pv_price) * 100);
                    }
                    if (pv.product_variation_in_stock != null)
                    {
                        obj.pv_in_stock = (int)pv.product_variation_in_stock;
                    }
                    if (pv.allow_delivery != null)
                    {
                        obj.allow_delivery = (bool)pv.allow_delivery;
                    }
                    if (pv.allow_ordering_while_out_of_stock_status != null)
                    {
                        obj.allow_ordering_while_out_of_stock_status = (bool)pv.allow_ordering_while_out_of_stock_status;
                    }
                    list_pe.Add(obj);
                }
            }

            return Json(new { list_pve = list_pe }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetNumInStock(long product_variation_id)
        {
            bool status = true;
            int num_in_stock = 0;
            if (pv_provider.getNumInStock(product_variation_id, ref num_in_stock) == false)
            {
                status = false;
            }
            return Json(new { status = status, num_in_stock = num_in_stock }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckInStockWhenAddToCart(long product_variation_id, int number_input)
        {
            bool status = true;
            int num_in_stock = 0;
            if (pv_provider.checkInStock(product_variation_id, number_input, ref num_in_stock) == false)
            {
                status = false;
            }
            return Json(new { status = status, num_in_stock = num_in_stock }, JsonRequestBehavior.AllowGet);
        }

    }
}