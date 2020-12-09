using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CouponModel
    {
        public int coupon_id { set; get; }
        public string coupon_code { set; get; }
        public bool allow_use_with_promotion_status { set; get; }
        public int coupon_number_of_uses { set; get; }
        public int discount_type_id { set; get; }
        public decimal discount_value { set; get; }
        public int condition_for_coupon_id { set; get; }
        public decimal coupon_order_cost_from { set; get; }
        public long category_id { set; get; }
        public long product_id { set; get; }
        public long customer_id { set; get; }
        public int province_id { set; get; }
        public int object_of_coupon_id { set; get; }
        public DateTime coupon_start_date { set; get; }
        public DateTime coupon_end_date { set; get; }
        public string coupon_description { set; get; }
        public bool coupon_status { set; get; }

    }
}