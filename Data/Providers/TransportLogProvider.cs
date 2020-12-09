using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TransportLogProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<transport_log> getByTransportId(long transport_id)
        {
            try
            {
                return db.transport_logs.Where(tl => tl.transport_id == transport_id).OrderByDescending(ol => ol.transport_log_create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertTransportLog(transport_log model)
        {
            try
            {
                db.transport_logs.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteListTranportLogInOrder(List<transport_log> list_tl)
        {
            try
            {
                db.transport_logs.DeleteAllOnSubmit(list_tl);
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public List<transport_log> getByDate(DateTime date_to_get, long transport_id)
        {
            try
            {
                return db.transport_logs.Where(ol => ol.transport_id == transport_id && ol.transport_log_create_datetime.Date == date_to_get.Date).OrderByDescending(ol => ol.transport_log_create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<DateTime> getDateTimeOfOrderLogByTransportId(long transport_id)
        {
            try
            {
                var list_return = new List<DateTime>();
                foreach (var transport in getByTransportId(transport_id))
                {
                    if (!list_return.Contains((DateTime)transport.transport_log_create_datetime.Date))
                    {
                        list_return.Add((DateTime)transport.transport_log_create_datetime.Date);
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public bool deleteTransportLogByTransportId(long transport_id)
        {
            try
            {
                var list_to_delete = getByTransportId(transport_id);
                db.transport_logs.DeleteAllOnSubmit(list_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
