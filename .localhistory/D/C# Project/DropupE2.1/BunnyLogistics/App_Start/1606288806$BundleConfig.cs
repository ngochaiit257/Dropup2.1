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
           "~/Assets/css/animate.css",
           "~/Assets/css/icofont.min.css",
           "~/Assets/css/owl.carousel.min.css",
           "~/Assets/css/owl.theme.default.min.css",
           "~/Assets/css/magnific-popup.css",
           "~/Assets/css/meanmenu.css",
           "~/Assets/css/responsive.css",
           "~/Assets/css/style.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/PagedList.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
