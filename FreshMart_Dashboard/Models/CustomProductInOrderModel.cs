using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CustomProductInOrderModel
    {
        public int custom_product_in_order_id { set; get; }
        public string custom_product_in_order_name { set; get; }
        public decimal custom_product_in_order_price { set; get; }
        public int custom_product_in_order_quantity { set; get; }
        public string unit_name { set; get; }
    }
}