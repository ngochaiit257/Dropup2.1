using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class DraftOrderModel
    {
        public long order_id { set; get; }
        public long customer_id { set; get; }
        public string order_note { set; get; }
        public string source_order { set; get; }
        public int delivery_address_id { set; get; }
        public decimal discount_amount { set; get; }
        public string reason_of_promotion { set; get; }
        public string name_of_shipping { set; get; }
        public decimal cost_of_shipping { set; get; }
    }
}