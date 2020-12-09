using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class ElementVariationModel
    {
        public long element_variation_id { set; get; }
        public long product_variation_id { set; get; }
        public int product_variation_property_id { set; get; }
        public string element_variation_name { set; get; }
    }
}