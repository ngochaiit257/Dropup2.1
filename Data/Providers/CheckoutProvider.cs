using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CheckoutProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<order> getAll()
        {
            try
            {
                return db.orders.OrderBy(o => o.order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public order getByOderId(long order_id)
        {
            try
            {
                return db.orders.FirstOrDefault(o => o.order_id == order_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertOrder(order model, ref long order_id)
        {
            try
            {
                db.orders.InsertOnSubmit(model);
                db.SubmitChanges();
                order_id = model.order_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteOrder(long order_id)
        {
            try
            {
                var model_to_delete = getByOderId(order_id);
                db.orders.DeleteOnSubmit(model_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    

        

        //public List<payment_by> getAllPaymentBy()
        //{
        //    try
        //    {
        //        return db.payment_bies.OrderBy(p => p.payment_by_id).ToList();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search_string"></param>
        /// <param name="quickFilterValue"></param>
        /// <returns></returns>
        //public List<order> quickFilter(string search_string, string quickFilterValue)
        //{
        //    try
        //    {
        //        if (search_string == null)
        //        {
        //            search_string = "";
        //        }
        //        if (quickFilterValue == "this_month")
        //        {
        //            return db.orders.Where(s => s.customer_name.Contains(search_string) && (s.create_date.Month == System.DateTime.Now.Month)).OrderByDescending(ord => ord.create_date).ToList();
        //        }
        //        else
        //        {
        //            return db.orders.Where(s => s.customer_name.Contains(search_string)).OrderBy(s => s.order_id).OrderByDescending(ord => ord.create_date).ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

      

        //public decimal getTotalCostByOrderId(long order_id)
        //{
        //    try
        //    {
        //        var list_order_detail = getOrderDetailByOderId(order_id);
        //        decimal total_cost = 0;
        //        foreach(var item in list_order_detail)
        //        {
        //            total_cost += (decimal)(item.quantity * item.product.promotion_price);
        //        }
        //        return total_cost;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}
    }
}
