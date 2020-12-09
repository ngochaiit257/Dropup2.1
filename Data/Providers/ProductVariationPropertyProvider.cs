using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ProductVariationPropertyProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        ElementVariationProvider ev_prodvider = new ElementVariationProvider();

        public List<product_variation_property> getAll()
        {
            try
            {
                return db.product_variation_properties.OrderBy(pvp => pvp.product_variation_property_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public product_variation_property getById(int pvp_id)
        {
            try
            {
                return db.product_variation_properties.FirstOrDefault(pvp => pvp.product_variation_property_id == pvp_id);
            }
            catch
            {
                return null;
            }
        }

        public product_variation_property getByName(string name)
        {
            try
            {
                return db.product_variation_properties.FirstOrDefault(pvp => pvp.product_variation_property_name.ToLower() == name.ToLower());
            }
            catch
            {
                return null;
            }
        }

        public bool insertProductVariationProperty(product_variation_property model, ref int id)
        {
            try
            {
                if (!checkDuplicate(model.product_variation_property_name))
                {
                    id = getByName(model.product_variation_property_name).product_variation_property_id;
                }
                else
                {
                    db.product_variation_properties.InsertOnSubmit(model);
                    db.SubmitChanges();
                    id = model.product_variation_property_id;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkDuplicate(string product_variation_property_name)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.product_variation_property_name.ToLower() == product_variation_property_name.ToLower())
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

        public List<product_variation_property> getByProductId(long product_id)
        {
            try
            {
                var list_return = new List<product_variation_property>();
                List<int> list_int = new List<int>();
                var list_pv = pv_provider.getByProductId(product_id);
                foreach(var pv in list_pv)
                {
                    var list_ev = ev_prodvider.getByProductVariationId(pv.product_variation_id);
                    if(list_ev.Count() > 0)
                    {
                        foreach (var ev in list_ev)
                        {
                            if (!list_int.Contains((int)ev.product_variation_property_id))
                            {
                                list_int.Add((int)ev.product_variation_property_id);
                            }
                        }
                    }
                }
                if(list_int.Count() > 0)
                {
                    foreach(var id in list_int)
                    {
                        list_return.Add(getById(id));
                    }
                }
                return list_return.OrderBy(pvp => pvp.product_variation_property_id).ToList();
            }
            catch
            {
                return null;
            }
        }

      
    }
}
