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
                "~/Assets/vendor/jquery/dist/jquery.min.js"
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
           "~/Assets/vendor/font-awesome/css/fontawesome-all.min.css",
           "~/Assets/css/font-electro.css",
           "~/Assets/vendor/animate.css/animate.min.css",
           "~/Assets/vendor/hs-megamenu/src/hs.megamenu.css",
           "~/Assets/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css",
           "~/Assets/vendor/fancybox/jquery.fancybox.css",
           "~/Assets/vendor/slick-carousel/slick/slick.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/responsive.min.css"

           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
