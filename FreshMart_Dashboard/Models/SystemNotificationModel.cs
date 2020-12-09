using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DropupManagement.Models
{
    public class SystemNotificationModel
    {
        public long id { set; get; }
        public string title { set; get; }
        public string obj_url { set; get; }
        public DateTime create_datetime { set; get; }
    }
}