using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ProductElementProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<product_element> getAll()
        {
            try
            {
                return db.product_elements.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<product_element> getByProductIdAnfPropertyId(long product_id, int property_id)
        {
            try
            {
                return db.product_elements.Where(pe => pe.product_id == product_id && pe.property_id == property_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertProductElement(product_element model)
        {
            try
            {
                if (!checkPEDuplicate(model))
                {
                    db.product_elements.InsertOnSubmit(model);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkPEDuplicate(product_element model)
        {
            try
            {
                bool status = false;
                if (getAll().Count() > 0)
                {
                    foreach (var pe in getAll())
                    {
                        if (pe.product_id == model.product_id && pe.property_id == model.property_id && pe.element_value == model.element_value)
                        {
                            status = true;
                        }
                    }
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public List<product_element> getByProductId(long product_id)
        {
            try
            {
                return db.product_elements.Where(pe => pe.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool DeletePEByProductId(long product_id)
        {
            try
            {
                var list_pe = getByProductId(product_id);
                db.product_elements.DeleteAllOnSubmit(list_pe);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
