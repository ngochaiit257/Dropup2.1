using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CartProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        ProductVariationProvider pv_provider = new ProductVariationProvider();

        public List<cart_product_variation> getCartForUser(long user_id)
        {
            try
            {
                var cart_of_user = db.cart_of_users.FirstOrDefault(cu => cu.user_id == user_id);
                if(cart_of_user != null)
                {
                    return db.cart_product_variations.Where(cpv => cpv.cart_of_user_id == cart_of_user.cart_or_user_id).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<product_variation> getListPvinCart(long user_id, ref decimal total_cost)
        {
            try
            {
                decimal total_cost_return = 0;
                var list_get = getCartForUser(user_id);
                var list_pv_return = new List<product_variation>();
                foreach(var item in list_get)
                {
                    var product_variation = pv_provider.getById(item.product_variation_id);
                    list_pv_return.Add(product_variation);
                    if (product_variation.product.vat_status == true)
                    {
                        total_cost_return += (decimal)product_variation.product_variation_price * item.quantity;
                    }
                    else
                    {
                        total_cost_return += ((decimal)product_variation.product_variation_price + (decimal)((double)product_variation.product_variation_price * 0.1)) * item.quantity;
                    }
                }
                total_cost = total_cost_return;
                return list_pv_return;
            }
            catch
            {
                return null;
            }
        }

        public bool insertCartOfUser(long user_id, ref long cart_of_user_id)
        {
            try
            {
                cart_of_user model = new cart_of_user();
                model.user_id = user_id;
                db.cart_of_users.InsertOnSubmit(model);
                db.SubmitChanges();
                cart_of_user_id = model.cart_or_user_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateCartForUser(long user_id, List<cart_product_variation> new_list)
        {
            try
            {
                if (getCartForUser(user_id) != null)
                {
                    db.cart_product_variations.DeleteAllOnSubmit(getCartForUser(user_id));
                    var cart_of_user_to_delete = db.cart_of_users.FirstOrDefault(c => c.user_id == user_id);
                    db.cart_of_users.DeleteOnSubmit(cart_of_user_to_delete);
                    db.SubmitChanges();
                }
                long cart_of_user_id = 0;
                if (insertCartOfUser(user_id, ref cart_of_user_id))
                {
                    foreach (var item in new_list)
                    {
                        cart_product_variation model = new cart_product_variation();
                        model.cart_of_user_id = cart_of_user_id;
                        model.product_variation_id = item.product_variation_id;
                        model.quantity = item.quantity;
                        db.cart_product_variations.InsertOnSubmit(model);
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

        public bool deleteCartForUser(long user_id)
        {
            try
            {
                db.cart_product_variations.DeleteAllOnSubmit(getCartForUser(user_id));
                var cart_of_user_to_delete = db.cart_of_users.FirstOrDefault(c => c.user_id == user_id);
                db.cart_of_users.DeleteOnSubmit(cart_of_user_to_delete);
                db.SubmitChanges();
                return true; 
            }
            catch
            {
                return false;
            }
        }

        public List<cart_product_variation> getByProductVariationId(long pv_id)
        {
            try
            {
                return db.cart_product_variations.Where(cpv => cpv.product_variation_id == pv_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteByProductVariationIdInCart(long pv_id)
        {
            try
            {
                db.cart_product_variations.DeleteAllOnSubmit(getByProductVariationId(pv_id));
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
