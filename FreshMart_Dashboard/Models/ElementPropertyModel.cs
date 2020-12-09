using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class ElementPropertyModel
    {
        public int property_id { set; get; }
        public string property_name { set; get; }
        public List<string> list_tag_value { set; get; }
    }
}