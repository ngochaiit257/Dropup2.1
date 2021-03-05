using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuynhBoki.Models
{
    public class ProductVariationModel
    {
        public string product_name { set; get; }
        public string product_variation_name { set; get; }
        public long product_id { set; get; }
        public int supplier_id { set; get; }
        public string suppiler_name { set; get; }
        public long product_variation_id { set; get; }
        public string product_variation_image { set; get; }
        public string product_variation_alt_image { set; get; }
        public int product_variation_in_stock { set; get; }
        public decimal product_variation_price { set; get; }
        public decimal product_variation_old_price { set; get; }
        public int discount_percent { set; get; }
        public string coupon_code { set; get; }
        public string unit_name { set; get; }
        public int quantity_in_cart { set; get; }
        public int num_of_product_variation { set; get; }
        public string product_seo_alias { set; get; }
        public bool allow_ordering_while_out_of_stock_status { set; get; }
        public bool warehouse_management_status { set; get; }
        public bool allow_delivery { set; get; }
    }
}