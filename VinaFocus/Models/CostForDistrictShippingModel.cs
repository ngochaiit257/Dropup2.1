using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinaFocus.Models
{
    public class CostForDistrictShippingModel
    {
        public int cost_for_district_shipping_id { set; get; }
        public int district_id { set; get; }
        public int cost_of_local_shipping_id { set; get; }
        public decimal price_for_district_shipping_final { set; get; }
        public decimal price_for_district_shipping_adjust { set; get; }
        public bool status_district_shipping { set; get; }
        public string district_name { set; get; }
        public string province_name { set; get; }
        public string name_of_cost { set; get; }
    }
}