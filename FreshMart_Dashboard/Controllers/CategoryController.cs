using Data;
using Data.Providers;
using ExtendMethod;
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
    public class CategoryController : Controller
    {
        CategoryProvider category_provider = new CategoryProvider();
        TagProvider tag_provider = new TagProvider();
        TagCategoryProvider tag_category_provider = new TagCategoryProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        TypeOfCategoryProvider toc_provider = new TypeOfCategoryProvider();

        string prefix_url = System.Configuration.ConfigurationManager.AppSettings["urlPresentationFreshMart"];
        // GET: Category

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }
        public ActionResult Index(int? page_num, string search_string, string quick_filter_value)
        {
            if (quick_filter_value == null)
            {
                quick_filter_value = "all";
            }
            ViewBag.QuickFilter = quick_filter_value;
            ViewBag.FilterValue = search_string;
            ViewBag.page_num = page_num;
            return View();
        }

        public PartialViewResult GetCategory(int? page_num, string search_string, string quick_filter_value)
        {
            var lst = new List<category>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }

            ViewBag.page_num = page_num;
            int Size_Of_Page = 10;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;

            ViewBag.QuickFilter = quick_filter_value;

            lst = category_provider.quickFilter(search_string, quick_filter_value);
            ViewBag.NumberOfList = lst.Count();

            return PartialView("_PartialViewCategory", lst.ToPagedList(No_Of_Page, Size_Of_Page));

        }

        public JsonResult GetSuggestTag(string tag_name)
        {
            return new JsonResult { Data = tag_provider.suggestTag(tag_name), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult InsertCategory()
        {
            ViewData["ListTypeOfCategory"] = toc_provider.getByShowStatusTrue();
            ViewData["ListCategoryParent"] = category_provider.getListParentCategory();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertCategory(category model, List<string> tags, string category_date_to_show, string category_time_to_show)
        {
            category category = new category();
            category.category_name = model.category_name;
            category.category_meta_title = MetaTitle.ConvertToUnSign(category.category_name).ToLower();
            category.description = model.description;
            category.create_date = System.DateTime.Now;
            category.update_date = System.DateTime.Now;
            category.user_id = getUserLoged().user_id;
            category.seo_header = model.seo_header;
            category.seo_description = model.seo_description;
            category.seo_alias = model.seo_alias;
            category.type_of_category_id = model.type_of_category_id;
            category.category_parent_id = model.category_parent_id;
            DateTime show_datetime = new DateTime();

            if(category_date_to_show != null && category_time_to_show != null)
            {
                if (category_date_to_show != "" && category_time_to_show != "")
                {
                    category.status = false;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else if (category_date_to_show != "" && category_time_to_show == "")
                {
                    category.status = false;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else if (category_date_to_show == "" && category_time_to_show != "")
                {
                    category.status = false;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else
                {
                    category.status = true;
                    category.show_datetime = System.DateTime.Now;
                }
            }
            else
            {
                category.status = true;
                category.show_datetime = System.DateTime.Now;
            }

            HttpPostedFileBase file = Request.Files["category_image"];
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/category_image")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                category.image = path_e;
                category.alt_image = model.alt_image;
            }

            long catId = 0;
            long tagId = 0;
            if (category_provider.insertCategory(category, ref catId))
            {
                if(tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_category_provider.insertTagCategory(catId, tagId);
                        }
                    }
                }
                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo danh mục " + model.category_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = catId;
                system_log.system_log_url = "/Category/CategoryDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { category_id = catId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool checkDuplicate(string category_name)
        {

            return category_provider.checkDuplicate(category_name);
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        [HttpPost]
        public ActionResult HideStatusCategory(List<long> list_category_id, int? page_num, string search_string, string quick_filter_value)
        {

            if (category_provider.hideStatusCategory(list_category_id))
            {
                foreach (var category_id in list_category_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã ẩn danh mục " + category_provider.getByCategoryId(category_id).category_name;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Category/CategoryDetail";
                    system_log.system_log_object_id = category_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter_value = quick_filter_value });
        }

        [HttpPost]
        public ActionResult ShowStatusCategory(List<long> list_category_id, int? page_num, string search_string, string quick_filter_value)
        {
            if (category_provider.showStatusCategory(list_category_id))
            {
                foreach (var category_id in list_category_id)
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã hiển thị danh mục " + category_provider.getByCategoryId(category_id).category_name;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Category/CategoryDetail";
                    system_log.system_log_object_id = category_id;
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter_value = quick_filter_value });
        }

        [HttpPost]
        public ActionResult DeleteCategory(List<long> list_category_id, int? page_num, string search_string, string quick_filter_value)
        {
            foreach (var category_id in list_category_id)
            {
                tag_category_provider.deleteTagCategoryByCategoryId(category_id);

                var category_name_to_delete = category_provider.getByCategoryId(category_id).category_name;
                if (category_provider.deleteCategory(category_provider.getByCategoryId(category_id)))
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã xóa danh mục " + category_name_to_delete;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Category/Index";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter_value = quick_filter_value });
        }

        public ActionResult CategoryDetail(long id)
        {
            ViewData["ListTypeOfCategory"] = toc_provider.getByShowStatusTrue();
            ViewData["ListCategoryParent"] = category_provider.getListParentCategory();
            ViewData["ListTagCategory"] = tag_provider.getByCategoryId(id);
            ViewData["ListCategory"] = category_provider.getAll();
            var category = category_provider.getByCategoryId(id);
            ViewBag.PrefixUrl = prefix_url;
            return View(category_provider.getByCategoryId(id));
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateCategory(category model, long category_id, List<string> tags, string category_date_to_show, string category_time_to_show, bool is_showing, bool status_image_change)
        {
            var old_model = category_provider.getByCategoryId(category_id);
            category category = new category();
            category.category_name = model.category_name;
            category.category_meta_title = MetaTitle.ConvertToUnSign(category.category_name).ToLower();
            category.description = model.description;
            category.update_date = System.DateTime.Now;
            category.user_id = getUserLoged().user_id;
            category.seo_header = model.seo_header;
            category.seo_description = model.seo_description;
            category.seo_alias = model.seo_alias;
            category.type_of_category_id = model.type_of_category_id;
            category.category_parent_id = model.category_parent_id;
            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (category_date_to_show != "" && category_time_to_show != "")
                {
                    category.status = true;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else if (category_date_to_show != "" && category_time_to_show == "")
                {
                    category.status = true;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else if (category_date_to_show == "" && category_time_to_show != "")
                {
                    category.status = true;
                    string str = category_date_to_show.Replace('/', '-') + " " + category_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    category.show_datetime = show_datetime;
                }
                else
                {
                    category.status = false;
                    category.show_datetime = null;
                }
            }
            else
            {
                category.status = true;
                category.show_datetime = System.DateTime.Now;
            }

            if (status_image_change == true)
            {
                HttpPostedFileBase file = Request.Files["category_image"];
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/category_image")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    file.SaveAs(filePath);
                    category.image = path_e;
                    category.alt_image = model.alt_image;
                }
                else
                {
                    category.image = null;
                    category.alt_image = null;
                }
            }
            else
            {
                category.image = old_model.image;
                category.alt_image = old_model.alt_image;
            }

            long tagId = 0;
            if (category_provider.updateCategory(category, category_id))
            {
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_category_provider.insertTagCategory(category_id, tagId);
                        }
                    }
                }
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật danh mục " + model.category_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = category_id;
                system_log.system_log_url = "/Category/CategoryDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { category_id = category_id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTagCategory(long category_id)
        {
            var list = tag_provider.getByCategoryId(category_id);
            ViewBag.CategoryId = category_id;
            return PartialView("_PartialViewTagCategory", list);
        }

        [HttpPost]
        public ActionResult DeleteTagCategory(long tag_id, long category_id)
        {
            if (tag_category_provider.deleteTagCategoryByCategoryIdAndTagId(tag_id, category_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa tag khỏi danh mục " + category_provider.getByCategoryId(category_id).category_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = category_id;
                system_log.system_log_url = "/Category/CategoryDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Remove tag Success");
        }


    }
}