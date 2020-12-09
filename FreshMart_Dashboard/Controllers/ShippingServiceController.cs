using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;
using System.Web.Security;
using DropupManagement.Models;

namespace DropupManagement.Controllers
{
    public class ShippingServiceController : Controller
    {
        LocalProvider local_provider = new LocalProvider();
        ShippingServiceProvider shipping_service_provider = new ShippingServiceProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }
        // GET: ShippingService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertShippingService()
        {
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            ViewData["ListLocalShippingService"] = shipping_service_provider.getAllLocalShippingService();
            ViewData["ListStandardShipping"] = shipping_service_provider.getAllStandardForShipping();
            return View();
        }

        [HttpPost]
        public ActionResult InsertShippingService(int province_id)
        {
            LocalShippingServiceModel lssm = new LocalShippingServiceModel();
            CostOfLocalShippingModel colsm = new CostOfLocalShippingModel();
            StandardForShippingModel sfsm = new StandardForShippingModel();

            local_shipping_service model = new local_shipping_service();
            model.province_id = province_id;

            int local_shipping_service_id = 0;
            if (shipping_service_provider.insertLocalShippingService(model, ref local_shipping_service_id))
            {
                cost_of_local_shipping obj = new cost_of_local_shipping();
                obj.name_of_cost = "Giao hàng tận nơi";
                obj.local_shipping_service_id = local_shipping_service_id;
                obj.price_of_cost = 0;
                obj.standard_for_shipping_id = 1;
                obj.min_value_for_standard = 0;
                obj.max_value_for_standard = 0;

                int cost_of_local_shipping_id = 0;
                if (shipping_service_provider.insertCostOfLocalShipping(obj, ref cost_of_local_shipping_id))
                {
                    if (shipping_service_provider.insertCostForDistrictShipping(cost_of_local_shipping_id, 0, province_id))
                    {
                        system_log sl = new system_log();
                        sl.system_log_description = "đã thêm vận chuyển khu vực " + local_provider.getNameOfProvinceByProvinceId(province_id);
                        sl.user_id = getUserLoged().user_id;
                        sl.system_log_time = System.DateTime.Now;
                        sl.system_log_url = "/ShippingService/InsertShippingService";
                        system_log_provider.insertSystemLog(sl);

                        lssm.local_shipping_service_id = local_shipping_service_id;
                        lssm.province_id = province_id;
                        lssm.province_name = local_provider.getNameOfProvinceByProvinceId(province_id);
                        //---------------
                        colsm.cost_of_local_shipping_id = cost_of_local_shipping_id;
                        colsm.local_shipping_service_id = local_shipping_service_id;
                        colsm.name_of_cost = obj.name_of_cost;
                        colsm.standard_for_shipping_id = 1;
                        colsm.price_of_cost = 0;
                        colsm.min_value_for_standard = 0;
                        colsm.max_value_for_standard = 0;
                        //--------------
                        sfsm.standard_for_shipping_id = 1;
                        sfsm.standard_name = shipping_service_provider.getNameStandardForShippingByStandardId(1);
                    }
                }
            }
            var data = new { lssm = lssm, colsm = colsm, sfsm = sfsm };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool checkDuplicateProvinceShipping(int province_id)
        {
            return shipping_service_provider.checkDuplicateProvinceShipping(province_id);
        }

        [HttpPost]
        public ActionResult GetAllCostForDistrictShippingByCostOfLocalShippingID(int cost_of_local_shipping_id)
        {
            var list_obj = shipping_service_provider.getAllCostForDistrictShippingByCostOfLocalShippingID(cost_of_local_shipping_id);
            var list_return = new List<CostForDistrictShippingModel>();
            foreach (var item in list_obj)
            {
                var model = new CostForDistrictShippingModel();
                model.cost_for_district_shipping_id = item.cost_for_district_shipping_id;
                model.cost_of_local_shipping_id = (Int32)item.cost_of_local_shipping_id;
                model.district_id = (Int32)item.district_id;
                model.price_for_district_shipping_final = (decimal)item.price_for_district_shipping_final;
                model.price_for_district_shipping_adjust = (decimal)item.price_for_district_shipping_adjust;
                model.status_district_shipping = (bool)item.status_district_shipping;
                model.district_name = local_provider.getNameOfDistrictByDistrictId((Int32)item.district_id);
                list_return.Add(model);
            }
            return Json(list_return, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetInfoCostOfLocalShipping(int cost_of_local_shipping_id)
        {
            var list_obj = shipping_service_provider.getAllCostForDistrictShippingByCostOfLocalShippingID(cost_of_local_shipping_id);
            var list_return = new List<CostForDistrictShippingModel>();
            foreach (var item in list_obj)
            {
                var model = new CostForDistrictShippingModel();
                model.cost_for_district_shipping_id = item.cost_for_district_shipping_id;
                model.cost_of_local_shipping_id = (Int32)item.cost_of_local_shipping_id;
                model.district_id = (Int32)item.district_id;
                model.price_for_district_shipping_final = (decimal)item.price_for_district_shipping_final;
                model.price_for_district_shipping_adjust = (decimal)item.price_for_district_shipping_adjust;
                model.status_district_shipping = (bool)item.status_district_shipping;
                model.district_name = local_provider.getNameOfDistrictByDistrictId((Int32)item.district_id);
                list_return.Add(model);
            }

            CostOfLocalShippingModel cols_model = new CostOfLocalShippingModel();
            var cost_of_local_shipping = shipping_service_provider.getCostOfLocalShippingById(cost_of_local_shipping_id);
            cols_model.cost_of_local_shipping_id = cost_of_local_shipping.cost_of_local_shipping_id;
            cols_model.local_shipping_service_id = (Int32)cost_of_local_shipping.local_shipping_service_id;
            cols_model.name_of_cost = cost_of_local_shipping.name_of_cost;
            cols_model.min_value_for_standard = (decimal)cost_of_local_shipping.min_value_for_standard;
            cols_model.max_value_for_standard = (decimal)cost_of_local_shipping.max_value_for_standard;
            cols_model.price_of_cost = (decimal)cost_of_local_shipping.price_of_cost;
            cols_model.standard_name = cost_of_local_shipping.standard_for_shipping.standard_name;
            cols_model.standard_for_shipping_id = (Int32)cost_of_local_shipping.standard_for_shipping_id;

            string province_name = shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id);

            var data = new { cols_model = cols_model, list_cost_for_district = list_return, province_name = province_name };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getInforLocalShippingService(int local_shipping_service_id)
        {
            var model = shipping_service_provider.getLocalShippingServiceById(local_shipping_service_id);
            var data = new { province_name = model.province.Name };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteLocalShippingService(int local_shipping_service_id)
        {
            string province_name = "";
            if (shipping_service_provider.deleteLocalShippingService(local_shipping_service_id, ref province_name))
            {
                system_log sl = new system_log();
                sl.system_log_description = "đã xóa dịch vụ vận chuyển thuộc khu vực " + province_name;
                sl.user_id = getUserLoged().user_id;
                sl.system_log_time = System.DateTime.Now;
                sl.system_log_url = "/ShippingService/InsertShippingService";
                system_log_provider.insertSystemLog(sl);
            }
            return Json(new { province_name = province_name }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertCostOfLocalShipping(int local_shipping_service_id, string name_of_cost, int standard_for_shipping_id,
            decimal min_value_for_standard, decimal max_value_for_standard, decimal price_of_cost)
        {
            cost_of_local_shipping model_to_insert = new cost_of_local_shipping();
            model_to_insert.local_shipping_service_id = local_shipping_service_id;
            model_to_insert.name_of_cost = name_of_cost;
            model_to_insert.standard_for_shipping_id = standard_for_shipping_id;
            model_to_insert.min_value_for_standard = min_value_for_standard;
            model_to_insert.max_value_for_standard = max_value_for_standard;
            model_to_insert.price_of_cost = price_of_cost;

            int cost_of_local_shipping_id = 0;
            if (shipping_service_provider.insertCostOfLocalShipping(model_to_insert, ref cost_of_local_shipping_id))
            {
                int province_id = shipping_service_provider.getProvinceIdByCostOfLocalShippingID(cost_of_local_shipping_id);
                if (shipping_service_provider.insertCostForDistrictShipping(cost_of_local_shipping_id, price_of_cost, province_id))
                {
                    system_log sl = new system_log();
                    sl.system_log_description = "đã thêm giá vận chuyển mới cho khu vực " + shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id);
                    sl.user_id = getUserLoged().user_id;
                    sl.system_log_time = System.DateTime.Now;
                    sl.system_log_url = "/ShippingService/InsertShippingService";
                    system_log_provider.insertSystemLog(sl);
                }
            }
            string province_name = shipping_service_provider.getProvinceNameByLocalShippingServiceID(local_shipping_service_id);
            string standard_name = shipping_service_provider.getNameStandardById(standard_for_shipping_id);
            string unit_standard = "";
            if (standard_for_shipping_id == 1)
            {
                unit_standard = "vnd";
            }
            else
            {
                unit_standard = "gam";
            }
            var data = new { province_name = province_name, cost_of_local_shipping_id = cost_of_local_shipping_id, standard_name = standard_name, unit_standard = unit_standard };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCostOfLocalShipping(int cost_of_local_shipping_id)
        {
            string name_of_cost = "";
            string province_name = "";
            int count_in_list = 0;
            int local_shipping_service_id = 0;
            if (shipping_service_provider.deleteCostOfLocalShipping(cost_of_local_shipping_id, ref name_of_cost, ref province_name, ref count_in_list, ref local_shipping_service_id))
            {
                system_log sl = new system_log();
                sl.system_log_description = "đã xóa dịch vụ vận chuyển " + name_of_cost + " thuộc khu vực " + province_name;
                sl.user_id = getUserLoged().user_id;
                sl.system_log_time = System.DateTime.Now;
                sl.system_log_url = "/ShippingService/InsertShippingService";
                system_log_provider.insertSystemLog(sl);
            }
            return Json(new { name_of_cost = name_of_cost, province_name = province_name, count_in_list = count_in_list, local_shipping_service_id = local_shipping_service_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateTitleAndStandard(int cost_of_local_shipping_id, int standard_for_shipping_id, string name_of_cost, decimal min_value_for_standard, decimal max_value_for_standard)
        {
            if(shipping_service_provider.updateTitleAndStandard(cost_of_local_shipping_id, name_of_cost, standard_for_shipping_id, min_value_for_standard, max_value_for_standard))
            {
                system_log sl = new system_log();
                sl.system_log_description = "đã cập nhật tiêu đề và tiêu chuẩn vận chuyển cho " + name_of_cost + " - " + shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id);
                sl.user_id = getUserLoged().user_id;
                sl.system_log_time = System.DateTime.Now;
                sl.system_log_url = "/ShippingService/InsertShippingService";
                system_log_provider.insertSystemLog(sl);
            }
            return Json(new { name_of_cost = name_of_cost, province_name = shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id) });
        }

        [HttpPost]
        public ActionResult UpdateCostForLocalShippingAndCostForDistrictShipping(int cost_of_local_shipping_id, decimal price_of_cost, 
            List<CostForDistrictShippingModel> list_cost_for_district_shipping)
        {
            var list_to_update = new List<cost_for_district_shipping>();
            foreach(var model in list_cost_for_district_shipping)
            {
                var cost_district_obj = new cost_for_district_shipping();
                cost_district_obj.cost_for_district_shipping_id = model.cost_for_district_shipping_id;
                cost_district_obj.status_district_shipping = model.status_district_shipping;
                cost_district_obj.price_for_district_shipping_adjust = model.price_for_district_shipping_adjust;
                cost_district_obj.price_for_district_shipping_final = model.price_for_district_shipping_adjust + price_of_cost;
                list_to_update.Add(cost_district_obj);
            }
            if(shipping_service_provider.updateCostForLocalShippingAndCostForDistrictShipping(cost_of_local_shipping_id, price_of_cost, list_to_update))
            {
                system_log sl = new system_log();
                sl.system_log_description = "đã cập nhật giá vận chuyển cho " + shipping_service_provider.getCostOfLocalShippingById(cost_of_local_shipping_id).name_of_cost + " - " + shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id);
                sl.user_id = getUserLoged().user_id;
                sl.system_log_time = System.DateTime.Now;
                sl.system_log_url = "/ShippingService/InsertShippingService";
                system_log_provider.insertSystemLog(sl);
            }
            return Json(new {price_of_cost = price_of_cost, name_of_cost = shipping_service_provider.getCostOfLocalShippingById(cost_of_local_shipping_id).name_of_cost, province_name = shipping_service_provider.getProvinceNameByCostOfLocalShippingID(cost_of_local_shipping_id) });
        }

        [HttpPost]
        public ActionResult GetShippingCostForOrderByDistrict(int district_id, decimal value_for_standard)
        {
            var list_return_json = new List<CostForDistrictShippingModel>();
            var list_cost = shipping_service_provider.getCostForDistrictShippingForOrder(district_id, value_for_standard);
            foreach(var obj in list_cost)
            {
                var model = new CostForDistrictShippingModel();
                model.cost_for_district_shipping_id = obj.cost_for_district_shipping_id;
                model.name_of_cost = obj.cost_of_local_shipping.name_of_cost;
                model.price_for_district_shipping_final = (decimal)obj.price_for_district_shipping_final;
                list_return_json.Add(model);
            }
            return Json(list_return_json,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetShippingCostForOrderByProvince(int province_id, decimal value_for_standard)
        {
            var list_return_json = new List<CostOfLocalShippingModel>();
            var list_cost = shipping_service_provider.getCostOfLocalShippingForOrder(province_id, value_for_standard);
            foreach (var obj in list_cost)
            {
                var model = new CostOfLocalShippingModel();
                model.cost_of_local_shipping_id = obj.cost_of_local_shipping_id;
                model.name_of_cost = obj.name_of_cost;
                model.price_of_cost = (decimal)obj.price_of_cost;
                list_return_json.Add(model);
            }
            return Json(list_return_json, JsonRequestBehavior.AllowGet);
        }
    }
}