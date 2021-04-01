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
                "~/Assets/js/popper.min.js",
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.nice-select.min.js",
                "~/Assets/js/jquery.fancybox.min.js",
                "~/Assets/js/slick.min.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/isotope.pkgd.min.js",
                "~/Assets/js/plugins.js",
                "~/Assets/js/countdown.min.js",
                "~/Assets/js/jquery.meanmenu.min.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.lazyload.js",
                "~/Assets/js/main.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
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
