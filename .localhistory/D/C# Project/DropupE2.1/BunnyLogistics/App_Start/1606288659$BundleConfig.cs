using System.Web;
using System.Web.Optimization;

namespace BunnyLogistics
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
               "~/Assets/js/jquery.min.1.11.08cea.js"
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
           "~/Assets/css/jquery.fancybox.min8cea.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/flexslider8cea.css",
           "~/Assets/css/styles8cea.css",
           "~/Assets/css/swiper.min8cea.css",
           "~/Assets/css/icon.css",
           "~/Assets/css/custom.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
