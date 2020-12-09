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

        public store_partner getById(int partner_id)
        {
            try
            {
                return db.store_partners.FirstOrDefault(p => p.store_partner_id == partner_id);
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

        public bool updateP(store_partner model)
        {
            try
            {
                var old_model = getById(model.store_partner_id);
                old_model.name = model.name;
                old_model.link_url = model.link_url;
                old_model.alt = model.alt;
                old_model.image = model.image;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteCS(int cs_id)
        {
            try
            {
                db.store_customer_says.DeleteOnSubmit(getById(cs_id));
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
