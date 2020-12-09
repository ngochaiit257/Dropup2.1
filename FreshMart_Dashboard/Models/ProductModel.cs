using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class ProductModel
    {
        public long product_id { set; get; }
        public  string product_name { set; get; }
        public string product_image { set; get; }
    }
}