using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class PaymentInformationProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public payment_information getByOrderLogId(int order_log_id)
        {
            try
            {
                return db.payment_informations.FirstOrDefault(pi => pi.order_log_id == order_log_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertPaymentInformation(payment_information model)
        {
            try
            {
                db.payment_informations.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deletePaymentInfomation(payment_information model)
        {
            try
            {
                db.payment_informations.DeleteOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deletePaymentInfomationByOrderLogId(int order_log_id)
        {
            try
            {
                var list_to_delete = getByOrderLogId(order_log_id);
                db.payment_informations.DeleteOnSubmit(list_to_delete);
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
