using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TopProductVariationSaleModel
    {
        public long product_id { set; get; }
        public long product_variation_id { set; get; }
        public string show_name { set; get; }
        public decimal price { set; get; }
        public int num_of_sold { set; get; }
        public int total_num { set; get; }
        public bool sell_status { set; get; }
        public string img { set; get; }
        public string img_alt { set; get; }

    }
}
