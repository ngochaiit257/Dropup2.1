using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElysienVietNam.Controllers
{
    public class BlogController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlogDetail()
        {
            ViewData["ListMyPham"] = product_provider.getByCategoryIdOnSite(55);
            ViewData["ListMayMoc"] = product_provider.getByCategoryIdOnSite(56);
            ViewData["ListDacBiet"] = product_provider.getByCategoryIdOnSite(57);
            return View();
        }
    }
}