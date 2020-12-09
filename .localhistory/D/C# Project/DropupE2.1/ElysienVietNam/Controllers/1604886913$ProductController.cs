using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace ElysienVietNam.Controllers
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

        public ActionResult ProductDetail(string seo_alias)
        {
            var model = product_provider.getBySEOalias(seo_alias);
            ViewBag.ListImageMore = image_more_provider.getByProductId(model.product_id);
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

        [HttpPost]
        public ActionResult LoadMoreProduct(int num_product, int index_arr)
        {
            var list_get_all_newest = product_provider.getNewestShowing().Take(30).ToList();
            var list_product_data = new List<product>();
            var list_product_model = new List<ProductLoadMoreModel>();
            int i = 0;
            int i_start_get = index_arr + 1;

            for (i = 0; i <= list_get_all_newest.Count; i++)
            {
                if (i > index_arr)
                {
                    if (i <= i_start_get + (num_product - 1))
                    {
                        list_product_data.Add(list_get_all_newest[i]);
                    }
                }
            }
            if (list_product_data.Count > 0)
            {
                foreach (var product in list_product_data)
                {
                    ProductLoadMoreModel model = new ProductLoadMoreModel();
                    model.product_name = product.product_name;
                    model.seo_alias = product.seo_alias;
                    model.product_img = product.product_image;
                    model.product_img_alt = product.alt_image;
                    var img_more = new ImageMoreProvider().get1ByProductId(product.product_id);
                    if (img_more != null)
                    {
                        model.product_img_hover = img_more.image_more_url;
                        model.product_img_hover_alt = img_more.image_more_alt;
                    }
                    if (product.price != null)
                    {
                        model.price = (decimal)product.price;
                    }
                    if (product.promotion_price != null)
                    {
                        model.promotion_price = (decimal)product.promotion_price;
                    }
                    list_product_model.Add(model);
                }
            }
            return Json(new { index_ss = list_get_all_newest.Count, list_product_model = list_product_model }, JsonRequestBehavior.AllowGet);
        }
    }
}