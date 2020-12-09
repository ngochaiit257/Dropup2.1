using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StorePartnerProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<store_partner> getAll()
        {
            try
            {
                return db.store_partners.ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertStorePartner(store_partner model)
        {
            try
            {
                db.store_partners.InsertOnSubmit(model);
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
