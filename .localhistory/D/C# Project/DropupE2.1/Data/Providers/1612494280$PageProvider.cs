using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class PageProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<page> getAll()
        {
            try
            {
                return db.pages.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<page> getListShow()
        {
            try
            {
                return getAll().Where(p => p.show_datetime <= DateTime.Now && p.status == true).ToList();
            }
            catch
            {
                return null;
            }
        }
        public page getById(int page_id)
        {
            try
            {
                return db.pages.FirstOrDefault(p => p.page_id == page_id);
            }
            catch
            {
                return null;
            }
        }

        public List<page> getByPageParentId(long page_parent_id)
        {
            try
            {
                return db.pages.Where(p => p.page_parent_id == page_parent_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public page getBySeoAlias(string seo_alias)
        {
            try
            {
                return db.pages.FirstOrDefault(p => p.page_seo_alias == seo_alias);
            }
            catch
            {
                return null;
            }
        }
        private string ToSeoFriendly(string title, int maxLength)
        {
            var match = Regex.Match(title.ToLower(), "[\\w]+");
            StringBuilder result = new StringBuilder("");
            bool maxLengthHit = false;
            while (match.Success && !maxLengthHit)
            {
                if (result.Length + match.Value.Length <= maxLength)
                {
                    result.Append(match.Value + "-");
                }
                else
                {
                    maxLengthHit = true;
                    // Handle a situation where there is only one word and it is greater than the max length.
                    if (result.Length == 0) result.Append(match.Value.Substring(0, maxLength));
                }
                match = match.NextMatch();
            }
            // Remove trailing '-'
            if (result[result.Length - 1] == '-') result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public bool checkDupplicateSeoAlias(string seo_alias)
        {
            try
            {
                bool status = false;
                var list_seo_alias = new List<string>();
                foreach (var obj in getAll())
                {
                    if (obj.page_seo_alias.ToLower() == seo_alias.ToLower())
                    {
                        status = true;
                    }
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool insertPage(page model, ref int page_id)
        {
            try
            {
                if (model.page_seo_header == null || model.page_seo_header == "")
                {
                    model.page_seo_header = model.page_title;
                }
                if (model.page_seo_description == null || model.page_seo_description == "")
                {
                    model.page_seo_description = model.page_title;
                }
                if (model.page_seo_alias == null || model.page_seo_alias == "")
                {
                    model.page_seo_alias = ToSeoFriendly(model.page_title, 255);
                }
                if (checkDupplicateSeoAlias(model.page_seo_alias))
                {
                    model.page_seo_alias = ToSeoFriendly(model.page_title, 255) + "-1";
                }
                db.pages.InsertOnSubmit(model);
                db.SubmitChanges();
                page_id = model.page_id;
                return true;
            }
            catch
            {
                return false; 
            }
        }

        public bool updatePage(page new_model, int old_page_id)
        {
            try
            {
                var old_model = getById(old_page_id);
                old_model.page_title = new_model.page_title;
                old_model.page_content = new_model.page_content;
                old_model.page_seo_alias = new_model.page_seo_alias;
                old_model.page_seo_description = new_model.page_seo_description;
                old_model.page_seo_header = new_model.page_seo_header;
                old_model.user_id_update = new_model.user_id_update;
                old_model.update_datetime = new_model.update_datetime;
                old_model.show_datetime = new_model.show_datetime;
                old_model.status = new_model.status;
                if (old_model.page_seo_header == null || old_model.page_seo_header == "")
                {
                    old_model.page_seo_header = old_model.page_title;
                }
                if (old_model.page_seo_description == null || old_model.page_seo_description == "")
                {
                    old_model.page_seo_description = old_model.page_title;
                }
                if (old_model.page_seo_alias == null || old_model.page_seo_alias == "")
                {
                    old_model.page_seo_alias = ToSeoFriendly(old_model.page_title, 255);
                }
                //if (checkDupplicateSeoAlias(old_model.page_seo_alias))
                //{
                //    old_model.page_seo_alias = ToSeoFriendly(old_model.page_title, 255) + "-1";
                //}
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updatePageFull(page new_model, int old_page_id)
        {
            try
            {
                var old_model = getById(old_page_id);
                old_model.page_title = new_model.page_title;
                old_model.page_summary = new_model.page_summary;
                old_model.page_content = new_model.page_content;
                old_model.page_image = new_model.page_image;
                old_model.page_image_alt = new_model.page_image_alt;
                old_model.page_seo_alias = new_model.page_seo_alias;
                old_model.page_seo_description = new_model.page_seo_description;
                old_model.page_seo_header = new_model.page_seo_header;
                old_model.user_id_update = new_model.user_id_update;
                old_model.update_datetime = new_model.update_datetime;
                old_model.show_datetime = new_model.show_datetime;
                old_model.status = new_model.status;
                old_model.page_parent_id = new_model.page_parent_id;
                old_model.mount_on_header_menu_status = new_model.mount_on_header_menu_status;
                old_model.mount_on_footer_menu_status = new_model.mount_on_footer_menu_status;

                if (old_model.page_seo_header == null || old_model.page_seo_header == "")
                {
                    old_model.page_seo_header = old_model.page_title;
                }
                if (old_model.page_seo_description == null || old_model.page_seo_description == "")
                {
                    old_model.page_seo_description = old_model.page_title;
                }
                if (old_model.page_seo_alias == null || old_model.page_seo_alias == "")
                {
                    old_model.page_seo_alias = ToSeoFriendly(old_model.page_title, 255);
                }
                //if (checkDupplicateSeoAlias(old_model.page_seo_alias))
                //{
                //    old_model.page_seo_alias = ToSeoFriendly(old_model.page_title, 255) + "-1";
                //}
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool hideStatusPage(List<int> list_page_id)
        {
            try
            {
                foreach (var page_id in list_page_id)
                {
                    var page = getById(page_id);
                    page.status = false;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool showStatusPage(List<int> list_page_id)
        {
            try
            {
                foreach (var page_id in list_page_id)
                {
                    var page = getById(page_id);
                    page.status = true;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deletePage(int page_id)
        {
            try
            {
                var page = getById(page_id);
                var list_page_child = db.pages.Where(p => p.page_parent_id == page_id).ToList();
                foreach(var p in list_page_child)
                {
                    p.page_parent_id = null;
                }
                db.pages.DeleteOnSubmit(page);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool updateImagePage(int page_id, string img_url, string page_summary)
        {
            try
            {
                var old_model = getById(page_id);
                old_model.page_image = img_url;
                old_model.page_summary = page_summary;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkPageTitleDuplicate(string page_title)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.page_title.ToLower() == page_title.ToLower())
                    {
                        status = true;
                    }
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool checkPageSeoAliasDuplicate(string page_seo_alias)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.page_seo_alias.ToLower() == page_seo_alias.ToLower())
                    {
                        status = true;
                    }
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search_string"></param>
        /// <param name="quickFilterValue"></param>
        /// <returns></returns>
        public List<page> quickFilter(string search_string, string quickFilter, string quickFilterValue)
        {
            try
            {
                var list_page_return = new List<page>();
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quickFilter == "page_status")
                {
                    if (quickFilterValue == "show")
                    {
                        list_page_return = db.pages.Where(s => s.page_title.ToLower().Contains(search_string.ToLower()) && (s.show_datetime <= DateTime.Now || (s.show_datetime < DateTime.Now && s.status == true))).OrderByDescending(s => s.create_datetime).ToList();
                    }
                    else if (quickFilterValue == "hidden")
                    {
                        list_page_return = db.pages.Where(s => s.page_title.ToLower().Contains(search_string.ToLower()) && s.status == false).OrderByDescending(s => s.create_datetime).ToList();
                    }
                    else
                    {
                        list_page_return = db.pages.Where(s => s.page_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                    }
                }
                else
                {
                    list_page_return = db.pages.Where(s => s.page_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                }
                return list_page_return;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
