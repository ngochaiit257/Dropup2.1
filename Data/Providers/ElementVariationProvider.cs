using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ElementVariationProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public bool insertElementVariation(element_variation model)
        {
            try
            {
                if (!checkElementVariationExist((int)model.product_variation_id, (int)model.product_variation_property_id, model.element_variation_name))
                {
                    db.element_variations.InsertOnSubmit(model);
                    db.SubmitChanges();
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool checkElementVariationExist(long product_variation_id, int product_variation_property_id, string element_variation_name)
        {
            try
            {
                bool status = false;
                foreach (var obj in getByProductVariationId(product_variation_id))
                {
                    if (obj.product_variation_property_id == product_variation_property_id && obj.element_variation_name.ToLower() == element_variation_name.ToLower())
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

        public element_variation getByProductVariationIdAndPropertyId(long pv_id, int prop_id)
        {
            try
            {
                return db.element_variations.FirstOrDefault(ev => ev.product_variation_id == pv_id && ev.product_variation_property_id == prop_id);
            }
            catch
            {
                return null;
            }
        }


        public List<element_variation> getByProductVariationId(long pv_id)
        {
            try
            {
                var list_return = db.element_variations.Where(ev => ev.product_variation_id == pv_id).OrderBy(ev => ev.product_variation_property_id).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

       
        public List<long> getListElementVariationIdByProductVariationId(long pv_id)
        {
            try
            {
                return db.element_variations.Where(ev => ev.product_variation_id == pv_id).Select(ev => ev.element_variation_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool updateElementVariationName(long ev_id, string ev_name)
        {
            try
            {
                var ev_model = db.element_variations.FirstOrDefault(ev => ev.element_variation_id == ev_id);
                ev_model.element_variation_name = ev_name;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteElementVariationByPVId(long pv_id)
        {
            try
            {
                var list_to_delete = getByProductVariationId(pv_id);
                db.element_variations.DeleteAllOnSubmit(list_to_delete);
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
