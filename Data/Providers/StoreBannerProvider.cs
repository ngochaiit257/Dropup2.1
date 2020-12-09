using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreBannerProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public store_banner getbyId(int banner_id)
        {
            try
            {
                return db.store_banners.FirstOrDefault(b => b.store_banner_id == banner_id);
            }
            catch
            {
                return null;
            }
        }

        public bool updateStoreBanner(store_banner model)
        {
            try
            {
                var old_model = getbyId(model.store_banner_id);
                old_model.alt = model.alt;
                old_model.main_text = model.main_text;
                old_model.child_text = model.child_text;
                old_model.image = model.image;
                old_model.video_url = model.video_url;
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
