using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TagBlogProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertTagBlog(long blog_id, long tag_id)
        {
            try
            {
                tag_blog model = new tag_blog();
                model.blog_id = blog_id;
                model.tag_id = tag_id;
                db.tag_blogs.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTagCategoryByBlogIdAndTagId(long tag_id, long blog_id)
        {
            try
            {
                var tag_to_delete = db.tag_blogs.FirstOrDefault(t => t.tag_id == tag_id && t.blog_id == blog_id);
                db.tag_blogs.DeleteOnSubmit(tag_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTagBlogByBlogId(long blog_id)
        {
            try
            {
                var list = db.tag_blogs.Where(tb => tb.blog_id == blog_id).ToList();
                db.tag_blogs.DeleteAllOnSubmit(list);
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
