using Data;
using Data.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dino.Models;

namespace Dino.Controllers
{
    public class CartController : Controller
    {
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();
        CartProvider cart_provider = new CartProvider();
        // GET: Cart
        public ActionResult Index()
        {
            var list_return = new List<ProductVariationInCartModel>();
            if (Request.Cookies["VOSELLA_CART"] != null)
            {
                //var request_cookie = JsonConvert.SerializeObject(Request.Cookies["VOSELLA_CART"].Value);
                //ViewData["CartInCookie"] = request_cookie;

                string objCartListString = HttpUtility.UrlDecode(Request.Cookies["VOSELLA_CART"].Value.ToString());
                list_return = JsonConvert.DeserializeObject<List<ProductVariationInCartModel>>(objCartListString);
            }

            return View(list_return);
        }

        [HttpPost]
        public ActionResult GetAllInCart(List<ProductVariationInCartModel> list_in_cart)
        {
            var list_return = new List<ProductVariationModel>();
            int num_in_cart = 0;
            decimal cost_all_product = 0;
            decimal cost_all_product_vat = 0;
            if (list_in_cart != null)
            {
                foreach (var obj in list_in_cart)
                {
                    num_in_cart += obj.quantity;
                    var pv = product_variation_provider.getById(obj.id);
                    if (pv != null)
                    {
                        ProductVariationModel model = new ProductVariationModel();


                        model.quantity_in_cart = obj.quantity;
                        model.product_variation_price = (decimal)pv.product_variation_price;
                        cost_all_product += model.product_variation_price * model.quantity_in_cart;
                        if (pv.product.vat_status == true)
                        {
                            cost_all_product_vat += model.product_variation_price * model.quantity_in_cart;
                        }
                        else
                        {
                            cost_all_product_vat += (model.product_variation_price + (decimal)((double)model.product_variation_price * 0.1)) * model.quantity_in_cart;
                        }
                        model.product_variation_id = pv.product_variation_id;
                        model.product_id = pv.product_id;

                        if (pv.allow_ordering_while_out_of_stock_status != null)
                        {
                            model.allow_ordering_while_out_of_stock_status = (bool)pv.allow_ordering_while_out_of_stock_status;
                        }
                        if (pv.product.warehouse_management_status != null)
                        {
                            model.warehouse_management_status = (bool)pv.product.warehouse_management_status;
                        }
                        if (pv.allow_delivery != null)
                        {
                            model.allow_delivery = (bool)pv.allow_delivery;
                        }

                        model.num_of_product_variation = product_variation_provider.getByProductId(model.product_id).Count();
                        model.supplier_id = (int)pv.product.supplier_id;
                        model.product_variation_name = pv.product_variation_name;
                        model.product_name = pv.product.product_name;
                        model.product_seo_alias = pv.product.seo_alias;
                        if (pv.product.supplier_id != null)
                        {
                            model.suppiler_name = pv.product.supplier.supplier_name;
                        }
                        model.product_variation_image = pv.product_variation_image;
                        model.product_variation_alt_image = pv.product_variation_image_alt;
                        if (model.product_variation_alt_image == "" || model.product_variation_alt_image == null)
                        {
                            model.product_variation_alt_image = pv.product.alt_image;
                        }
                        if (pv.product_variation_in_stock != null)
                        {
                            model.product_variation_in_stock = (int)pv.product_variation_in_stock;
                        }

                        if (pv.product.price != null)
                        {
                            model.product_variation_old_price = (decimal)pv.product.price;
                            model.discount_percent = (int)Math.Round((double)((pv.product.price - pv.product_variation_price) / pv.product.price) * 100);
                        }
                        if (pv.product.unit_id != null)
                        {
                            model.unit_name = pv.product.unit.unit_name;
                        }

                        list_return.Add(model);
                    }
                }
            }

            ViewBag.NumInCart = num_in_cart;
            ViewBag.CostAllProduct = cost_all_product;
            ViewBag.CostAllProductVAT = cost_all_product_vat;
            return PartialView("_PartialViewItemInCart", list_return);
        }

