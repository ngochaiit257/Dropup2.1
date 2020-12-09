using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data;
using Data.Providers;
using DropupManagement.Models;

namespace DropupManagement.Controllers
{
    public class ConfigurationController : Controller
    {
        SystemLogProvider system_log_provider = new SystemLogProvider();
        PageProvider page_provider = new PageProvider();
        StoreLogoProvider store_logo_provider = new StoreLogoProvider();
        StorePartnerProvider store_partner_provider = new StorePartnerProvider();
        StoreCustomerSayProvider store_cs_provider = new StoreCustomerSayProvider();
        StoreSlideProvider store_slide_provider = new StoreSlideProvider();
        StoreContactProvider store_contact_provider = new StoreContactProvider();
        StoreMembershipProvider store_membership_provider = new StoreMembershipProvider();
        StoreBannerProvider store_banner_provider = new StoreBannerProvider();
        // GET: Configuration
        public ActionResult Index()
        {
            return View();
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult GetLogoBanner()
        {
            var list = store_slide_provider.getAll();
            return PartialView("_PartialViewLogoBanner", list);
        }

        [HttpPost]
        public ActionResult UpdateStoreBanner(store_banner sb, page page, HttpPostedFileBase image, HttpPostedFileBase page_image)
        {
            var store_banner = new store_banner();
            store_banner.store_banner_id = 1;
            store_banner.alt = sb.alt;
            store_banner.main_text = sb.main_text;
            store_banner.child_text = sb.child_text;
            store_banner.video_url = sb.video_url;
            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_banner")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                image.SaveAs(filePath);
                store_banner.image = path_e;
            }
            else
            {
                var old_store_banner = store_banner_provider.getbyId(1);
                store_banner.image = old_store_banner.image;
            }
            if (store_banner_provider.updateStoreBanner(store_banner))
            {
                var page_model = new page();
                page_model.page_summary = page.page_summary;
                if (page_image != null)
                {
                    var fileName = Path.GetFileName(page_image.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_page")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    page_image.SaveAs(filePath);
                    page_model.page_image = path_e;
                }
                else
                {
                    var old_page_model = page_provider.getById(1);
                    page_model.page_image = old_page_model.page_image;
                }
                if (page_provider.updateImagePage(1, page_model.page_image, page_model.page_summary))
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã cập nhật Banner trang chủ";
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = 1;
                    system_log.system_log_url = "/Configuration/Index";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json("Success");
        }

        [HttpPost]
        public ActionResult UpdateStoreLogo(HttpPostedFileBase favicon, HttpPostedFileBase logo_header, HttpPostedFileBase logo_footer, HttpPostedFileBase logo_mobile)
        {
            bool status_update = false;
            if (favicon != null)
            {
                var fileName = Path.GetFileName(favicon.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/logo_presentation_site")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                favicon.SaveAs(filePath);
                store_logo_provider.updateLogoStore(4, path_e);
                status_update = true;
            }
            if (logo_header != null)
            {
                var fileName = Path.GetFileName(logo_header.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/logo_presentation_site")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                logo_header.SaveAs(filePath);
                store_logo_provider.updateLogoStore(1, path_e);
                status_update = true;
            }
            if (logo_footer != null)
            {
                var fileName = Path.GetFileName(logo_footer.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/logo_presentation_site")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                logo_footer.SaveAs(filePath);
                store_logo_provider.updateLogoStore(2, path_e);
                status_update = true;
            }
            if (logo_mobile != null)
            {
                var fileName = Path.GetFileName(logo_mobile.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/logo_presentation_site")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                logo_mobile.SaveAs(filePath);
                store_logo_provider.updateLogoStore(3, path_e);
                status_update = true;
            }

            if (status_update == true)
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật hình ảnh logo cửa hàng";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }

            return Json("Update Success.");
        }

        public virtual ActionResult GetArraySlideId()
        {
            var list = store_slide_provider.getAll();
            var list_id = new List<int>();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    list_id.Add(item.store_slide_id);
                }
            }

            return Json(new { list_slide_id = list_id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateSlide(store_slide model, HttpPostedFileBase image)
        {
            store_slide slide = new store_slide();
            slide.link_url = model.link_url;
            slide.main_text = model.main_text;
            slide.child_text_1 = model.child_text_1;
            slide.child_text_2 = model.child_text_2;
            slide.is_showing = model.is_showing;
            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_slide")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                image.SaveAs(filePath);
                slide.image = path_e;
                slide.alt = model.alt;
            }
            if (store_slide_provider.insertStoreSlide(slide))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã thêm Slide mới";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Success");
        }

        [HttpPost]
        public ActionResult UpdateSlide(List<StoreSlideModel> list_slide)
        {
            if (list_slide.Count() > 0)
            {
                foreach (var model in list_slide)
                {
                    store_slide slide = new store_slide();
                    slide.store_slide_id = model.store_slide_id;
                    slide.link_url = model.link_url;
                    slide.main_text = model.main_text;
                    slide.child_text_1 = model.child_text_1;
                    slide.child_text_2 = model.child_text_2;
                    slide.is_showing = model.is_showing;
                    slide.alt = model.alt;
                    if (model.image != null)
                    {
                        HttpPostedFileBase image = model.image;
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_slide")), fileName);
                        //Đổi sang đường dẫn ảo
                        string path_e = GetVirtualPath(filePath);
                        image.SaveAs(filePath);
                        slide.image = path_e;
                    }
                    else
                    {
                        var old_model = store_slide_provider.getById(model.store_slide_id);
                        slide.image = old_model.image;
                    }
                    store_slide_provider.updateStoreSlide(slide);
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã cập nhật Slide";
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = 1;
                    system_log.system_log_url = "/Configuration/Index";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json("Success");
        }

        [HttpPost]
        public ActionResult DeleteSlide(int slide_id)
        {
            if (store_slide_provider.deleteSlide(slide_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa Slide";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Delete Success");
        }

        public ActionResult GetCustomerSay()
        {
            var list = store_cs_provider.getAll().OrderByDescending(sp => sp.store_customer_say_id).ToList();
            return PartialView("_PartialViewCustomerSay", list);
        }

        public ActionResult CreateCustomerSay(store_customer_say model, HttpPostedFileBase image)
        {
            store_customer_say scs = new store_customer_say();
            scs.cname = model.cname;
            scs.cposition = model.cposition;
            scs.ccomment = model.ccomment;
            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_customer_say")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                image.SaveAs(filePath);
                scs.cimage = path_e;
                scs.cimage_alt = "Nhận xét khách hàng" + scs.cname;
            }
            if (store_cs_provider.insertStoreCustomerSay(scs))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã thêm Feedback khách hàng";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Success");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateCustomerSay(store_customer_say model, HttpPostedFileBase image, bool avatar_store_customer_say_is_change)
        {
            store_customer_say cs = new store_customer_say();
            cs.store_customer_say_id = model.store_customer_say_id;
            cs.cname = model.cname;
            cs.cposition = model.cposition;
            cs.ccomment = model.ccomment;
            if (avatar_store_customer_say_is_change == true)
            {
                if (image != null)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_customer_say")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    image.SaveAs(filePath);
                    cs.cimage = path_e;
                }
                else
                {
                    cs.cimage = null;
                }
            }
            else
            {
                var old_model = store_membership_provider.getById(model.membership_id);
                mem.mavatar = old_model.mavatar;
            }

            if (store_membership_provider.updateMS(mem))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật thông tin nhân sự " + mem.mname;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { mem_id = model.membership_id });
        }


        public ActionResult GetPartnerImage()
        {
            var list = store_partner_provider.getAll().OrderByDescending(sp => sp.store_partner_id).ToList();
            return PartialView("_PartialViewPartnerImage", list);
        }

        public ActionResult CreatePartner(store_partner model, HttpPostedFileBase image)
        {
            store_partner sp = new store_partner();
            sp.name = model.name;
            sp.link_url = model.link_url;
            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_partner")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                image.SaveAs(filePath);
                sp.image = path_e;
                sp.alt = sp.name;
            }
            if (store_partner_provider.insertStorePartner(sp))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã thêm hình ảnh đối tác";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Success");
        }


        public ActionResult GetAboutUs()
        {
            return PartialView("_PartialViewAboutUs", page_provider.getById(1));
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateAboutUs(string page_title, string page_content, string page_seo_alias, string page_seo_description, string page_seo_header
            , string about_us_date_to_show, string about_us_time_to_show, bool is_showing)
        {
            page model = new page();
            model.page_title = page_title;
            model.page_content = page_content;
            model.page_seo_alias = page_seo_alias;
            model.page_seo_description = page_seo_description;
            model.page_seo_header = page_seo_header;
            model.user_id_update = getUserLoged().user_id;
            model.update_datetime = System.DateTime.Now;

            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (about_us_date_to_show != "" && about_us_time_to_show != "")
                {
                    model.status = true;
                    string str = about_us_date_to_show.Replace('/', '-') + " " + about_us_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.show_datetime = show_datetime;
                }
                else if (about_us_date_to_show != "" && about_us_time_to_show == "")
                {
                    model.status = true;
                    string str = about_us_date_to_show.Replace('/', '-') + " " + about_us_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.show_datetime = show_datetime;
                }
                else if (about_us_date_to_show == "" && about_us_time_to_show != "")
                {
                    model.status = true;
                    string str = about_us_date_to_show.Replace('/', '-') + " " + about_us_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    model.show_datetime = show_datetime;
                }
                else
                {
                    model.status = false;
                    model.show_datetime = null;
                }
            }
            else
            {
                model.status = true;
                model.show_datetime = System.DateTime.Now;
            }

            if (page_provider.updatePage(model, 1))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật trang giới thiệu";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { page_id = 1 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetStoreContact()
        {
            ViewBag.StoreContactId = store_contact_provider.get1Contact().store_contact_id;
            return PartialView("_PartialViewStoreContact", store_contact_provider.get1Contact());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateStoreContact(string address, string email, string phone_number, string working_day, string working_hour, string map)
        {
            store_contact model = new store_contact();
            model.store_contact_id = 1;
            model.address = address;
            model.email = email;
            model.phone_number = phone_number;
            model.working_day = working_day;
            model.working_hour = working_hour;
            model.map = map;

            if (store_contact_provider.updateStoreContact(model))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật thông tin liên hệ cho cửa hàng";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }

            return Json("Update Success.");
        }


        //STORE MEMBERSHIP
        public ActionResult GetStoreMembership()
        {
            return PartialView("_PartialViewStoreMembership", store_membership_provider.getAll());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateMembership(store_membership model, HttpPostedFileBase image)
        {
            store_membership mem = new store_membership();
            mem.mname = model.mname;
            mem.mposition = model.mposition;
            mem.mfacebook_url = model.mfacebook_url;
            mem.mzalo_url = model.mzalo_url;
            mem.minstagram_url = model.minstagram_url;
            mem.mtwitter_url = model.mtwitter_url;
            mem.myoutube_url = model.myoutube_url;
            mem.msummary = model.msummary;
            if (image != null)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_membership")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                image.SaveAs(filePath);
                mem.mavatar = path_e;
            }
            if (store_membership_provider.insertMS(mem))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã thêm thông tin nhân sự mới";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Success");
        }

        [HttpPost]
        public ActionResult DeleteMembership(int mem_id)
        {
            var mem = store_membership_provider.getById(mem_id);
            if (store_membership_provider.deleteMS(mem_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa thông tin nhân sự " + mem.mname;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Delete Success");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateMembership(store_membership model, HttpPostedFileBase image, bool avatar_store_membership_is_change)
        {
            store_membership mem = new store_membership();
            mem.membership_id = model.membership_id;
            mem.mname = model.mname;
            mem.mposition = model.mposition;
            mem.mfacebook_url = model.mfacebook_url;
            mem.mzalo_url = model.mzalo_url;
            mem.minstagram_url = model.minstagram_url;
            mem.mtwitter_url = model.mtwitter_url;
            mem.myoutube_url = model.myoutube_url;
            mem.msummary = model.msummary;
            if (avatar_store_membership_is_change == true)
            {
                if (image != null)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/images/store/store_membership")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    image.SaveAs(filePath);
                    mem.mavatar = path_e;
                }
                else
                {
                    mem.mavatar = null;
                }
            }
            else
            {
                var old_model = store_membership_provider.getById(model.membership_id);
                mem.mavatar = old_model.mavatar;
            }

            if (store_membership_provider.updateMS(mem))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật thông tin nhân sự " + mem.mname;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = 1;
                system_log.system_log_url = "/Configuration/Index";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { mem_id = model.membership_id });
        }

    }
}