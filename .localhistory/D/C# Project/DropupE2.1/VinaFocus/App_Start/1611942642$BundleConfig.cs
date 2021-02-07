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
                "~/Assets/js/jquery.cookie.js",
                "~/Assets/js/autoNumeric.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/js/toaster-box.js",
                "~/Assets/js/jquery.lazyload.js",
                "~/Assets/vendor/jquery-migrate/dist/jquery-migrate.min.js",
                "~/Assets/vendor/popper.js/dist/umd/popper.min.js",
                "~/Assets/vendor/bootstrap/bootstrap.min.js",

                "~/Assets/vendor/appear.js",
                "~/Assets/vendor/jquery.countdown.min.js",
                "~/Assets/vendor/hs-megamenu/src/hs.megamenu.js",
                "~/Assets/vendor/svg-injector/dist/svg-injector.min.js",
                "~/Assets/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",
                "~/Assets/vendor/jquery-validation/dist/jquery.validate.min.js",
                "~/Assets/vendor/fancybox/jquery.fancybox.min.js",
                "~/Assets/vendor/typed.js/lib/typed.min.js",
                "~/Assets/vendor/slick-carousel/slick/slick.js",
                "~/Assets/vendor/bootstrap-select/dist/js/bootstrap-select.min.js",

                "~/Assets/js/hs.core.js",
                "~/Assets/js/components/hs.countdown.js",
                "~/Assets/js/components/hs.header.js",
                "~/Assets/js/components/hs.hamburgers.js",
                "~/Assets/js/components/hs.unfold.js",
                "~/Assets/js/components/hs.focus-state.js",
                "~/Assets/js/components/hs.malihu-scrollbar.js",
                "~/Assets/js/components/hs.validation.js",
                "~/Assets/js/components/hs.fancybox.js",
                "~/Assets/js/components/hs.onscroll-animation.js",
                "~/Assets/js/components/hs.slick-carousel.js",
                "~/Assets/js/components/hs.show-animation.js",
                "~/Assets/js/components/hs.svg-injector.js",
                "~/Assets/js/components/hs.go-to.js",
                "~/Assets/js/components/hs.selectpicker.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/styleCore").Include(
           "~/Assets/vendor/font-awesome/css/fontawesome-all.min.css",
           "~/Assets/css/font-electro.css",
           "~/Assets/vendor/animate.css/animate.min.css",
           "~/Assets/vendor/hs-megamenu/src/hs.megamenu.css",
           "~/Assets/vendor/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.css",
           "~/Assets/vendor/fancybox/jquery.fancybox.css",
           "~/Assets/vendor/slick-carousel/slick/slick.css",
           "~/Assets/vendor/bootstrap-select/dist/css/bootstrap-select.min.css",
           "~/Assets/css/theme.css",
           "~/Assets/css/custom.css",
           "~/Assets/css/toaster-box.css",
           "~/Assets/css/PagedList.css"

           ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
