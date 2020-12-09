using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ProductVariationInOrderProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<product_variation_in_order> getByOrderId(long order_id)
        {
            try
            {
                return db.product_variation_in_orders.Where(np => np.order_id == order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteListProductVariationInOrder(List<product_variation_in_order> list_pvio)
        {
            try
            {
                db.product_variation_in_orders.DeleteAllOnSubmit(list_pvio);
                db.SubmitChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
