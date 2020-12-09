using Common;
using Data;
using Data.Providers;
using ElysienVietNam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ElysienVietNam.Controllers
{
    public class CheckoutController : Controller
    {
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();
        CartProvider cart_provider = new CartProvider();
        CouponProvider coupon_provider = new CouponProvider();
        OrderProvider order_provider = new OrderProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        OrderLogProvider order_log_provider = new OrderLogProvider();
        PaymentInformationProvider payment_information_provider = new PaymentInformationProvider();
        SystemNotificationProvider sn_provider = new SystemNotificationProvider();
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        CustomerProvider customer_provider = new CustomerProvider();
        DeliveryAddressProvider da_provider = new DeliveryAddressProvider();
        UserProvider user_provider = new UserProvider();
        StatisticsProvider statistics_provider = new StatisticsProvider();
        ProductVariationInOrderProvider pvio_provider = new ProductVariationInOrderProvider();
        NewProductInOrderProvider cpio_provider = new NewProductInOrderProvider();

        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");

        // GET: Checkout
        public ActionResult Index()
        {
            if (Request.Cookies["DINO_CART"] != null)
            {
                var list_from_cookie = GetListFromCookie("DINO_CART");
                decimal cost_all_product_vat = 0;
                var list_checkout = GetAllInCartCheckout(list_from_cookie, ref cost_all_product_vat);
                ViewBag.CostAllProductVAT = cost_all_product_vat;
                return View(list_checkout);
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }

        public user getUserLoged()
        {
            user user = new user();
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string username = "";
            if (authCookie != null)
            {
                username = FormsAuthentication.Decrypt(authCookie.Value).Name;
            }
            user = user_provider.GetByUserName(username);
            return (user);
        }

        public ActionResult GetShipping()
        {
            var list_from_cookie = GetListFromCookie("DINO_CART");
            decimal cost_all_product_vat = 0;
            var list_checkout = GetAllInCartCheckout(list_from_cookie, ref cost_all_product_vat);
            ViewBag.CostAllProductVAT = cost_all_product_vat;
            return PartialView("_PartialViewShipping");
        }

        public ActionResult GetPayment(DeliveryAddressModel da_model)
        {
            return PartialView("_PartialViewPayment", da_model);
        }

        public ActionResult GetOrderSuccess(long order_id)
        {
            return PartialView("_PartialViewOrderSuccess", order_provider.getById(order_id));
        }
        public List<ProductVariationInCartModel> GetListFromCookie(string name)
        {
            List<ProductVariationInCartModel> list_return_from_cookie = new List<ProductVariationInCartModel>();
            if (Request.Cookies["DINO_CART"] != null)
            {
                string objCartListString = HttpUtility.UrlDecode(Request.Cookies["DINO_CART"].Value.ToString());
                list_return_from_cookie = JsonConvert.DeserializeObject<List<ProductVariationInCartModel>>(objCartListString);
            }
            return list_return_from_cookie;
        }

        public List<ProductVariationModel> GetAllInCartCheckout(List<ProductVariationInCartModel> list_in_cart, ref decimal cost_all_product_vat)
        {
            var list_return = new List<ProductVariationModel>();
            int num_in_cart = 0;
            decimal cost_all_product = 0;
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

            //ViewBag.NumInCart = num_in_cart;
            //ViewBag.CostAllProduct = cost_all_product;
            //ViewBag.CostAllProductVAT = cost_all_product_vat;
            return list_return;
        }

        [HttpPost]
        public ActionResult CheckCoupon(string coupon_code, long? customer_id, decimal total_cost_of_product_in_order, decimal cost_of_shipping_for_this_order, int province_id,
           List<long> list_product_variation_id, List<ProductVariationInOrderModel> list_quantity_pv)
        {
            string coupon_summary = "";
            string invalid_name = "";
            string invalid_description = "";
            bool status = false;
            var coupon_to_check = coupon_provider.getByCouponCode(coupon_code);
            DateTime current_datetime = System.DateTime.Now;
            decimal discount_value_return = 0;
            int coupon_id = 0;
            if (coupon_to_check != null)
            {
                if (coupon_to_check.coupon_start_date > current_datetime)
                {
                    invalid_name = "coupon_not_yet_acitve"; invalid_description = "Mã giảm giá chưa đến ngày áp dụng."; status = false;
                }
                else if (coupon_to_check.coupon_end_date < current_datetime)
                {
                    invalid_name = "coupon_out_of_date"; invalid_description = "Mã giảm giá đã hết hạn sử dụng."; status = false;
                }
                else if (coupon_to_check.coupon_status == false)
                {
                    invalid_name = "coupon_stop_active"; invalid_description = "Mã giảm giá đã ngừng áp dụng."; status = false;
                }
                else if (coupon_to_check.coupon_number_of_uses == coupon_to_check.count_used)
                {
                    invalid_name = "coupon_out_of_use"; invalid_description = "Mã giảm giá đã hết số lần sử dụng."; status = false;
                }

                else if (customer_id == null || customer_id == 0)
                {
                    if (coupon_to_check.object_of_coupon_id == 1)
                    {
                        if (coupon_to_check.coupon_order_cost_from != null && total_cost_of_product_in_order < coupon_to_check.coupon_order_cost_from)
                        {
                            invalid_name = "coupon_invalid_cost_order_from"; invalid_description = "Tổng giá trị sản phẩm trên đơn hàng này không thỏa mãn điều kiện mã giảm giá."; status = false;
                        }
                        else if (list_product_variation_id == null)
                        {
                            invalid_name = "product_variation_null"; invalid_description = "Yêu cầu thêm sản phẩm cho đơn hàng để xét mã giảm giá này."; status = false;
                        }
                        else if (list_product_variation_id != null)
                        {
                            if ((coupon_to_check.discount_type_id == 1002 && province_id > 0) || (coupon_to_check.discount_type_id == 1002 && province_id <= 0))
                            {
                                invalid_name = "province_null"; invalid_description = "Yêu cầu nhập địa chỉ giao hàng để được xét mã giảm giá này"; status = false;
                            }
                            else if (coupon_to_check.condition_for_coupon_id == 3 && category_provider.getListCategoryIdByListProductVariationId(list_product_variation_id).Contains((long)coupon_to_check.category_id) == false)
                            {
                                invalid_name = "category_invalid"; invalid_description = "Danh mục của các sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                            }
                            else if (coupon_to_check.condition_for_coupon_id == 4 && product_provider.getListProductIdByListProductVariationId(list_product_variation_id).Contains((long)coupon_to_check.product_id) == false)
                            {
                                invalid_name = "product_invalid"; invalid_description = "Các sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                            }
                            else if (coupon_to_check.condition_for_coupon_id == 1002 && product_variation_provider.checkListProductVariationCouponAndListProductVariationOrder(product_variation_provider.getListProductVariationIdByCouponCode(coupon_code), list_product_variation_id) == false)
                            {
                                invalid_name = "product_variation_invalid"; invalid_description = "Phân loại sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                            }
                            else
                            {
                                discount_value_return = getValuePromotionByCouponCode(coupon_code, total_cost_of_product_in_order, cost_of_shipping_for_this_order, province_id, list_product_variation_id, list_quantity_pv);
                                invalid_name = "valid"; invalid_description = "Mã giảm giá hợp lệ."; status = true;
                                coupon_id = coupon_to_check.coupon_id;
                                coupon_summary = coupon_to_check.coupon_description;
                            }
                        }
                    }
                    else
                    {
                        invalid_name = "coupon_object_null"; invalid_description = "Yêu cầu điền hoặc chọn thông tin khách hàng để được xét mã giảm giá này."; status = false;
                    }
                }
                else if (customer_id != null && customer_id != 0)
                {
                    if (coupon_to_check.coupon_order_cost_from != null && total_cost_of_product_in_order < coupon_to_check.coupon_order_cost_from)
                    {
                        invalid_name = "coupon_invalid_cost_order_from"; invalid_description = "Tổng giá trị sản phẩm trên đơn hàng này không thỏa mãn điều kiện mã giảm giá."; status = false;
                    }
                    else if (coupon_provider.validationGroupCustomerInOrder(coupon_code, (long)customer_id) == false)
                    {
                        invalid_name = "coupon_not_for_this_customer"; invalid_description = "Mã giảm giá không áp dụng cho khách hàng này."; status = false;
                    }
                    else if (list_product_variation_id == null)
                    {
                        invalid_name = "product_variation_null"; invalid_description = "Yêu cầu thêm sản phẩm cho đơn hàng để xét mã giảm giá này."; status = false;
                    }
                    else if (list_product_variation_id != null)
                    {
                        if ((coupon_to_check.discount_type_id == 1002 && province_id > 0) || (coupon_to_check.discount_type_id == 1002 && province_id <= 0))
                        {
                            invalid_name = "province_null"; invalid_description = "Yêu cầu nhập địa chỉ giao hàng để được xét mã giảm giá này"; status = false;
                        }
                        else if (coupon_to_check.condition_for_coupon_id == 3 && category_provider.getListCategoryIdByListProductVariationId(list_product_variation_id).Contains((long)coupon_to_check.category_id) == false)
                        {
                            invalid_name = "category_invalid"; invalid_description = "Danh mục của các sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                        }
                        else if (coupon_to_check.condition_for_coupon_id == 4 && product_provider.getListProductIdByListProductVariationId(list_product_variation_id).Contains((long)coupon_to_check.product_id) == false)
                        {
                            invalid_name = "product_invalid"; invalid_description = "Các sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                        }
                        else if (coupon_to_check.condition_for_coupon_id == 1002 && product_variation_provider.checkListProductVariationCouponAndListProductVariationOrder(product_variation_provider.getListProductVariationIdByCouponCode(coupon_code), list_product_variation_id) == false)
                        {
                            invalid_name = "product_variation_invalid"; invalid_description = "Phân loại sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                        }
                        else
                        {
                            discount_value_return = getValuePromotionByCouponCode(coupon_code, total_cost_of_product_in_order, cost_of_shipping_for_this_order, province_id, list_product_variation_id, list_quantity_pv);
                            invalid_name = "valid"; invalid_description = "Mã giảm giá hợp lệ."; status = true;
                            coupon_id = coupon_to_check.coupon_id;
                            coupon_summary = coupon_to_check.coupon_description;
                        }
                    }
                }
            }
            else
            {
                invalid_name = "coupon_not_exist"; invalid_description = "Mã giảm giá không tồn tại. Vui lòng kiểm tra lại."; status = false;
            }
            return Json(new { coupon_invalid_description = invalid_description, coupon_summary = coupon_summary, coupon_invalid_status = status, discount_value_return = discount_value_return, coupon_id = coupon_id }, JsonRequestBehavior.AllowGet);
        }

        public decimal getValuePromotionByCouponCode(string coupon_code, decimal total_cost_of_product_in_order,
           decimal cost_of_shipping_for_this_order, int province_id, List<long> list_product_variation_id,
           List<ProductVariationInOrderModel> list_quantity_pv)
        {
            try
            {
                decimal value_to_return = 0;
                var coupon = coupon_provider.getByCouponCode(coupon_code);
                if (coupon.condition_for_coupon_id == 1 || coupon.condition_for_coupon_id == 2)
                {
                    if (coupon.discount_type_id == 1)
                    {
                        value_to_return = (decimal)((coupon.discount_value / 100) * total_cost_of_product_in_order);
                    }
                    else if (coupon.discount_type_id == 2)
                    {
                        value_to_return = (decimal)coupon.discount_value;
                    }
                    else if (coupon.discount_type_id == 1002 && coupon.province_id == province_id)
                    {
                        value_to_return = cost_of_shipping_for_this_order;
                    }
                    else
                    {
                        value_to_return = 0;
                    }
                }
                else if (coupon.condition_for_coupon_id == 3)
                {
                    if (coupon.discount_type_id == 1)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv.product_variation_id).Contains((long)coupon.category_id))
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price * (pv.product_variation_quantity));
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv.product_variation_id).Contains((long)coupon.category_id))
                            {
                                value_to_return += (decimal)(coupon.discount_value * pv.product_variation_quantity);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv.product_variation_id).Contains((long)coupon.category_id) && (product_variation.product_variation_price >= coupon.discount_value))
                            {
                                value_to_return += (decimal)((product_variation.product_variation_price - coupon.discount_value) * pv.product_variation_quantity);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 1002 && coupon.province_id == province_id)
                    {
                        if (category_provider.getListCategoryIdByListProductVariationId(list_product_variation_id).Contains((long)coupon.category_id))
                        {
                            value_to_return = cost_of_shipping_for_this_order;
                        }
                    }
                    else
                    {
                        value_to_return = 0;
                    }
                }
                else if (coupon.condition_for_coupon_id == 4)
                {
                    if (coupon.discount_type_id == 1)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price * pv.product_variation_quantity);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)(coupon.discount_value * pv.product_variation_quantity);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)((product_variation.product_variation_price - coupon.discount_value) * pv.product_variation_quantity);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 1002 && coupon.province_id == province_id)
                    {
                        var list_product_id = product_provider.getListProductIdByListProductVariationId(list_product_variation_id);
                        if (list_product_id.Contains((long)coupon.product_id))
                        {
                            value_to_return = cost_of_shipping_for_this_order;
                        }
                    }
                    else
                    {
                        value_to_return = 0;
                    }
                }
                else if (coupon.condition_for_coupon_id == 1002)
                {
                    if (coupon.discount_type_id == 1)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv.product_variation_id))
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price * pv.product_variation_quantity);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv.product_variation_id))
                            {
                                value_to_return += (decimal)(coupon.discount_value * pv.product_variation_quantity);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv in list_quantity_pv)
                        {
                            var product_variation = product_variation_provider.getById(pv.product_variation_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv.product_variation_id))
                            {
                                value_to_return += (decimal)((product_variation.product_variation_price - coupon.discount_value) * pv.product_variation_quantity);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 1002 && coupon.province_id == province_id)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = product_variation_provider.getById(pv_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv_id))
                            {
                                value_to_return = cost_of_shipping_for_this_order;
                            }
                        }
                    }
                }
                else
                {
                    value_to_return = 0;
                }
                return Math.Abs(value_to_return);
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        public async Task<ActionResult> MakeOrderPending(DeliveryAddressModel da, decimal discount_amount,
            string reason_of_promotion,
            string name_of_shipping, decimal cost_of_shipping, int payment_method_id,
            decimal amount_received, int coupon_id)
        {
            var list_cart_pv = new List<cart_product_variation>();
            long user_id = 0;
            long customer_id = 0;
            string user_name = "";
            if (getUserLoged() != null)
            {
                user_id = getUserLoged().user_id;
                list_cart_pv = cart_provider.getCartForUser(user_id);
                customer_id = customer_provider.getByUserId(user_id).customer_id;
            }
            else
            {
                user user = new user();
                user.username = da.customer_email;
                user.lastname = da.last_name;
                user.phone_number = da.phone_number;
                user.role_id = 3;
                user.email = da.customer_email;
                user.province_id = da.province_id;
                user.district_id = da.district_id;
                user.ward_id = da.ward_id;
                user.address_detail = da.address_detail;
                user.join_date = System.DateTime.Now;
                user_provider.insertUser(user, ref user_id, ref user_name);

                customer customer = new customer();
                customer.customer_last_name = da.last_name;
                customer.customer_phone_number = da.phone_number;
                customer.customer_email = da.customer_email;
                customer.province_id = da.province_id;
                customer.district_id = da.district_id;
                customer.ward_id = da.ward_id;
                customer.address_detail = da.address_detail;
                customer.user_id = user_id;
                customer.customer_note = "Khách truy cập website";
                customer.marketing_status = true;
                customer.black_list_status = false;
                customer.create_datetime = System.DateTime.Now;
                customer_provider.insertCustomer(customer, ref customer_id);

                var list_from_cookie = GetListFromCookie("DINO_CART");
                var list_to_update = new List<cart_product_variation>();
                foreach (var item in list_from_cookie)
                {
                    var model = new cart_product_variation();
                    model.product_variation_id = item.id;
                    model.quantity = item.quantity;
                    list_to_update.Add(model);
                }
                cart_provider.updateCartForUser(user_id, list_to_update);
                list_cart_pv = cart_provider.getCartForUser(user_id);
            }

            order order = new order();
            if (coupon_id != 0)
            {
                coupon_provider.updateNumOfUse(coupon_id);
            }
            order.customer_id = customer_provider.getByUserId(user_id).customer_id;
            order.order_note = da.customer_note;
            order.source_order = "Website";
            order.discount_amount = discount_amount;
            order.reason_of_promotion = reason_of_promotion;
            order.name_of_shipping = name_of_shipping;
            order.cost_of_shipping = cost_of_shipping;
            order.user_id_create = user_id;
            order.user_id_update = user_id;
            order.create_datetime = System.DateTime.Now;
            order.update_datetime = System.DateTime.Now;
            order.draft_status = false;
            order.confirm_order_status = false;
            order.payment_status = false;
            order.payment_method_id = payment_method_id;

            delivery_address da_model_insert = new delivery_address();
            da_model_insert.last_name = da.last_name;
            da_model_insert.phone_number = da.phone_number;
            da_model_insert.province_id = da.province_id;
            da_model_insert.district_id = da.district_id;
            da_model_insert.ward_id = da.ward_id;
            da_model_insert.address_detail = da.address_detail;
            da_model_insert.customer_id = customer_id;
            int delivery_address_id_in_order = 0;
            if (da_provider.insertDeliveryAddress(da_model_insert, ref delivery_address_id_in_order))
            {
                order.delivery_address_id = delivery_address_id_in_order;
            }

            long order_id_ref = 0;
            if (order_provider.insertOrder(order, ref order_id_ref))
            {
                if (list_cart_pv != null)
                {
                    foreach (var product_variation_in_order in list_cart_pv)
                    {
                        order_provider.insertProductVariationInOrder(product_variation_in_order.product_variation_id,
                            product_variation_in_order.quantity, order_id_ref);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo đơn hàng " + "#" + order_id_ref + " từ website";
                system_log.user_id = user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order_id_ref;
                system_log.system_log_url = "/Order/OrderDetail";
                system_log_provider.insertSystemLog(system_log);


                int order_log_id_1 = 0;
                int order_log_id_2 = 0;
                order_log order_log_1 = new order_log();
                order_log_1.order_id = order_id_ref;
                order_log_1.order_log_description = "<strong>" + user_provider.getByUserId(user_id).username + "</strong>" + " đã tạo đơn hàng #" + "<strong>" + order_id_ref + "</strong> từ website";
                order_log_1.order_log_create_datetime = DateTime.Now;
                order_log_1.order_log_type = "create";
                if (order_log_provider.insertOrderLog(order_log_1, ref order_log_id_1))
                {
                    string currency = String.Format(elGR, "{0:0,0}", amount_received);
                    order_log order_log_2 = new order_log();
                    order_log_2.order_id = order_id_ref;
                    order_log_2.order_log_description = "Thanh toán " + "<strong>" + currency + " ₫</strong> đang chờ xử lý";
                    order_log_2.order_log_create_datetime = DateTime.Now;
                    order_log_2.order_log_type = "payment";
                    if (order_log_provider.insertOrderLog(order_log_2, ref order_log_id_2))
                    {
                        payment_information pi = new payment_information();
                        pi.order_log_id = order_log_id_2;
                        if (order.payment_method_id == 3)
                        {
                            pi.description = "Trì hoãn Thanh toán khi giao hàng (COD) từ người mua";
                        }
                        else
                        {
                            pi.description = "Trì hoãn Chuyển khoản ngân hàng từ người mua";
                        }
                        pi.value_money = amount_received;
                        if (order.payment_method_id == 3)
                        {
                            pi.pay_gate = "Thanh toán khi giao hàng (COD)";
                        }
                        else
                        {
                            pi.pay_gate = "Chuyển khoản qua ngân hàng";
                        }

                        pi.pay_status = false;
                        pi.pay_type = "Chờ xử lý";
                        pi.user_id = user_id;
                        pi.datetime_confirm = DateTime.Now;
                        payment_information_provider.insertPaymentInformation(pi);

                        //Send Email for Admin
                        await SendEmailForAdmin(order_id_ref);

                        system_notification sn = new system_notification();
                        sn.system_notification_title = "Đơn hàng #" + order_id_ref + " được tạo.";
                        sn.system_notification_create_datetime = DateTime.Now;
                        sn.system_notification_url_obj = "/Order/OrderDetail?id=" + order_id_ref;
                        sn_provider.insertSystemNotification(sn);
                    }
                }
            }
            return Json(new { order_id = order_id_ref }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SendEmailForAdmin(long order_id)
        {
            var order = order_provider.getById(order_id);
            string email_receiver = System.Configuration.ConfigurationManager.AppSettings["ToEmailAddress"];
            decimal order_value = statistics_provider.getRevenuePerOrder(order.order_id);
            string order_reason_promotion = "";
            if (order.reason_of_promotion != null)
            {
                order_reason_promotion = order.reason_of_promotion;
            }
            decimal order_total_sum_cost = statistics_provider.getTotalValuePerOrder(order.order_id);
            string store_website = System.Configuration.ConfigurationManager.AppSettings["store_website"];
            string store_name = System.Configuration.ConfigurationManager.AppSettings["store_name"];
            string store_logo_url = System.Configuration.ConfigurationManager.AppSettings["store_logo_url"];
            string store_color = System.Configuration.ConfigurationManager.AppSettings["store_color"];
            string store_email = System.Configuration.ConfigurationManager.AppSettings["store_email"];
            string store_phone_number = System.Configuration.ConfigurationManager.AppSettings["store_phone_number"];
            string order_detail_url = System.Configuration.ConfigurationManager.AppSettings["urlCMS"] + "/Order/OrderDetail?id=" + order_id;

            string customer_fullname = "";
            if (order.customer.customer_first_name != null && order.customer.customer_last_name != null)
            {
                customer_fullname = order.customer.customer_first_name + order.customer.customer_last_name;
            }
            else
            {
                if (order.customer.customer_first_name == null)
                {
                    customer_fullname = order.customer.customer_last_name;
                }
                else
                {
                    customer_fullname = order.customer.customer_email;
                }
            }
            string customer_phone_number = "";
            if (order.customer.customer_phone_number != null)
            {
                customer_phone_number = order.customer.customer_phone_number;
            }

            string da_detail = order.delivery_address.address_detail + ", " + order.delivery_address.ward.Type + " " + order.delivery_address.ward.Name +
                ", " + order.delivery_address.district.Type + " " + order.delivery_address.district.Name +
                ", " + order.delivery_address.province.Name + ", Việt Nam";

            await SendEmail(email_receiver, order_id, order.create_datetime, (decimal)order.cost_of_shipping, order.payment_method.payment_method_description,
                order_value, (decimal)order.discount_amount, order_reason_promotion, order_total_sum_cost, order_detail_url, store_website, store_name, store_logo_url, store_color,
                store_email, store_phone_number,
                customer_fullname, order.customer.customer_email, customer_phone_number,
                order.delivery_address.last_name, da_detail, order.delivery_address.phone_number, ContentEmailListProduct(order_id));
            return Json(new { email = email_receiver }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thay đổi content và gửi Email
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(string email_receiver, long order_id, DateTime order_create_datetime,
            decimal order_shipping_fee, string order_payment_method,
            decimal order_value, decimal order_discount, string order_reason_promotion,
            decimal order_sum_cost, string order_detail_url,

            string store_website, string store_name, string store_logo_url, string store_color, string store_email, string store_phone_number,

            string customer_fullname, string customer_email, string customer_phone_number,

            string delivery_address_fullname, string delivery_address_detail, string delivery_address_phone_number,

            string order_list_product)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/YouHaveOrder.html"));

            string discount = "";
            if (order_discount == 0)
            {
                discount = "0 ₫";
            }
            else
            {
                discount = FormatCurrency(order_discount);
            }

            content = content.Replace("{{order_id}}", order_id.ToString());
            content = content.Replace("{{order_create_datetime}}", "(Ngày " + order_create_datetime.ToString("dd/MM/yyy") + " lúc " + order_create_datetime.ToString("HH:mm") + ")");
            content = content.Replace("{{order_shipping_fee}}", FormatCurrency(order_shipping_fee));
            content = content.Replace("{{order_payment_method}}", order_payment_method);
            content = content.Replace("{{order_value}}", FormatCurrency(order_value));
            content = content.Replace("{{order_discount}}", discount);
            content = content.Replace("{{order_reason_promotion}}", order_reason_promotion);
            content = content.Replace("{{order_sum_cost}}", FormatCurrency(order_sum_cost));
            content = content.Replace("{{order_detail_url}}", order_detail_url);

            content = content.Replace("{{store_website}}", store_website);
            content = content.Replace("{{store_name}}", store_name);
            content = content.Replace("{{store_logo_url}}", store_logo_url);
            content = content.Replace("{{store_color}}", store_color);
            content = content.Replace("{{store_email}}", store_email);
            content = content.Replace("{{store_phone_number}}", store_phone_number);

            content = content.Replace("{{customer_fullname}}", customer_fullname);
            content = content.Replace("{{customer_email}}", customer_email);
            content = content.Replace("{{customer_phone_number}}", customer_phone_number);

            content = content.Replace("{{delivery_address_fullname}}", delivery_address_fullname);
            content = content.Replace("{{delivery_address_detail}}", delivery_address_detail);
            content = content.Replace("{{delivery_address_phone_number}}", delivery_address_phone_number);

            content = content.Replace("{{order_list_product}}", order_list_product);

            await new MailHelper().SendMail(email_receiver, "[DROPUP] Đơn hàng #" + order_id.ToString() + " được đặt bởi " + customer_fullname, content);
            return true;
        }

        public string ContentEmailListProduct(long order_id)
        {
            string content = "";
            var list_pvio = pvio_provider.getByOrderId(order_id);
            var list_cpio = cpio_provider.getByOrderId(order_id);

            if (list_pvio != null)
            {
                foreach (var pv in list_pvio)
                {
                    string pv_name = "";
                    if (pv.product_variation_name == "Default Title")
                    {
                        pv_name = pv.product_name;
                    }
                    else
                    {
                        pv_name = pv.product_name + " - " + pv.product_variation_name;
                    }
                    content += "<tr>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + pv.product_variation.product.product_code + " </span></td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + ">" +
                              "<span> " + pv_name + " </span>" +
                              "</td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)pv.product_variation_price) + " </span></td>" +
                              "<td align = " + "center" + " style = " + "padding:3px 9px" + " valign = " + "top" + "> " + pv.product_variation_quantity + " </td>" +
                              "<td align = " + "right" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)(pv.product_variation_quantity * pv.product_variation_price)) + " </span></td>" +
                              "</tr> ";
                }
            }

            if (list_cpio != null)
            {
                foreach (var cp in list_cpio)
                {

                    content += "<tr>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + cp.custom_product_in_order_id + " </span></td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + ">" +
                              "<span> " + cp.custom_product_in_order_name + " </span>" +
                              "</td>" +
                              "<td align = " + "left" + "style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)cp.custom_product_in_order_price) + " </span></td>" +
                              "<td align = " + "center" + "style = " + "padding:3px 9px" + " valign = " + "top" + "> " + cp.custom_product_in_order_quantity + " </td>" +
                              "<td align = " + "right" + "style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)(cp.custom_product_in_order_quantity * cp.custom_product_in_order_price)) + " </span></td>" +
                              "</tr> ";
                }
            }
            return content;
        }

        public string FormatCurrency(decimal value)
        {
            return (String.Format(elGR, "{0:0,0}", value) + " ₫");
        }
    }

}