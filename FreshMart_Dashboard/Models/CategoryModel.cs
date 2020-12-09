using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CategoryModel
    {
        public long category_id { set; get; }
        public string category_name { set; get; }
        public string description { set; get; }
        public string image { set; get; }
    }
}