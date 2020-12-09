using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreContactProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public store_contact get1Contact()
        {
            try
            {
                return db.store_contacts.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public bool updateStoreContact(store_contact model)
        {
            try
            {
                var old_model = db.store_contacts.FirstOrDefault(sc => sc.store_contact_id == model.store_contact_id);
                old_model.address = model.address;
                old_model.email = model.email;
                old_model.working_day = model.working_day;
                old_model.working_hour = model.working_hour;
                old_model.phone_number = model.phone_number;
                old_model.map = model.map;
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
