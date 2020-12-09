using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CouponValidationModel
    {
        public string invalid_name { set; get; }
        public string invalid_description { set; get; }
        public bool status { set; get; }
    }
}
