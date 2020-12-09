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

namespace DropupManagement.Controllers
{
    public class BlogController : Controller
    {
        BlogProvider blog_provider = new BlogProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        TagProvider tag_provider = new TagProvider();
        TagBlogProvider tag_blog_provider = new TagBlogProvider();
        string prefix_url = System.Configuration.ConfigurationManager.AppSettings["urlPresentationFreshMart"];

        // GET: Blog
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

        public PartialViewResult GetBlog(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<blog>();
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
            list_return = blog_provider.quickFilter(search_string, quick_filter, quick_filter_value);
            ViewBag.NumberOfList = list_return.Count();
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.blog_title).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.blog_title).ToList();
            }
            else if (sort_list_return == "lastest")
            {
                list_return = list_return.OrderByDescending(p => p.create_datetime).ToList();
            }

            return PartialView("_PartialViewBlog", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult InsertBlog()
        {
            return View();
        }

        [HttpPost]
        public bool CheckBlogTitleDuplicate(string blog_title)
        {

            return blog_provider.checkBlogTitleDuplicate(blog_title);
        }

        [HttpPost]
        public bool CheckSeoAliasDuplicate(string blog_seo_alias)
        {

            return blog_provider.checkBlogSeoAliasDuplicate(blog_seo_alias);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertBlog(blog model, List<string> tags, string blog_date_to_show, string blog_time_to_show, bool is_showing)
        {
            DateTime currentDateTime = System.DateTime.Now;
            blog blog = new blog();
            blog.blog_title = model.blog_title;
            blog.blog_seo_description = model.blog_seo_description;
            blog.blog_seo_alias = model.blog_seo_alias;
            blog.blog_seo_header = model.blog_seo_header;
            blog.blog_summary = model.blog_summary;
            blog.blog_content = model.blog_content;
            blog.user_id = getUserLoged().user_id;
            blog.blog_category_id = model.blog_category_id;
            blog.create_datetime = System.DateTime.Now;


            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (blog_date_to_show != "" && blog_time_to_show != "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else if (blog_date_to_show != "" && blog_time_to_show == "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else if (blog_date_to_show == "" && blog_time_to_show != "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else
                {
                    blog.status = false;
                    blog.show_datetime = null;
                }
            }
            else
            {
                blog.status = true;
                blog.show_datetime = System.DateTime.Now;
            }

            HttpPostedFileBase file = Request.Files["blog_image"];
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/blog_image")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                blog.blog_image = path_e;
                blog.blog_alt_image = model.blog_alt_image;
            }

            long blogId = 0;
            long tagId = 0;
            if (blog_provider.insertBlog(blog, ref blogId))
            {
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_blog_provider.insertTagBlog(blogId, tagId);
                        }
                    }
                }
                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo blog mới";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = blogId;
                system_log.system_log_url = "/Blog/BlogDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { blog_id = blogId }, JsonRequestBehavior.AllowGet);
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

        public ActionResult BlogDetail(long id)
        {
            ViewBag.PrefixUrl = prefix_url;
            return View(blog_provider.getById(id));
        }

        public ActionResult GetTagBlog(long blog_id)
        {
            var list = tag_provider.getByBlogId(blog_id);
            ViewBag.BlogID = blog_id;
            return PartialView("_PartialViewTagBlog", list);
        }

        [HttpPost]
        public ActionResult DeleteTagBlog(long tag_id, long blog_id)
        {
            if (tag_blog_provider.deleteTagCategoryByBlogIdAndTagId(tag_id, blog_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa tag khỏi blog " + blog_provider.getById(blog_id).blog_title;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = blog_id;
                system_log.system_log_url = "/Blog/BlogDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Remove tag Success");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateBlog(blog model, long blog_id, List<string> tags, string blog_date_to_show, string blog_time_to_show, bool is_showing, bool status_image_change)
        {
            DateTime currentDateTime = System.DateTime.Now;
            blog blog = new blog();
            blog.blog_title = model.blog_title;
            blog.blog_seo_description = model.blog_seo_description;
            blog.blog_seo_alias = model.blog_seo_alias;
            blog.blog_seo_header = model.blog_seo_header;
            blog.blog_summary = model.blog_summary;
            blog.blog_content = model.blog_content;
            blog.user_id = getUserLoged().user_id;
            blog.blog_category_id = model.blog_category_id;
            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (blog_date_to_show != "" && blog_time_to_show != "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else if (blog_date_to_show != "" && blog_time_to_show == "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else if (blog_date_to_show == "" && blog_time_to_show != "")
                {
                    blog.status = true;
                    string str = blog_date_to_show.Replace('/', '-') + " " + blog_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    blog.show_datetime = show_datetime;
                }
                else
                {
                    blog.status = false;
                    blog.show_datetime = null;
                }
            }
            else
            {
                blog.status = true;
                blog.show_datetime = System.DateTime.Now;
            }

            if (status_image_change == true)
            {
                HttpPostedFileBase file = Request.Files["blog_image"];
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/blog_image")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    file.SaveAs(filePath);
                    blog.blog_image = path_e;
                    blog.blog_alt_image = model.blog_alt_image;
                }
                else
                {
                    blog.blog_image = null;
                    blog.blog_alt_image = null;
                }
            }
            else
            {
                var old_model = blog_provider.getById(blog_id);
                blog.blog_image = old_model.blog_image;
                blog.blog_alt_image = model.blog_alt_image;
            }

            long tagId = 0;
            if (blog_provider.updateBlog(blog, blog_id))
            {
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_blog_provider.insertTagBlog(blog_id, tagId);
                        }
                    }
                }
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật blog";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = blog_id;
                system_log.system_log_url = "/Blog/BlogDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { blog_id = blog_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteBlog(List<long> list_blog_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {
            if(list_blog_id != null)
            {
                foreach(var blog_id in list_blog_id)
                {
                    var blog_name = blog_provider.getById(blog_id).blog_title;
                    if (blog_provider.deleteBlog(blog_id))
                    {
                        system_log system_log = new system_log();
                        system_log.system_log_description = "đã xóa blog " + blog_name;
                        system_log.user_id = getUserLoged().user_id;
                        system_log.system_log_time = System.DateTime.Now;
                        system_log.system_log_url = "/Blog/Index";
                        system_log_provider.insertSystemLog(system_log);
                    }
                }
            }
            
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        [HttpPost]
        public ActionResult HideStatusBlog(List<long> list_blog_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {

            if (blog_provider.hideStatusBlog(list_blog_id))
            {
                foreach (var blog_id in list_blog_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã ẩn blog " + blog_provider.getById(blog_id).blog_title;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Blog/BlogDetail";
                    system_log.system_log_object_id = blog_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        [HttpPost]
        public ActionResult ShowStatusBlog(List<long> list_blog_id, int? page_num, string search_string, string quick_filter, string quick_filter_value, string size_of_page, string sort_list_return)
        {
            if (blog_provider.showStatusBlog(list_blog_id))
            {
                foreach (var blog_id in list_blog_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã hiển thị blog " + blog_provider.getById(blog_id).blog_title;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Blog/BlogDetail";
                    system_log.system_log_object_id = blog_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter = quick_filter, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }
    }
}