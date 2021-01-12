using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dino.Models
{
    public class ElementVariationModel
    {
        public long product_variation_id { set; get; }
        public int prop_id { set; get; }
        public string ev_name { set; get; }
        public string pv_image_url { set; get; }
        public decimal pv_price { set; get; }
        public decimal pv_price_comparison { set; get; }
        public int pv_promotion_percent { set; get; }
        public int pv_in_stock { set; get; }
        public bool allow_ordering_while_out_of_stock_status { set; get; }
        public bool allow_delivery { set; get; }
    }
}