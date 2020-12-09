using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data;
using Data.Providers;

namespace DropupManagement.Controllers
{
    public class TransportController : Controller
    {
        TransportProvider transport_provider = new TransportProvider();
        OrderProvider order_provider = new OrderProvider();
        TransportLogProvider transport_log_provider = new TransportLogProvider();
        ShippingStatusProvider shipping_status_provider = new ShippingStatusProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        DeliveryAddressProvider delivery_address_provider = new DeliveryAddressProvider();
        CustomerProvider customer_provider = new CustomerProvider();

        CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
        // GET: Transport
        public ActionResult Index()
        {
            ViewBag.Qua24GioChuaLay = order_provider.getListOrderQua24GioChuaLayHang().Count();
            ViewBag.ChuaTaoVanChuyen = order_provider.getListOrderHaveNotTransport(order_provider.getAllOfficialOrder()).Count();
            return View();
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult TransportDetail(long id)
        {
            var model = transport_provider.getById(id);
            decimal total_price_list_pv = 0;
            decimal total_price_list_cp = 0;
            ViewBag.Order = order_provider.getById(model.order_id);
            ViewData["ListProductVariationInOrder"] = order_provider.getProductVariationInOrderByOrderId(model.order_id, ref total_price_list_pv);
            ViewData["ListCustomProductInOrder"] = order_provider.getCustomProductInOrderByOrderId(model.order_id, ref total_price_list_cp);
            ViewBag.TotalPriceAllProduct = total_price_list_cp + total_price_list_pv;
            ViewBag.CostCOD = model.get_cost_cod;
            ViewData["ListDateTimeTransportLog"] = transport_log_provider.getDateTimeOfOrderLogByTransportId(id);
            ViewData["ListShippingStatus"] = shipping_status_provider.getAll();

            var order = order_provider.getOfficialOrderById(model.order_id);
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
            return View(model);
        }

        public ActionResult UpdateShippingStatus(long transport_id, int shipping_status_id)
        {
            if (transport_provider.updateShippingStatus(transport_id, shipping_status_id))
            {
                transport_provider.updateUserDatetimeUpdate(transport_id, getUserLoged().user_id, DateTime.Now);

                transport_log tl = new transport_log();
                tl.transport_id = transport_id;
                tl.transport_log_description = "Cập nhật trạng thái vận chuyển: <strong>" + shipping_status_provider.getNameShippingStatus(shipping_status_id) + "</strong>. Người cập nhận: <strong>" + getUserLoged().username + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật vận chuyển " + "#T" + transport_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = transport_id;
                system_log.system_log_url = "/Transport/TransportDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { shipping_status_name = shipping_status_provider.getNameShippingStatus(shipping_status_id), user_update = getUserLoged().username },
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateGetMoneyCODStatus(long transport_id)
        {
            if (transport_provider.updateGetMoneyStatus(transport_id))
            {
                transport_provider.updateUserDatetimeUpdate(transport_id, getUserLoged().user_id, DateTime.Now);

                transport_log tl = new transport_log();
                tl.transport_id = transport_id;
                tl.transport_log_description = "Cập nhật trạng thái thu hộ (COD): <strong>Đã nhận</strong>. Người cập nhập: <strong>" + getUserLoged().username + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật vận chuyển " + "#T" + transport_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = transport_id;
                system_log.system_log_url = "/Transport/TransportDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCompareWaybill(long transport_id)
        {
            if (transport_provider.updateCompareWaybillCodeStatus(transport_id))
            {
                transport_provider.updateUserDatetimeUpdate(transport_id, getUserLoged().user_id, DateTime.Now);

                transport_log tl = new transport_log();
                tl.transport_id = transport_id;
                tl.transport_log_description = "Cập nhật trạng thái đối soát: <strong>Đã đối soát</strong>. Người cập nhập: <strong>" + getUserLoged().username + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật vận chuyển " + "#T" + transport_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = transport_id;
                system_log.system_log_url = "/Transport/TransportDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateShippingFee(long transport_id, decimal shipping_fee)
        {
            var transport = transport_provider.getById(transport_id);
            decimal shipping_fee_before = (decimal)transport.shipping_fee;
            if (transport_provider.updateShippingFee(transport_id, shipping_fee))
            {
                transport_provider.updateUserDatetimeUpdate(transport_id, getUserLoged().user_id, DateTime.Now);

                //var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                transport_log tl = new transport_log();
                tl.transport_id = transport_id;
                string currency_shipping_fee_before = String.Format(elGR, "{0:0,0}", shipping_fee_before);
                string currency_shipping_fee = String.Format(elGR, "{0:0,0}", shipping_fee);
                if (shipping_fee_before == 0)
                {
                    currency_shipping_fee_before = "0";
                }
                if (shipping_fee == 0)
                {
                    currency_shipping_fee = "0";
                }
                tl.transport_log_description = "Cập nhật phí vận chuyển từ <strong>"+ currency_shipping_fee_before + " ₫</strong> sang <strong>" + currency_shipping_fee + " ₫</strong>. Người cập nhập: <strong>" + getUserLoged().username + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật vận chuyển " + "#T" + transport_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = transport_id;
                system_log.system_log_url = "/Transport/TransportDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateWaybillCode(long transport_id, string waybill_code)
        {
            if (transport_provider.updateWaybillCode(transport_id, waybill_code))
            {
                transport_provider.updateUserDatetimeUpdate(transport_id, getUserLoged().user_id, DateTime.Now);

                transport_log tl = new transport_log();
                tl.transport_id = transport_id;
                tl.transport_log_description = "Cập nhật mã vận đơn thành <strong>" + waybill_code + "</strong>. Người cập nhập: <strong>" + getUserLoged().username + "</strong>";
                tl.transport_log_create_datetime = DateTime.Now;
                transport_log_provider.insertTransportLog(tl);

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật vận chuyển " + "#T" + transport_id + "";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = transport_id;
                system_log.system_log_url = "/Transport/TransportDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { user_update = getUserLoged().username }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransportLog(long transport_id)
        {
            var list = transport_log_provider.getDateTimeOfOrderLogByTransportId(transport_id);
            ViewBag.TransportId = transport_id;
            return PartialView("_PartialViewTransportLog", list);
        }
        public ActionResult AddTransportLog(long transport_id, string content)
        {
            transport_log tl = new transport_log();
            tl.transport_id = transport_id;
            tl.transport_log_description = "<strong>" + getUserLoged().username + ":</strong> " + content;
            tl.transport_log_create_datetime = DateTime.Now;
            transport_log_provider.insertTransportLog(tl);

            return Json("Insert Success");
        }
    }
}