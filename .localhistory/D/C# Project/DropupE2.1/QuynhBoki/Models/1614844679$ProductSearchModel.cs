using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElysienVietNam.Models
{
    public class ProductSearchModel
    {
        public long id { set; get; }
        public string seo_url { set; get; }
        public string name { set; get; }
        public decimal promotion_price { set; get; }
        public decimal price { set; get; }
        public string img { set; get; }
        public string img_alt { set; get; }
    }
}