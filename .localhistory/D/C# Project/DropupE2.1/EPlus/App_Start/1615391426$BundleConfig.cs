using System.Web;
using System.Web.Optimization;

namespace EPlus
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
                "~/Assets/js/tether.min.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.easing.js",
                "~/Assets/js/jquery-waypoints.js",
                "~/Assets/js/jquery-validate.js",
                "~/Assets/js/slick.js",
                "~/Assets/js/slick.min.js",
                "~/Assets/js/jquery.prettyPhoto.js",
                "~/Assets/js/numinate.min6959.js",
                "~/Assets/js/main.js",
                "~/Assets/js/autoNumeric.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/revolution/js/revolution.tools.min.js",
                "~/Assets/revolution/js/rs6.min.js",
                "~/Assets/revolution/js/slider.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/vendors/bootstrap/bootstrap.min.css",
           "~/Assets/vendors/fontawesome/css/all.min.css",
           "~/Assets/vendors/jquery-nice-select/nice-select.css",
           "~/Assets/vendors/OwlCarousel2/owl.carousel.min.css",
           "~/Assets/vendors/magnific-popup/css/magnific-popup.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/PagedList.css",
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
