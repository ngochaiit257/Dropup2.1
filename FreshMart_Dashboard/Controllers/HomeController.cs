using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Data;
using Data.Providers;
using ExtendMethod;

namespace DropupManagement.Controllers
{
    public class HomeController : Controller
    {
        StatisticsProvider statistics_provider = new StatisticsProvider();
        GetNumberWeek get_number_week = new GetNumberWeek();
        OrderProvider order_provider = new OrderProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        ProductProvider product_provider = new ProductProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        StoreMessageProvider sm_provider = new StoreMessageProvider();

        //[OutputCache(Duration = 259200, VaryByParam = "none", SqlDependency = "Dropup:order;Dropup:product;Dropup:product_variation;Dropup:product_variation_in_order;Dropup:system_log;Dropup:store_message;Dropup:system_notification")]
        public ActionResult Index()
        {
            //Statistic 1
            int getNumberOfOrdersThisMonth = 0;
            int getNumberOfOrdersLastMonth = 0;
            if (statistics_provider.numberOfOrdersByTimeLine("this_month").Count() > 0)
            {
                getNumberOfOrdersThisMonth = statistics_provider.numberOfOrdersByTimeLine("this_month").Sum();
            }
            if (statistics_provider.numberOfOrdersByTimeLine("last_month").Count() > 0)
            {
                getNumberOfOrdersLastMonth = statistics_provider.numberOfOrdersByTimeLine("last_month").Sum();
            }

            ViewBag.GetNumberOfOrdersByThisMonth = getNumberOfOrdersThisMonth;
            ViewBag.GetNumberOfOrdersByLastMonth = getNumberOfOrdersLastMonth;
            if (getNumberOfOrdersLastMonth > 0)
            {
                ViewData["Growth"] = Math.Round(((double)(100 * Math.Abs(getNumberOfOrdersThisMonth - getNumberOfOrdersLastMonth)) / getNumberOfOrdersLastMonth), 2);
            }

            //Statistic 2
            Decimal getRevenueThisMonth = statistics_provider.getRevenueByTimeLine("this_month").Sum();
            Decimal getRevenueLastMonth = statistics_provider.getRevenueByTimeLine("last_month").Sum();

            ViewBag.GetRevenueThisMonth = getRevenueThisMonth;
            ViewBag.GetRevenueLastMonth = getRevenueLastMonth;
            ViewBag.GetNumberWeek = get_number_week.WeekNumber(System.DateTime.Now);
            if (getRevenueLastMonth != 0)
            {
                ViewData["GrowthRevenueThisMonth"] = Math.Round(((double)(100 * Math.Abs((int)(getRevenueThisMonth - getRevenueLastMonth) / getRevenueLastMonth))), 2);
            }

            ////Statistic 3
            ViewBag.AllOrder = 0;
            if (order_provider.getAllOfficialOrder().Count() > 0)
            {
                ViewBag.AllOrder = order_provider.getAllOfficialOrder().Count();
            }

            ////Log
            ViewData["SystemLog"] = system_log_provider.getAll().Take(50).ToList();


            return View();
        }

        //public ActionResult Index()
        //{
        //    //Statistic 1
        //    int getNumberOfOrdersThisMonth = 0;
        //    int getNumberOfOrdersLastMonth = 0;
        //    if (statistics_provider.numberOfOrdersByTimeLine("this_month") != null)
        //    {
        //        getNumberOfOrdersThisMonth = statistics_provider.numberOfOrdersByTimeLine("this_month").Sum();
        //    }
        //    if (statistics_provider.numberOfOrdersByTimeLine("last_month") != null)
        //    {
        //        getNumberOfOrdersLastMonth = statistics_provider.numberOfOrdersByTimeLine("last_month").Sum();
        //    }

