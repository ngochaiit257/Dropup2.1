using System.Web;
using System.Web.Optimization;

namespace QuynhBoki
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
                "~/Assets/js/jquery.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsCoreSecond").Include(
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/popper.min.js",
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/slick.js",
                "~/Assets/js/jquery.counterup.min.js",
                "~/Assets/js/counterup.min.js",
                "~/Assets/js/swiper.min.js",
                "~/Assets/js/swiper-bundle.min.js",
                "~/Assets/js/toaster-box.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/custom.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
               "~/Assets/css/styles.css",
               "~/Assets/css/swiper.min.css",
               "~/Assets/css/swiper-bundle.min.css",
               "~/Assets/css/colors.css",
               "~/Assets/css/toaster-box.css",
               "~/Assets/css/custom.css",
               "~/Assets/css/PagedList.css",
               "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
