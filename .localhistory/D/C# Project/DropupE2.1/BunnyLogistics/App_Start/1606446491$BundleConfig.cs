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
               "~/Assets/js/jquery-2.2.4.min.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/jsCoreSecond").Include(
                "~/Assets/js/popper.min.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.ajaxchimp.min.js",
                "~/Assets/js/form-validator.min.js",
                "~/Assets/js/contact-form-script.js",
                "~/Assets/js/owl.carousel.min.js",
                "~/Assets/js/jquery.magnific-popup.min.js",
                "~/Assets/js/wow.min.js",
                "~/Assets/js/meanmenu.js",
                "~/Assets/js/custom.js",
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
