using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreMessageProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<store_message> getAll()
        {
            try
            {
                return db.store_messages.OrderByDescending(sm => sm.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public int getListNotActive()
        {
            try
            {
                return db.store_messages.OrderByDescending(sm => sm.create_datetime).ToList().Count();
            }
            catch
            {
                return -1;
            }
        }

        public bool insertStoreMessage(store_message model)
        {
            try
            {
                db.store_messages.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateReadingStatus(long mess_id)
        {
            try
            {
                var mess = db.store_messages.FirstOrDefault(m => m.store_message_id == mess_id);
                mess.reading_status = true;
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
