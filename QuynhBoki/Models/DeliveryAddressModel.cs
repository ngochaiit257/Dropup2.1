using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuynhBoki.Models
{
    public class DeliveryAddressModel
    {
        public int delivery_address_id { set; get; }
        public string first_name { set; get; }
        public string last_name { set; get; }
        public int country_id { set; get; }
        public int province_id { set; get; }
        public int district_id { set; get; }
        public int ward_id { set; get; }
        public string address_detail { set; get; }
        public string phone_number { set; get; }
        public string address_zip { set; get; }
        public long customer_id { set; get; }
        public string province_name { set; get; }
        public string district_name { set; get; }
        public string ward_name { set; get; }
        public string customer_note { set; get; }
        public string customer_email { set; get; }
        public decimal total_cost_product { set; get; }
        public long order_id { set; get; }
    }
}