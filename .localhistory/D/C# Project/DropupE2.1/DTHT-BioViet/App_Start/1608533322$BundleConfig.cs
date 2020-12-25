using System.Web;
using System.Web.Optimization;

namespace DTHT_BioViet
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
           "~/Assets/css/bootstrap.min.css",
           "~/Assets/css/animate.css",
           "~/Assets/css/slick.css",
           "~/Assets/css/slick-theme.css",
           "~/Assets/css/font-awesome.css",
           "~/Assets/css/themify-icons.css",
           "~/Assets/css/flaticon.css",
           "~/Assets/revolution/css/rs6.css",
           "~/Assets/css/prettyPhoto.css",
           "~/Assets/css/shortcodes.css",
           "~/Assets/css/main.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
