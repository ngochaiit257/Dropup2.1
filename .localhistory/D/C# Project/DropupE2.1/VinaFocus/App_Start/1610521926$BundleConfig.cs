using System.Web;
using System.Web.Optimization;

namespace VinaFocus
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
                "~/Assets/js/vendor/modernizr-3.7.1.min.js",
                "~/Assets/js/plugins.min.js",
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/toaster-box.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.lazyload.js",
                "~/Assets/js/main.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/main.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/vendor/plugins.min.css",
           "~/Assets/css/style.min.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/toaster-box.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/responsive.min.css"

           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
