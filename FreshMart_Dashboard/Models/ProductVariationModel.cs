using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class ProductVariationModel
    {
        public long product_variation_id { set; get; }
        public string product_variation_name { set; get; }
        public string product_variation_code { set; get; }
        public string product_variation_barcode { set; get; }
        public float product_variation_weight { set; get; }
        public decimal product_variation_price { set; get; }
        public decimal product_variation_rent_cost { set; get; }
        public HttpPostedFileBase product_variation_image { set; get; }
        public string product_variation_image_alt { set; get; }
        public bool allow_ordering_while_out_of_stock_status { set; get; }
        public bool allow_delivery { set; get; }
        public bool warehouse_management_status { set; get; }
        public decimal product_variation_price_comparison { set; get; }
        public int product_variation_in_stock { set; get; }
        public bool is_change_image { set; get; }
        public List<long> get_list_ev { set; get; }

    }
}