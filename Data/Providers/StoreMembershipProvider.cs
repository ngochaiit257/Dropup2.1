using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreMembershipProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<store_membership> getAll()
        {
            try
            {
                return db.store_memberships.ToList();
            }
            catch
            {
                return null;
            }
        }

        public store_membership getById(int mem_id)
        {
            try
            {
                return db.store_memberships.FirstOrDefault(m => m.membership_id == mem_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertMS(store_membership model)
        {
            try
            {
                db.store_memberships.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateMS(store_membership model)
        {
            try
            {
                var old_model = getById(model.membership_id);
                old_model.mname = model.mname;
                old_model.mposition = model.mposition;
                old_model.msummary = model.msummary;
                old_model.mfacebook_url = model.mfacebook_url;
                old_model.mzalo_url = model.mzalo_url;
                old_model.myoutube_url = model.myoutube_url;
                old_model.mtwitter_url = model.mtwitter_url;
                old_model.minstagram_url = model.minstagram_url;
                old_model.mavatar = model.mavatar;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteMS(int mem_id)
        {
            try
            {
                db.store_memberships.DeleteOnSubmit(getById(mem_id));
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
