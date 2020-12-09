using Data;
using Data.Providers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FreshMart_Dashboard.Controllers
{
    public class PageController : Controller
    {
        SystemLogProvider system_log_provider = new SystemLogProvider();
        PageProvider page_provider = new PageProvider();
        string prefix_url = System.Configuration.ConfigurationManager.AppSettings["urlPresentationFreshMart"];

        // GET: Page
        public ActionResult Index(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                    int? size_of_page, string sort_list_return)
        {

            if (quick_filter == null)
            {
                quick_filter = "all";
            }

            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilter = quick_filter;
            ViewBag.FilterValue = search_string;
            ViewBag.page_num = page_num;
            if (size_of_page == null)
            {
                size_of_page = 10;
            }
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            return View();
        }

        public PartialViewResult GetPage(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<page>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }

            ViewBag.page_num = page_num;
            int Size_Of_Page = size_of_page;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilter = quick_filter;
            list_return = page_provider.quickFilter(search_string, quick_filter, quick_filter_value);
            ViewBag.NumberOfList = list_return.Count();
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.page_title).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.page_title).ToList();
            }
            else if (sort_list_return == "lastest")
            {
                list_return = list_return.OrderByDescending(p => p.create_datetime).ToList();
            }

            return PartialView("_PartialViewPage", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }



        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        public ActionResult InsertPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertPage(page model, string page_date_to_show, string page_time_to_show, bool is_showing)
        {
            page page = new page();
            page.page_title = model.page_title;
            page.page_seo_description = model.page_seo_description;
            page.page_seo_alias = model.page_seo_alias;
            page.page_seo_header = model.page_seo_header;
            page.page_summary = model.page_summary;
            page.page_content = model.page_content;
            page.user_id_create = getUserLoged().user_id;
            page.user_id_update = getUserLoged().user_id;
            page.update_datetime = System.DateTime.Now;
            page.create_datetime = System.DateTime.Now;
            page.mount_on_header_menu_status = model.mount_on_header_menu_status;
            page.mount_on_footer_menu_status = model.mount_on_footer_menu_status;
            page.page_parent_id = model.page_parent_id;
            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (page_date_to_show != "" && page_time_to_show != "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else if (page_date_to_show != "" && page_time_to_show == "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else if (page_date_to_show == "" && page_time_to_show != "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else
                {
                    page.status = false;
                    page.show_datetime = null;
                }
            }
            else
            {
                page.status = true;
                page.show_datetime = System.DateTime.Now;
            }

            HttpPostedFileBase file = Request.Files["page_image"];
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/page_image")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                page.page_image = path_e;
                page.page_image_alt = model.page_image_alt;
            }

            int pageId = 0;
            if (page_provider.insertPage(page, ref pageId))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo trang " + page_provider.getById(pageId).page_title;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = pageId;
                system_log.system_log_url = "/Page/PageDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { page_id = pageId }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Updatepage(page model, int page_id, string page_date_to_show, string page_time_to_show, bool is_showing, bool status_image_change)
        {
            page page = new page();
            page.page_title = model.page_title;
            page.page_seo_description = model.page_seo_description;
            page.page_seo_alias = model.page_seo_alias;
            page.page_seo_header = model.page_seo_header;
            page.page_summary = model.page_summary;
            page.page_content = model.page_content;
            page.user_id_update = getUserLoged().user_id;
            page.update_datetime = System.DateTime.Now;
            page.mount_on_header_menu_status = model.mount_on_header_menu_status;
            page.mount_on_footer_menu_status = model.mount_on_footer_menu_status;
            page.page_parent_id = model.page_parent_id;
            DateTime show_datetime = new DateTime();

            if (is_showing == false)
            {
                if (page_date_to_show != "" && page_time_to_show != "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else if (page_date_to_show != "" && page_time_to_show == "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else if (page_date_to_show == "" && page_time_to_show != "")
                {
                    page.status = true;
                    string str = page_date_to_show.Replace('/', '-') + " " + page_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    page.show_datetime = show_datetime;
                }
                else
                {
                    page.status = false;
                    page.show_datetime = null;
                }
            }
            else
            {
                page.status = true;
                page.show_datetime = System.DateTime.Now;
            }

            if (status_image_change == true)
            {
                HttpPostedFileBase file = Request.Files["page_image"];
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/page_image")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    file.SaveAs(filePath);
                    page.page_image = path_e;
                    page.page_image_alt = model.page_image_alt;
                }
                else
                {
                    page.page_image = null;
                    page.page_image_alt = null;
                }
            }
            else
            {
                var old_model = page_provider.getById(page_id);
                page.page_image = old_model.page_image;
                page.page_image_alt = model.page_image_alt;
            }

            if (page_provider.updatePageFull(page, page_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật trang " + model.page_title;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = page_id;
                system_log.system_log_url = "/Page/PageDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { page_id = page_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Deletepage(List<int> list_page_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {
            if (list_page_id != null)
            {
                foreach (var page_id in list_page_id)
                {
                    var page_name = page_provider.getById(page_id).page_title;
                    if (page_provider.deletePage(page_id))
                    {
                        system_log system_log = new system_log();
                        system_log.system_log_description = "đã xóa trang " + page_name;
                        system_log.user_id = getUserLoged().user_id;
                        system_log.system_log_time = System.DateTime.Now;
                        system_log.system_log_url = "/Page/Index";
                        system_log_provider.insertSystemLog(system_log);
                    }
                }
            }

            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        [HttpPost]
        public ActionResult HideStatuspage(List<int> list_page_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {

            if (page_provider.hideStatusPage(list_page_id))
            {
                foreach (var page_id in list_page_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã ẩn trang " + page_provider.getById(page_id).page_title;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Page/PageDetail";
                    system_log.system_log_object_id = page_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        [HttpPost]
        public ActionResult ShowStatuspage(List<int> list_page_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {
            if (page_provider.showStatusPage(list_page_id))
            {
                foreach (var page_id in list_page_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã hiển thị trang " + page_provider.getById(page_id).page_title;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Page/PageDetail";
                    system_log.system_log_object_id = page_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        [HttpPost]
        public bool CheckPageTitleDuplicate(string page_title)
        {

            return page_provider.checkPageTitleDuplicate(page_title);
        }

        [HttpPost]
        public bool CheckSeoAliasDuplicate(string page_seo_alias)
        {

            return page_provider.checkPageSeoAliasDuplicate(page_seo_alias);
        }


        public ActionResult PageDetail(int id)
        {
            ViewBag.PrefixUrl = prefix_url;
            return View(page_provider.getById(id));
        }
    }
}