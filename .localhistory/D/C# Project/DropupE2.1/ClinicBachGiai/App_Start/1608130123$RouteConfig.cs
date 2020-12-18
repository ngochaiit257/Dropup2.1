using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClinicBachGiai
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "GioiThieu",
               url: "pages/gioi-thieu",
               defaults: new { controller = "Page", action = "GioiThieu", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "DoiNguBacSi",
               url: "pages/doi-ngu-bac-si",
               defaults: new { controller = "Page", action = "DoiNguBacSi", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "CoSoVatChat",
               url: "pages/co-so-vat-chat",
               defaults: new { controller = "Page", action = "CoSoVatChat", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "Contact",
               url: "pages/lien-he",
               defaults: new { controller = "Page", action = "PageContact", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Service",
               url: "service/{seo_alias}",
               defaults: new { controller = "Category", action = "ServiceDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Article",
               url: "articles/{seo_alias}",
               defaults: new { controller = "Article", action = "ArticleDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Page",
               url: "pages/{seo_alias}",
               defaults: new { controller = "Page", action = "PageDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "BlogDetail",
               url: "blogs/{seo_alias}",
               defaults: new { controller = "Blog", action = "BlogDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Blog",
               url: "blogs",
               defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Checkout",
               url: "checkout",
               defaults: new { controller = "Checkout", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Cart",
               url: "cart",
               defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "Account",
             url: "account",
             defaults: new { controller = "User", action = "Account", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Danh Sach Chuyen Muc",
                url: "{seo_alias}",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Category",
                url: "category/{seo_alias}",
                defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

}
