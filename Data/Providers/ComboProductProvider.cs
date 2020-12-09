using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ComboProductProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertComboProduct(combo_product model)
        {
            try
            {
                db.combo_products.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertListComboProduct(List<long> list_cp_id, long product_id)
        {
            try
            {
                if(list_cp_id.Count() > 0)
                {
                    foreach(var cp_id in list_cp_id)
                    {
                        combo_product model = new combo_product();
                        model.product_id = product_id;
                        model.product_variation_id = cp_id;
                        insertComboProduct(model);
                        db.SubmitChanges();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<combo_product> getByProductId(long product_id)
        {
            try
            {
                return db.combo_products.Where(cp => cp.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteComboByProductIdAndPvId(long product_id, long pv_id)
        {
            try
            {
                var model_to_delete = db.combo_products.FirstOrDefault(cp => cp.product_id == product_id && cp.product_variation_id == pv_id);
                db.combo_products.DeleteOnSubmit(model_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteComboByProductIdAndListPvId(long product_id, List<long> list_pv_id)
        {
            try
            {
                if(list_pv_id != null)
                {
                    foreach(var pv_id in list_pv_id)
                    {
                        deleteComboByProductIdAndPvId(product_id, pv_id);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateComboProduct(long product_id, List<long> list_pv_id)
        {
            try
            {
                var old_list = getByProductId(product_id);
                if(old_list.Count > 0)
                {
                    db.combo_products.DeleteAllOnSubmit(old_list);
                    db.SubmitChanges();
                }
                if (list_pv_id != null)
                {
                    foreach (var pv_id in list_pv_id)
                    {
                        combo_product model = new combo_product();
                        model.product_id = product_id;
                        model.product_variation_id = pv_id;
                        insertComboProduct(model);
                    }
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
