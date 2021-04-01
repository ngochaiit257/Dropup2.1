using System.Web;
using System.Web.Optimization;

namespace LisPharmaWebsite
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
                "~/Assets/vendor/bootstrap/js/bootstrap.min.js",
                "~/Assets/swiper.min.js",
                "~/Assets/js/swiper-bundle.min.js",
                "~/Assets/js/toaster-box.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.lazyload.js",
                "~/Assets/js/main.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/main.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/vendor/bootstrap-select/bootstrap-select.min.css",
           "~/Assets/vendor/owl-carousel/owl.carousel.css",
           "~/Assets/vendor/animate/animate.css",
           "~/Assets/css/swiper.min.css",
           "~/Assets/css/swiper-bundle.min.css",
           "~/Assets/css/style.css",
           "~/Assets/css/skin/skin-2.css",
           "~/Assets/vendor/switcher/switcher.css",
           "~/Assets/vendor/rangeslider/rangeslider.css",
           "~/Assets/css/font/font.css",
           "~/Assets/css/nice-select.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/custom.css"

           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
