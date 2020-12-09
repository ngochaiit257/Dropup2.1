using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class NewProductInOrderProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<custom_product_in_order> getAll()
        {
            try
            {
                return db.custom_product_in_orders.OrderBy(np => np.custom_product_in_order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<custom_product_in_order> getByOrderId(long order_id)
        {
            try
            {
                return db.custom_product_in_orders.Where(np => np.order_id == order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public custom_product_in_order getById(long custom_product_in_order_id)
        {
            try
            {
                return db.custom_product_in_orders.FirstOrDefault(np => np.custom_product_in_order_id == custom_product_in_order_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertNewProductInOrder(custom_product_in_order model)
        {
            try
            {
                db.custom_product_in_orders.InsertOnSubmit(model);
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
