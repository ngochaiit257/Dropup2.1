using Common;
using Data;
using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace VinaFocus.Controllers
{
    public class PageController : Controller
    {
        PageProvider page_provider = new PageProvider();
        StoreMessageProvider sm_provider = new StoreMessageProvider();
        // GET: Page
        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
           "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
           "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
           "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult PageDetail(string seo_alias)
        {
            return View(page_provider.getBySeoAlias(seo_alias));
        }

        [OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
           "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
           "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
           "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult PageContact()
        {
            return View();
        }

        public async Task<ActionResult> SendMessage(string sender_name, string sender_email, string sender_phone_number, string sender_message)
        {
            store_message sm = new store_message();
            sm.sender_name = sender_name;
            sm.sender_email = sender_email;
            sm.sender_phone_number = sender_phone_number;
            sm.sender_message = sender_message;
            sm.reading_status = false;
            sm.create_datetime = DateTime.Now;
            sm_provider.insertStoreMessage(sm);

            string email_receiver = System.Configuration.ConfigurationManager.AppSettings["ToEmailAddress"];
            string store_website = System.Configuration.ConfigurationManager.AppSettings["store_website"];
            string store_name = System.Configuration.ConfigurationManager.AppSettings["store_name"];
            string store_logo_url = System.Configuration.ConfigurationManager.AppSettings["store_logo_url"];
            string store_color = System.Configuration.ConfigurationManager.AppSettings["store_color"];
            string store_email = System.Configuration.ConfigurationManager.AppSettings["store_email"];
            string store_phone_number = System.Configuration.ConfigurationManager.AppSettings["store_phone_number"];

            await SendEmail(email_receiver, DateTime.Now, store_website, store_name, store_logo_url, store_color, store_email, store_phone_number,
                sm.sender_name, sm.sender_email, sm.sender_phone_number, sm.sender_subject, sm.sender_message);

            return Json("Success");
        }

        /// <summary>
        /// Thay đổi content và gửi Email
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(string email_receiver, DateTime create_datetime,

            string store_website, string store_name, string store_logo_url, string store_color, string store_email, string store_phone_number,

            string customer_fullname, string customer_email, string customer_phone_number, string subject, string content_message)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/YouHaveMessage.html"));

            if (customer_email == "" || customer_email == null)
            {
                customer_email = "Email chưa xác định";
            }

            content = content.Replace("{{order_create_datetime}}", "(Ngày " + create_datetime.ToString("dd/MM/yyy") + " lúc " + create_datetime.ToString("HH:mm") + ")");

            content = content.Replace("{{store_website}}", store_website);
            content = content.Replace("{{store_name}}", store_name);
            content = content.Replace("{{store_logo_url}}", store_logo_url);
            content = content.Replace("{{store_color}}", store_color);
            content = content.Replace("{{store_email}}", store_email);
            content = content.Replace("{{store_phone_number}}", store_phone_number);

            content = content.Replace("{{customer_fullname}}", customer_fullname);
            content = content.Replace("{{customer_email}}", customer_email);
            content = content.Replace("{{customer_phone_number}}", customer_phone_number);
            content = content.Replace("{{subject}}", subject);
            content = content.Replace("{{content_message}}", content_message);

            await new MailHelper().SendMail(email_receiver, "[DROPUP] Tin nhắn vừa được khách hàng gửi về", content);
            return true;
        }
    }
}