        //    ViewBag.GetNumberOfOrdersByThisMonth = getNumberOfOrdersThisMonth;
        //    ViewBag.GetNumberOfOrdersByLastMonth = getNumberOfOrdersLastMonth;
        //    if (getNumberOfOrdersLastMonth != 0)
        //    {
        //        ViewData["Growth"] = Math.Round(((double)(100 * Math.Abs(getNumberOfOrdersThisMonth - getNumberOfOrdersLastMonth)) / getNumberOfOrdersLastMonth), 2);
        //    }

        //    //Statistic 2
        //    Decimal getRevenueThisMonth = statistics_provider.getRevenueByTimeLine("this_month").Sum();
        //    Decimal getRevenueLastMonth = statistics_provider.getRevenueByTimeLine("last_month").Sum();

        //    ViewBag.GetRevenueThisMonth = getRevenueThisMonth;
        //    ViewBag.GetRevenueLastMonth = getRevenueLastMonth;
        //    ViewBag.GetNumberWeek = get_number_week.WeekNumber(System.DateTime.Now);
        //    if (getRevenueLastMonth != 0)
        //    {
        //        ViewData["GrowthRevenueThisMonth"] = Math.Round(((double)(100 * Math.Abs((int)(getRevenueThisMonth - getRevenueLastMonth) / getRevenueLastMonth))), 2);
        //    }

        //    ////Statistic 3
        //    ViewBag.AllOrder = 0;
        //    if (order_provider.getAllOfficialOrder() != null)
        //    {
        //        ViewBag.AllOrder = order_provider.getAllOfficialOrder().Count();
        //    }

        //    ////Log
        //    ViewData["SystemLog"] = system_log_provider.getAll();


        //    return View();
        //}

        //[ChildActionOnly]
        //[OutputCache(Duration = 259200, VaryByParam = "none", SqlDependency = "Dropup:product_variation_in_order")]
        public ActionResult GetTop10ProductVariationSale()
        {
            ViewData["GetTop10ProductVariationSale"] = pv_provider.getAll().OrderByDescending(pv => pv.product.create_date).Take(10);
            //ViewData["GetTop10ProductVariationSale"] = statistics_provider.getTop10ProductVariationSale();
            return PartialView("_PartialViewTop10ProductSale");
        }

        //[OutputCache(Duration = 259200, VaryByParam = "none", SqlDependency = "Dropup:order")]
        public virtual ActionResult GetNumberOfOrdersByTimeLine()
        {
            int[] arr = statistics_provider.numberOfOrdersByTimeLine("this_month");
            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(Duration = 259200, VaryByParam = "none", SqlDependency = "Dropup:order")]
        public virtual ActionResult GetRevenueByTimeLine()
        {
            List<decimal> list_revenue = new List<decimal>();
            foreach (var revenue in statistics_provider.getListRevenueFromFirstDateOfMonthToCurrent())
            {
                list_revenue.Add(revenue);
            }
            Decimal[] arr = list_revenue.ToArray();
            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetOrderByHour()
        {
            int[] arr = statistics_provider.getOrderByHour();
            return Json(arr, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetNotifications()
        //{
        //    var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
        //    PushNotification PN = new PushNotification();
        //    var list = PN.GetData(notificationRegisterTime);

        //    //update session here for get only new added notification (notification)  
        //    Session["LastUpdate"] = DateTime.Now;
        //    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public virtual ActionResult GetNotifications()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            PushNotification PN = new PushNotification();
            var list = PN.GetData(notificationRegisterTime);

            //update session here for get only new added notification (notification)  
            Session["LastUpdate"] = DateTime.Now;
            return Json(new { Data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStoreMessage()
        {
            var list = sm_provider.getAll();
            ViewBag.CountNotActive = sm_provider.getListNotActive();
            return PartialView("_PartialViewStoreMessage", list);
        }

        public ActionResult UpdateReadingStatus(long mess_id)
        {
            sm_provider.updateReadingStatus(mess_id);
            return Json("Update Success!");
        }
    }
}