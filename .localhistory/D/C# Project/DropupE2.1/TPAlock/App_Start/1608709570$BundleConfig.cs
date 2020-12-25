﻿using System.Web;
using System.Web.Optimization;

namespace TPAlock
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
                "~/Assets/vendor/jquery-1.12.4.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsCoreSecond").Include(
                "~/Assets/js/vendor/modernizr-3.5.0.min.js",
                "~/Assets/js/popper.min.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.inputarrow.js",
                "~/Assets/js/jquery.nice-select.min.js",
                "~/Assets/js/jquery.fancybox.min.js",
                "~/Assets/js/slick.min.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/isotope.pkgd.min.js",
                "~/Assets/js/plugins.js",
                "~/Assets/js/countdown.min.js",
                "~/Assets/js/jquery.meanmenu.min.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/main.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/bootstrap.min.css",
           "~/Assets/css/fontawesome-all.min.css",
           "~/Assets/css/slick.css",
           "~/Assets/css/magnific-popup.html",
           "~/Assets/css/nice-select.css",
           "~/Assets/css/animate.css",
           "~/Assets/css/meanmenu.css",
           "~/Assets/css/flaticon.css",
           "~/Assets/css/jquery.fancybox.min.css",
           "~/Assets/css/icomoon.css",
           "~/Assets/css/default.css",
           "~/Assets/css/style.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
