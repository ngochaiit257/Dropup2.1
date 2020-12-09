using System.Web;
using System.Web.Optimization;

namespace HM_Media
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
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/form-validator.min.js",
                "~/Assets/js/contact-form-script.js",
                "~/Assets/js/jquery.ajaxchimp.min.js",
                "~/Assets/js/jquery.meanmenu.js",
                "~/Assets/js/wow.min.js",
                "~/Assets/js/owl.carousel.min.js",
                "~/Assets/js/jquery-modal-video.min.js",
                "~/Assets/js/odometer.min.js",
                "~/Assets/js/jquery.appear.min.js",
                //"~/Assets/js/smoothscroll.min.js",
                "~/Assets/js/custom.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/bootstrap.min.css",
           "~/Assets/css/boxicons.min.css",
           "~/Assets/fonts/flaticon.css",
           "~/Assets/css/meanmenu.css",
           "~/Assets/css/animate.min.css",
           "~/Assets/css/owl.carousel.min.css",
           "~/Assets/css/owl.theme.default.min.css",
           "~/Assets/css/flaticon.css",
           "~/Assets/css/modal-video.min.css",
           "~/Assets/css/odometer.min.css",
           "~/Assets/css/odometer.min.css",
           "~/Assets/css/style.css",
           "~/Assets/css/responsive.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/PagedList.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
