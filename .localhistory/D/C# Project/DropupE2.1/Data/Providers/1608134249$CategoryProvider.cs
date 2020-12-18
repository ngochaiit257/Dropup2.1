using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using System.Text.RegularExpressions;

namespace Data.Providers
{
    public class CategoryProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<category> getAll()
        {
            try
            {
                return db.categories.OrderBy(p => p.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getByTypeNameOnSite(string type_name)
        {
            try
            {
                return db.categories.Where(c => c.type_of_category.type_of_category_name.ToLower() == type_name.ToLower()).OrderBy(c => c.category_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getByCategoryParentId(long cp_id)
        {
            try
            {
                return db.categories.Where(c => c.category_parent_id == cp_id).OrderBy(c => c.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getByCategoryParentName(string category_parent_name)
        {
            try
            {
                return db.categories.Where(c => c.category1.category_name.ToLower() == category_parent_name.ToLower()).OrderBy(c => c.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getByStatusTrue()
        {
            try
            {
                return db.categories.Where(p => p.status == true).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getByProductId(long product_id)
        {
            try
            {
                var list_category_product = db.category_products.Where(p => p.product_id == product_id).ToList();
                var lst_category = getAll();
                var lst_return = new List<category>();
                foreach (var cp in list_category_product)
                {
                    foreach (var category in lst_category)
                    {
                        if (cp.category_id == category.category_id)
                        {
                            lst_return.Add(category);
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

        public List<category> getListCategoryAreShowing()
        {
            try
            {
                return db.categories.Where(c => c.create_date <= DateTime.Now).OrderBy(c => c.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<long> getCategoryIdByProductVariationId(long product_variation_id)
        {
            try
            {
                var list_return = new List<long>();
                var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == product_variation_id);
                var product = db.products.FirstOrDefault(p => p.product_id == product_variation.product_id);
                var list_category_product = db.category_products.Where(cp => cp.product_id == product.product_id).ToList();
                foreach (var cp in list_category_product)
                {
                    list_return.Add(cp.category_id);
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public category getByCategoryId(long category_id)
        {
            try
            {
                return db.categories.FirstOrDefault(p => p.category_id == category_id);
            }
            catch
            {
                return null;
            }
        }

        public category getBySeoAlias(String seo_alias)
        {
            try
            {
                return db.categories.FirstOrDefault(p => p.seo_alias == seo_alias);
            }
            catch
            {
                return null;
            }
        }

        public List<long> getListCategoryIdByListProductVariationId(List<long> list_product_variation_id)
        {
            try
            {
                List<long> list_category_id = new List<long>();
                foreach (var product_variation_id in list_product_variation_id)
                {
                    long product_id = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == product_variation_id).product_id;
                    var list_category_product = db.category_products.Where(cp => cp.product_id == product_id).ToList();
                    foreach (var cp in list_category_product)
                    {
                        foreach (var category in db.categories.ToList())
                        {
                            if (cp.category_id == category.category_id)
                            {
                                list_category_id.Add(category.category_id);
                            }
                        }
                    }
                }
                return list_category_id;
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
                foreach(var obj in getAll())
                {
                    if (obj.seo_alias.ToLower() == seo_alias.ToLower())
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

        public bool insertCategory(category model, ref long catId)
        {
            try
            {
                if (model.seo_header == null || model.seo_header == "")
                {
                    model.seo_header = model.category_name;
                }
                if (model.seo_description == null || model.seo_description == "")
                {
                    model.seo_description = model.category_name;
                }
                if (model.seo_alias == null || model.seo_alias == "")
                {
                    model.seo_alias = ToSeoFriendly(model.category_name, 255);
                }
                //if (checkDupplicateSeoAlias(model.seo_alias))
                //{
                //    model.seo_alias = ToSeoFriendly(model.category_name, 255) + "-1";
                //}
                db.categories.InsertOnSubmit(model);
                db.SubmitChanges();
                catId = model.category_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteListCategory(List<long> list_category_id)
        {
            try
            {
                foreach (var category_id in list_category_id)
                {
                    var category = getByCategoryId(category_id);
                    db.categories.DeleteOnSubmit(category);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool deleteCategory(category model)
        {
            try
            {
                var model_to_delete = getByCategoryId(model.category_id);

                var list_category_child_of_this_category = getAll().Where(c => c.category_parent_id == model_to_delete.category_id).ToList();
                if(list_category_child_of_this_category != null)
                {
                    foreach(var category_child in list_category_child_of_this_category)
                    {
                        category_child.category_parent_id = null;
                    }
                    db.SubmitChanges();
                }

                var list_category_product = db.category_products.Where(cp => cp.category_id == model_to_delete.category_id).ToList();
                if(list_category_product != null)
                {
                    db.category_products.DeleteAllOnSubmit(list_category_product);
                }

                db.categories.DeleteOnSubmit(model_to_delete);
                db.SubmitChanges();
                return true;
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
        public List<category> quickFilter(string search_string, string quickFilterValue)
        {
            try
            {
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quickFilterValue == "show")
                {
                    return db.categories.Where(s => s.category_name.ToLower().Contains(search_string.ToLower()) && s.status == true).OrderByDescending(s => s.category_name).ToList();
                }
                else if (quickFilterValue == "hidden")
                {
                    return db.categories.Where(s => s.category_name.ToLower().Contains(search_string.ToLower()) && s.status == false).OrderByDescending(s => s.category_name).ToList();
                }
                else
                {
                    return db.categories.Where(s => s.category_name.ToLower().Contains(search_string.ToLower())).OrderBy(s => s.category_id).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool checkDuplicate(string category_name)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.category_name.ToLower() == category_name.ToLower())
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




        public List<category> getBySearchString(string search_string)
        {
            try
            {
                if (search_string == null)
                {
                    search_string = "";
                }
                return db.categories.Where(c => c.category_name.Contains(search_string)).OrderBy(c => c.category_name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool hideStatusCategory(List<long> list_category_id)
        {
            try
            {
                foreach (var category_id in list_category_id)
                {
                    var category = getByCategoryId(category_id);
                    category.status = false;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool showStatusCategory(List<long> list_category_id)
        {
            try
            {
                foreach (var category_id in list_category_id)
                {
                    var category = getByCategoryId(category_id);
                    category.status = true;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public List<product> getByConditionOfProduct(bool condition_status, List<ConditionOfProductModel> list_model_condition)
        //{
        //    try
        //    {
        //        var list_return = new List<product>();
        //        foreach (var model in list_model_condition)
        //        {
        //            var list_by_price = new List<product>();
        //            var list_by_string = new List<product>();
        //            if(model.condition_name == "product_price")
        //            {
        //                if(model.condition_filter == "equal")
        //                {
        //                    list_by_price = db.products.Where(p => p.price == model.condition_product_price_value).ToList();
        //                }
        //                else if(model.condition_filter == "more")
        //                {
        //                    list_by_price = db.products.Where(p => p.price > model.condition_product_price_value).ToList();
        //                }
        //                else if(model.condition_filter == "less")
        //                {
        //                    list_by_price = db.products.Where(p => p.price < model.condition_product_price_value).ToList();
        //                }
        //            }
        //            else if(model.condition_filter == "product_name")
        //            {
        //                list_by_string = db.products.Where(p => p.product_name.Contains(model.condition_product_name_contain_value)).ToList();
        //            }
        //            else if (model.condition_filter == "product_supplier")
        //            {
        //                list_by_string = db.products.Where(p => p.supplier.supplier_name.Contains(model.condition_product_name_contain_value)).ToList();
        //            }
        //            else if (model.condition_filter == "product_tag")
        //            {
        //                var list_tag_product = db.tag_products.Where(t => t.tag.tag_name.Contains(model.condition_product_name_contain_value));
        //                foreach (var tag_product in list_tag_product)
        //                {
        //                    foreach(var product in db.products.OrderBy(p => p.product_id).ToList())
        //                    {
        //                        if(tag_product.product_id == product.product_id)
        //                        {
        //                            list_by_string.Add(product);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public List<category> getListParentCategory()
        {
            try
            {
                return db.categories.Where(c => c.category_parent_id == null).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool updateCategory(category model, long category_id)
        {
            try
            {
                var old_model = getByCategoryId(category_id);
                old_model.category_name = model.category_name;
                old_model.description = model.description;
                old_model.seo_alias = model.seo_alias;
                old_model.seo_description = model.seo_description;
                old_model.seo_header = model.seo_header;
                old_model.status = model.status;
                old_model.show_datetime = model.show_datetime;
                old_model.image = model.image;
                old_model.alt_image = model.alt_image;
                old_model.type_of_category_id = model.type_of_category_id;
                old_model.category_parent_id = model.category_parent_id;
                old_model.update_date = model.update_date;
                if (old_model.seo_header == null || old_model.seo_header == "")
                {
                    old_model.seo_header = old_model.category_name;
                }
                if (old_model.seo_description == null || old_model.seo_description == "")
                {
                    old_model.seo_description = old_model.category_name;
                }
                if (old_model.seo_alias == null || old_model.seo_alias == "")
                {
                    old_model.seo_alias = ToSeoFriendly(old_model.category_name, 255);

                }
                //if (checkDupplicateSeoAlias(old_model.seo_alias))
                //{
                //    old_model.seo_alias = ToSeoFriendly(old_model.category_name, 255) + "-1";
                //}
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public category getCategoryParentByCategoryId(long category_id)
        {
            try
            {
                var category = getByCategoryId(category_id);
                return getByCategoryId((long)category.category_parent_id);
            }
            catch
            {
                return null;
            }
        }

        public List<category> getCategoryChildByCategoryId(long category_id)
        {
            try
            {
                return db.categories.Where(c => c.category_parent_id == category_id).ToList();
            }
            catch
            {
                return null;
            }
        }

       

        public List<category> getListBenhNamKhoa()
        {
            try
            {
                return getListParentCategory().Where(c => (c.status == true && c.show_datetime <= DateTime.Now) && c.type_of_category_id == 1).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getListBenhXaHoi()
        {
            try
            {
                return getListParentCategory().Where(c => (c.status == true && c.show_datetime <= DateTime.Now) && c.type_of_category_id == 2).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<category> getListBenhChinh()
        {
            try
            {
                return getAll().Where(c => (c.status == true && c.show_datetime <= DateTime.Now) && (c.image != "../Assets/img/category_image/Logo Bach Giai.png" && c.image != "../Assets/img/category_image/BG.png" && c.image != "")).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
