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
                "~/Assets/js/scripts8cea.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.lazyload.js",
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
