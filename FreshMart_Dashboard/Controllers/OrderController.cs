using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DropupManagement.Models;
using Data;
using System.Web.Security;
using System.Globalization;
using PagedList;
using System.Threading.Tasks;
using Common;

namespace DropupManagement.Controllers
{
    public class OrderController : Controller
    {
        LocalProvider local_provider = new LocalProvider();
        UserProvider user_provider = new UserProvider();
        UnitProvider unit_provider = new UnitProvider();
        OrderProvider order_provider = new OrderProvider();
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        DeliveryAddressProvider delivery_address_provider = new DeliveryAddressProvider();
        CustomerProvider customer_provider = new CustomerProvider();
        OrderLogProvider order_log_provider = new OrderLogProvider();
        CouponProvider coupon_provider = new CouponProvider();
        TransportProvider transport_provider = new TransportProvider();
        TransportLogProvider transport_log_provider = new TransportLogProvider();
        PaymentInformationProvider payment_information_provider = new PaymentInformationProvider();
        StatisticsProvider statistics_provider = new StatisticsProvider();
        ProductVariationInOrderProvider pvio_provider = new ProductVariationInOrderProvider();
        NewProductInOrderProvider cpio_provider = new NewProductInOrderProvider();

        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
        // GET: Order

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

        public PartialViewResult GetOfficialOrder(int? page_num, string search_string, string quick_filter_select, string quick_filter_condition, int? quick_filter_value, string quick_filer_tab,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<order>();
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
            list_return = order_provider.getListOrderFilter(search_string, quick_filter_select, quick_filter_condition, (decimal)(quick_filter_value), quick_filer_tab);
            if (list_return == null)
            {
                ViewBag.NumberOfList = 0;
            }
            else
            {
                ViewBag.NumberOfList = list_return.Count();
            }

            if (sort_list_return == "moiNhat")
            {
                list_return = list_return.OrderByDescending(p => p.create_datetime).ToList();
            }
            else if (sort_list_return == "cuNhat")
            {
                list_return = list_return.OrderBy(p => p.create_datetime).ToList();
            }
            else
            {
                list_return = list_return.OrderByDescending(p => p.create_datetime).ToList();
            }
            return PartialView("_PartialViewOfficialOrder", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult IndexDraftOrder()
        {
            return View();
        }

        public ActionResult IndexNotCompletedOrder()
        {
            return View();
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult InsertOrder()
        {
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            ViewData["ListUnit"] = unit_provider.getAll();
            return View();
        }

        public ActionResult InsertDraftOrder(DraftOrderModel model, List<ProductVariationInOrderModel> list_product_variation_in_order,
            List<CustomProductInOrderModel> list_custom_product_in_order, DeliveryAddressModel delivery_address_in_order)
        {
            order order = new order();

            order.order_note = model.order_note;
            order.source_order = model.source_order;
            order.discount_amount = model.discount_amount;
            order.reason_of_promotion = model.reason_of_promotion;
            order.name_of_shipping = model.name_of_shipping;
            order.cost_of_shipping = model.cost_of_shipping;
            order.user_id_create = getUserLoged().user_id;
            order.user_id_update = getUserLoged().user_id;
            order.create_datetime = System.DateTime.Now;
            order.update_datetime = System.DateTime.Now;
            order.draft_status = true;
            order.confirm_order_status = false;

            long order_id_ref = 0;
            int delivery_address_id = 0;

            delivery_address delivery_address = new delivery_address();
            if (order.customer_id != null || model.customer_id != 0)
            {
                order.customer_id = model.customer_id;
                delivery_address.first_name = delivery_address_in_order.first_name;
                delivery_address.last_name = delivery_address_in_order.last_name;
                delivery_address.phone_number = delivery_address_in_order.phone_number;
                delivery_address.customer_id = delivery_address_in_order.customer_id;
                delivery_address.province_id = delivery_address_in_order.province_id;
                delivery_address.district_id = delivery_address_in_order.district_id;
                delivery_address.ward_id = delivery_address_in_order.ward_id;
                delivery_address.delivery_address_id = delivery_address_in_order.delivery_address_id;
                delivery_address.address_detail = delivery_address_in_order.address_detail;
                if (delivery_address.province_id == -1 || delivery_address.province_id == 0)
                {
                    delivery_address.province_id = null;
                }
                if (delivery_address.district_id == -1 || delivery_address.district_id == 0)
                {
                    delivery_address.district_id = null;
                }
                if (delivery_address.ward_id == -1 || delivery_address.ward_id == 0)
                {
                    delivery_address.ward_id = null;
                }
                if (delivery_address_provider.insertDeliveryAddress(delivery_address, ref delivery_address_id))
                {
                    order.delivery_address_id = delivery_address_id;
                }
            }

            if (order_provider.insertOrder(order, ref order_id_ref))
            {
                if (list_product_variation_in_order != null)
                {
                    foreach (var product_variation_in_order in list_product_variation_in_order)
                    {
                        order_provider.insertProductVariationInOrder(product_variation_in_order.product_variation_id,
                            product_variation_in_order.product_variation_quantity, order_id_ref);
                    }
                }

                if (list_custom_product_in_order != null)
                {
                    foreach (var custom_product_in_order in list_custom_product_in_order)
                    {
                        custom_product_in_order cp_in_order = new custom_product_in_order();
                        cp_in_order.custom_product_in_order_name = custom_product_in_order.custom_product_in_order_name;
                        cp_in_order.custom_product_in_order_quantity = custom_product_in_order.custom_product_in_order_quantity;
                        cp_in_order.order_id = order_id_ref;
                        cp_in_order.custom_product_in_order_price = custom_product_in_order.custom_product_in_order_price;
                        cp_in_order.unit_name = custom_product_in_order.unit_name;
                        order_provider.insertCustomProductInOrder(cp_in_order);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo đơn hàng nháp " + "#" + order_id_ref;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order_id_ref;
                system_log.system_log_url = "/Order/DraftOrderDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { draft_order_id = order_id_ref }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DraftOrderDetail(long id)
        {
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            ViewData["ListUnit"] = unit_provider.getAll();
            var do_detail = order_provider.getDraftOrderById(id);
            ViewBag.DraftOrderDetail = do_detail;
            ViewBag.UsernameUpdate = user_provider.getByUserId((long)do_detail.user_id_update).username;

            ViewData["ListDistrictByProvince"] = null;
            ViewData["ListWardByDistrict"] = null;
            ViewData["ListDeliveryAddress"] = null;
            ViewBag.Customer = null;
            ViewBag.NumberOfOrderCustomer = null;
            if (do_detail.customer_id != null)
            {
                ViewBag.Customer = customer_provider.getByCustomerId((long)do_detail.customer_id);
                ViewBag.NumberOfOrderCustomer = order_provider.getOfficialOrderByCustomerId((long)do_detail.customer_id).Count();
                var delivery_address_in_order = delivery_address_provider.getById((int)do_detail.delivery_address_id);
                ViewBag.DeliveryAddress = delivery_address_in_order;
                ViewBag.DeliveryAddressProvince = delivery_address_in_order.province.Name;
                ViewBag.DeliveryAddressDistrict = delivery_address_in_order.district.Name;
                ViewBag.DeliveryAddressWard = delivery_address_in_order.ward.Name;

                if (delivery_address_in_order.province_id != null)
                {
                    ViewData["ListDistrictByProvince"] = local_provider.GetAllDistrictByProvinceId((int)delivery_address_in_order.province_id);
                }
                if (delivery_address_in_order.district_id != null)
                {
                    ViewData["ListWardByDistrict"] = local_provider.GetAllWardByDistrictId((int)delivery_address_in_order.district_id);
                }

                ViewData["ListDeliveryAddress"] = delivery_address_provider.getByCustomerId((long)do_detail.customer_id);
            }
            ViewBag.SourceOrder = do_detail.source_order;
            return View(order_provider.getDraftOrderById(id));
        }

        public ActionResult GetAllOfProductInOrder(long order_id)
        {
            var order = order_provider.getById(order_id);
            var list_product_variation_in_order = order_provider.getProductVariationInOrderByOrderId(order_id);
            var list_custom_product_in_order = order_provider.getCustomProductInOrderByOrderId(order_id);

            var list_pv_in_order_return = new List<ProductVariationInOrderModel>();
            var list_cp_in_order_return = new List<CustomProductInOrderModel>();
            if (list_product_variation_in_order != null)
            {
                foreach (var pv in list_product_variation_in_order)
                {
                    ProductVariationInOrderModel pv_model = new ProductVariationInOrderModel();
                    pv_model.product_variation_id = (long)pv.product_variation_id;
                    pv_model.product_variation_name = pv.product_variation_name;
                    pv_model.product_variation_price = (decimal)pv.product_variation_price;
                    pv_model.product_variation_quantity = (int)pv.product_variation_quantity;
                    pv_model.product_variation_image = pv.product_variation_image;
                    pv_model.product_name = pv.product_name;
                    pv_model.unit_name = pv.unit_name;
                    list_pv_in_order_return.Add(pv_model);
                }
            }
            if (list_custom_product_in_order != null)
            {
                foreach (var cp in list_custom_product_in_order)
                {
                    CustomProductInOrderModel cp_model = new CustomProductInOrderModel();
                    cp_model.custom_product_in_order_id = (int)cp.custom_product_in_order_id;
                    cp_model.custom_product_in_order_name = cp.custom_product_in_order_name;
                    cp_model.custom_product_in_order_price = (decimal)cp.custom_product_in_order_price;
                    cp_model.custom_product_in_order_quantity = (int)cp.custom_product_in_order_quantity;
                    cp_model.unit_name = cp.unit_name;
                    list_cp_in_order_return.Add(cp_model);
                }
            }
            return Json(new
            {
                list_product_variation_in_order = list_pv_in_order_return,
                list_custom_product_in_order = list_cp_in_order_return,
                discount_amount = order.discount_amount,
                reason_of_promotion = order.reason_of_promotion,
                name_of_shipping = order.name_of_shipping,
                cost_of_shipping = order.cost_of_shipping
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDraftOrder(DraftOrderModel model, List<ProductVariationInOrderModel> list_product_variation_in_order,
            List<CustomProductInOrderModel> list_custom_product_in_order, DeliveryAddressModel delivery_address_in_order)
        {
            order order = new order();

            if (model.customer_id != null || model.customer_id != 0)
            {
                order.customer_id = model.customer_id;
            }
            order.order_id = model.order_id;
            order.order_note = model.order_note;
            order.source_order = model.source_order;
            order.discount_amount = model.discount_amount;
            order.reason_of_promotion = model.reason_of_promotion;
            order.name_of_shipping = model.name_of_shipping;
            order.cost_of_shipping = model.cost_of_shipping;
            order.user_id_update = getUserLoged().user_id;
            order.update_datetime = System.DateTime.Now;
            order.draft_status = true;
            order.confirm_order_status = false;

            string user_name_update = user_provider.getByUserId((long)order.user_id_update).username;

            if (order.customer_id != null || model.customer_id != 0)
            {
                delivery_address delivery_address = new delivery_address();
                delivery_address.delivery_address_id = delivery_address_in_order.delivery_address_id;
                delivery_address.first_name = delivery_address_in_order.first_name;
                delivery_address.last_name = delivery_address_in_order.last_name;
                delivery_address.phone_number = delivery_address_in_order.phone_number;
                delivery_address.customer_id = delivery_address_in_order.customer_id;
                delivery_address.province_id = delivery_address_in_order.province_id;
                delivery_address.district_id = delivery_address_in_order.district_id;
                delivery_address.ward_id = delivery_address_in_order.ward_id;
                delivery_address.delivery_address_id = delivery_address_in_order.delivery_address_id;
                delivery_address.address_detail = delivery_address_in_order.address_detail;
                if (delivery_address.province_id == -1 || delivery_address.province_id == 0)
                {
                    delivery_address.province_id = null;
                }
                if (delivery_address.district_id == -1 || delivery_address.district_id == 0)
                {
                    delivery_address.district_id = null;
                }
                if (delivery_address.ward_id == -1 || delivery_address.ward_id == 0)
                {
                    delivery_address.ward_id = null;
                }
                if (delivery_address_provider.updateDeliveryAddress(delivery_address))
                {
                    order.delivery_address_id = delivery_address.delivery_address_id;
                }
            }

            if (order_provider.updateOrder(order) && order_provider.deleteListCustomProductInOrder(order.order_id) && order_provider.deleteListProductVariationInOrder(order.order_id))
            {
                if (list_product_variation_in_order != null)
                {
                    foreach (var product_variation_in_order in list_product_variation_in_order)
                    {
                        order_provider.insertProductVariationInOrder(product_variation_in_order.product_variation_id,
                            product_variation_in_order.product_variation_quantity, order.order_id);
                    }
                }

                if (list_custom_product_in_order != null)
                {
                    foreach (var custom_product_in_order in list_custom_product_in_order)
                    {
                        custom_product_in_order cp_in_order = new custom_product_in_order();
                        cp_in_order.custom_product_in_order_name = custom_product_in_order.custom_product_in_order_name;
                        cp_in_order.custom_product_in_order_quantity = custom_product_in_order.custom_product_in_order_quantity;
                        cp_in_order.order_id = order.order_id;
                        cp_in_order.custom_product_in_order_price = custom_product_in_order.custom_product_in_order_price;
                        cp_in_order.unit_name = custom_product_in_order.unit_name;
                        order_provider.insertCustomProductInOrder(cp_in_order);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật đơn hàng nháp " + "#" + order.order_id;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order.order_id;
                system_log.system_log_url = "/Order/DraftOrderDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { draft_order_id = order.order_id, update_datetime = order.update_datetime, user_update = user_name_update }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDetail(long id)
        {
            var order = order_provider.getOfficialOrderById(id);
            if (order.customer_id != null)
            {
                var customer = customer_provider.getByCustomerId((long)order.customer_id);
                ViewBag.Customer = customer;
                ViewBag.NumberOfOrderCustomer = order_provider.getOfficialOrderByCustomerId((long)customer.customer_id).Count();
            }
            if (order.delivery_address_id != null)
            {
                var delivery_address = delivery_address_provider.getById((int)order.delivery_address_id);
                ViewBag.DeliveryAddress = delivery_address;
            }
            ViewBag.Transport = transport_provider.getByOrderId(order.order_id);
            ViewBag.UsernameUpdate = user_provider.getByUserId((long)order.user_id_update).username;
            decimal total_price_list_pv = 0;
            decimal total_price_list_cp = 0;
            ViewData["ListProductVariationInOrder"] = order_provider.getProductVariationInOrderByOrderId(id, ref total_price_list_pv);
            ViewData["ListCustomProductInOrder"] = order_provider.getCustomProductInOrderByOrderId(id, ref total_price_list_cp);
            ViewBag.TotalPriceAllProduct = total_price_list_cp + total_price_list_pv;
            ViewBag.TotalCostOfOrder = total_price_list_cp + total_price_list_pv + order.cost_of_shipping - order.discount_amount;
            ViewData["ListOrderLog"] = order_log_provider.getByOrderId(id);
            //ViewData["ListDateTimeOrderLog"] = order_log_provider.getDateTimeOfOrderLogByOrderId(id);
            return View(order_provider.getOfficialOrderById(id));
        }

        //[ChildActionOnly]
        public ActionResult GetOrderLog(long order_id)
        {
            ViewData["ListDateTimeOrderLog"] = order_log_provider.getDateTimeOfOrderLogByOrderId(order_id);
            var list = order_log_provider.getDateTimeOfOrderLogByOrderId(order_id);
            ViewBag.OrderId = order_id;
            return PartialView("_PartialViewOrderLog", list);
        }

        public ActionResult AddOrderLog(long order_id, string content)
        {
            int order_log_id = 0;
            order_log order_log = new order_log();
            order_log.order_id = order_id;
            order_log.order_log_description = "<strong>" + getUserLoged().username + ":</strong> " + content;
            order_log.order_log_create_datetime = DateTime.Now;
            order_log.order_log_type = "comment";
            order_log_provider.insertOrderLog(order_log, ref order_log_id);

            return Json("Insert Success");
        }

        public ActionResult MakeOrderPaid(OrderModel model, List<ProductVariationInOrderModel> list_product_variation_in_order,
            List<CustomProductInOrderModel> list_custom_product_in_order, DeliveryAddressModel delivery_address_in_order, int payment_method_id,
            decimal amount_received, int coupon_id)
        {
            order order = new order();

            if (coupon_id != 0 || coupon_id != null)
            {
                coupon_provider.updateNumOfUse(coupon_id);
            }

            order.order_note = model.order_note;
            order.source_order = model.source_order;
            order.discount_amount = model.discount_amount;
            order.reason_of_promotion = model.reason_of_promotion;
            order.name_of_shipping = model.name_of_shipping;
            order.cost_of_shipping = model.cost_of_shipping;
            order.user_id_create = getUserLoged().user_id;
            order.user_id_update = getUserLoged().user_id;
            order.create_datetime = System.DateTime.Now;
            order.update_datetime = System.DateTime.Now;
            order.draft_status = false;
            order.confirm_order_status = true;
            order.payment_status = true;
            order.payment_method_id = payment_method_id;

            long order_id_ref = 0;
            int delivery_address_id = 0;

            delivery_address delivery_address = new delivery_address();
            if (order.customer_id != null || model.customer_id != 0)
            {
                order.customer_id = model.customer_id;
                delivery_address.first_name = delivery_address_in_order.first_name;
                delivery_address.last_name = delivery_address_in_order.last_name;
                delivery_address.phone_number = delivery_address_in_order.phone_number;
                delivery_address.customer_id = delivery_address_in_order.customer_id;
                delivery_address.province_id = delivery_address_in_order.province_id;
                delivery_address.district_id = delivery_address_in_order.district_id;
                delivery_address.ward_id = delivery_address_in_order.ward_id;
                delivery_address.delivery_address_id = delivery_address_in_order.delivery_address_id;
                delivery_address.address_detail = delivery_address_in_order.address_detail;
                if (delivery_address.province_id == -1 || delivery_address.province_id == 0)
                {
                    delivery_address.province_id = null;
                }
                if (delivery_address.district_id == -1 || delivery_address.district_id == 0)
                {
                    delivery_address.district_id = null;
                }
                if (delivery_address.ward_id == -1 || delivery_address.ward_id == 0)
                {
                    delivery_address.ward_id = null;
                }
                if (delivery_address_provider.insertDeliveryAddress(delivery_address, ref delivery_address_id))
                {
                    order.delivery_address_id = delivery_address_id;
                }
            }

            if (order_provider.insertOrder(order, ref order_id_ref))
            {
                if (list_product_variation_in_order != null)
                {
                    foreach (var product_variation_in_order in list_product_variation_in_order)
                    {
                        order_provider.insertProductVariationInOrder(product_variation_in_order.product_variation_id,
                            product_variation_in_order.product_variation_quantity, order_id_ref);
                    }
                }

                if (list_custom_product_in_order != null)
                {
                    foreach (var custom_product_in_order in list_custom_product_in_order)
                    {
                        custom_product_in_order cp_in_order = new custom_product_in_order();
                        cp_in_order.custom_product_in_order_name = custom_product_in_order.custom_product_in_order_name;
                        cp_in_order.custom_product_in_order_quantity = custom_product_in_order.custom_product_in_order_quantity;
                        cp_in_order.order_id = order_id_ref;
                        cp_in_order.custom_product_in_order_price = custom_product_in_order.custom_product_in_order_price;
                        cp_in_order.unit_name = custom_product_in_order.unit_name;
                        order_provider.insertCustomProductInOrder(cp_in_order);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo đơn hàng " + "#" + order_id_ref + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order_id_ref;
                system_log.system_log_url = "/Order/OrderDetail";
                system_log_provider.insertSystemLog(system_log);


                int order_log_id_1 = 0;
                int order_log_id_2 = 0;
                int order_log_id_3 = 0;
                order_log order_log_1 = new order_log();
                order_log_1.order_id = order_id_ref;
                order_log_1.order_log_description = "<strong>" + getUserLoged().username + "</strong>" + " đã tạo đơn hàng #" + "<strong>" + order_id_ref + "</strong>";
                order_log_1.order_log_create_datetime = DateTime.Now;
                order_log_1.order_log_type = "create";
                if (order_log_provider.insertOrderLog(order_log_1, ref order_log_id_1))
                {
                    order_log order_log_2 = new order_log();
                    order_log_2.order_id = order_id_ref;
                    order_log_2.order_log_description = "Đơn hàng được xác thực bởi " + "<strong>" + getUserLoged().username + "</strong>";
                    order_log_2.order_log_create_datetime = DateTime.Now;
                    order_log_2.order_log_type = "confirm";
                    order_log_provider.insertOrderLog(order_log_2, ref order_log_id_2);

                    //var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                    string currency = String.Format(elGR, "{0:0,0}", amount_received);
                    order_log order_log_3 = new order_log();
                    order_log_3.order_id = order_id_ref;
                    order_log_3.order_log_description = "Xác nhận thanh toán thành công " + "<strong>" + currency + " ₫</strong>";
                    order_log_3.order_log_create_datetime = DateTime.Now;
                    order_log_3.order_log_type = "payment";
                    if (order_log_provider.insertOrderLog(order_log_3, ref order_log_id_3))
                    {
                        payment_information pi = new payment_information();
                        pi.order_log_id = order_log_id_3;
                        if (order.payment_method_id == 3)
                        {
                            pi.description = "Xác nhận Thanh toán khi giao hàng (COD) đã nhận";
                        }
                        else
                        {
                            pi.description = "Xác nhận Chuyển khoản qua ngân hàng đã nhận";
                        }
                        pi.value_money = amount_received;
                        if (order.payment_method_id == 3)
                        {
                            pi.pay_gate = "Thanh toán khi giao hàng (COD)";
                        }
                        else
                        {
                            pi.pay_gate = "Chuyển khoản qua ngân hàng";
                        }
                        pi.pay_status = true;
                        pi.pay_type = "Xác nhận thanh toán";
                        pi.user_id = getUserLoged().user_id;
                        pi.datetime_confirm = DateTime.Now;
                        payment_information_provider.insertPaymentInformation(pi);
                    }
                }

            }
            return Json(new { order_id = order_id_ref }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeOrderPending(OrderModel model, List<ProductVariationInOrderModel> list_product_variation_in_order,
            List<CustomProductInOrderModel> list_custom_product_in_order, DeliveryAddressModel delivery_address_in_order, int payment_method_id,
            decimal amount_received, int coupon_id)
        {
            order order = new order();

            if (coupon_id != 0 || coupon_id != null)
            {
                coupon_provider.updateNumOfUse(coupon_id);
            }

            order.order_note = model.order_note;
            order.source_order = model.source_order;
            order.discount_amount = model.discount_amount;
            order.reason_of_promotion = model.reason_of_promotion;
            order.name_of_shipping = model.name_of_shipping;
            order.cost_of_shipping = model.cost_of_shipping;
            order.user_id_create = getUserLoged().user_id;
            order.user_id_update = getUserLoged().user_id;
            order.create_datetime = System.DateTime.Now;
            order.update_datetime = System.DateTime.Now;
            order.draft_status = false;
            order.confirm_order_status = true;
            order.payment_status = false;
            order.payment_method_id = payment_method_id;

            long order_id_ref = 0;
            int delivery_address_id = 0;

            delivery_address delivery_address = new delivery_address();
            if (order.customer_id != null || model.customer_id != 0)
            {
                order.customer_id = model.customer_id;
                delivery_address.first_name = delivery_address_in_order.first_name;
                delivery_address.last_name = delivery_address_in_order.last_name;
                delivery_address.phone_number = delivery_address_in_order.phone_number;
                delivery_address.customer_id = delivery_address_in_order.customer_id;
                delivery_address.province_id = delivery_address_in_order.province_id;
                delivery_address.district_id = delivery_address_in_order.district_id;
                delivery_address.ward_id = delivery_address_in_order.ward_id;
                delivery_address.delivery_address_id = delivery_address_in_order.delivery_address_id;
                delivery_address.address_detail = delivery_address_in_order.address_detail;
                if (delivery_address.province_id == -1 || delivery_address.province_id == 0)
                {
                    delivery_address.province_id = null;
                }
                if (delivery_address.district_id == -1 || delivery_address.district_id == 0)
                {
                    delivery_address.district_id = null;
                }
                if (delivery_address.ward_id == -1 || delivery_address.ward_id == 0)
                {
                    delivery_address.ward_id = null;
                }
                if (delivery_address_provider.insertDeliveryAddress(delivery_address, ref delivery_address_id))
                {
                    order.delivery_address_id = delivery_address_id;
                }
            }

            if (order_provider.insertOrder(order, ref order_id_ref))
            {
                if (list_product_variation_in_order != null)
                {
                    foreach (var product_variation_in_order in list_product_variation_in_order)
                    {
                        order_provider.insertProductVariationInOrder(product_variation_in_order.product_variation_id,
                            product_variation_in_order.product_variation_quantity, order_id_ref);
                    }
                }

                if (list_custom_product_in_order != null)
                {
                    foreach (var custom_product_in_order in list_custom_product_in_order)
                    {
                        custom_product_in_order cp_in_order = new custom_product_in_order();
                        cp_in_order.custom_product_in_order_name = custom_product_in_order.custom_product_in_order_name;
                        cp_in_order.custom_product_in_order_quantity = custom_product_in_order.custom_product_in_order_quantity;
                        cp_in_order.order_id = order_id_ref;
                        cp_in_order.custom_product_in_order_price = custom_product_in_order.custom_product_in_order_price;
                        cp_in_order.unit_name = custom_product_in_order.unit_name;
                        order_provider.insertCustomProductInOrder(cp_in_order);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo đơn hàng " + "#" + order_id_ref + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order_id_ref;
                system_log.system_log_url = "/Order/OrderDetail";
                system_log_provider.insertSystemLog(system_log);


                int order_log_id_1 = 0;
                int order_log_id_2 = 0;
                int order_log_id_3 = 0;
                order_log order_log_1 = new order_log();
                order_log_1.order_id = order_id_ref;
                order_log_1.order_log_description = "<strong>" + getUserLoged().username + "</strong>" + " đã tạo đơn hàng #" + "<strong>" + order_id_ref + "</strong>";
                order_log_1.order_log_create_datetime = DateTime.Now;
                order_log_1.order_log_type = "create";
                if (order_log_provider.insertOrderLog(order_log_1, ref order_log_id_1))
                {
                    order_log order_log_2 = new order_log();
                    order_log_2.order_id = order_id_ref;
                    order_log_2.order_log_description = "Đơn hàng được xác thực bởi " + "<strong>" + getUserLoged().username + "</strong>";
                    order_log_2.order_log_create_datetime = DateTime.Now;
                    order_log_2.order_log_type = "confirm";
                    order_log_provider.insertOrderLog(order_log_2, ref order_log_id_2);

                    //var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

                    string currency = String.Format(elGR, "{0:0,0}", amount_received);
                    order_log order_log_3 = new order_log();
                    order_log_3.order_id = order_id_ref;
                    order_log_3.order_log_description = "Thanh toán " + "<strong>" + currency + " ₫</strong> đang chờ xử lý";
                    order_log_3.order_log_create_datetime = DateTime.Now;
                    order_log_3.order_log_type = "payment";
                    if (order_log_provider.insertOrderLog(order_log_3, ref order_log_id_3))
                    {
                        payment_information pi = new payment_information();
                        pi.order_log_id = order_log_id_3;
                        if (order.payment_method_id == 3)
                        {
                            pi.description = "Trì hoãn Thanh toán khi giao hàng (COD) từ người mua";
                        }
                        else
                        {
                            pi.description = "Trì hoãn Chuyển khoản ngân hàng từ người mua";
                        }
                        pi.value_money = amount_received;
                        if (order.payment_method_id == 3)
                        {
                            pi.pay_gate = "Thanh toán khi giao hàng (COD)";
                        }
                        else
                        {
                            pi.pay_gate = "Chuyển khoản qua ngân hàng";
                        }

                        pi.pay_status = false;
                        pi.pay_type = "Chờ xử lý";
                        pi.user_id = getUserLoged().user_id;
                        pi.datetime_confirm = DateTime.Now;
                        payment_information_provider.insertPaymentInformation(pi);
                    }
                }

            }
            return Json(new { order_id = order_id_ref }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateTransport(decimal get_cost_cod, string transport_description,
            float package_lenght, float package_width, float package_height, float package_weight,
            string waybill_code, long order_id, bool allow_check_product, string transport_service, string transport_method,
            string warehouse_to_get)
        {
            transport model = new transport();
            model.get_cost_cod = get_cost_cod;
            model.transport_description = transport_description;
            model.package_lenght = package_lenght;
            model.package_width = package_width;
            model.package_height = package_height;
            model.package_weight = package_weight;
            model.waybill_code = waybill_code;
            model.allow_check_product = allow_check_product;
            model.order_id = order_id;
            model.warehouse_to_get = warehouse_to_get;
            model.transport_method = transport_method;
            model.transport_service = transport_service;
            model.user_id_create = getUserLoged().user_id;
            model.user_id_update = getUserLoged().user_id;
            model.create_datetime = DateTime.Now;
            model.update_datetime = DateTime.Now;
            model.compare_waybill_code_status = false;
            model.shipping_status_id = 1;
            model.shipping_fee = order_provider.getById(order_id).cost_of_shipping;
            model.get_money_status_id = false;

            if (model.package_weight == null)
            {
                model.package_weight = 0;
            }
            if (model.package_width == null)
            {
                model.package_width = 0;
            }
            if (model.package_lenght == null)
            {
                model.package_lenght = 0;
            }
            if (model.package_height == null)
            {
                model.package_height = 0;
            }

            long transport_id_ref = 0;
            if (transport_provider.insertTransport(model, ref transport_id_ref))
            {
                order_log ol = new order_log();
                ol.order_id = order_id;
                ol.order_log_create_datetime = DateTime.Now;
                ol.order_log_type = "transport";
                ol.order_log_description = "Đơn hàng và các sản phẩm đã được gửi đến nhà vận chuyển";
                int order_log_id_ref = 0;
                order_log_provider.insertOrderLog(ol, ref order_log_id_ref);

                transport_log tl = new transport_log();
                tl.transport_id = transport_id_ref;
                tl.transport_log_description = "Phiếu vận chuyển được tạo từ đơn hàng <strong>#" + order_id + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo vận chuyển cho đơn hàng " + "#" + order_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = order_id;
                system_log.system_log_url = "/Order/OrderDetail";
                system_log_provider.insertSystemLog(system_log);

                order_provider.updateInfoOfUpdate(order_id, getUserLoged().user_id, DateTime.Now);
                ViewData["ListOrderLog"] = order_log_provider.getByOrderId(order_id);
            }
            return Json(new { transport_id = transport_id_ref, user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmPayment(long order_id, decimal amount_received)
        {
            var order = order_provider.getById(order_id);
            int order_log_id = 0;
            if (order_provider.updatePaymentStatus(order_id, getUserLoged().user_id, DateTime.Now))
            {
                //var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                string currency = String.Format(elGR, "{0:0,0}", amount_received);
                order_log order_log = new order_log();
                order_log.order_id = order_id;
                order_log.order_log_description = "Xác nhận thanh toán thành công " + "<strong>" + currency + " ₫</strong>";
                order_log.order_log_create_datetime = DateTime.Now;
                order_log.order_log_type = "payment";
                if (order_log_provider.insertOrderLog(order_log, ref order_log_id))
                {
                    payment_information pi = new payment_information();
                    pi.order_log_id = order_log_id;
                    if (order.payment_method_id == 3)
                    {
                        pi.description = "Xác nhận Thanh toán khi giao hàng (COD) đã nhận";
                    }
                    else
                    {
                        pi.description = "Xác nhận Chuyển khoản qua ngân hàng đã nhận";
                    }
                    pi.value_money = amount_received;
                    if (order.payment_method_id == 3)
                    {
                        pi.pay_gate = "Thanh toán khi giao hàng (COD)";
                    }
                    else
                    {
                        pi.pay_gate = "Chuyển khoản qua ngân hàng";
                    }
                    pi.pay_status = true;
                    pi.pay_type = "Xác nhận thanh toán";
                    pi.user_id = getUserLoged().user_id;
                    pi.datetime_confirm = DateTime.Now;
                    payment_information_provider.insertPaymentInformation(pi);

                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã cập nhật đơn hàng " + "#" + order_id + "";
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = order_id;
                    system_log.system_log_url = "/Order/OrderDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json(new { order_log_id = order_log_id, user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ConfirmOrder(long order_id)
        {
            var order = order_provider.getById(order_id);
            int order_log_id = 0;
            int order_log2_id = 0;
            if (order_provider.updateConfirmStatus(order_id, getUserLoged().user_id, DateTime.Now))
            {
                if (order.customer.marketing_status == true && (order.customer.customer_email != null || order.customer.customer_email != ""))
                {
                    string email_receiver = order.customer.customer_email;
                    decimal order_value = statistics_provider.getRevenuePerOrder(order.order_id);
                    string order_reason_promotion = "";
                    if (order.reason_of_promotion != null)
                    {
                        order_reason_promotion = order.reason_of_promotion;
                    }
                    decimal order_total_sum_cost = statistics_provider.getTotalValuePerOrder(order.order_id);
                    string store_website = System.Configuration.ConfigurationManager.AppSettings["store_website"];
                    string store_name = System.Configuration.ConfigurationManager.AppSettings["store_name"];
                    string store_logo_url = System.Configuration.ConfigurationManager.AppSettings["store_logo_url"];
                    string store_color = System.Configuration.ConfigurationManager.AppSettings["store_color"];
                    string store_email = System.Configuration.ConfigurationManager.AppSettings["store_email"];
                    string store_phone_number = System.Configuration.ConfigurationManager.AppSettings["store_phone_number"];
                    string order_detail_url = store_website;
                    //string order_detail_url = store_website + "chi-tiet-don-hang-" + order_id;

                    string customer_fullname = "";
                    if (order.customer.customer_first_name != null && order.customer.customer_last_name != null)
                    {
                        customer_fullname = order.customer.customer_first_name + order.customer.customer_last_name;
                    }
                    else
                    {
                        if (order.customer.customer_first_name == null)
                        {
                            customer_fullname = order.customer.customer_last_name;
                        }
                        else
                        {
                            customer_fullname = order.customer.customer_email;
                        }
                    }
                    string customer_phone_number = "";
                    if (order.customer.customer_phone_number != null)
                    {
                        customer_phone_number = order.customer.customer_phone_number;
                    }

                    string da_detail = order.delivery_address.address_detail + ", " + order.delivery_address.ward.Type + " " + order.delivery_address.ward.Name +
                        ", " + order.delivery_address.district.Type + " " + order.delivery_address.district.Name +
                        ", " + order.delivery_address.province.Name + ", Việt Nam";

                    await SendEmailConfirmOrder(email_receiver, order_id, order.create_datetime, (decimal)order.cost_of_shipping, order.payment_method.payment_method_description,
                        order_value, (decimal)order.discount_amount, order_reason_promotion, order_total_sum_cost, order_detail_url, store_website, store_name, store_logo_url, store_color,
                        store_email, store_phone_number,
                        customer_fullname, order.customer.customer_email, customer_phone_number,
                        order.delivery_address.last_name, da_detail, order.delivery_address.phone_number, ContentEmailListProduct(order_id));
                }

                order_log order_log = new order_log();
                order_log.order_id = order_id;
                order_log.order_log_description = "Đơn hàng được xác thực bởi " + "<strong>" + getUserLoged().username + "</strong>";
                order_log.order_log_create_datetime = DateTime.Now;
                order_log.order_log_type = "confirm";
                if (order.customer.marketing_status == true && (order.customer.customer_email != null || order.customer.customer_email != ""))
                {
                    order_log order_log2 = new order_log();
                    order_log2.order_id = order_id;
                    order_log2.order_log_description = "Email xác nhận đơn hàng đã được gửi đến khách hàng";
                    order_log2.order_log_create_datetime = DateTime.Now;
                    order_log2.order_log_type = "send_email";
                    order_log_provider.insertOrderLog(order_log2, ref order_log2_id);
                }
                if (order_log_provider.insertOrderLog(order_log, ref order_log_id))
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã cập nhật đơn hàng " + "#" + order_id + "";
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = order_id;
                    system_log.system_log_url = "/Order/OrderDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json(new { order_log_id = order_log_id, user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Thay đổi content và gửi Email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailConfirmOrder(string email_receiver, long order_id, DateTime order_create_datetime,
            decimal order_shipping_fee, string order_payment_method,
            decimal order_value, decimal order_discount, string order_reason_promotion,
            decimal order_sum_cost, string order_detail_url,

            string store_website, string store_name, string store_logo_url, string store_color, string store_email, string store_phone_number,

            string customer_fullname, string customer_email, string customer_phone_number,

            string delivery_address_fullname, string delivery_address_detail, string delivery_address_phone_number,

            string order_list_product)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/EmailConfirmOrder.html"));

            string discount = "";
            if (order_discount == 0)
            {
                discount = "0 ₫";
            }
            else
            {
                discount = FormatCurrency(order_discount);
            }

            content = content.Replace("{{order_id}}", order_id.ToString());
            content = content.Replace("{{order_create_datetime}}", "(Ngày " + order_create_datetime.ToString("dd/MM/yyy") + " lúc " + order_create_datetime.ToString("HH:mm") + ")");
            content = content.Replace("{{order_shipping_fee}}", FormatCurrency(order_shipping_fee));
            content = content.Replace("{{order_payment_method}}", order_payment_method);
            content = content.Replace("{{order_value}}", FormatCurrency(order_value));
            content = content.Replace("{{order_discount}}", discount);
            content = content.Replace("{{order_reason_promotion}}", order_reason_promotion);
            content = content.Replace("{{order_sum_cost}}", FormatCurrency(order_sum_cost));
            content = content.Replace("{{order_detail_url}}", order_detail_url);

            content = content.Replace("{{store_website}}", store_website);
            content = content.Replace("{{store_name}}", store_name);
            content = content.Replace("{{store_logo_url}}", store_logo_url);
            content = content.Replace("{{store_color}}", store_color);
            content = content.Replace("{{store_email}}", store_email);
            content = content.Replace("{{store_phone_number}}", store_phone_number);

            content = content.Replace("{{customer_fullname}}", customer_fullname);
            content = content.Replace("{{customer_email}}", customer_email);
            content = content.Replace("{{customer_phone_number}}", customer_phone_number);

            content = content.Replace("{{delivery_address_fullname}}", delivery_address_fullname);
            content = content.Replace("{{delivery_address_detail}}", delivery_address_detail);
            content = content.Replace("{{delivery_address_phone_number}}", delivery_address_phone_number);

            content = content.Replace("{{order_list_product}}", order_list_product);

            await new MailHelper().SendMail(email_receiver, "Xác nhận đơn hàng #" + order_id.ToString(), content);
            return true;
        }

        public string ContentEmailListProduct(long order_id)
        {
            string content = "";
            var list_pvio = pvio_provider.getByOrderId(order_id);
            var list_cpio = cpio_provider.getByOrderId(order_id);

            if (list_pvio != null)
            {
                foreach (var pv in list_pvio)
                {
                    string pv_name = "";
                    if (pv.product_variation_name == "Default Title")
                    {
                        pv_name = pv.product_name;
                    }
                    else
                    {
                        pv_name = pv.product_name + " - " + pv.product_variation_name;
                    }
                    content += "<tr>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + pv.product_variation.product.product_code + " </span></td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + ">" +
                              "<span> " + pv_name + " </span>" +
                              "</td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)pv.product_variation_price) + " </span></td>" +
                              "<td align = " + "center" + " style = " + "padding:3px 9px" + " valign = " + "top" + "> " + pv.product_variation_quantity + " </td>" +
                              "<td align = " + "right" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)(pv.product_variation_quantity * pv.product_variation_price)) + " </span></td>" +
                              "</tr> ";
                }
            }

            if (list_cpio != null)
            {
                foreach (var cp in list_cpio)
                {

                    content += "<tr>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + cp.custom_product_in_order_id + " </span></td>" +
                              "<td align = " + "left" + " style = " + "padding:3px 9px" + " valign = " + "top" + ">" +
                              "<span> " + cp.custom_product_in_order_name + " </span>" +
                              "</td>" +
                              "<td align = " + "left" + "style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)cp.custom_product_in_order_price) + " </span></td>" +
                              "<td align = " + "center" + "style = " + "padding:3px 9px" + " valign = " + "top" + "> " + cp.custom_product_in_order_quantity + " </td>" +
                              "<td align = " + "right" + "style = " + "padding:3px 9px" + " valign = " + "top" + "><span> " + FormatCurrency((decimal)(cp.custom_product_in_order_quantity * cp.custom_product_in_order_price)) + " </span></td>" +
                              "</tr> ";
                }
            }
            return content;
        }

        public string FormatCurrency(decimal value)
        {
            return (String.Format(elGR, "{0:0,0}", value) + " ₫");
        }

        public ActionResult PrintInvoice(long order_id)
        {
            var order = order_provider.getById(order_id);
            return PartialView("_PartialViewInvoice", order);
        }

        [HttpPost]
        public ActionResult DeleteOrder(long order_id)
        {
            if (order_provider.deleteOrder(order_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa đơn hàng #" + order_id;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_url = "/Order/Index";
                system_log_provider.insertSystemLog(system_log);
            }

            return RedirectToAction("Index", "Order");
        }
    }
}