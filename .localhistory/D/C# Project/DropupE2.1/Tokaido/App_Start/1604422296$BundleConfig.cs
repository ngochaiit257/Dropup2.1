using System.Web;
using System.Web.Optimization;

namespace Tokaido
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
                "~/Assets/js/jquery.datetimepicker.full.min.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.easing.js",
                "~/Assets/js/imagesloaded.min.js",
                "~/Assets/js/jquery.isotope.min.js",
                "~/Assets/js/jquery-waypoints.js",
                "~/Assets/js/jquery.magnific-popup.min.js",
                "~/Assets/js/jquery.flexslider-min.js",
                "~/Assets/js/owl.carousel.js",
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/jquery-validate.js",
                "~/Assets/js/parallax.js",
                "~/Assets/js/jquery.flexslider-min.js",
                "~/Assets/js/gmap3.min.js",
                "~/Assets/js/jquery.fancybox.js",
                "~/Assets/js/main.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/bootstrap.css",
           "~/Assets/css/style.css",
           "~/Assets/css/responsive.css",
           "~/Assets/css/jquery.datetimepicker.css",
           "~/Assets/css/colors/color1.css",
           "~/Assets/css/animate.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/custom.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
