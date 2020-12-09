using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CustomerModel
    {
        public string customer_first_name { set; get; }
        public string customer_last_name { set; get; }
        public string customer_email { set; get; }
        public int country_id { set; get; }
        public int province_id { set; get; }
        public int district_id { set; get; }
        public int ward_id { set; get; }
        public string address_detail { set; get; }
        public string customer_phone_number { set; get; }
        public string customer_note { set; get; }
        public string customer_birthday { set; get; }
        public int gender_id { set; get; }
        public int black_list_status { set; get; }
        public long user_id { set; get; }
    }
}