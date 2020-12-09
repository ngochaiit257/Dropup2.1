using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CommentProductProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<comment_product> getAll()
        {
            try
            {
                return db.comment_products.OrderByDescending(cp => cp.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<comment_product> getNotSpam()
        {
            try
            {
                return db.comment_products.Where(cp => cp.is_spam == false).OrderByDescending(cp => cp.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<comment_product> getNotSpamByProductId(long product_id)
        {
            try
            {
                return db.comment_products.Where(cp => cp.product_id == product_id && cp.is_spam == false).OrderByDescending(cp => cp.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertCommentProduct(comment_product model)
        {
            try
            {
                db.comment_products.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteCommentProductByProductId(long product_id)
        {
            try
            {
                var list_to_delete = db.comment_products.Where(c => c.product_id == product_id).ToList();
                db.comment_products.DeleteAllOnSubmit(list_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<comment_product> getCommentParent(long product_id)
        {
            try
            {
                return db.comment_products.Where(cp => cp.product_id == product_id && cp.comment_parent == null && cp.is_spam == false).OrderBy(cp => cp.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<comment_product> getByCommentParentId(long comment_parent_id)
        {
            try
            {
                return db.comment_products.Where(cp => cp.comment_parent == comment_parent_id && cp.is_spam == false).OrderBy(cp => cp.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
