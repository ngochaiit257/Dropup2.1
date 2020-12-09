using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HM_Media
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(con);

            #region Sql Cache Dependency
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string[] arr_table_cache = { "blog", "blog_category", "cart_product_variation", "cart_of_user" ,"product", "category","category_product","combo_product"
                    ,"product_variation_in_order", "order", "system_log",
                "store_logo","store_banner","store_slide","store_membership", "store_branch","store_partner", "store_customer_say","store_contact" };
            System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connectionString);
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, arr_table_cache);

            #endregion
        }

        protected void Application_End()
        {
            //here we will stop Sql Dependency  
            SqlDependency.Stop(con);
        }
    }
}
