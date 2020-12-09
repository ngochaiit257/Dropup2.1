using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;
using System.Web.Security;
using PagedList;
using DropupManagement.Models;

namespace DropupManagement.Controllers
{
    public class CustomerController : Controller
    {
        LocalProvider local_provider = new LocalProvider();
        UserProvider user_provider = new UserProvider();
        CustomerProvider customer_provider = new CustomerProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        TagProvider tag_provider = new TagProvider();
        TagCategoryProvider tag_category_provider = new TagCategoryProvider();
        TagCustomerProvider tag_customer_provider = new TagCustomerProvider();
        DeliveryAddressProvider delivery_address_provider = new DeliveryAddressProvider();
        OrderProvider order_provider = new OrderProvider();

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        // GET: Customer
        public ActionResult Index(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab,
                                    int? size_of_page, string sort_list_return)
        {
            if (quick_filer_tab == null)
            {
                quick_filer_tab = "";
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterTab = quick_filer_tab;
            if (size_of_page == null)
            {
                size_of_page = 10;
            }
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            return View();
        }

        public PartialViewResult GetCustomer(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<customer>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            int Size_Of_Page = size_of_page;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterTab = quick_filer_tab;
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            list_return = customer_provider.getListCustomerFilter(search_string, quick_filter_select, quick_filter_condition, (decimal)(quick_filter_value), quick_filer_tab);
            ViewBag.NumberOfList = list_return.Count();

            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.customer_last_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.customer_last_name).ToList();
            }
            return PartialView("_PartialViewCustomer", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult IndexCustomerListSelect(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab, int? size_of_page, string sort_list_return)
        {
            if (quick_filer_tab == null)
            {
                quick_filer_tab = "";
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterTab = quick_filer_tab;
            if (size_of_page == null)
            {
                size_of_page = 10;
            }
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            return View();
        }

        public PartialViewResult GetCustomerListSelect(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab, int size_of_page, string sort_list_return)
        {
            var list_return = new List<customer>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            int Size_Of_Page = size_of_page;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterTab = quick_filer_tab;
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            list_return = customer_provider.getListCustomerFilter(search_string, quick_filter_select, quick_filter_condition, (decimal)(quick_filter_value), quick_filer_tab);
            ViewBag.NumberOfList = list_return.Count();

            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.customer_last_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.customer_last_name).ToList();
            }
            return PartialView("_PartialViewCustomerListSelect", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult IndexCustomerToSelectMultiple(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab, int? size_of_page, string sort_list_return)
        {
            if (quick_filer_tab == null)
            {
                quick_filer_tab = "";
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterTab = quick_filer_tab;
            if (size_of_page == null)
            {
                size_of_page = 10;
            }
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            return View();
        }

        public PartialViewResult GetCustomerToSelectMultiple(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab, int size_of_page, string sort_list_return)
        {
            var list_return = new List<customer>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }
            if (quick_filter_value.ToString() == null)
            {
                quick_filter_value = 0;
            }
            ViewBag.page_num = page_num;
            int Size_Of_Page = size_of_page;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterSelect = quick_filter_select;
            ViewBag.QuickFilterCondition = quick_filter_condition;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilterTab = quick_filer_tab;
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            list_return = customer_provider.getListCustomerFilter(search_string, quick_filter_select, quick_filter_condition, (decimal)(quick_filter_value), quick_filer_tab);
            ViewBag.NumberOfList = list_return.Count();

            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.customer_last_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.customer_last_name).ToList();
            }
            return PartialView("_PartialViewCustomerToSelectMultiple", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult InsertCustomer()
        {
            ViewData["ListRole"] = user_provider.getListRole();
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            return View();
        }

        [HttpPost]
        public ActionResult InsertCustomer(string first_name, string last_name, int? gender_id, string customer_email, DateTime? customer_birthday,
                                            string customer_phone_number, string customer_note, int? province_id, int? district_id,
                                            int? ward_id, bool marketing_status, string address_detail, List<string> tags)
        {
            customer customer = new customer();
            delivery_address da = new delivery_address();
            customer.black_list_status = false;
            customer.create_datetime = System.DateTime.Now;
            if (first_name != "")
            {
                customer.customer_first_name = first_name;
                da.first_name = first_name;
            }
            if (last_name != "")
            {
                customer.customer_last_name = last_name;
                da.last_name = last_name;
            }
            if (customer_email != "")
            {
                customer.customer_email = customer_email;
            }
            if (customer_phone_number != "")
            {
                customer.customer_phone_number = customer_phone_number;
                da.phone_number = customer_phone_number;
            }
            if (address_detail != "")
            {
                customer.address_detail = address_detail;
                da.address_detail = address_detail;
            }
            if (customer_note != "")
            {
                customer.customer_note = customer_note;

            }
            if (gender_id == null)
            {
                customer.gender_id = 4;
            }
            else
            {
                customer.gender_id = gender_id;
            }
            customer.customer_birthday = customer_birthday;
            //customer.country_id = country_id;
            if (province_id == -1 || province_id == null)
            {
                customer.province_id = null;
            }
            else
            {
                customer.province_id = province_id;
                da.province_id = province_id;
            }
            if (district_id == -1 || district_id == null)
            {
                customer.district_id = null;
            }
            else
            {
                customer.district_id = district_id;
                da.district_id = district_id;
            }
            if (ward_id == -1 || ward_id == null)
            {
                customer.ward_id = null;
            }
            else
            {
                customer.ward_id = ward_id;
                da.ward_id = ward_id;
            }
            customer.marketing_status = marketing_status;

            long customer_id = 0;
            long tagId = 0;
            int delivery_id = 0;
            if (customer_provider.insertCustomer(customer, ref customer_id))
            {
                da.customer_id = customer_id;
                da.default_status = true;
                if (delivery_address_provider.insertDeliveryAddress(da, ref delivery_id))
                {
                    if (tags != null)
                    {
                        foreach (var tag in tags)
                        {
                            if (tag_provider.insertTag(tag, ref tagId))
                            {
                                tag_customer_provider.insertTagCustomer(customer_id, tagId);
                            }
                        }
                    }

                    system_log system_log = new system_log();
                    if (customer.customer_first_name == null && customer.customer_last_name == null)
                    {
                        system_log.system_log_description = "đã thêm khách hàng " + customer.customer_email;
                    }
                    else
                    {
                        system_log.system_log_description = "đã thêm khách hàng " + customer_provider.getByCustomerId(customer_id).customer_first_name + " " + customer_provider.getByCustomerId(customer_id).customer_last_name;
                    }
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = customer_id;
                    system_log.system_log_url = "/Customer/CustomerDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json("Insert Success");

        }

        [HttpPost]
        public ActionResult InsertCustomerInOrder(string first_name, string last_name, string customer_email,
                                            string customer_phone_number, int? province_id, int? district_id,
                                            int? ward_id, bool marketing_status, string address_detail)
        {
            customer customer = new customer();
            delivery_address da = new delivery_address();
            customer.black_list_status = false;
            customer.create_datetime = System.DateTime.Now;
            if (first_name != "")
            {
                customer.customer_first_name = first_name;
                da.first_name = first_name;
            }
            if (last_name != "")
            {
                customer.customer_last_name = last_name;
                da.last_name = last_name;
            }
            if (customer_email != "")
            {
                customer.customer_email = customer_email;
            }
            if (customer_phone_number != "")
            {
                customer.customer_phone_number = customer_phone_number;
                da.phone_number = customer_phone_number;
            }
            if (address_detail != "")
            {
                customer.address_detail = address_detail;
                da.address_detail = address_detail;
            }
            //customer.country_id = country_id;
            if (province_id == -1)
            {
                customer.province_id = null;
            }
            else
            {
                customer.province_id = province_id;
                da.province_id = province_id;
            }
            if (district_id == -1)
            {
                customer.district_id = null;
            }
            else
            {
                customer.district_id = district_id;
                da.district_id = district_id;
            }
            if (ward_id == -1)
            {
                customer.ward_id = null;
            }
            else
            {
                customer.ward_id = ward_id;
                da.ward_id = ward_id;
            }
            customer.marketing_status = marketing_status;

            long customer_id = 0;
            int delivery_id = 0;
            long tagId = 0;
            if (customer_provider.insertCustomer(customer, ref customer_id))
            {
                da.customer_id = customer_id;
                da.default_status = true;
                if (delivery_address_provider.insertDeliveryAddress(da, ref delivery_id))
                {
                    system_log system_log = new system_log();
                    if (customer.customer_first_name == null && customer.customer_last_name == null)
                    {
                        system_log.system_log_description = "đã thêm khách hàng " + customer.customer_email;
                    }
                    else
                    {
                        system_log.system_log_description = "đã thêm khách hàng " + customer_provider.getByCustomerId(customer_id).customer_first_name + " " + customer_provider.getByCustomerId(customer_id).customer_last_name;
                    }
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = customer_id;
                    system_log.system_log_url = "/Customer/CustomerDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json(new { customer_id = customer_id, delivery_id = delivery_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool checkDuplicateEmail(string customer_email)
        {
            return customer_provider.checkDuplicateEmail(customer_email);
        }

        [HttpPost]
        public bool checkDuplicatePhoneNumber(string customer_phone_number)
        {
            return customer_provider.checkDuplicatePhoneNumber(customer_phone_number);
        }

        [HttpPost]
        public ActionResult GetCustomerByCustomerId(long customer_id)
        {
            var customer = customer_provider.getByCustomerId(customer_id);
            string customer_province = "";
            string customer_district = "";
            string customer_ward = "";
            //Get default Delivery address
            var default_delivery_address = delivery_address_provider.getDefaultDeliveryAddressByCustomerId(customer_id);
            if (default_delivery_address.province_id != null)
            {
                customer_province = default_delivery_address.province.Name;
            }
            if (default_delivery_address.district_id != null)
            {
                customer_district = default_delivery_address.district.Name;
            }
            if (default_delivery_address.ward_id != null)
            {
                customer_ward = default_delivery_address.ward.Name;
            }
            var customer_data = new
            {
                customer_id = customer.customer_id,
                customer_first_name = default_delivery_address.first_name,
                customer_last_name = default_delivery_address.last_name,
                customer_province_id = default_delivery_address.province_id,
                customer_province = customer_province,
                customer_district_id = default_delivery_address.district_id,
                customer_district = customer_district,
                customer_ward_id = default_delivery_address.ward_id,
                customer_ward = customer_ward,
                customer_phone_number = default_delivery_address.phone_number,
                customer_email = customer.customer_email,
                customer_address_detail = default_delivery_address.address_detail,
                customer_marketing_status = customer.marketing_status,
                customer_default_delivery_address = default_delivery_address.delivery_address_id
            };

            var list_delivery_address = new List<DeliveryAddressModel>();
            foreach (var da in delivery_address_provider.getByCustomerId(customer_id))
            {
                DeliveryAddressModel da_model = new DeliveryAddressModel();
                da_model.delivery_address_id = da.delivery_address_id;
                da_model.first_name = da.first_name;
                da_model.last_name = da.last_name;
                da_model.phone_number = da.phone_number;
                if (da.country_id != null)
                {
                    da_model.country_id = (int)da.country_id;
                }
                if (da.province_id != null)
                {
                    da_model.province_id = (int)da.province_id;
                    da_model.province_name = local_provider.getNameOfProvinceByProvinceId(da_model.province_id);
                }
                if (da.district_id != null)
                {
                    da_model.district_id = (int)da.district_id;
                    da_model.district_name = local_provider.getNameOfDistrictByDistrictId(da_model.district_id);
                }
                if (da.ward_id != null)
                {
                    da_model.ward_id = (int)da.ward_id;
                    da_model.ward_name = local_provider.getNameOfWardByWardId(da_model.ward_id);
                }
                da_model.address_detail = da.address_detail;

                list_delivery_address.Add(da_model);
            }
            int number_official_order = order_provider.getOfficialOrderByCustomerId(customer_id).Count();
            return Json(new { customer_data = customer_data, list_da = list_delivery_address, number_official_order = number_official_order }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDeliveryAddressById(int delivery_address_id)
        {
            var obj = delivery_address_provider.getById(delivery_address_id);
            DeliveryAddressModel model = new DeliveryAddressModel();
            model.delivery_address_id = obj.delivery_address_id;
            model.first_name = obj.first_name;
            model.last_name = obj.last_name;
            model.phone_number = obj.phone_number;
            model.address_detail = obj.address_detail;
            model.customer_id = (long)obj.customer_id;
            if (obj.country_id != null)
            {
                model.country_id = (int)obj.country_id;
            }
            if (obj.province_id != null)
            {
                model.province_id = (int)obj.province_id;
                model.province_name = local_provider.getNameOfProvinceByProvinceId(model.province_id);
            }
            if (obj.district_id != null)
            {
                model.district_id = (int)obj.district_id;
                model.district_name = local_provider.getNameOfDistrictByDistrictId(model.district_id);
            }
            if (obj.ward_id != null)
            {
                model.ward_id = (int)obj.ward_id;
                model.ward_name = local_provider.getNameOfWardByWardId(model.ward_id);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateEmail(long customer_id, string customer_email)
        {
            var customer = customer_provider.getByCustomerId(customer_id);
            string email_before = customer.customer_email;
            if(customer_provider.updateEmailCustomer(customer_id, customer_email) && email_before != customer_email)
            {
                system_log system_log = new system_log();
                if (customer.customer_first_name == null && customer.customer_last_name == null)
                {
                    system_log.system_log_description = "đã cập nhật email của khách hàng " + customer.customer_email;
                }
                else
                {
                    system_log.system_log_description = "đã cập nhật email của khách hàng " + customer.customer_first_name + " " + customer.customer_last_name;
                }
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = customer_id;
                system_log.system_log_url = "/Customer/CustomerDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Update Success.");
        }

        public ActionResult CustomerDetail(long id)
        {
            return View(customer_provider.getByCustomerId(id));
        }

    }
}