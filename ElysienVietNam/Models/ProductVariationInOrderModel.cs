using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElysienVietNam.Models
{
    public class ProductVariationInOrderModel
    {
        public long product_variation_id { set; get; }
        public int product_variation_quantity { set; get; }
        public string product_variation_name { set; get; }
        public decimal product_variation_price { set; get; }
        public string unit_name { set; get; }
        public string product_name { set; get; }
        public string product_variation_image { set; get; }
    }
}