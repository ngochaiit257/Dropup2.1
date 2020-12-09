using System.Web;
using System.Web.Optimization;

namespace ElysienVietNam
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
                "~/Assets/js/vendor/jquery-3.5.1.min.js"
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
           "~/Assets/css/vendor/plugins.min.css",
           "~/Assets/css/style.min.css",
           "~/Assets/css/responsive.min.css",

           "~/Assets/css/PagedList.css",
           "~/Assets/css/toaster-box.css",
           "~/Assets/css/custom.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
