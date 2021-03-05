using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace EPlus.Controllers
{
    public class CourseController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CourseDetail(string seo_alias)
        {
            return View();
        }
    }
}