        [HttpPost]
        public ActionResult GetAllInCartForQuickViewCart(List<ProductVariationInCartModel> list_in_cart)
        {
            var list_return = new List<ProductVariationModel>();
            int num_in_cart = 0;
            decimal cost_all_product = 0;
            decimal cost_all_product_vat = 0;
            if (list_in_cart != null)
            {
                foreach (var obj in list_in_cart)
                {
                    num_in_cart += obj.quantity;
                    var pv = product_variation_provider.getById(obj.id);
                    if (pv != null)
                    {
                        ProductVariationModel model = new ProductVariationModel();


                        model.quantity_in_cart = obj.quantity;
                        model.product_variation_price = (decimal)pv.product_variation_price;
                        cost_all_product += model.product_variation_price * model.quantity_in_cart;
                        if (pv.product.vat_status == true)
                        {
                            cost_all_product_vat += model.product_variation_price * model.quantity_in_cart;
                        }
                        else
                        {
                            cost_all_product_vat += (model.product_variation_price + (decimal)((double)model.product_variation_price * 0.1)) * model.quantity_in_cart;
                        }
                        model.product_variation_id = pv.product_variation_id;
                        model.product_id = pv.product_id;

                        if (pv.allow_ordering_while_out_of_stock_status != null)
                        {
                            model.allow_ordering_while_out_of_stock_status = (bool)pv.allow_ordering_while_out_of_stock_status;
                        }
                        if (pv.product.warehouse_management_status != null)
                        {
                            model.warehouse_management_status = (bool)pv.product.warehouse_management_status;
                        }
                        if (pv.allow_delivery != null)
                        {
                            model.allow_delivery = (bool)pv.allow_delivery;
                        }

                        model.num_of_product_variation = product_variation_provider.getByProductId(model.product_id).Count();
                        model.supplier_id = (int)pv.product.supplier_id;
                        model.product_variation_name = pv.product_variation_name;
                        model.product_name = pv.product.product_name;
                        model.product_seo_alias = pv.product.seo_alias;
                        if (pv.product.supplier_id != null)
                        {
                            model.suppiler_name = pv.product.supplier.supplier_name;
                        }
                        model.product_variation_image = pv.product_variation_image;
                        model.product_variation_alt_image = pv.product_variation_image_alt;
                        if (model.product_variation_alt_image == "" || model.product_variation_alt_image == null)
                        {
                            model.product_variation_alt_image = pv.product.alt_image;
                        }
                        if (pv.product_variation_in_stock != null)
                        {
                            model.product_variation_in_stock = (int)pv.product_variation_in_stock;
                        }

                        if (pv.product.price != null)
                        {
                            model.product_variation_old_price = (decimal)pv.product.price;
                            model.discount_percent = (int)Math.Round((double)((pv.product.price - pv.product_variation_price) / pv.product.price) * 100);
                        }
                        if (pv.product.unit_id != null)
                        {
                            model.unit_name = pv.product.unit.unit_name;
                        }

                        list_return.Add(model);
                    }
                }
            }

            ViewBag.NumInCart = num_in_cart;
            ViewBag.CostAllProduct = cost_all_product;
            ViewBag.CostAllProductVAT = cost_all_product_vat;
            return PartialView("_PartialViewQuickViewCart", list_return);
        }

        [ChildActionOnly]
        public PartialViewResult GetItemCart()
        {
            return PartialView("_PartialViewItemInCart");
        }

        [HttpPost]
        public ActionResult UpdateCartWhenLogin(long user_id, List<ProductVariationInCartModel> list_from_cookie)
        {
            if (user_id > 0)
            {
                var list_to_update = new List<cart_product_variation>();
                if (list_from_cookie != null && list_from_cookie.Count() > 0)
                {
                    foreach (var item in list_from_cookie)
                    {
                        var model = new cart_product_variation();
                        model.product_variation_id = item.id;
                        model.quantity = item.quantity;
                        list_to_update.Add(model);
                    }
                    cart_provider.updateCartForUser(user_id, list_to_update);
                }
                else
                {
                    cart_provider.deleteCartForUser(user_id);
                }
            }
            return Json("Update Success.");
        }

        [HttpPost]
        public ActionResult GetCartFromDbToUpdateCookie(long user_id)
        {
            var list_get = cart_provider.getCartForUser(user_id);
            var list_to_update_cookie = new List<ProductVariationInCartModel>();
            if (list_get != null)
            {
                foreach (var item in list_get)
                {
                    var model = new ProductVariationInCartModel();
                    model.id = item.product_variation_id;
                    model.quantity = item.quantity;
                    list_to_update_cookie.Add(model);
                }
            }
            return Json(new { list_return = list_to_update_cookie });
        }
    }

}