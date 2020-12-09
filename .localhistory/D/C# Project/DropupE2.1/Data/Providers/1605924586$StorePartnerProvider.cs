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
                old_model.cname = model.cname;
                old_model.cposition = model.cposition;
                old_model.ccomment = model.ccomment;
                old_model.cimage = model.cimage;
                old_model.cimage_alt = model.cimage_alt;
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
