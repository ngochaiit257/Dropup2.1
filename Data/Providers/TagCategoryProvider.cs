using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TagCategoryProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertTagCategory(long category_id, long tag_id)
        {
            try
            {
                tag_category model = new tag_category();
                model.category_id = category_id;
                model.tag_id = tag_id;
                db.tag_categories.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

       

        public List<tag_category> getTagByCategoryId(long category_id)
        {
            try
            {
                return db.tag_categories.Where(tc => tc.category_id == category_id).OrderBy(tc => tc.category.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteTagCategoryByCategoryId(long category_id)
        {
            try
            {
                var list_to_delete = getTagByCategoryId(category_id);
                foreach(var model in list_to_delete)
                {
                    db.tag_categories.DeleteOnSubmit(model);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTagCategoryByCategoryIdAndTagId(long tag_id, long category_id)
        {
            try
            {
                var tag_to_delete = db.tag_categories.FirstOrDefault(t => t.tag_id == tag_id && t.category_id == category_id);
                db.tag_categories.DeleteOnSubmit(tag_to_delete);
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
