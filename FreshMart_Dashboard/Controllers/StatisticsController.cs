using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;
using Data.Models;

namespace DropupManagement.Controllers
{
    public class StatisticsController : Controller
    {
        StatisticsProvider statistics_provider = new StatisticsProvider();
        // GET: Statistics
        public ActionResult Index()
        {
            ViewData["ListStatisticsThisMonth"] = statistics_provider.getListRevenueThisMonthToShow();
            return View();
        }

        public virtual ActionResult GetListRevenueFromFirstDateOfMonthToCurrent()
        {
            List<string> list_day = new List<string>();
            List<decimal> list_revenue = new List<decimal>();
            foreach (var day in statistics_provider.getListDateFromFirstDateOfMonthToCurrent())
            {
                list_day.Add(day.Date.ToString());
            }
            foreach (var revenue in statistics_provider.getListRevenueFromFirstDateOfMonthToCurrent())
            {
                list_revenue.Add(revenue);
            }
            return Json(new { arr_day = list_day.ToArray(), arr_revenue = list_revenue.ToArray() }, JsonRequestBehavior.AllowGet);
        }
    }
}