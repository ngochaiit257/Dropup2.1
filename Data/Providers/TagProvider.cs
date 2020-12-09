using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TagProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<tag> getAll()
        {
            try
            {
                return db.tags.OrderBy(p => p.tag_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<tag> getByCategoryId(long category_id)
        {
            try
            {
                var lst_tag_category = db.tag_categories.Where(p => p.category_id == category_id);
                var lst_tag = getAll();
                var lst_return = new List<tag>();
                foreach (var tag_category in lst_tag_category)
                {
                    foreach (var tag in lst_tag)
                    {
                        if (tag_category.tag_id == tag.tag_id)
                        {
                            lst_return.Add(tag);
                        }
                    }
                }
                return lst_return;
            }
            catch
            {
                return null;
            }
        }

        public List<tag> getByProductId(long product_id)
        {
            try
            {
                var lst_tag_product = db.tag_products.Where(p => p.product_id == product_id);
                var lst_tag = getAll();
                var lst_return = new List<tag>();
                foreach (var tag_product in lst_tag_product)
                {
                    foreach (var tag in lst_tag)
                    {
                        if (tag_product.tag_id == tag.tag_id)
                        {
                            lst_return.Add(tag);
                        }
                    }
                }
                return lst_return;
            }
            catch
            {
                return null;
            }
        }

        public List<tag> getByBlogId(long blog_id)
        {
            try
            {
                var lst_tag_blog = db.tag_blogs.Where(p => p.blog_id == blog_id);
                var lst_tag = getAll();
                var lst_return = new List<tag>();
                foreach (var tag_blog in lst_tag_blog)
                {
                    foreach (var tag in lst_tag)
                    {
                        if (tag_blog.tag_id == tag.tag_id)
                        {
                            lst_return.Add(tag);
                        }
                    }
                }
                return lst_return;
            }
            catch
            {
                return null;
            }
        }

        public bool insertTag(string tag_name_input, ref long tagId)
        {
            try
            {
                tag model = new tag();
                var lst = getAll();
                bool check = true;
                if(lst.Count() > 0)
                {
                    foreach (var obj in lst)
                    {
                        if (obj.tag_name != tag_name_input || obj.tag_name == null || obj.tag_name == "")
                        {
                            model.tag_name = tag_name_input.ToLower();
                            check = true;
                        }
                        else
                        {
                            check = false;
                            tagId = obj.tag_id;
                        }
                    }
                }
                else
                {
                    model.tag_name = tag_name_input.ToLower();
                    check = true;
                }
                if(check == true)
                {
                    db.tags.InsertOnSubmit(model);
                    db.SubmitChanges();
                    tagId = model.tag_id;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        

        public tag getByTagId(long tag_id)
        {
            try
            {
                return db.tags.FirstOrDefault(p => p.tag_id == tag_id);
            }
            catch
            {
                return null;
            }
        }

        public bool updateTag(tag model)
        {
            try
            {
                var old_model = getByTagId(model.tag_id);
                old_model.tag_name = model.tag_name;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<tag> suggestTag(string tag_name)
        {
            try
            {
                return db.tags.Where(tag => tag.tag_name.Contains(tag_name)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<tag> getListTagByProductId(long product_id)
        {
            try
            {
                var lst_tag_product = db.tag_products.Where(p => p.product_id == product_id).ToList();
                var lst_all_tag = getAll();
                var lst_return = new List<tag>();
                foreach(var tag_product in lst_tag_product)
                {
                    foreach(var tag in lst_all_tag)
                    {
                        if(tag_product.tag_id == tag.tag_id)
                        {
                            lst_return.Add(tag);
                        }
                    }
                }
                return lst_return;
            }
            catch
            {
                return null;
            }
        }
    }
}
