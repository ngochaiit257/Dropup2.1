using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ShippingStatusProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<shipping_status> getAll()
        {
            try
            {
                return db.shipping_status.OrderBy(s => s.shipping_status_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public string getNameShippingStatus(int shipping_status_id)
        {
            try
            {
                return db.shipping_status.FirstOrDefault(ss => ss.shipping_status_id == shipping_status_id).shipping_status_description;
            }
            catch
            {
                return "";
            }
        }
    }
}
