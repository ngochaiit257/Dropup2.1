using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class ConditionOfProductModel
    {
        public string condition_name { set; get; }

        public string condition_filter { set; get; }

        public string condition_product_name_contain_value { set; get; }

        public decimal condition_product_price_value { set; get; }
    }
}