using DropupManagement;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DTHT_BioViet
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
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            SqlDependency.Start(con);

            #region Sql Cache Dependency
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string[] arr_table_cache = { "product", "product_variation", "product_variation_in_order", "order", "system_log", "store_message", "system_notification" };
            //System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connectionString);
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, arr_table_cache);

            #endregion
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            PushNotification PN = new PushNotification();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            PN.RegisterNotification(currentTime);
        }

        protected void Application_End()
        {
            //here we will stop Sql Dependency  
            SqlDependency.Stop(con);
        }
    }

}
