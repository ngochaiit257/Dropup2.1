using System.Web;
using System.Web.Optimization;

namespace ClinicBachGiai
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
                "~/Assets/js/jquery.min.js"
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
           "~/Assets/css/animate.min.css",
           "~/Assets/css/meanmenu.css",
           "~/Assets/css/boxicons.min.css",
           "~/Assets/css/flaticon.css",
           "~/Assets/css/odometer.min.css",
           "~/Assets/css/owl.carousel.min.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/owl.theme.default.min.css",
           "~/Assets/css/magnific-popup.min.css",
           "~/Assets/css/nice-select.css",
           "~/Assets/css/datetimepicker.min.css",
           "~/Assets/css/month.min.css",
           "~/Assets/css/imagelightbox.min.css",
           "~/Assets/css/style.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
