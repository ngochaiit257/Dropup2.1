using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class DeliveryAddressProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        UserProvider user_provider = new UserProvider();
        CustomerProvider customer_provider = new CustomerProvider();

        public List<delivery_address> getAll()
        {
            try
            {
                return db.delivery_addresses.OrderBy(d => d.delivery_address_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public delivery_address getById(int delivery_address_id)
        {
            try
            {
                return db.delivery_addresses.FirstOrDefault(da => da.delivery_address_id == delivery_address_id);
            }
            catch
            {
                return null;
            }
        }

        public List<delivery_address> getByCustomerId(long customer_id)
        {
            try
            {
                return db.delivery_addresses.Where(d => d.customer_id == customer_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public delivery_address getDefaultDeliveryAddressByCustomerId(long customer_id)
        {
            try
            {
                return db.delivery_addresses.FirstOrDefault(d => d.customer_id == customer_id && d.default_status == true);
            }
            catch
            {
                return null;
            }
        }


        public bool insertDeliveryAddress(delivery_address model, ref int delivery_address_id)
        {
            try
            {
                db.delivery_addresses.InsertOnSubmit(model);
                db.SubmitChanges();
                delivery_address_id = model.delivery_address_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateDeliveryAddress(delivery_address model)
        {
            try
            {
                var old_model = getById(model.delivery_address_id);
                old_model.first_name = model.first_name;
                old_model.last_name = model.last_name;
                old_model.country_id = model.country_id;
                old_model.province_id = model.province_id;
                old_model.ward_id = model.ward_id;
                old_model.district_id = model.district_id;
                old_model.phone_number = model.phone_number;
                old_model.address_detail = model.address_detail;
                old_model.customer_id = model.customer_id;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateWhenCheckout(long user_id, string fullname, string phone_number, int province_id, int district_id, int ward_id, string address_detail, bool default_status, ref int da_id)
        {
            try
            {
                var user = user_provider.getByUserId(user_id);
                var customer = customer_provider.getByUserId(user_id);
                var da_test = getByCustomerId(customer_provider.getByUserId(user_id).customer_id).FirstOrDefault(da => da.default_status == true);
                if (da_test != null)
                {
                    if(default_status == true)
                    {
                        da_test.default_status = false;

                        delivery_address da = new delivery_address();
                        da.last_name = fullname;
                        da.phone_number = phone_number;
                        da.province_id = province_id;
                        da.district_id = district_id;
                        da.ward_id = ward_id;
                        da.address_detail = address_detail;
                        da.default_status = true;
                        da.customer_id = customer.customer_id;
                        db.delivery_addresses.InsertOnSubmit(da);
                        db.SubmitChanges();
                        da_id = da.delivery_address_id;
                    }
                    else
                    {
                        delivery_address da = new delivery_address();
                        da.last_name = fullname;
                        da.phone_number = phone_number;
                        da.province_id = province_id;
                        da.district_id = district_id;
                        da.ward_id = ward_id;
                        da.address_detail = address_detail;
                        da.default_status = false;
                        da.customer_id = customer.customer_id;
                        db.delivery_addresses.InsertOnSubmit(da);
                        db.SubmitChanges();
                        da_id = da.delivery_address_id;
                    }
                }
                else
                {
                    user.lastname = fullname;
                    user.phone_number = phone_number;
                    user.province_id = province_id;
                    user.district_id = district_id;
                    user.ward_id = ward_id;
                    user.address_detail = address_detail;

                    customer.customer_last_name = fullname;
                    customer.customer_phone_number = phone_number;
                    customer.province_id = province_id;
                    customer.district_id = district_id;
                    customer.ward_id = ward_id;
                    customer.address_detail = address_detail;

                    delivery_address da = new delivery_address();
                    da.last_name = fullname;
                    da.phone_number = phone_number;
                    da.province_id = province_id;
                    da.district_id = district_id;
                    da.ward_id = ward_id;
                    da.address_detail = address_detail;
                    da.default_status = true;
                    da.customer_id = customer.customer_id;
                    db.delivery_addresses.InsertOnSubmit(da);
                    db.SubmitChanges();
                    da_id = da.delivery_address_id;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
