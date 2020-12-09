using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TagProductProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertTagProduct(long product_id, long tag_id)
        {
            try
            {
                tag_product model = new tag_product();
                model.product_id = product_id;
                model.tag_id = tag_id;
                db.tag_products.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<tag_product> getTagByProductId(long product_id)
        {
            try
            {
                return db.tag_products.Where(tc => tc.product_id == product_id).OrderBy(tc => tc.product.product_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteTagProductByProductId(long product_id)
        {
            try
            {
                var list_to_delete = getTagByProductId(product_id);
                foreach (var model in list_to_delete)
                {
                    db.tag_products.DeleteOnSubmit(model);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteTagProductByProductIdAndTagId(long tag_id, long product_id)
        {
            try
            {
                var tag_to_delete = db.tag_products.FirstOrDefault(t => t.tag_id == tag_id && t.product_id == product_id);
                db.tag_products.DeleteOnSubmit(tag_to_delete);
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
