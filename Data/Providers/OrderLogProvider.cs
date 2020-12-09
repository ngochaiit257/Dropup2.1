using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class OrderLogProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<order_log> getByOrderId(long order_id)
        {
            try
            {
                return db.order_logs.Where(ol => ol.order_id == order_id).OrderByDescending(ol => ol.order_log_create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteListOrderLogInOrder(List<order_log> list_ol)
        {
            try
            {
                db.order_logs.DeleteAllOnSubmit(list_ol);
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool insertOrderLog(order_log model, ref int order_log_id)
        {
            try
            {
                db.order_logs.InsertOnSubmit(model);
                db.SubmitChanges();
                order_log_id = model.order_log_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<order_log> getByDate(DateTime date_to_get, long order_id)
        {
            try
            {
                return db.order_logs.Where(ol => ol.order_id == order_id && ol.order_log_create_datetime.Value.Date == date_to_get.Date).OrderByDescending(ol => ol.order_log_create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<DateTime> getDateTimeOfOrderLogByOrderId(long order_id)
        {
            try
            {
                var list_return = new List<DateTime>();
                foreach(var order in getByOrderId(order_id))
                {
                    if (!list_return.Contains((DateTime)order.order_log_create_datetime.Value.Date))
                    {
                        list_return.Add((DateTime)order.order_log_create_datetime.Value.Date);
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
    }
}
