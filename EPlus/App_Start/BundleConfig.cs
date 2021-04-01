using System.Web;
using System.Web.Optimization;

namespace EPlus
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
               "~/Assets/vendors/jQuery/jquery.min.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/jsCoreSecond").Include(
                "~/Assets/vendors/bootstrap/bootstrap.min.js",
                "~/Assets/vendors/jquery-nice-select/jquery.nice-select.min.js",
                "~/Assets/vendors/OwlCarousel2/owl.carousel.min.js",
                "~/Assets/vendors/counterup/jquery.counterup.min.js",
                "~/Assets/vendors/counterup/waypoints.min.js",
                "~/Assets/vendors/magnific-popup/js/magnific-popup.min.js",

                "~/Assets/js/script.js",
                "~/Assets/js/custom.js",
                "~/Assets/js/autoNumeric.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/vendors/bootstrap/bootstrap.min.css",
           "~/Assets/vendors/fontawesome/css/all.min.css",
           "~/Assets/vendors/jquery-nice-select/nice-select.css",
           "~/Assets/vendors/OwlCarousel2/owl.carousel.min.css",
           "~/Assets/vendors/magnific-popup/css/magnific-popup.css",
           "~/Assets/css/style.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/custom.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
