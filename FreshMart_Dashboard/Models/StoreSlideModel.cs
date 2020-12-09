using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class StoreSlideModel
    {
        public int store_slide_id { set; get; }
        public string link_url { set; get; }
        public string alt { set; get; }
        public string main_text { set; get; }
        public string child_text_1 { set; get; }
        public string child_text_2 { set; get; }
        public bool is_showing { set; get; }
        public HttpPostedFileBase image { set; get; }
    }
}