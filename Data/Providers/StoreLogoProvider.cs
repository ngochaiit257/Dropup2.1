using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreLogoProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public store_logo getFavicon()
        {
            try
            {
                return db.store_logos.FirstOrDefault(f => f.type_logo_id == 1);
            }
            catch
            {
                return null;
            }
        }

        public store_logo getLogoHeader()
        {
            try
            {
                return db.store_logos.FirstOrDefault(f => f.type_logo_id == 2);
            }
            catch
            {
                return null;
            }
        }

        public store_logo getLogoFooter()
        {
            try
            {
                return db.store_logos.FirstOrDefault(f => f.type_logo_id == 3);
            }
            catch
            {
                return null;
            }
        }

        public store_logo getLogoMobile()
        {
            try
            {
                return db.store_logos.FirstOrDefault(f => f.type_logo_id == 4);
            }
            catch
            {
                return null;
            }
        }

        public store_logo getById(int logo_id)
        {
            try
            {
                return db.store_logos.FirstOrDefault(l => l.store_logo_id == logo_id);
            }
            catch
            {
                return null;
            }
        }

        public bool updateLogoStore(int logo_id, string image_url)
        {
            try
            {
                var logo = getById(logo_id);
                if(logo.image != image_url)
                {
                    logo.image = image_url;
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
