using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class SupplierProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<supplier> getAll()
        {
            try
            {
                return db.suppliers.OrderBy(s => s.supplier_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public supplier getBySupplierId(int supplier_id)
        {
            try
            {
                return db.suppliers.FirstOrDefault(s => s.supplier_id == supplier_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertSupplier(supplier model)
        {
            try
            {
                db.suppliers.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateSupplier(supplier model)
        {
            try
            {
                var old_model = getBySupplierId(model.supplier_id);
                old_model.supplier_name = model.supplier_name;
                old_model.supplier_phone_number = model.supplier_phone_number;
                old_model.supplier_email = model.supplier_email;
                old_model.supplier_address = model.supplier_address;
                old_model.date_of_contract = model.date_of_contract;
                if (model.supplier_image != null || model.supplier_image != "")
                {
                    old_model.supplier_image = model.supplier_image;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteSupplier(supplier model)
        {
            try
            {
                var model_to_delete = getBySupplierId(model.supplier_id);
                db.suppliers.DeleteOnSubmit(model_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkSupplierNameDuplicate(string supplier_name)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.supplier_name == supplier_name)
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
    }
}
