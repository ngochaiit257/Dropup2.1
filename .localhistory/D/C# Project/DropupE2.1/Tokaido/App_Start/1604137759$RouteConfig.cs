using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tokaido
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Thankyou",
               url: "thank-you",
               defaults: new { controller = "Checkout", action = "GetOrderSuccess", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "ComingSoon",
               url: "coming-soon",
               defaults: new { controller = "ComingSoon", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Contact",
               url: "pages/lien-he",
               defaults: new { controller = "Page", action = "PageContact", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "AboutUs",
               url: "gioi-thieu",
               defaults: new { controller = "AboutUs", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Service",
               url: "service/{seo_alias}",
               defaults: new { controller = "Category", action = "ServiceDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Page",
               url: "pages/{seo_alias}",
               defaults: new { controller = "Page", action = "PageDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Search",
              url: "search/keyword={keyword}",
              defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "BlogDetail",
               url: "articles/{seo_alias}",
               defaults: new { controller = "Blog", action = "BlogDetail", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Blog",
               url: "articles",
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
                name: "Product Detail",
                url: "{seo_alias}",
                defaults: new { controller = "Product", action = "ProductDetail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Category Product",
                url: "products/{seo_alias}",
                defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Category Parent",
                url: "collections/all",
                defaults: new { controller = "Category", action = "CategoryParent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Category Child 1",
                url: "collections/{seo_alias}",
                defaults: new { controller = "Category", action = "CategoryChild1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Category",
                url: "collections/{seo_alias}",
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
