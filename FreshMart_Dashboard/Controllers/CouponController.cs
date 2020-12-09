using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DropupManagement.Models;
using Data;
using Data.Providers;
using System.Globalization;
using System.Web.Security;
using PagedList;

namespace DropupManagement.Controllers
{
    public class CouponController : Controller
    {
        CustomerProvider customer_provider = new CustomerProvider();
        LocalProvider local_provider = new LocalProvider();
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();
        CouponProvider coupon_provider = new CouponProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        // GET: Coupon
        public ActionResult Index(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value,
                                    string quick_filer_tab,
                                    int? size_of_page, string sort_list_return)
        {
            if (quick_filer_tab == null)
            {
                quick_filer_tab = "";
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterTab = quick_filer_tab;
            if (size_of_page == null)
            {
                size_of_page = 10;
            }
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            return View();
        }

        public PartialViewResult GetCoupon(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<coupon>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            int Size_Of_Page = size_of_page;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterTab = quick_filer_tab;
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            list_return = coupon_provider.getListCouponFilter(search_string, quick_filter_select, quick_filter_condition, (int)(quick_filter_value), quick_filer_tab);
            ViewBag.NumberOfList = list_return.Count();

            if (sort_list_return == "code_az")
            {
                list_return = list_return.OrderBy(p => p.coupon_code).ToList();
            }
            else if (sort_list_return == "code_za")
            {
                list_return = list_return.OrderByDescending(p => p.coupon_code).ToList();
            }
            else if (sort_list_return == "number_of_use_asc")
            {
                list_return = list_return.OrderBy(p => p.coupon_number_of_uses).ToList();
            }
            else if (sort_list_return == "number_of_use_dec")
            {
                list_return = list_return.OrderByDescending(p => p.coupon_number_of_uses).ToList();
            }
            return PartialView("_PartialViewCoupon", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult InsertCoupon()
        {
            ViewData["ListCustomer"] = customer_provider.getAll();
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            return View();
        }

        public ActionResult GetCategoryForCoupon()
        {
            var list_return = new List<CategoryModel>();
            foreach (var category in category_provider.getAll())
            {
                CategoryModel model = new CategoryModel();
                model.category_id = category.category_id;
                model.category_name = category.category_name;
                list_return.Add(model);
            }
            return Json(list_return, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertCoupon(CouponModel obj, List<long> list_product_variation_id,
            string coupon_start_date, string coupon_start_time, string coupon_end_date, string coupon_end_time)
        {
            coupon model = new coupon();
            model.coupon_code = obj.coupon_code;
            model.allow_use_with_promotion_status = obj.allow_use_with_promotion_status;
            model.coupon_status = true;
            if (obj.coupon_number_of_uses == 0)
            {
                model.coupon_number_of_uses = null;
            }
            else
            {
                model.coupon_number_of_uses = obj.coupon_number_of_uses;
            }
            model.count_used = 0;
            if (obj.discount_type_id == 4)
            {
                model.discount_type_id = 1002;
            }
            else
            {
                model.discount_type_id = obj.discount_type_id;
            }
            model.discount_value = obj.discount_value;
            if (obj.condition_for_coupon_id == 5)
            {
                model.condition_for_coupon_id = 1002;
            }
            else
            {
                model.condition_for_coupon_id = obj.condition_for_coupon_id;
            }

            model.coupon_order_cost_from = obj.coupon_order_cost_from;
            model.create_date = DateTime.Now;
            model.product_id = obj.product_id;
            if (model.product_id == 0)
            {
                model.product_id = null;
            }
            model.category_id = obj.category_id;
            if (model.category_id == 0)
            {
                model.category_id = null;
            }
            model.province_id = obj.province_id;
            if (model.province_id == 0)
            {
                model.province_id = null;
            }
            model.customer_id = obj.customer_id;
            if (model.customer_id == 0)
            {
                model.customer_id = null;
            }
            model.object_of_coupon_id = obj.object_of_coupon_id;
            DateTime coupon_start_date_and_time = new DateTime();
            if (coupon_start_date != null && coupon_start_time != null)
            {
                if (coupon_start_date != "" && coupon_start_time != "")
                {
                    string str = coupon_start_date.Replace('/', '-') + " " + coupon_start_time;
                    coupon_start_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_start_date = coupon_start_date_and_time;
                }
                else if (coupon_start_date != "" && coupon_start_time == "")
                {
                    string str = coupon_start_date.Replace('/', '-') + " " + coupon_start_time;
                    coupon_start_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_start_date = coupon_start_date_and_time;
                }
                else if (coupon_start_date == "" && coupon_start_time != "")
                {
                    string str = coupon_start_date.Replace('/', '-') + " " + coupon_start_time;
                    coupon_start_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_start_date = coupon_start_date_and_time;
                }
                else
                {
                    model.coupon_start_date = System.DateTime.Now;
                }
            }
            else
            {
                model.coupon_start_date = System.DateTime.Now;
            }
            DateTime coupon_end_date_and_time = new DateTime();
            if (coupon_end_date != null && coupon_end_time != null)
            {
                if (coupon_end_date != "" && coupon_end_time != "")
                {
                    string str = coupon_end_date.Replace('/', '-') + " " + coupon_end_time;
                    coupon_end_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_end_date = coupon_end_date_and_time;
                }
                else if (coupon_end_date != "" && coupon_end_time == "")
                {
                    string str = coupon_end_date.Replace('/', '-') + " " + coupon_end_time;
                    coupon_end_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_end_date = coupon_end_date_and_time;
                }
                else if (coupon_end_date == "" && coupon_end_time != "")
                {
                    string str = coupon_end_date.Replace('/', '-') + " " + coupon_end_time;
                    coupon_end_date_and_time = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.coupon_end_date = coupon_end_date_and_time;
                }
                else
                {
                    model.coupon_end_date = null;
                }
            }
            else
            {
                model.coupon_end_date = null;
            }

            //DESCRIPTION
            decimal discount_value_description = (decimal)model.discount_value;
            string coupon_description = "";
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            if (model.discount_type_id == 1)
            {
                coupon_description = "Giảm " + (int)discount_value_description + "%" + " cho " + coupon_provider.getNameConditionForCoupon(model.condition_for_coupon_id);
            }
            else if (model.discount_type_id == 2)
            {
                if (discount_value_description != 0)
                {
                    coupon_description = "Giảm " + String.Format(info, "{0:c}", (double)discount_value_description) + " cho " + coupon_provider.getNameConditionForCoupon(model.condition_for_coupon_id);
                }
                else
                {
                    coupon_description = "Giảm " + String.Format(info, "{0:c}", (double)discount_value_description) + " cho " + coupon_provider.getNameConditionForCoupon(model.condition_for_coupon_id);
                }
            }
            else if (model.discount_type_id == 3)
            {
                coupon_description = "Đồng giá " + String.Format(info, "{0:c}", (double)discount_value_description) +
                coupon_provider.getUnitDiscountType(model.discount_type_id) + " cho " + coupon_provider.getNameConditionForCoupon(model.condition_for_coupon_id);
            }
            else
            {
                coupon_description = "Miễn phí vận chuyển khu vực " + local_provider.getNameOfProvinceByProvinceId((int)model.province_id) + " cho " + coupon_provider.getNameConditionForCoupon(model.condition_for_coupon_id);
            }
            //CONDITION
            if (model.condition_for_coupon_id == 2)
            {
                coupon_description = coupon_description + " " + String.Format(info, "{0:c}", model.coupon_order_cost_from);
            }
            else if (model.condition_for_coupon_id == 3)
            {
                coupon_description = coupon_description + " " + coupon_provider.getNameCategory((long)(model.category_id));
            }
            else if (model.condition_for_coupon_id == 4)
            {
                coupon_description = coupon_description + " " + coupon_provider.getNameProduct((long)(model.product_id));
            }

            model.coupon_description = coupon_description;
            int coupon_id = 0;
            if (coupon_provider.insertCoupon(model, ref coupon_id))
            {
                if (list_product_variation_id != null)
                {
                    coupon_provider.insertProductVariationCoupon(list_product_variation_id, coupon_id);
                }
                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo mã giảm giá " + model.coupon_code;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_url = "/Coupon/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { coupon_code = model.coupon_code }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckCoupon(string coupon_code, long? customer_id, decimal total_cost_of_product_in_order, decimal cost_of_shipping_for_this_order, int province_id,
            List<long> list_product_variation_id, List<ProductVariationInOrderModel> list_quantity_pv)
        {
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
                    invalid_name = "coupon_object_null"; invalid_description = "Yêu cầu điền hoặc chọn thông tin khách hàng để được xét mã giảm giá này."; status = false;
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
                        if ((coupon_to_check.discount_type_id == 1002 && province_id == null) || (coupon_to_check.discount_type_id == 1002 && province_id <= 0))
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
                            invalid_name = "product_variation_invalid"; invalid_description = "Các biến thể sản phẩm trên đơn hàng này không thỏa mãn điều kiện của mã giảm giá."; status = false;
                        }
                        else
                        {
                            discount_value_return = getValuePromotionByCouponCode(coupon_code, total_cost_of_product_in_order, cost_of_shipping_for_this_order, province_id, list_product_variation_id, list_quantity_pv);
                            invalid_name = "valid"; invalid_description = "Mã giảm giá hợp lệ."; status = true;
                            coupon_id = coupon_to_check.coupon_id;
                        }
                    }
                }
            }
            else
            {
                invalid_name = "coupon_not_exist"; invalid_description = "Mã giảm giá không tồn tại. Vui lòng kiểm tra lại."; status = false;
            }
            return Json(new { coupon_invalid_description = invalid_description, coupon_invalid_status = status, discount_value_return = discount_value_return, coupon_id = coupon_id }, JsonRequestBehavior.AllowGet);
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
    }
}