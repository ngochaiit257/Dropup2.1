using System.Web;
using System.Web.Optimization;

namespace DropupManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));



            bundles.Add(new ScriptBundle("~/bundles/jsCoreFirst").Include(
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Assets/js/Pace.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsCoreSecond").Include(
                "~/Assets/js/vendors.js",
                "~/Assets/js/app.js",
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/autoNumeric.js",
                "~/Assets/js/jquery.timeago.js",
                "~/Assets/js/date.format.js",
                "~/Assets/js/StringProcessing.js",
                "~/Assets/js/clockface.js",
                "~/Assets/js/jquery.dirrty.js",
                //"~/Assets/js/push.js",
                "~/Assets/js/jquery.easeScroll.js",
                "~/Assets/js/croppie.js",
                "~/Assets/js/chosen.jquery.js",
                "~/Assets/js/Jquery.print.js",
                "~/Assets/js/bootstrap-grid.js",
                "~/Assets/js/summernote-table-of-content.js",
                //"~/Assets/js/print.min.js",
                "~/Assets/js/ImageSelect.jquery.js"
                //"~/Assets/js/jquery.signalR-2.4.1.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/css/vendors.css",
           "~/Assets/css/style.css",
           "~/Assets/css/OrderStyle.css",
           "~/Assets/css/InsertProductCustomStyle.css",
           "~/Assets/css/PagedList.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/Pace.css",
           "~/Assets/css/clockface.css",
           "~/Assets/css/croppie.css",
           "~/Assets/css/chosen.css",
           "~/Assets/css/ImageSelect.css",
           //"~/Assets/css/print.min.css",
           "~/Assets/css/AnimationIcon.css"
           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
