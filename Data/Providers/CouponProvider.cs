using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CouponProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        DateTime current_datetime = System.DateTime.Now;
        CustomerProvider customer_provider = new CustomerProvider();
        OrderProvider order_provider = new OrderProvider();
        CategoryProvider category_provider = new CategoryProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();
        public List<coupon> getAll()
        {
            try
            {
                return db.coupons.OrderBy(c => c.create_date).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertCoupon(coupon model, ref int coupon_id)
        {
            try
            {
                db.coupons.InsertOnSubmit(model);
                db.SubmitChanges();
                coupon_id = model.coupon_id;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool insertProductCoupon(long product_id, int coupon_id)
        {
            try
            {
                product_coupon model = new product_coupon();
                model.product_id = product_id;
                model.coupon_id = coupon_id;
                db.product_coupons.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertProductVariationCoupon(List<long> list_product_variation_id, int coupon_id)
        {
            try
            {
                foreach (var product_variation_id in list_product_variation_id)
                {
                    product_variation_coupon model = new product_variation_coupon();
                    model.product_variation_id = product_variation_id;
                    model.coupon_id = coupon_id;
                    db.product_variation_coupons.InsertOnSubmit(model);
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        

        public string getNameDiscountType(int discount_type_id)
        {
            try
            {
                return db.discount_types.FirstOrDefault(d => d.discount_type_id == discount_type_id).discount_type_name;
            }
            catch
            {
                return "";
            }
        }
        public string getUnitDiscountType(int discount_type_id)
        {
            try
            {
                return db.discount_types.FirstOrDefault(d => d.discount_type_id == discount_type_id).discount_type_unit;
            }
            catch
            {
                return "";
            }
        }

        public string getNameConditionForCoupon(int condition_for_coupon_id)
        {
            try
            {
                return db.condition_for_coupons.FirstOrDefault(d => d.condition_for_coupon_id == condition_for_coupon_id).condition_for_coupon_name;
            }
            catch
            {
                return "";
            }
        }

        public string getNameCategory(long category_id)
        {
            try
            {
                return db.categories.FirstOrDefault(c => c.category_id == category_id).category_name;
            }
            catch
            {
                return "";
            }
        }

        public string getNameProduct(long product_id)
        {
            try
            {
                return db.products.FirstOrDefault(c => c.product_id == product_id).product_name;
            }
            catch
            {
                return "";
            }
        }

        public List<coupon> getListCouponUnexpried(List<coupon> list_input)
        {
            try
            {
                var current_datetime = DateTime.Now;
                return list_input.Where(c => (c.coupon_start_date <= current_datetime && c.coupon_end_date >= current_datetime) || (c.coupon_start_date <= current_datetime && c.coupon_end_date == null)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponExpried()
        {
            try
            {
                return getAll().Where(c => c.coupon_end_date < current_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponActive()
        {
            try
            {

                return getAll().Where(c => (c.coupon_status == true && (current_datetime >= c.coupon_start_date && current_datetime <= c.coupon_end_date) || (current_datetime >= c.coupon_start_date && c.coupon_end_date == null))).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponUnactive()
        {
            try
            {
                return getAll().Where(c => (c.coupon_status == true && current_datetime < c.coupon_start_date) || (c.coupon_status == false && current_datetime >= c.coupon_start_date && current_datetime <= c.coupon_start_date)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponByTabFilter(string quick_filter_tab, List<coupon> list_all_coupon)
        {
            try
            {
                var list_return = new List<coupon>();
                var list_unexpried = getListCouponUnexpried(list_all_coupon);
                if (quick_filter_tab == "tab_active_coupon")
                {
                    list_return = getListCouponActive();
                }
                else if (quick_filter_tab == "tab_unactive_coupon")
                {
                    list_return = getListCouponUnactive();
                }
                else if (quick_filter_tab == "tab_expried_coupon")
                {
                    list_return = getListCouponExpried();
                }
                else
                {
                    list_return = getAll();
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponByDiscountTypeId(int discount_type_id, List<coupon> list_coupon)
        {
            try
            {
                return list_coupon.Where(c => c.discount_type_id == discount_type_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponByNumberOfUse(string quick_filter_condition, int quick_filter_value, List<coupon> list_input)
        {
            try
            {
                var list_return = new List<coupon>();
                if (quick_filter_condition != null)
                {
                    if (quick_filter_condition == "number_of_use_coupon_>")
                    {
                        list_return = list_input.Where(c => c.coupon_number_of_uses > quick_filter_value).ToList();
                    }
                    else if (quick_filter_condition == "number_of_use_coupon_<")
                    {
                        list_return = list_input.Where(c => c.coupon_number_of_uses < quick_filter_value).ToList();
                    }
                    else if (quick_filter_condition == "number_of_use_coupon_=")
                    {
                        list_return = list_input.Where(c => c.coupon_number_of_uses == quick_filter_value).ToList();
                    }
                    else
                    {
                        list_return = list_input.Where(c => c.coupon_number_of_uses != quick_filter_value).ToList();
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<coupon> getListCouponFilter(string search_string, string quick_filter_select, string quick_filter_condition,
                                                int quick_filter_value, string quick_filter_tab)
        {
            try
            {
                var list_return = new List<coupon>();
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quick_filter_condition != null)
                {
                    if (quick_filter_select == "discount_type_coupon")
                    {
                        list_return = getListCouponByDiscountTypeId(Convert.ToInt32(quick_filter_condition), getListCouponByTabFilter(quick_filter_tab, getAll())).Where(c => c.coupon_code.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_date).ToList();
                    }
                    else if (quick_filter_select == "number_of_use_coupon")
                    {
                        list_return = getListCouponByNumberOfUse(quick_filter_condition, quick_filter_value, getListCouponByTabFilter(quick_filter_tab, getAll())).Where(c => c.coupon_code.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_date).ToList();
                    }
                    else if (quick_filter_select == "time_appication_coupon")
                    {
                        //test
                        list_return = null;
                    }
                    else
                    {
                        list_return = getListCouponByTabFilter(quick_filter_tab, getAll()).Where(c => c.coupon_code.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_date).ToList();
                    }
                }
                else
                {
                    list_return = getListCouponByTabFilter(quick_filter_tab, getAll()).Where(c => c.coupon_code.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_date).ToList();
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public bool validationGroupCustomerInOrder(string coupon_code, long customer_id)
        {
            try
            {
                bool status = true;
                var customer = db.customers.FirstOrDefault(cus => cus.customer_id == customer_id);
                var coupon = db.coupons.FirstOrDefault(c => c.coupon_code == coupon_code);
                if (coupon.object_of_coupon_id == 1 && customer.black_list_status == false)
                {
                    status = true;
                }
                else if (coupon.object_of_coupon_id == 2 && customer.black_list_status == false && customer.marketing_status == true)
                {
                    status = true;
                }
                else if (coupon.object_of_coupon_id == 3 && order_provider.getByCustomerId(customer_id).Count() > 1)
                {
                    status = true;
                }
                else if(coupon.object_of_coupon_id == 5 && coupon.customer_id == customer_id)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public coupon getByCouponCode(string coupon_code)
        {
            try
            {
                return db.coupons.FirstOrDefault(c => c.coupon_code.ToLower() == coupon_code.ToLower());
            }
            catch
            {
                return null;
            }
        }

        public object checkCoupon(string coupon_code, int customer_id, decimal total_cost_of_product_in_order,
            List<long> list_category_id, List<long> list_product_id, List<long> list_product_variation_id)
        {
            try
            {
                var model = new { invalid_name = "", invalid_description = "", status = false };
                var coupon_to_check = db.coupons.FirstOrDefault(c => c.coupon_code.ToLower() == coupon_code.ToLower());
                if (coupon_to_check != null)
                {
                    if (coupon_to_check.coupon_status == false)
                    {
                        model = new { invalid_name = "coupon_stop_active", invalid_description = "Mã giảm giá đã ngừng áp dụng.", status = false };
                    }
                    else if (coupon_to_check.coupon_end_date < current_datetime)
                    {
                        model = new { invalid_name = "coupon_out_of_date", invalid_description = "Mã giảm giá đã hết hạn sử dụng.", status = false };
                    }
                    else if (coupon_to_check.coupon_number_of_uses == coupon_to_check.count_used)
                    {
                        model = new { invalid_name = "coupon_out_of_use", invalid_description = "Mã giảm giá đã hết số lần sử dụng.", status = false };
                    }
                    else if (coupon_to_check.coupon_start_date > current_datetime)
                    {
                        model = new { invalid_name = "coupon_not_yet_acitve", invalid_description = "Mã giảm giá chưa đến ngày áp dụngg.", status = false };
                    }
                    else if (validationGroupCustomerInOrder(coupon_code, customer_id) == false)
                    {
                        model = new { invalid_name = "coupon_not_for_this_customer", invalid_description = "Mã giảm giá không áp dụng cho khách hàng này.", status = false };
                    }
                }
                else
                {
                    model = new { invalid_name = "coupon_not_exist", invalid_description = "Mã giảm giá không tồn tại. Vui lòng kiểm tra lại.", status = false };
                }
                return model;
            }
            catch
            {
                return null;
            }
        }

        public decimal getValuePromotionByCouponCode(string coupon_code, decimal total_cost_of_product_in_order,
            decimal cost_of_shipping_for_this_order, int province_id,
            List<long> list_product_variation_id)
        {
            try
            {
                decimal value_to_return = 0;
                var coupon = getByCouponCode(coupon_code);
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
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv_id).Contains((long)coupon.category_id))
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv_id).Contains((long)coupon.category_id))
                            {
                                value_to_return += (decimal)product_variation.product_variation_price;
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (category_provider.getCategoryIdByProductVariationId(pv_id).Contains((long)coupon.category_id))
                            {
                                value_to_return += (decimal)(product_variation.product_variation_price - coupon.discount_value);
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
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)(coupon.discount_value);
                            }
                        }
                    }
                    else if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation.product_id == coupon.product_id)
                            {
                                value_to_return += (decimal)(product_variation.product_variation_price - coupon.discount_value);
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
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv_id))
                            {
                                value_to_return += (decimal)((coupon.discount_value / 100) * product_variation.product_variation_price);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 2)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv_id))
                            {
                                value_to_return += (decimal)(coupon.discount_value);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 3)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                            if (product_variation_provider.getListProductVariationIdByCouponCode(coupon_code).Contains(pv_id))
                            {
                                value_to_return += (decimal)(product_variation.product_variation_price - coupon.discount_value);
                            }
                        }
                    }
                    if (coupon.discount_type_id == 1002 && coupon.province_id == province_id)
                    {
                        foreach (var pv_id in list_product_variation_id)
                        {
                            var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
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

        public bool updateNumOfUse(int coupon_id)
        {
            try
            {
                var coupon = db.coupons.FirstOrDefault(c => c.coupon_id == coupon_id);
                coupon.count_used++;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<coupon> getListCouponForProduct()
        {
            try
            {
                DateTime current_datetime = DateTime.Now;
                var list_coupon_for_product = db.coupons.Where(c => c.product_id != null && (c.coupon_start_date < current_datetime && (c.coupon_end_date >= current_datetime || c.coupon_end_date == null)) && c.coupon_status == true).ToList();
                return list_coupon_for_product;
            }
            catch
            {
                return null;
            }
        }
    }
}
