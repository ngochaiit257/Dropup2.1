using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Data;
using Data.Providers;
using DropupManagement.Models;

namespace DropupManagement
{
    public class PushNotification
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            //string sqlCommand = @"SELECT [system_notification_id], [system_notification_title], [system_notification_create_datetime] from [dbo].[system_notification] where [system_notification_create_datetime] > @system_notification_create_datetime";
            string sqlCommand = @"SELECT [system_notification_id], [system_notification_title], [system_notification_create_datetime],[system_notification_url_obj] from [dbo].[system_notification]";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@system_notification_create_datetime", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now  
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record  
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client  
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                //re-register notification  
                RegisterNotification(DateTime.Now);
            }
        }

        //public List<system_notification> GetData(DateTime afterDate)
        //{
        //    using (MyPushNotificationEntities dc = new MyPushNotificationEntities())
        //    {
        //        return dc.system_notification.Where(a => a.system_notification_create_datetime > afterDate).OrderByDescending(a => a.system_notification_create_datetime).ToList();
        //    }
        //}

        public List<SystemNotificationModel> GetData(DateTime afterDate)
        {
            SystemNotificationProvider sn_provider = new SystemNotificationProvider();
            var list = sn_provider.getAll().Where(a => a.system_notification_create_datetime > afterDate).OrderByDescending(a => a.system_notification_create_datetime).ToList();
            var list_return = new List<SystemNotificationModel>();
            foreach (var obj in list)
            {
                SystemNotificationModel model = new SystemNotificationModel();
                model.id = obj.system_notification_id;
                model.title = obj.system_notification_title;
                model.create_datetime = (DateTime)obj.system_notification_create_datetime;
                model.obj_url = obj.system_notification_url_obj;
                list_return.Add(model);
            }
            return list_return;

        }
    }
}
