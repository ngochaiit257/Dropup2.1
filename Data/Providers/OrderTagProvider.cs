using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class OrderTagProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        public List<order_tag> getByOrderId(long order_id)
        {
            try
            {
                return db.order_tags.Where(ot => ot.order_id == order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteListOrderTagInOrder(List<order_tag> list_ot)
        {
            try
            {
                db.order_tags.DeleteAllOnSubmit(list_ot);
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
