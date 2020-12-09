using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreCustomerSayProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<store_customer_say> getAll()
        {
            try
            {
                return db.store_customer_says.ToList();
            }
            catch
            {
                return null;
            }
        }

        public store_customer_say getById(int cs_id)
        {
            try
            {
                return db.store_customer_says.FirstOrDefault(cs => cs.store_customer_say_id == cs_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertStoreCustomerSay(store_customer_say model)
        {
            try
            {
                db.store_customer_says.InsertOnSubmit(model);
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
