using System.Web;
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
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/swiper.min8cea.js",
                "~/Assets/js/jquery.fancybox.min8cea.js",
                "~/Assets/js/jquery.flexslider8cea.js",
                "~/Assets/js/plugins8cea.js",
                "~/Assets/js/scripts8cea.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.lazyload.js",
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
           "~/Assets/css/PagedList.css",
           "~/Assets/css/custom.css"
           "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
