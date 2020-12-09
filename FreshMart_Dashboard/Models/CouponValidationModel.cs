using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class CouponValidationModel
    {
        public string invalid_name { set; get; }
        public string invalid_description { set; get; }
        public bool status { set; get; }
    }
}