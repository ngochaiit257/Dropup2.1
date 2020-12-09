using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class NewProductInOrderModel
    {
        public string new_product_in_order_name { set; get; }

        public decimal new_product_in_order_price { set; get; }

        public int new_product_in_order_quantity { set; get; }
    }
}