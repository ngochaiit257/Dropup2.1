using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class CustomerProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        OrderProvider order_provider = new OrderProvider();
        TagProvider tag_provider = new TagProvider();

        public List<customer> getAll()
        {
            try
            {
                return db.customers.OrderBy(c => c.customer_last_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public customer getByCustomerId(long customer_id)
        {
            try
            {
                return db.customers.FirstOrDefault(c => c.customer_id == customer_id);
            }
            catch
            {
                return null;
            }
        }

        public customer getByUserId(long user_id)
        {
            try
            {
                return db.customers.FirstOrDefault(c => c.user_id == user_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertCustomer(customer model, ref long customer_id)
        {
            try
            {
                db.customers.InsertOnSubmit(model);
                db.SubmitChanges();
                customer_id = model.customer_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateCustomer(customer model)
        {
            try
            {
                var old_model = getByCustomerId(model.customer_id);
                old_model.customer_first_name = model.customer_first_name;
                old_model.customer_last_name = model.customer_last_name;
                old_model.customer_email = model.customer_email;
                old_model.customer_note = model.customer_note;
                old_model.customer_phone_number = model.customer_phone_number;
                old_model.country_id = model.country_id;
                old_model.province_id = model.province_id;
                old_model.district_id = model.district_id;
                old_model.ward_id = model.ward_id;
                old_model.address_detail = model.address_detail;
                old_model.customer_birthday = model.customer_birthday;
                old_model.marketing_status = model.marketing_status;
                old_model.gender_id = old_model.gender_id;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateEmailCustomer(long customer_id, string email)
        {
            try
            {
                var customer = getByCustomerId(customer_id);
                customer.customer_email = email;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

            public bool deleteCustomer(customer model)
            {
                try
                {
                    db.customers.DeleteOnSubmit(getByCustomerId(model.customer_id));
                    db.SubmitChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool checkDuplicateEmail(string email)
            {
                try
                {
                    bool status = false;
                    foreach (var obj in getAll())
                    {
                        if (obj.customer_email.ToLower() == email.ToLower())
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

            public bool checkDuplicatePhoneNumber(string phone_numer)
            {
                try
                {
                    bool status = false;
                    foreach (var obj in getAll())
                    {
                        if (obj.customer_phone_number == phone_numer)
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

            public List<customer> getCustomerValid()
            {
                try
                {
                    return db.customers.Where(c => c.black_list_status == false).ToList();
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// Get list customer required to receive marketing
            /// </summary>
            /// <returns></returns>
            public List<customer> getMarketingCustomer(List<customer> list_customer)
            {
                try
                {
                    var list_return = new List<customer>();
                    foreach (var customer in list_customer)
                    {
                        if (customer.marketing_status == true && customer.black_list_status == false)
                        {
                            list_return.Add(customer);
                        }
                    }
                    return list_return;
                }
                catch
                {
                    return null;
                }
            }


            /// <summary>
            /// Get list customer has number of order more 1
            /// </summary>
            /// <returns></returns>
            public List<customer> getCloseCustomerByAmountOrder(List<customer> list_customer)
            {
                try
                {
                    var list_return = new List<customer>();
                    foreach (var customer in list_customer)
                    {
                        var listOrderByCustomer = order_provider.getOfficialOrderByCustomerId(customer.customer_id);
                        if (listOrderByCustomer.Count() > 1 && customer.black_list_status == false)
                        {
                            list_return.Add(customer);
                        }
                    }
                    return list_return;
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// This function get list customer has order was canceled
            /// </summary>
            /// <returns></returns>
            //public List<customer> getCustomerCanceledOrder(List<customer> list_customer)
            //{
            //    try
            //    {
            //        bool status_customer_canceled_order = false;
            //        var list_return = new List<customer>();

            //        foreach (var customer in list_customer)
            //        {
            //            var listOrderByCustomer = order_provider.getByCustomerId(customer.customer_id);
            //            foreach (var order in listOrderByCustomer)
            //            {
            //                if (order.reason_for_cancel_id != null)
            //                {
            //                    status_customer_canceled_order = true;
            //                }
            //            }
            //            if (status_customer_canceled_order == true)
            //            {
            //                list_return.Add(customer);
            //            }
            //        }
            //        return list_return;
            //    }
            //    catch
            //    {
            //        return null;
            //    }
            //}

            public List<customer> getCustomerByProvinceName(string province_name)
            {
                try
                {
                    return db.customers.Where(c => c.province.Name == province_name).ToList();
                }
                catch
                {
                    return null;
                }
            }

            public List<customer> getCustomerByMonthOfBirth(int month)
            {
                try
                {
                    return db.customers.Where(c => c.customer_birthday.Value.Month == month).ToList();
                }
                catch
                {
                    return null;
                }
            }

            public List<customer> getCustomerByNumberOfOrder(string quick_filter_condition, decimal quick_filter_value)
            {
                try
                {
                    var list_return = new List<customer>();
                    foreach (var customer in getAll())
                    {
                        var listOrderByCustomer = order_provider.getByCustomerId(customer.customer_id);
                        if (quick_filter_condition == "number_of_order_>")
                        {
                            if (listOrderByCustomer.Count() > quick_filter_value)
                            {
                                list_return.Add(customer);
                            }
                        }
                        else if (quick_filter_condition == "number_of_order_<")
                        {
                            if (listOrderByCustomer.Count() < quick_filter_value)
                            {
                                list_return.Add(customer);
                            }
                        }
                        else if (quick_filter_condition == "number_of_order_=")
                        {
                            if (listOrderByCustomer.Count() == quick_filter_value)
                            {
                                list_return.Add(customer);
                            }
                        }
                        else
                        {
                            if (listOrderByCustomer.Count() != quick_filter_value)
                            {
                                list_return.Add(customer);
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

            public List<customer> getListCustomerByTag(string customer_tag)
            {
                try
                {
                    var list_tag_by_tag_string = tag_provider.getAll().Where(t => t.tag_name.Contains(customer_tag));
                    List<customer> list_customer_return = new List<customer>();
                    if (list_tag_by_tag_string != null)
                    {
                        foreach (var tag in list_tag_by_tag_string)
                        {
                            var tag_customer = db.tag_customers.Where(tc => tc.tag_id == tag.tag_id).ToList();
                            foreach (var tag_customer_obj in tag_customer)
                            {
                                customer model = getByCustomerId(tag_customer_obj.customer_id);
                                list_customer_return.Add(model);
                            }
                        }
                    }
                    return list_customer_return;
                }
                catch
                {
                    return null;
                }
            }

            public List<customer> getListCustomerByMonthOfBirth(string month)
            {
                try
                {
                    var list_return = new List<customer>();
                    var list_customer_has_birthday = getAll().Where(c => c.customer_birthday != null).ToList();
                    switch (month)
                    {
                        case "1":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 1).ToList();
                            break;
                        case "2":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 2).ToList();
                            break;
                        case "3":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 3).ToList();
                            break;
                        case "4":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 4).ToList();
                            break;
                        case "5":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 5).ToList();
                            break;
                        case "6":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 6).ToList();
                            break;
                        case "7":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 7).ToList();
                            break;
                        case "8":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 8).ToList();
                            break;
                        case "9":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 9).ToList();
                            break;
                        case "10":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 10).ToList();
                            break;
                        case "11":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 11).ToList();
                            break;
                        case "12":
                            list_return = list_customer_has_birthday.Where(c => c.customer_birthday.Value.Month == 12).ToList();
                            break;
                        default:
                            break;
                    }
                    return list_return;
                }
                catch
                {
                    return null;
                }
            }

            public List<customer> getByTabCustomer(string quick_filter_tab, List<customer> list_customer)
            {
                try
                {
                    List<customer> list_return = new List<customer>();
                    if (quick_filter_tab == "tab_marketing_customer")
                    {
                        list_return = getMarketingCustomer(list_customer);
                    }
                    else if (quick_filter_tab == "tab_close_customer")
                    {
                        list_return = getCloseCustomerByAmountOrder(list_customer);
                    }
                    //else if (quick_filter_tab == "tab_customer_has_canceled_order")
                    //{
                    //    list_return = getCustomerCanceledOrder(list_customer);
                    //}
                    else if (quick_filter_tab == "tab_customer_in_black_list")
                    {
                        list_return = db.customers.Where(c => c.black_list_status == true).ToList();
                    }
                    else
                    {
                        list_return = list_customer;
                    }
                    return list_return;
                }
                catch
                {
                    return null;
                }
            }

            public List<customer> getListCustomerFilter(string search_string, string quick_filter_select, string quick_filter_condition,
                                                        decimal quick_filter_value, string quick_filer_tab)
            {
                try
                {
                    var list_return = new List<customer>();
                    if (search_string == null)
                    {
                        search_string = "";
                    }
                    if (quick_filter_condition != null)
                    {
                        if (quick_filter_select == "customer_number_of_order")
                        {
                            list_return = getByTabCustomer(quick_filer_tab, getCustomerByNumberOfOrder(quick_filter_condition, quick_filter_value)).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                        }
                        else if (quick_filter_select == "customer_tag")
                        {
                            list_return = getByTabCustomer(quick_filer_tab, getListCustomerByTag(quick_filter_value.ToString())).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                        }
                        else if (quick_filter_select == "customer_address")
                        {
                            list_return = getByTabCustomer(quick_filer_tab, getCustomerByProvinceName(quick_filter_condition)).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                        }
                        else if (quick_filter_select == "customer_month_of_birth")
                        {
                            list_return = getByTabCustomer(quick_filer_tab, getListCustomerByMonthOfBirth(quick_filter_condition)).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                        }
                        else
                        {
                            list_return = getByTabCustomer(quick_filer_tab, getAll()).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                        }
                    }
                    else
                    {
                        list_return = getByTabCustomer(quick_filer_tab, getAll()).Where(s =>
                            (s.customer_first_name + " " + s.customer_last_name + " " + s.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(c => c.create_datetime)
                            .ToList();
                    }
                    return list_return;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

        public customer getByOrderId(long order_id)
        {
            try
            {
                return db.orders.FirstOrDefault(o => o.order_id == order_id).customer;
            }
            catch
            {
                return null;
            }
        }
        }
    }
