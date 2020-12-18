using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class BlogProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        BlogCategoryProvider bc_provider = new BlogCategoryProvider();
        TagBlogProvider tb_provider = new TagBlogProvider();

        public List<blog> getAll()
        {
            try
            {
                return db.blogs.OrderByDescending(b => b.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public blog getById(long blog_id)
        {
            try
            {
                return db.blogs.FirstOrDefault(b => b.blog_id == blog_id);
            }
            catch
            {
                return null;
            }
        }

        public List<blog> getIsShowing()
        {
            try
            {
                return db.blogs.Where(b => b.status == true && b.show_datetime <= System.DateTime.Now).OrderByDescending(b => b.create_datetime).ToList();
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
                    if (obj.blog_seo_alias.ToLower() == seo_alias.ToLower())
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

        public bool insertBlog(blog model, ref long blog_id)
        {
            try
            {
                if (model.blog_seo_header == null || model.blog_seo_header == "")
                {
                    model.blog_seo_header = model.blog_title;
                }
                if (model.blog_seo_description == null || model.blog_seo_description == "")
                {
                    model.blog_seo_description = model.blog_title;
                }
                if (model.blog_seo_alias == null || model.blog_seo_alias == "")
                {
                    model.blog_seo_alias = ToSeoFriendly(model.blog_title, 255);
                }
                //if (checkDupplicateSeoAlias(model.blog_seo_alias))
                //{
                //    model.blog_seo_alias = ToSeoFriendly(model.blog_title, 255) + "-1";
                //}
                db.blogs.InsertOnSubmit(model);
                db.SubmitChanges();
                blog_id = model.blog_id;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool checkBlogTitleDuplicate(string blog_title)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.blog_title.ToLower() == blog_title.ToLower())
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

        public bool checkBlogSeoAliasDuplicate(string blog_seo_alias)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.blog_seo_alias.ToLower() == blog_seo_alias.ToLower())
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

        public List<blog> getByCategoryName(string blog_category_name)
        {
            try
            {
                return db.blogs.Where(b => b.blog_category.blog_category_name == blog_category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<blog> getByCategoryIdShowing(int blog_category_id)
        {
            try
            {
                return db.blogs.Where(b => b.blog_category.blog_category_id == blog_category_id && b.status == true && b.show_datetime <= DateTime.Now).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search_string"></param>
        /// <param name="quickFilterValue"></param>
        /// <returns></returns>
        public List<blog> quickFilter(string search_string, string quickFilter, string quickFilterValue)
        {
            try
            {
                var list_blog_return = new List<blog>();
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quickFilter == "blog_status")
                {
                    if (quickFilterValue == "show")
                    {
                        list_blog_return = db.blogs.Where(s => s.blog_title.ToLower().Contains(search_string.ToLower()) && (s.show_datetime <= DateTime.Now || (s.show_datetime < DateTime.Now && s.status == true))).OrderByDescending(s => s.create_datetime).ToList();
                    }
                    else if (quickFilterValue == "hidden")
                    {
                        list_blog_return = db.blogs.Where(s => s.blog_title.ToLower().Contains(search_string.ToLower()) && s.status == false).OrderByDescending(s => s.create_datetime).ToList();
                    }
                    else
                    {
                        list_blog_return = db.blogs.Where(s => s.blog_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                    }
                }
                else if (quickFilter == "blog_category")
                {
                    if (quickFilterValue != "" || quickFilterValue != null)
                    {
                        var list_blog = getByCategoryName(quickFilterValue);
                        list_blog_return = list_blog.Where(p => p.blog_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                    }
                    else
                    {
                        list_blog_return = db.blogs.Where(s => s.blog_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                    }
                }
                else
                {
                    list_blog_return = db.blogs.Where(s => s.blog_title.ToLower().Contains(search_string.ToLower())).OrderByDescending(s => s.create_datetime).ToList();
                }
                return list_blog_return;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<blog> getListShowOnClientSite()
        {
            try
            {
                return getIsShowing().OrderByDescending(b => b.show_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<blog> getListShowOnClientSite(long blog_category_id)
        {
            try
            {
                return getIsShowing().Where(b => b.blog_category_id == blog_category_id).OrderByDescending(b => b.show_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public blog getBySeoAlias(string seo_alias)
        {
            try
            {
                return db.blogs.FirstOrDefault(b => b.blog_seo_alias == seo_alias);
            }
            catch
            {
                return null;
            }
        }

        public bool updateBlog(blog model, long blog_id)
        {
            try
            {
                var old_model = getById(blog_id);
                old_model.blog_title = model.blog_title;
                old_model.blog_content = model.blog_content;
                old_model.blog_summary = model.blog_summary;
                old_model.show_datetime = model.show_datetime;
                old_model.status = model.status;
                old_model.user_id = model.user_id;
                old_model.blog_seo_alias = model.blog_seo_alias;
                old_model.blog_seo_description = model.blog_seo_description;
                old_model.blog_seo_header = model.blog_seo_header;
                old_model.blog_alt_image = model.blog_alt_image;
                old_model.blog_image = model.blog_image;
                old_model.blog_category_id = model.blog_category_id;
                if (old_model.blog_seo_header == null || old_model.blog_seo_header == "")
                {
                    old_model.blog_seo_header = old_model.blog_title;
                }
                if (old_model.blog_seo_description == null || old_model.blog_seo_description == "")
                {
                    old_model.blog_seo_description = old_model.blog_title;
                }
                if (old_model.blog_seo_alias == null || old_model.blog_seo_alias == "")
                {
                    old_model.blog_seo_alias = ToSeoFriendly(old_model.blog_title, 255);

                }
                //if (checkDupplicateSeoAlias(old_model.blog_seo_alias))
                //{
                //    old_model.blog_seo_alias = ToSeoFriendly(old_model.blog_title, 255) + "-1";
                //}
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool hideStatusBlog(List<long> list_blog_id)
        {
            try
            {
                foreach (var blog_id in list_blog_id)
                {
                    var blog = getById(blog_id);
                    blog.status = false;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool showStatusBlog(List<long> list_blog_id)
        {
            try
            {
                foreach (var blog_id in list_blog_id)
                {
                    var blog = getById(blog_id);
                    blog.status = true;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteListBlog(List<long> list_blog_id)
        {
            try
            {
                foreach (var blog_id in list_blog_id)
                {
                    var blog = getById(blog_id);
                    db.blogs.DeleteOnSubmit(blog);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteBlog(long blog_id)
        {
            try
            {
                if (tb_provider.deleteTagBlogByBlogId(blog_id))
                {
                    var blog = getById(blog_id);
                    db.blogs.DeleteOnSubmit(blog);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
