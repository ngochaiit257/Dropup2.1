using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TransportProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public transport getById(long transport_id)
        {
            try
            {
                return db.transports.FirstOrDefault(t => t.transport_id == transport_id);
            }
            catch
            {
                return null;
            }
        }
        public transport getByOrderId(long order_id)
        {
            try
            {
                return db.transports.FirstOrDefault(t => t.order_id == order_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertTransport(transport model, ref long transport_id)
        {
            try
            {
                db.transports.InsertOnSubmit(model);
                db.SubmitChanges();
                transport_id = model.transport_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTransport(transport model)
        {
            try
            {
                db.transports.DeleteOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateUserDatetimeUpdate(long transport_id, long user_id, DateTime update_datetime)
        {
            try
            {
                var transport = getById(transport_id);
                transport.user_id_update = user_id;
                transport.update_datetime = update_datetime;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateShippingStatus(long transport_id, int shipping_status_id)
        {
            try
            {
                var model = getById(transport_id);
                model.shipping_status_id = shipping_status_id;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateGetMoneyStatus(long transport_id)
        {
            try
            {
                var model = getById(transport_id);
                model.get_money_status_id = !model.get_money_status_id;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool updateCompareWaybillCodeStatus(long transport_id)
        {
            try
            {
                var model = getById(transport_id);
                model.compare_waybill_code_status = !model.compare_waybill_code_status;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateShippingFee(long transport_id, decimal shipping_fee)
        {
            try
            {
                var transport = getById(transport_id);
                var order = db.orders.FirstOrDefault(o => o.order_id == transport.order_id);
                order.cost_of_shipping = shipping_fee;
                transport.shipping_fee = shipping_fee;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateWaybillCode(long transport_id, string waybill_code)
        {
            try
            {
                var transport = getById(transport_id);
                transport.waybill_code = waybill_code;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<transport> getByShippingStatus(int shipping_status_id)
        {
            try
            {
                return db.transports.Where(t => t.shipping_status_id == shipping_status_id).ToList();
            }
            catch
            {
                return null;
            }
        }


    }
}
