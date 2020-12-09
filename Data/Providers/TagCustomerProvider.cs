using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TagCustomerProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertTagCustomer(long customer_id, long tag_id)
        {
            try
            {
                tag_customer model = new tag_customer();
                model.customer_id = customer_id;
                model.tag_id = tag_id;
                db.tag_customers.InsertOnSubmit(model);
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
