using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ProductProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        CategoryProvider category_provider = new CategoryProvider();
        SupplierProvider supplier_provider = new SupplierProvider();
        ElementVariationProvider ev_provider = new ElementVariationProvider();
        CategoryProductProvider category_product_provider = new CategoryProductProvider();
        TagProductProvider tag_product_provider = new TagProductProvider();
        CommentProductProvider comment_product_provider = new CommentProductProvider();
        ProductElementProvider pe_provider = new ProductElementProvider();
        ComboProductProvider combo_product_provider = new ComboProductProvider();
        TagProvider tag_provider = new TagProvider();

        public List<product> getAll()
        {
            try
            {
                return db.products.OrderBy(p => p.product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<product> getNewestShowing()
        {
            try
            {
                return db.products.OrderByDescending(p => p.show_datetime).Where(p => p.show_datetime <= System.DateTime.Now && p.status == true).ToList();
            }
            catch
            {
                return null;
            }
        }


        public product getById(long product_id)
        {
            try
            {
                return db.products.FirstOrDefault(p => p.product_id == product_id);
            }
            catch
            {
                return null;
            }
        }


        public List<product> getByCategoryNameOnSite(string category_name)
        {
            try
            {
                var list_return = new List<product>();
                var list_category_product = db.category_products.Where(cp => cp.category.category_name.ToLower() == category_name.ToLower()).ToList();
                foreach (var obj in list_category_product)
                {
                    foreach (var product in getAll())
                    {
                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                        {
                            list_return.Add(product);
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByCategoryIdOnSite(long category_id)
        {
            try
            {
                var list_return = new List<product>();

                var list_category_child_1 = db.categories.Where(c => c.category_parent_id == category_id);
                if (list_category_child_1.Count() > 0)
                {
                    foreach (var category_child_1 in list_category_child_1)
                    {
                        var list_category_child_2 = db.categories.Where(c => c.category_parent_id == category_child_1.category_id);
                        if (list_category_child_2.Count() > 0)
                        {
                            foreach (var category_child_2 in list_category_child_2)
                            {
                                var list_category_product_2 = db.category_products.Where(cp => cp.category.category_id == category_child_2.category_id).ToList();
                                foreach (var obj in list_category_product_2)
                                {
                                    foreach (var product in getAll())
                                    {
                                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                                        {
                                            if (!list_return.Contains(product))
                                            {
                                                list_return.Add(product);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var list_category_product_1 = db.category_products.Where(cp => cp.category.category_id == category_child_1.category_id).ToList();
                        foreach (var obj in list_category_product_1)
                        {
                            foreach (var product in getAll())
                            {
                                if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                                {
                                    if (!list_return.Contains(product))
                                    {
                                        list_return.Add(product);
                                    }
                                }
                            }
                        }
                    }
                }

                var list_category_product = db.category_products.Where(cp => cp.category.category_id == category_id).ToList();
                foreach (var obj in list_category_product)
                {
                    foreach (var product in getAll())
                    {
                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                        {
                            if (!list_return.Contains(product))
                            {
                                list_return.Add(product);
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByCategorySeoAliasOnSite(string seo_alias)
        {
            try
            {
                var category = db.categories.FirstOrDefault(c => c.seo_alias.ToLower() == seo_alias.ToLower());
                return getByCategoryIdOnSite(category.category_id);
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByKeyWordOnSite(string keyword, string select_order)
        {
            try
            {
                try
                {
                    var list_return = new List<product>();
                    if (select_order == "newest")
                    {
                        list_return = db.products.Where(p => p.product_name.ToLower().Contains(keyword.ToLower())).OrderByDescending(p => p.create_date).ToList();
                    }
                    else if (select_order == "name_az")
                    {
                        list_return = db.products.Where(p => p.product_name.ToLower().Contains(keyword.ToLower())).OrderBy(p => p.product_name).ToList();
                    }
                    else if (select_order == "name_za")
                    {
                        list_return = db.products.Where(p => p.product_name.ToLower().Contains(keyword.ToLower())).OrderByDescending(p => p.product_name).ToList();
                    }
                    else
                    {
                        list_return = db.products.Where(p => p.product_name.ToLower().Contains(keyword.ToLower())).ToList();
                    }
                    return list_return;
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public product getBySEOalias(string seo_alias)
        {
            try
            {
                return db.products.FirstOrDefault(p => p.seo_alias == seo_alias);
            }
            catch
            {
                return null;
            }
        }

        public long getProductIdByProductVariationId(long product_variation_id)
        {
            try
            {
                List<long> list_category_id = new List<long>();
                long product_id = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == product_variation_id).product_id;
                return product_id;
            }
            catch
            {
                return -1;
            }
        }

        public List<long> getListProductIdByListProductVariationId(List<long> list_product_variation_id)
        {
            try
            {
                var list_return = new List<long>();
                foreach (var product_variation_id in list_product_variation_id)
                {
                    list_return.Add(getProductIdByProductVariationId(product_variation_id));
                }
                return list_return;
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
        public bool insertProduct(product model, ref long proId)
        {
            try
            {
                if (model.seo_header == null || model.seo_header == "")
                {
                    model.seo_header = model.product_name;
                }
                if (model.seo_description == null || model.seo_description == "")
                {
                    model.seo_description = model.product_name;
                }
                if (model.seo_alias == null || model.seo_alias == "")
                {
                    model.seo_alias = ToSeoFriendly(model.product_name, 255);
                }
                //if (checkDupplicateSeoAlias(model.seo_alias))
                //{
                //    model.seo_alias = ToSeoFriendly(model.product_name, 255) + "-1";
                //}
                db.products.InsertOnSubmit(model);
                db.SubmitChanges();
                proId = model.product_id;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool checkDuplicate(string product_name)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.product_name.ToLower() == product_name.ToLower())
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

        public List<product> getByCategoryName(string category_name)
        {
            try
            {
                var list_category_product = db.category_products.Where(cp => cp.category.category_name == category_name);
                var list_product = new List<product>();
                foreach (var category_product in list_category_product)
                {
                    foreach (var product in getAll())
                    {
                        if (category_product.product_id == product.product_id)
                        {
                            list_product.Add(product);
                        }
                    }
                }
                return list_product;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getBySupplierName(string supplier_name)
        {
            try
            {
                return db.products.Where(p => p.supplier.supplier_name == supplier_name).ToList();
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
        public List<product> quickFilter(string search_string, string quickFilter, string quickFilterValue)
        {
            try
            {
                var list_category = category_provider.getAll();
                var list_supplier = supplier_provider.getAll();
                var list_product_return = new List<product>();

                if (search_string == null)
                {
                    search_string = "";
                }
                if (quickFilter == "product_status")
                {
                    if (quickFilterValue == "show")
                    {
                        list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower()) && s.status == true).OrderBy(s => s.product_name).ToList();
                    }
                    else if (quickFilterValue == "hidden")
                    {
                        list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower()) && s.status == false).OrderBy(s => s.product_name).ToList();
                    }
                    else
                    {
                        list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(s => s.product_name).ToList();
                    }
                }
                else if (quickFilter == "category")
                {
                    if (quickFilterValue != "" || quickFilterValue != null)
                    {
                        var list_product = getByCategoryName(quickFilterValue);
                        list_product_return = list_product.Where(p => p.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(p => p.product_name).ToList();
                    }
                    else
                    {
                        list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(s => s.product_name).ToList();
                    }
                }
                else if (quickFilter == "supplier")
                {
                    if (quickFilterValue != "" || quickFilterValue != null)
                    {
                        var list_product = getBySupplierName(quickFilterValue);
                        list_product_return = list_product.Where(p => p.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(p => p.product_name).ToList();
                    }
                    else
                    {
                        list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(s => s.product_name).ToList();
                    }
                }
                else
                {
                    list_product_return = db.products.Where(s => s.product_name.ToLower().Contains(search_string.ToLower())).OrderBy(s => s.product_name).ToList();
                }
                return list_product_return;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<product> filterOnClientSiteByTab(string tab_filter, List<product> list_product_input)
        {
            try
            {
                var list_return = new List<product>();
                if (tab_filter == "newest")
                {
                    list_return = list_product_input.OrderByDescending(p => p.create_date).ToList();
                }
                else if (tab_filter == "price_asc")
                {
                    list_return = list_product_input.OrderBy(p => p.promotion_price).ToList();
                }
                else if (tab_filter == "price_desc")
                {
                    list_return = list_product_input.OrderByDescending(p => p.promotion_price).ToList();
                }
                else if (tab_filter == "promotion")
                {
                    list_return = list_product_input.Where(p => p.price != null && p.promotion_price != null).OrderByDescending(p => (p.price - p.promotion_price)).ToList();
                }
                else
                {
                    list_return = list_product_input;
                }
                return list_return;
            }
            catch
            {
                return list_product_input;
            }
        }

        public List<product> OrderBySelectOrder(string select_order, List<product> list_product_input)
        {
            try
            {
                var list_return = new List<product>();
                if (select_order == "newest")
                {
                    list_return = list_product_input.OrderByDescending(p => p.create_date).ToList();
                }
                else if (select_order == "name_az")
                {
                    list_return = list_product_input.OrderBy(p => p.product_name).ToList();
                }
                else if (select_order == "name_za")
                {
                    list_return = list_product_input.OrderByDescending(p => p.product_name).ToList();
                }
                else
                {
                    list_return = list_product_input;
                }
                return list_return;
            }
            catch
            {
                return list_product_input;
            }
        }

        public List<product> getBySupplierId(List<int> list_supplier_id, List<product> list_input)
        {
            try
            {
                var list_get = new List<product>();
                var list_return = new List<product>();
                if (list_supplier_id != null && list_supplier_id.Count() != 0)
                {
                    foreach (int supplier_id in list_supplier_id)
                    {
                        list_get = list_input.Where(p => p.supplier_id == supplier_id).ToList();
                        foreach (var product in list_get)
                        {
                            if (!list_return.Contains(product))
                            {
                                list_return.Add(product);
                            }
                        }
                    }
                }
                else
                {
                    list_return = list_input;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByConditionMoney(List<string> list_condition_money, List<product> list_input)
        {
            try
            {
                var list_get = new List<product>();
                var list_return = new List<product>();
                if (list_condition_money != null && list_condition_money.Count() != 0)
                {
                    foreach (var condition_money in list_condition_money)
                    {
                        if (condition_money == "<100k")
                        {
                            list_get = list_input.Where(p => p.promotion_price < 100000).ToList();
                        }
                        else if (condition_money == "100k-200k")
                        {
                            list_get = list_input.Where(p => p.promotion_price >= 100000 && p.promotion_price <= 200000).ToList();
                        }
                        else if (condition_money == "200k-300k")
                        {
                            list_get = list_input.Where(p => p.promotion_price >= 200000 && p.promotion_price <= 300000).ToList();
                        }
                        else if (condition_money == "300k-500k")
                        {
                            list_get = list_input.Where(p => p.promotion_price >= 300000 && p.promotion_price <= 500000).ToList();
                        }
                        else if (condition_money == "500k-1000k")
                        {
                            list_get = list_input.Where(p => p.promotion_price >= 500000 && p.promotion_price <= 1000000).ToList();
                        }
                        else if (condition_money == ">1000k")
                        {
                            list_get = list_input.Where(p => p.promotion_price > 1000000).ToList();
                        }
                        else
                        {
                            list_return = list_input;
                        }
                        foreach (var product in list_get)
                        {
                            if (!list_return.Contains(product))
                            {
                                list_return.Add(product);
                            }
                        }
                    }
                }
                else
                {
                    list_return = list_input;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByMoneyRange(decimal min_price, decimal max_price, List<product> list_input)
        {
            try
            {
                var list_return = new List<product>();
                if (min_price >= 0 && max_price >= 0)
                {
                    list_return = list_input.Where(p => p.promotion_price >= min_price && p.promotion_price <= max_price).ToList();
                }
                else
                {
                    list_return = list_input;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByColor(string color, List<product> list_input)
        {
            try
            {
                var list_return = new List<product>();
                if (color == "" || color == "Tất cả")
                {
                    list_return = list_input;
                }
                else
                {
                    foreach (var product in list_input)
                    {
                        bool is_need = false;
                        var list_pv = db.product_variations.Where(pv => pv.product_id == product.product_id).ToList();
                        foreach (var pv in list_pv)
                        {
                            foreach (var ev in ev_provider.getByProductVariationId(pv.product_variation_id))
                            {
                                if (ev.product_variation_property.product_variation_property_name == "Màu sắc" && ev.element_variation_name == color)
                                {
                                    is_need = true;
                                }
                            }
                        }
                        if (is_need)
                        {
                            list_return.Add(product);
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> filterOnClientSite(string seo_alias, string search_string, string tab_filter, string color, decimal? min_price, decimal? max_price)
        {
            try
            {
                var list_input = new List<product>();
                list_input = getByCategorySeoAliasOnSite(seo_alias);
                var list_return = new List<product>();
                if (search_string == null)
                {
                    search_string = "";
                }
                list_return = filterOnClientSiteByTab(tab_filter,
                                                        getByColor(color,
                                                            getByMoneyRange((decimal)min_price, (decimal)max_price, list_input)))
                                                                    .Where(p => (p.product_name + p.supplier.supplier_name).ToLower().Contains(search_string.ToLower())).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> filterOnClientSiteElysien(string seo_alias, string search_string, decimal? min_price, decimal? max_price)
        {
            try
            {
                var list_input = new List<product>();
                list_input = getByCategorySeoAliasOnSite(seo_alias);
                var list_return = new List<product>();
                if (search_string == null)
                {
                    search_string = "";
                }
                list_return = getByMoneyRange((decimal)min_price, (decimal)max_price, list_input).Where(p => (p.product_name + p.supplier.supplier_name).ToLower());
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> filterOnClientSiteMorcar(string seo_alias, decimal? min_price, decimal? max_price)
        {
            try
            {
                var list_input = new List<product>();
                list_input = getByCategorySeoAliasOnSite(seo_alias);
                var list_return = new List<product>();
                list_return = getByMoneyRange((decimal)min_price, (decimal)max_price, list_input).OrderByDescending(p => p.show_datetime).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> filterOnClientSite(string seo_alias)
        {
            try
            {
                return getByCategorySeoAliasOnSite(seo_alias);
            }
            catch
            {
                return null;
            }
        }

        public List<product> searchOnClientSite(string search_string)
        {
            try
            {
                if (search_string == null)
                {
                    search_string = "";
                }
                var list_return = new List<product>();
                var list_input = new List<product>();
                list_input = getAll().Where(p => p.show_datetime <= DateTime.Now && p.status == true).ToList();

                var list_product_by_name = new List<product>();
                list_product_by_name = list_input.Where(p => (p.product_name.ToLower().Contains(search_string.ToLower())) || (p.product_code.ToLower().Contains(search_string.ToLower()))).OrderByDescending(p => p.show_datetime).ToList();
                list_return = list_product_by_name;

                var list_tag = tag_provider.suggestTag(search_string);
                if (list_tag.Count > 0)
                {
                    foreach (var tag in list_tag)
                    {
                        foreach (var product_by_tag in getByTag(tag.tag_id))
                        {
                            if (!list_product_by_name.Contains(product_by_tag))
                            {
                                list_return.Add(product_by_tag);
                            }
                        }
                    }
                }

                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getByTag(long tag_id)
        {
            try
            {
                var list_tag_product = db.tag_products.Where(t => t.tag_id == tag_id).ToList();
                var list_return = new List<product>();
                if (list_tag_product.Count > 0)
                {
                    foreach (var tag_product in list_tag_product)
                    {
                        list_return.Add(getById(tag_product.product_id));
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> getThemeByCategoryThemeIdOnSite(long category_id)
        {
            try
            {
                var list_return = new List<product>();

                var list_category_child_1 = db.categories.Where(c => c.category_parent_id == category_id);
                if (list_category_child_1.Count() > 0)
                {
                    foreach (var category_child_1 in list_category_child_1)
                    {
                        var list_category_child_2 = db.categories.Where(c => c.category_parent_id == category_child_1.category_id);
                        if (list_category_child_2.Count() > 0)
                        {
                            foreach (var category_child_2 in list_category_child_1)
                            {
                                var list_category_product_2 = db.category_products.Where(cp => cp.category.category_id == category_child_2.category_id).ToList();
                                foreach (var obj in list_category_product_2)
                                {
                                    foreach (var product in getAll())
                                    {
                                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                                        {
                                            if (!list_return.Contains(product))
                                            {
                                                list_return.Add(product);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var list_category_product_1 = db.category_products.Where(cp => cp.category.category_id == category_child_1.category_id).ToList();
                        foreach (var obj in list_category_product_1)
                        {
                            foreach (var product in getAll())
                            {
                                if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                                {
                                    if (!list_return.Contains(product))
                                    {
                                        list_return.Add(product);
                                    }
                                }
                            }
                        }
                    }
                }

                var list_category_product = db.category_products.Where(cp => cp.category.category_id == category_id).ToList();
                foreach (var obj in list_category_product)
                {
                    foreach (var product in getAll())
                    {
                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                        {
                            if (!list_return.Contains(product))
                            {
                                list_return.Add(product);
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> filterOnClientSiteThemeList(long? category_theme_id, string search_string)
        {
            try
            {
                var list_input = getByCategorySeoAliasOnSite("kho-giao-dien");
                var list_return = list_input;
                if (category_theme_id > 0)
                {
                    list_return = getByCategoryIdOnSite((long)category_theme_id);
                }
                if (search_string == null)
                {
                    search_string = "";
                }
                list_return = list_return.Where(p => p.product_name.ToLower().Contains(search_string)).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> searchOnClientSiteMorcar(string search_string, decimal? min_price, decimal? max_price)
        {
            try
            {
                if (search_string == null)
                {
                    search_string = "";
                }
                var list_input = new List<product>();
                list_input = getAll().Where(p => p.show_datetime <= DateTime.Now && p.status == true).ToList();
                var list_return = new List<product>();
                list_return = getByMoneyRange((decimal)min_price, (decimal)max_price, list_input).Where(p => p.product_name.ToLower().Contains(search_string.ToLower())).OrderByDescending(p => p.show_datetime).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<product> OrderListProductOnClientSite(long category_id, string select_order)
        {
            try
            {
                var list_product_on_client_site_by_category = getByCategoryIdOnSite(category_id);
                var list_return = new List<product>();
                list_return = OrderBySelectOrder(select_order, list_product_on_client_site_by_category).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }


        public List<product> getBySearchString(string search_string)
        {
            try
            {
                var list = new List<product>();
                if (search_string == null)
                {
                    search_string = "";
                }
                if (search_string != "")
                {
                    list = db.products.Where(p => p.status == true && p.show_datetime <= DateTime.Now && p.product_name.Contains(search_string)).OrderBy(p => p.product_name).ToList();
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public bool updateProduct(product model, long product_id)
        {
            try
            {
                var old_model = getById(product_id);
                old_model.product_name = model.product_name;
                old_model.product_meta_title = model.product_meta_title;
                old_model.description = model.description;
                old_model.content = model.content;
                old_model.content_2 = model.content_2;
                old_model.content_3 = model.content_3;
                old_model.update_date = model.update_date;
                old_model.user_id = model.user_id;
                old_model.supplier_id = model.supplier_id;
                old_model.seo_alias = model.seo_alias;
                old_model.seo_description = model.seo_description;
                old_model.seo_header = model.seo_header;
                old_model.show_datetime = model.show_datetime;
                old_model.product_image = model.product_image;
                old_model.alt_image = model.alt_image;
                old_model.product_video = model.product_video;
                old_model.video_filename = model.video_filename;
                old_model.is_video_url = model.is_video_url;

                if (old_model.seo_header == null || old_model.seo_header == "")
                {
                    old_model.seo_header = old_model.product_name;
                }
                if (old_model.seo_description == null || old_model.seo_description == "")
                {
                    old_model.seo_description = old_model.product_name;
                }
                if (old_model.seo_alias == null || old_model.seo_alias == "")
                {
                    old_model.seo_alias = ToSeoFriendly(old_model.product_name, 255);
                }
                //if (checkDupplicateSeoAlias(old_model.seo_alias))
                //{
                //    old_model.seo_alias = ToSeoFriendly(old_model.product_name, 255) + "-1";
                //}
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateOverviewProduct(long product_id, string product_code, decimal rent_cost_promotion, decimal price_promotion)
        {
            try
            {
                var old_model = getById(product_id);
                old_model.product_code = product_code;
                old_model.rent_cost_promotion = rent_cost_promotion;
                old_model.promotion_price = price_promotion;
                if (old_model.promotion_price == 0)
                {
                    old_model.promotion_price = null;
                }
                if (old_model.rent_cost_promotion == 0)
                {
                    old_model.rent_cost_promotion = null;
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<product> getListRelatedProduct(long product_id, ref long category_id_get)
        {
            try
            {
                var product = getById(product_id);
                var list_return = new List<product>();
                var list_category = category_provider.getByProductId(product_id);
                if (list_category.Count() == 1)
                {
                    foreach (var item in list_category)
                    {
                        list_return = getByCategoryIdOnSite(item.category_id);
                        category_id_get = item.category_id;

                    }
                }
                else if (list_category.Count() == 0)
                {
                    list_return = null;
                    category_id_get = 0;
                }
                else
                {
                    var category = list_category.Where(c => c.category_parent_id != null).FirstOrDefault();
                    list_return = getByCategoryIdOnSite(category.category_id);
                    category_id_get = category.category_id;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<cart_product_variation> getCPVByProductVariationId(long pv_id)
        {
            try
            {
                return db.cart_product_variations.Where(cpv => cpv.product_variation_id == pv_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteCPVByProductVariationIdInCart(long pv_id)
        {
            try
            {
                db.cart_product_variations.DeleteAllOnSubmit(getCPVByProductVariationId(pv_id));
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteProduct(long product_id)
        {
            try
            {
                var list_pv = db.product_variations.Where(pv => pv.product_id == product_id).ToList();
                foreach (var pv in list_pv)
                {
                    deleteCPVByProductVariationIdInCart(pv.product_variation_id);
                    combo_product_provider.deleteComboByProductIdAndPvId(product_id, pv.product_variation_id);
                    ev_provider.deleteElementVariationByPVId(pv.product_variation_id);
                }

                pe_provider.DeletePEByProductId(product_id);

                db.product_variations.DeleteAllOnSubmit(list_pv);
                category_product_provider.deleteCategoryProduct(product_id);
                tag_product_provider.deleteTagProductByProductId(product_id);
                comment_product_provider.deleteCommentProductByProductId(product_id);
                var list_img_more = db.image_mores.Where(im => im.product_id == product_id).ToList();
                db.image_mores.DeleteAllOnSubmit(list_img_more);
                db.products.DeleteOnSubmit(getById(product_id));
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<product> getListRelatedByProductId(long product_id)
        {
            try
            {
                var list_return = new List<product>();
                var list_category = category_provider.getByProductId(product_id);
                if (list_category.Count() > 0)
                {
                    foreach (var c in list_category)
                    {
                        var list_product = getByCategoryIdOnSite(c.category_id);
                        if (list_product.Count() > 0)
                        {
                            foreach (var p in list_product)
                            {
                                if (!list_return.Contains(p) && p.product_id != product_id)
                                {
                                    list_return.Add(p);
                                }
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public bool updateManagementWarehouseStatus(long product_id, bool status)
        {
            try
            {
                var model = getById(product_id);
                if (model.warehouse_management_status != status)
                {
                    model.warehouse_management_status = status;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updatePriceForAll(long product_id, decimal new_price, decimal price_comparision)
        {
            try
            {
                var product = getById(product_id);
                var list_pv = db.product_variations.Where(pv => pv.product_id == product_id).ToList();

                if (new_price >= 0)
                {
                    product.promotion_price = new_price;
                    foreach (var pv in list_pv)
                    {
                        pv.product_variation_price = new_price;
                    }
                }

                if (price_comparision > 0)
                {
                    product.price = price_comparision;
                    foreach (var pv in list_pv)
                    {
                        pv.product_variation_price_comparison = price_comparision;
                    }
                }
                else
                {
                    product.price = null;
                    foreach (var pv in list_pv)
                    {
                        pv.product_variation_price_comparison = null;
                    }
                }

                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Bách Giai clinic
        public List<product> getByCategoryIdOnSiteBachGiai(long category_id)
        {
            try
            {
                var list_return = new List<product>();
                var list_category_product = db.category_products.Where(cp => cp.category.category_id == category_id).ToList();
                foreach (var obj in list_category_product)
                {
                    foreach (var product in getAll())
                    {
                        if (product.show_datetime <= DateTime.Now && product.status == true && product.product_id == obj.product_id)
                        {
                            if (!list_return.Contains(product))
                            {
                                list_return.Add(product);
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

    }
}
