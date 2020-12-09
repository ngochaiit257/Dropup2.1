using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class RevenuePerDayModel
    {
        public DateTime create_datetime { set; get; }
        public int count_order { set; get; }
        public decimal revenue { set; get; }
        public decimal net_revenue { set; get; }
        public decimal discount { set; get; }
        public decimal shipping_fee { set; get; }
        public decimal total_cost_order { set; get; }
        public decimal amount_received { set; get; }
        public decimal amount_repay { set; get; }
        public int count_product { set; get; }
        public int count_product_repay { set; get; }
    }
}
