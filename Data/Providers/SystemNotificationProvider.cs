using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class SystemNotificationProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<system_notification> getAll()
        {
            try
            {
                return db.system_notifications.OrderByDescending(sn => sn.system_notification_create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertSystemNotification(system_notification model)
        {
            try
            {
                db.system_notifications.InsertOnSubmit(model);
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
