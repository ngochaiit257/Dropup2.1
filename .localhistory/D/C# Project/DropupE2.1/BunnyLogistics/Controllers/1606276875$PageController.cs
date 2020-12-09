using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;


namespace BunnyLogistics.Controllers
{
    public class PageController : Controller
    {
        PageProvider page_provider = new PageProvider();
        // GET: Page
        public ActionResult PageDetail(string seo_alias)
        {
            return View(page_provider.getBySeoAlias(seo_alias));
        }

        public ActionResult PageContact()
        {
            return View();
        }


        public async Task<ActionResult> SendMessage(string sender_name, string sender_email, string sender_phone_number, string sender_subject, string sender_message)
        {
            store_message sm = new store_message();
            sm.sender_name = sender_name;
            sm.sender_email = sender_email;
            sm.sender_phone_number = sender_phone_number;
            sm.sender_subject = sender_subject;
            sm.sender_message = sender_message;
            sm.reading_status = false;
            sm.create_datetime = DateTime.Now;
            sm_provider.insertStoreMessage(sm);

            return Json("Success");
        }
    }
}