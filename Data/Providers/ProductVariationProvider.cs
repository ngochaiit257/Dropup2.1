using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ProductVariationProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        ProductProvider product_provider = new ProductProvider();

        public List<product_variation> getAll()
        {
            try
            {
                return db.product_variations.OrderBy(pv => pv.product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public product_variation getById(long product_variation_id)
        {
            try
            {
                return db.product_variations.FirstOrDefault(pv => pv.product_variation_id == product_variation_id);
            }
            catch
            {
                return null;
            }
        }
        public bool insertVariationForProduct(product_variation model)
        {
            try
            {
                db.product_variations.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool insertVariationForProductRefId(product_variation model, ref long pv_id)
        {
            try
            {
                db.product_variations.InsertOnSubmit(model);
                db.SubmitChanges();
                pv_id = model.product_variation_id;
                return true;
            }
            catch
            {
                return false;
            }

        }


        public List<product_variation> getByProductId(long product_id)
        {
            try
            {
                return db.product_variations.Where(pv => pv.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public int getSumNumberByProductId(long product_id)
        {
            try
            {
                return (int)getByProductId(product_id).Sum(pv => pv.product_variation_in_stock);
            }
            catch
            {
                return 0;
            }
        }

        public List<product_variation> getByListPvId(List<long> list_pv_id)
        {
            try
            {
                var list_return = new List<product_variation>();
                foreach(var pv_id in list_pv_id)
                {
                    list_return.Add(getById(pv_id));
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public product_variation get1ByProductId(long product_id)
        {
            try
            {
                return db.product_variations.FirstOrDefault(pv => pv.product_id == product_id);
            }
            catch
            {
                return null;
            }
        }

        public List<long> getListProductVariationIdByCouponCode(string coupon_code)
        {
            try
            {
                var list_return = new List<long>();
                var list_product_variation_coupon = db.product_variation_coupons.Where(pvc => pvc.coupon.coupon_code == coupon_code).ToList();
                foreach (var obj in list_product_variation_coupon)
                {
                    foreach (var pv in getAll())
                    {
                        if (obj.product_variation_id == pv.product_variation_id)
                        {
                            list_return.Add(pv.product_variation_id);
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public bool checkListProductVariationCouponAndListProductVariationOrder(List<long> list_product_variation_id_coupon, List<long> list_product_variation_id_order)
        {
            try
            {
                bool status = false;
                foreach (var pvc in list_product_variation_id_coupon)
                {
                    foreach (var pvo in list_product_variation_id_order)
                    {
                        if (pvc == pvo)
                        {
                            status = true;
                        }
                    }
                }

                return status;
            }
            catch
            {
                return false;
            }
        }

        //public string getProductVariationProperty(long product_id)
        //{
        //    try
        //    {
        //        string string_return = "";
        //        List<product_variation> product_variation_by_product_id = db.product_variations.Where(pv => pv.product_id == product_id).ToList();
        //        foreach (var item in product_variation_by_product_id)
        //        {
        //            string_return = item.product_variation_property.product_variation_property_name;
        //        }
        //        return string_return;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public bool updateInStock(product_variation model, int quantity_to_get)
        {
            try
            {
                if (model.product_variation_in_stock != null)
                {
                    var product = db.products.FirstOrDefault(p => p.product_id == model.product_id);
                    product.view_count -= quantity_to_get;
                    model.product_variation_in_stock -= quantity_to_get;
                    db.SubmitChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool getNumInStock(long product_variation_id, ref int num_in_stock)
        {
            try
            {
                bool status = true;
                var pv_model = getById(product_variation_id);
                if (pv_model.product_variation_in_stock != null && pv_model.product_variation_in_stock == 0 && pv_model.product.allow_ordering_while_out_of_stock_status == false)
                {
                    status = false;
                }
                if (pv_model.product_variation_in_stock != null)
                {
                    num_in_stock = (int)pv_model.product_variation_in_stock;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool checkInStock(long product_variation_id, int num_input, ref int num_in_stock)
        {
            try
            {
                bool status = true;
                var pv_model = getById(product_variation_id);
                if (pv_model.product_variation_in_stock != null && pv_model.product_variation_in_stock < num_input && pv_model.product.allow_ordering_while_out_of_stock_status == false)
                {
                    status = false;
                }
                if (pv_model.product_variation_in_stock != null)
                {
                    num_in_stock = (int)pv_model.product_variation_in_stock;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool checkInStockForProduct(long product_id)
        {
            try
            {
                bool status = false;
                int num_in_stock = 0;
                var list_pv = getByProductId(product_id);
                foreach(var pv in list_pv)
                {
                    if(checkInStock(pv.product_variation_id, 1, ref num_in_stock) == true)
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

        public bool checkInStock(long product_variation_id)
        {
            try
            {
                bool status = true;
                var pv_model = getById(product_variation_id);
                if (pv_model.product_variation_in_stock != null && pv_model.product_variation_in_stock <= 0 && pv_model.product.allow_ordering_while_out_of_stock_status == false)
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
        public List<product_variation> getIdByNameAndProductId(string name_string, long product_id)
        {
            try
            {
                return db.product_variations.Where(pv => pv.product_variation_name.Contains(name_string) && pv.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        //public long getIdByNameAndProductId(string name_string, long product_id)
        //{
        //    try
        //    {
        //        return db.product_variations.FirstOrDefault(pv => pv.product_variation_name.Contains(name_string) && pv.product_id == product_id).product_variation_id;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        public int num_of_property(long product_variation_id)
        {
            try
            {
                var list_ele = db.element_variations.Where(e => e.product_variation_id == product_variation_id).ToList();
                List<int> list_check_property_id = new List<int>();
                foreach (var item in list_ele)
                {
                    if (!list_check_property_id.Contains((int)item.product_variation_property_id))
                    {
                        list_check_property_id.Add((int)item.product_variation_property_id);
                    }
                }
                return list_check_property_id.Count();
            }
            catch
            {
                return 0;
            }
        }

        public bool updatedOverviewProductVariation(long pv_id, string pv_image, string pv_code, decimal pv_price, decimal pv_rent_cost, int in_stock)
        {
            try
            {
                var old_model = getById(pv_id);
                old_model.product_variation_image = pv_image;
                old_model.product_variation_code = pv_code;
                if (pv_price == 0)
                {
                    old_model.product_variation_price = null;
                }
                else
                {
                    old_model.product_variation_price = pv_price;
                }

                if (pv_rent_cost == 0)
                {
                    old_model.product_variation_rent_cost = null;
                }
                else
                {
                    old_model.product_variation_rent_cost = pv_rent_cost;
                }
                old_model.product_variation_in_stock = in_stock;

                var list_pv_by_product_id = getByProductId(old_model.product_id);
                if (list_pv_by_product_id.Count() == 1)
                {
                    var product = product_provider.getById(old_model.product_id);
                    product.product_code = pv_code;
                    product.promotion_price = pv_price;
                    product.rent_cost_promotion = pv_rent_cost;
                    product.view_count = in_stock;
                    product_provider.updateOverviewProduct(old_model.product_id, pv_code, pv_rent_cost, pv_price);
                }
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
