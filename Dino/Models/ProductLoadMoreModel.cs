using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dino.Models
{
    public class ProductLoadMoreModel
    {
        public string product_name { set; get; }
        public string product_img { set; get; }
        public string product_img_alt { set; get; }
        public string product_img_hover { set; get; }
        public string product_img_hover_alt { set; get; }
        public string seo_alias { set; get; }
        public decimal price { set; get; }
        public decimal promotion_price { set; get; }
        public DateTime show_datetime { set; get; }
    }
}