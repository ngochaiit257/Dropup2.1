using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CustomProductInOrder
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        public List<custom_product_in_order> getByOrderId(long order_id)
        {
            try
            {
                return db.custom_product_in_orders.Where(cp => cp.order_id == order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteListCustomProductInOrder(List<custom_product_in_order> list_cpio)
        {
            try
            {
                db.custom_product_in_orders.DeleteAllOnSubmit(list_cpio);
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
