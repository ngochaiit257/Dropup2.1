using System.Web;
using System.Web.Optimization;

namespace ClinicBachGiai
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
                "~/Assets/js/popper.min.js",
                "~/Assets/js/bootstrap.min.js",
                "~/Assets/js/jquery.meanmenu.js",
                "~/Assets/js/jquery.appear.min.js",
                "~/Assets/js/odometer.min.js",
                "~/Assets/js/owl.carousel.min.js",
                "~/Assets/js/jquery.magnific-popup.min.js",
                "~/Assets/js/jquery.nice-select.min.js",
                "~/Assets/js/datetimepicker.min.js",
                "~/Assets/js/month.min.js",
                "~/Assets/js/imagelightbox.min.js",
                "~/Assets/js/jquery.ajaxchimp.min.js",
                "~/Assets/js/form-validator.min.js",
                "~/Assets/js/contact-form-script.js",
                "~/Assets/js/form-validator.min.js",
                "~/Assets/js/wow.min.js",
                "~/Assets/js/jquery.lazyload.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/main.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/bootstrap.min.css",
           "~/Assets/css/animate.min.css",
           "~/Assets/css/meanmenu.css",
           "~/Assets/css/boxicons.min.css",
           "~/Assets/css/flaticon.css",
           "~/Assets/css/odometer.min.css",
           "~/Assets/css/owl.carousel.min.css",
           "~/Assets/css/owl.theme.default.min.css",
           "~/Assets/css/magnific-popup.min.css",
           "~/Assets/css/nice-select.css",
           "~/Assets/css/datetimepicker.min.css",
           "~/Assets/css/month.min.css",
           "~/Assets/css/imagelightbox.min.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/style.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/responsive.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
