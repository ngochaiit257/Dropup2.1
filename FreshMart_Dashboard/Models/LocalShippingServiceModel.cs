using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class LocalShippingServiceModel
    {
        public int local_shipping_service_id { set; get; }
        public int province_id { set; get; }
        public string province_name { set; get; }
    }
}