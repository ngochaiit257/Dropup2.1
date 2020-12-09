using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CategoryProductProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertCategoryProduct(category_product model)
        {
            try
            {
                if(!checkDupplicate(model.product_id, model.category_id))
                {
                    db.category_products.InsertOnSubmit(model);
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkDupplicate(long product_id, long category_id)
        {
            try
            {
                bool status = false;
                foreach (var obj in db.category_products.ToList())
                {
                    if (obj.product_id == product_id && obj.category_id == category_id)
                    {
                        status = true;
                    }
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteCategoryProduct(long product_id)
        {
            try
            {
                var list_to_delete = db.category_products.Where(cp => cp.product_id == product_id).ToList();
                db.category_products.DeleteAllOnSubmit(list_to_delete);
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
