using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CostOfLocalShippingModel
    {
        public int cost_of_local_shipping_id { set; get; }
        public int local_shipping_service_id { set; get; }
        public string name_of_cost { set; get; }
        public int standard_for_shipping_id { set; get; }
        public decimal min_value_for_standard { set; get; }
        public decimal max_value_for_standard { set; get; }
        public decimal price_of_cost { set; get; }
        public string standard_name { set; get; }
    }
}