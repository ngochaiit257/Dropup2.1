using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class StoreSlideProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<store_slide> getAll()
        {
            try
            {
                return db.store_slides.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<store_slide> getIsShowing()
        {
            try
            {
                return db.store_slides.Where(ss => ss.is_showing == true).ToList();
            }
            catch
            {
                return null;
            }
        }

        public store_slide getById(int slide_id)
        {
            try
            {
                return db.store_slides.FirstOrDefault(s => s.store_slide_id == slide_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertStoreSlide(store_slide model)
        {
            try
            {
                db.store_slides.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateStoreSlide(store_slide model)
        {
            try
            {
                var old_model = getById(model.store_slide_id);
                old_model.image = model.image;
                old_model.alt = model.alt;
                old_model.main_text = model.main_text;
                old_model.child_text_1 = model.child_text_1;
                old_model.child_text_2 = model.child_text_2;
                old_model.link_url = model.link_url;
                old_model.is_showing = model.is_showing;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteSlide(int slide_id)
        {
            try
            {
                db.store_slides.DeleteOnSubmit(getById(slide_id));
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
