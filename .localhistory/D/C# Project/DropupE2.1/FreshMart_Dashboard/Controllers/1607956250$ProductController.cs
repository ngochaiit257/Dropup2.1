using Data;
using Data.Providers;
//using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using DropupManagement.Models;
using ExtendMethod;
using System.Threading.Tasks;
using Common;

namespace DropupManagement.Controllers
{
    public class ProductController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        CategoryProductProvider category_product_provider = new CategoryProductProvider();
        CategoryProvider category_provider = new CategoryProvider();
        ProductVariationPropertyProvider pvp_provider = new ProductVariationPropertyProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        ImageMoreProvider image_more_provider = new ImageMoreProvider();
        TagProvider tag_provider = new TagProvider();
        TagProductProvider tag_product_provider = new TagProductProvider();
        ElementVariationProvider ev_provider = new ElementVariationProvider();
        ProductElementProvider pe_provider = new ProductElementProvider();
        ComboProductProvider combo_product_provider = new ComboProductProvider();
        string prefix_url = System.Configuration.ConfigurationManager.AppSettings["urlPresentationFreshMart"];

        // GET: Product
        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public static void CacheProductsList(List<product> list_product)
        {
            SqlCacheDependency sqlDependency = new SqlCacheDependency("FreshMart", "product");
            System.Web.HttpContext.Current.Cache.Insert("ProductsList", list_product, sqlDependency, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
        }

        private static List<product> GetCachedProductList()
        {
            return System.Web.HttpContext.Current.Cache["ProductsList"] as List<product>;
        }

        //[OutputCache(Duration = 3600, VaryByParam = "none", SqlDependency = "FreshMart:product")]
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

        public PartialViewResult GetProduct(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<product>();
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
            list_return = product_provider.quickFilter(search_string, quick_filter, quick_filter_value);
            ViewBag.NumberOfList = list_return.Count();
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.product_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.product_name).ToList();
            }

            return PartialView("_PartialViewProduct", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        //[ChildActionOnly]
        //[OutputCache(Duration = 3600, VaryByParam = "none", SqlDependency = "FreshMart:product")]
        public ActionResult IndexProductNotAction(int? page_num, string search_string, string quick_filter, string quick_filter_value)
        {
            if (quick_filter == null)
            {
                quick_filter = "all";
            }
            if (quick_filter_value == null)
            {
                quick_filter_value = "";
            }
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilter = quick_filter;
            ViewBag.FilterValue = search_string;
            ViewBag.page_num = page_num;

            ViewData["list_product"] = product_provider.getAll().Where(p => p.view_count > 0 || p.view_count == null || (p.view_count == 0 && p.allow_ordering_while_out_of_stock_status == true)).ToList();
            return View();
        }


        public PartialViewResult GetProductNotAction(int? page_num, string search_string, string quick_filter, string quick_filter_value)
        {
            if (quick_filter_value == null)
            {
                quick_filter_value = "";
            }

            var lst = new List<product>();
            if (!page_num.HasValue)
            {
                page_num = 1;
            }

            ViewBag.page_num = page_num;
            int Size_Of_Page = 5;
            int No_Of_Page = (page_num ?? 1);
            ViewBag.FilterValue = search_string;
            ViewBag.QuickFilterValue = quick_filter_value;
            ViewBag.QuickFilter = quick_filter;

            lst = product_provider.quickFilter(search_string, quick_filter, quick_filter_value).Where(p => p.view_count > 0 || p.view_count == null || (p.view_count == 0 && p.allow_ordering_while_out_of_stock_status == true)).ToList();
            ViewBag.NumberOfList = lst.Count();

            return PartialView("_PartialViewProductNotAction", lst.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult IndexProductListSelect(int? page_num, string search_string, string quick_filter, string quick_filter_value,
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

        public PartialViewResult GetProductListSelect(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<product>();
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
            list_return = product_provider.quickFilter(search_string, quick_filter, quick_filter_value);
            ViewBag.NumberOfList = list_return.Count();
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.product_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.product_name).ToList();
            }

            return PartialView("_PartialViewProductListSelect", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }
        public ActionResult IndexProductVariation(int? page_num, string search_string, string quick_filter, string quick_filter_value,
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

        public PartialViewResult GetProductVariation(int? page_num, string search_string, string quick_filter, string quick_filter_value,
                                            int size_of_page, string sort_list_return)
        {
            var list_return = new List<product>();
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
            list_return = product_provider.quickFilter(search_string, quick_filter, quick_filter_value);
            ViewBag.NumberOfList = list_return.Count();
            ViewBag.SizeOfPage = size_of_page;
            ViewBag.SortListReturn = sort_list_return;
            if (sort_list_return == "name_az")
            {
                list_return = list_return.OrderBy(p => p.product_name).ToList();
            }
            else if (sort_list_return == "name_za")
            {
                list_return = list_return.OrderByDescending(p => p.product_name).ToList();
            }

            return PartialView("_PartialViewProductVariation", list_return.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult InsertProduct()
        {
            ViewData["list_product_variation_property"] = pvp_provider.getAll();
            return View();
        }

        public virtual ActionResult GetProperty()
        {
            var list_return = new List<ProductVariationPropertyModel>();
            foreach (var item in pvp_provider.getAll())
            {
                ProductVariationPropertyModel model = new ProductVariationPropertyModel();
                model.product_variation_property_id = item.product_variation_property_id;
                model.product_variation_property_name = item.product_variation_property_name;
                list_return.Add(model);
            }
            return Json(new { list_property = list_return }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getWareHouse(string search_string, string filter_value)
        {
            if (filter_value == null)
            {
                filter_value = "";
            }
            //List<product> list_product_return = product_provider.filterBySelected(search_string, filter_value);
            List<product> list_product_return = product_provider.getAll();
            return View(list_product_return);
        }

        //public void ExportToExcel()
        //{

        //    CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");

        //    ExcelPackage pck = new ExcelPackage();
        //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách sản phẩm");

        //    ws.Cells["A1"].Value = "Danh sách sản phẩm";
        //    ws.Cells["B1"].Value = "Danh sách theo kết quả lọc";

        //    ws.Cells["A2"].Value = "Người tạo";
        //    ws.Cells["B2"].Value = getUserLoged().firstname + getUserLoged().lastname + " - " + getUserLoged().username;

        //    ws.Cells["A3"].Value = "Ngày tạo";
        //    ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} vào lúc {0:H: mm tt}", DateTimeOffset.Now);

        //    ws.Cells["A6"].Value = "Mã sản phẩm";
        //    ws.Cells["B6"].Value = "Tên sản phẩm";
        //    ws.Cells["C6"].Value = "Danh mục";
        //    ws.Cells["D6"].Value = "Nhà cung cấp";
        //    ws.Cells["E6"].Value = "Số lượng";
        //    ws.Cells["F6"].Value = "Giá hiện tại";
        //    ws.Cells["G6"].Value = "Giá khuyến mại";
        //    ws.Cells["H6"].Value = "Ngày cập nhật";
        //    ws.Cells[6, 1, 6, 8].Style.Font.Bold = true;
        //    int rowStart = 7;
        //    foreach (var item in product_provider.getAll())
        //    {
        //        //using (var range = ws.Cells[1, 1, 1, 2])
        //        //{
        //        //    range.Style.Font.Bold = true;
        //        //    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        //    range.Style.Fill.BackgroundColor.SetColor(Color.Transparent);
        //        //    range.Style.Font.Color.SetColor(Color.WhiteSmoke);
        //        //    range.Style.ShrinkToFit = false;
        //        //}
        //        //ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

        //        //ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("Transparent")));
        //        //ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(Color.Transparent);
        //        //if(rowStart % 2 == 0)
        //        //{
        //        //    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("green")));
        //        //}
        //        ws.Cells[string.Format("A{0}", rowStart)].Value = item.product_code;
        //        ws.Cells[string.Format("B{0}", rowStart)].Value = item.product_name;
        //        //ws.Cells[string.Format("C{0}", rowStart)].Value = item.category.category_name;
        //        ws.Cells[string.Format("D{0}", rowStart)].Value = item.supplier.supplier_name;
        //        ws.Cells[string.Format("E{0}", rowStart)].Value = String.Format(elGR, "{0:0,0}", item.view_count);
        //        ws.Cells[string.Format("F{0}", rowStart)].Value = String.Format(elGR, "{0:0,0} đ", item.price);
        //        ws.Cells[string.Format("G{0}", rowStart)].Value = String.Format(elGR, "{0:0,0} đ", item.promotion_price);
        //        ws.Cells[string.Format("H{0}", rowStart)].Value = string.Format("{0:dd MMMM yyyy}", item.update_date);
        //        rowStart++;
        //    }

        //    ws.Cells["A:AZ"].AutoFitColumns();
        //    Response.Clear();
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment: filename=" + "ProductListByFilter.xlsx");
        //    Response.BinaryWrite(pck.GetAsByteArray());
        //    Response.End();
        //}

        //private DataTable ReadFromExcelfile(string path, string sheetName)
        //{
        //    // Khởi tạo data table
        //    DataTable dt = new DataTable();
        //    // Load file excel và các setting ban đầu
        //    using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
        //    {
        //        if (package.Workbook.Worksheets.Count != 1)
        //        {
        //            //Log - Không có sheet nào tồn tại trong file excel của bạn 
        //            return null;
        //        }
        //        // Lấy Sheet đầu tiện trong file Excel để truy vấn 
        //        // Truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên 
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName) ?? package.Workbook.Worksheets.FirstOrDefault();
        //        // Đọc tất cả các header
        //        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //        {
        //            dt.Columns.Add(firstRowCell.Text);
        //        }
        //        // Đọc tất cả data bắt đầu từ row thứ 2
        //        for (var rowNumber = 2; rowNumber == workSheet.Dimension.End.Row; rowNumber++)
        //        {
        //            // Lấy 1 row trong excel để truy vấn
        //            var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //            // tạo 1 row trong data table
        //            var newRow = dt.NewRow();
        //            foreach (var cell in row)
        //            {
        //                newRow[cell.Start.Column - 1] = cell.Text;
        //            }
        //            dt.Rows.Add(newRow);
        //        }
        //    }
        //    return dt;
        //}

        [HttpPost]
        public bool checkDuplicate(string product_name)
        {

            return product_provider.checkDuplicate(product_name);
        }

        [HttpPost]
        public bool checkDuplicatePropertyName(string product_variation_property_name)
        {

            return pvp_provider.checkDuplicate(product_variation_property_name);
        }

        [HttpPost]
        public ActionResult InsertProductVariationProperty(string product_variation_property_name)
        {
            ProductVariationPropertyModel obj = new ProductVariationPropertyModel();
            product_variation_property model = new product_variation_property();
            model.product_variation_property_name = product_variation_property_name;

            int id_model = 0;
            if (pvp_provider.insertProductVariationProperty(model, ref id_model))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo thuộc tính " + model.product_variation_property_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_url = "#";
                system_log_provider.insertSystemLog(system_log);

                obj.product_variation_property_id = id_model;
                obj.product_variation_property_name = product_variation_property_name;
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InsertProduct(product model, List<int> list_category_id, List<string> tags, List<HttpPostedFileBase> files_more_image,
                                        List<ProductVariationModel> list_product_variation, List<long> list_combo_pv_id,
                                        string product_date_to_show, string product_time_to_show, bool is_showing,
                                        ElementPropertyModel ep_1, ElementPropertyModel ep_2, ElementPropertyModel ep_3,
                                        HttpPostedFileBase video)
        {
            DateTime currentDateTime = System.DateTime.Now;
            product product = new product();
            product.product_code = model.product_code;
            if (model.product_code == null)
            {
                product.product_code = currentDateTime.DayOfYear.ToString() + currentDateTime.Hour.ToString() + currentDateTime.Minute.ToString() + currentDateTime.Second.ToString();
            }
            else
            {
                product.product_code = model.product_code;
            }
            product.product_name = model.product_name;
            product.product_meta_title = model.seo_alias;
            product.price = model.price;
            product.description = model.description;
            product.promotion_price = model.promotion_price;
            product.rent_cost = model.rent_cost;
            product.rent_cost_promotion = model.rent_cost_promotion;
            product.create_date = currentDateTime;
            product.update_date = currentDateTime;
            product.user_id = getUserLoged().user_id;
            product.content = model.content;
            product.content_2 = model.content_2;
            product.content_3 = model.content_3;
            product.view_count = model.view_count;
            product.allow_ordering_while_out_of_stock_status = model.allow_ordering_while_out_of_stock_status;
            product.allow_delivery = model.allow_delivery;
            product.warehouse_management_status = model.warehouse_management_status;
            product.unit_id = model.unit_id;
            product.barcode = model.barcode;
            product.qrcode = model.qrcode;
            product.supplier_id = model.supplier_id;
            product.vat_status = model.vat_status;
            product.alt_image = model.alt_image;
            product.is_video_url = model.is_video_url;
            //SEO
            product.seo_alias = model.seo_alias;
            product.seo_description = model.seo_description;
            product.seo_header = model.seo_header;

            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (product_date_to_show != "" && product_time_to_show != "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else if (product_date_to_show != "" && product_time_to_show == "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else if (product_date_to_show == "" && product_time_to_show != "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else
                {
                    product.status = false;
                    product.show_datetime = null;
                }
            }
            else
            {
                product.status = true;
                product.show_datetime = System.DateTime.Now;
            }

            HttpPostedFileBase file = Request.Files["product_image"];
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                product.product_image = path_e;
            }

            HttpPostedFileBase get_video = Request.Files["video"];
            if (get_video != null)
            {
                var fileName = Path.GetFileName(get_video.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/video/product_video")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                get_video.SaveAs(filePath);
                product.product_video = path_e;
                product.video_filename = fileName;
            }
            else
            {
                product.product_video = model.product_video;
            }

            long productId = 0;
            long tagId = 0;
            if (product_provider.insertProduct(product, ref productId))
            {
                if (list_category_id != null)
                {
                    foreach (var category_id in list_category_id)
                    {
                        category_product obj = new category_product();
                        obj.category_id = category_id;
                        obj.product_id = productId;
                        category_product_provider.insertCategoryProduct(obj);
                    }
                }
                if (files_more_image != null)
                {
                    int i = 0;
                    foreach (HttpPostedFileBase image_more_of_product in files_more_image)
                    {
                        i++;
                        var fileName_moreImage = Path.GetFileName(image_more_of_product.FileName);
                        var filePath_moreImage = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), fileName_moreImage);
                        string path_of_more_img = GetVirtualPath(filePath_moreImage);
                        image_more_of_product.SaveAs(filePath_moreImage);
                        image_more image_more_model = new image_more();
                        image_more_model.image_more_url = path_of_more_img;
                        image_more_model.image_more_alt = model.alt_image + "-" + i;
                        image_more_model.product_id = productId;
                        image_more_provider.insertImageMore(image_more_model);
                    }
                }
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_product_provider.insertTagProduct(productId, tagId);
                        }
                    }
                }
                if (list_product_variation != null)
                {
                    foreach (var item in list_product_variation)
                    {
                        string path_of_product_variation_image = "";
                        if (item.product_variation_image != null)
                        {
                            var file_name_product_variation_image = Path.GetFileName(item.product_variation_image.FileName);
                            var file_path_product_variation_image = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), file_name_product_variation_image);
                            path_of_product_variation_image = GetVirtualPath(file_path_product_variation_image);
                            item.product_variation_image.SaveAs(file_path_product_variation_image);
                        }
                        product_variation pv_model = new product_variation();
                        pv_model.product_id = productId;
                        pv_model.product_variation_name = item.product_variation_name;
                        pv_model.product_variation_code = item.product_variation_code;
                        pv_model.product_variation_barcode = item.product_variation_barcode;
                        pv_model.product_variation_price = item.product_variation_price;
                        pv_model.product_variation_price_comparison = model.price;
                        pv_model.product_variation_rent_cost = model.rent_cost;
                        pv_model.product_variation_rent_cost_comparision = model.rent_cost_promotion;
                        pv_model.product_variation_image = path_of_product_variation_image;
                        pv_model.product_variation_image_alt = item.product_variation_image_alt;
                        pv_model.product_variation_weight = model.product_weight;
                        pv_model.allow_delivery = model.allow_delivery;
                        if (pv_model.allow_delivery == null)
                        {
                            pv_model.allow_delivery = true;
                        }
                        pv_model.allow_ordering_while_out_of_stock_status = model.allow_ordering_while_out_of_stock_status;
                        if (pv_model.allow_ordering_while_out_of_stock_status == null)
                        {
                            pv_model.allow_ordering_while_out_of_stock_status = true;
                        }
                        pv_model.product_variation_in_stock = item.product_variation_in_stock;
                        pv_provider.insertVariationForProduct(pv_model);
                    }
                    InsertElementProperty(productId, ep_1, ep_2, ep_3);
                }
                else
                {
                    long pv_id_ref = 0;
                    product_variation pv_model = new product_variation();
                    pv_model.product_id = productId;
                    pv_model.product_variation_name = "Default Title";
                    pv_model.product_variation_code = product.product_code;
                    pv_model.product_variation_barcode = product.barcode;
                    if (product.promotion_price != null)
                    {
                        pv_model.product_variation_price = (decimal)model.promotion_price;
                    }
                    if (product.price != null)
                    {
                        pv_model.product_variation_price_comparison = model.price;
                    }
                    if (product.rent_cost != null)
                    {
                        pv_model.product_variation_rent_cost_comparision = (decimal)model.rent_cost;
                    }
                    if (product.rent_cost_promotion != null)
                    {
                        pv_model.product_variation_rent_cost = model.rent_cost_promotion;
                    }
                    //pv_model.product_variation_property_id = 2002;
                    pv_model.product_variation_image = product.product_image;
                    pv_model.product_variation_image_alt = product.alt_image;
                    pv_model.product_variation_weight = model.product_weight;
                    pv_model.allow_delivery = model.allow_delivery;
                    pv_model.allow_ordering_while_out_of_stock_status = model.allow_ordering_while_out_of_stock_status;
                    if (pv_model.allow_delivery == null)
                    {
                        pv_model.allow_delivery = true;
                    }
                    if (pv_model.allow_ordering_while_out_of_stock_status == null)
                    {
                        pv_model.allow_ordering_while_out_of_stock_status = true;
                    }
                    if (pv_provider.insertVariationForProductRefId(pv_model, ref pv_id_ref))
                    {
                        element_variation obj = new element_variation();
                        obj.product_variation_id = pv_id_ref;
                        obj.product_variation_property_id = 2002;
                        obj.element_variation_name = "Default Title";
                        ev_provider.insertElementVariation(obj);
                    }
                }

                if (list_combo_pv_id != null)
                {
                    combo_product_provider.insertListComboProduct(list_combo_pv_id, productId);
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã tạo sản phẩm " + model.product_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = productId;
                system_log.system_log_url = "/Product/ProductDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { product_id = productId }, JsonRequestBehavior.AllowGet);
        }

        public bool InsertElementProperty(long product_id, ElementPropertyModel ep_1, ElementPropertyModel ep_2, ElementPropertyModel ep_3)
        {
            try
            {
                var list_pv = pv_provider.getByProductId(product_id);
                if (list_pv != null)
                {
                    foreach (var pv in list_pv)
                    {
                        if (pv.product_variation_name.Contains("/"))    //Có từ 2 loại biến thể trở lên
                        {
                            if (ep_1.list_tag_value != null && ep_2.list_tag_value != null && ep_3.list_tag_value == null)
                            {
                                foreach (var tag in ep_1.list_tag_value)
                                {
                                    int property_id_ref = 0;
                                    int property_id = 0;
                                    string str = tag + "/";
                                    var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                    foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                    {
                                        element_variation model = new element_variation();
                                        model.product_variation_id = get_id_pv.product_variation_id;
                                        model.element_variation_name = tag;
                                        model.product_variation_property_id = 0;
                                        if (ep_1.property_id != 0 && ep_1.property_name == null) //Thuộc tính tồn tại
                                        {
                                            model.product_variation_property_id = ep_1.property_id;
                                        }
                                        else //Thêm thuộc tính mới
                                        {
                                            product_variation_property obj = new product_variation_property();
                                            obj.product_variation_property_name = ep_1.property_name;
                                            if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                            {
                                                model.product_variation_property_id = property_id_ref;
                                            }
                                        }
                                        ev_provider.insertElementVariation(model);
                                        property_id = (int)model.product_variation_property_id;
                                    }

                                    product_element pe_model = new product_element();
                                    pe_model.product_id = product_id;
                                    pe_model.element_value = tag;
                                    pe_model.property_id = property_id;
                                    pe_provider.insertProductElement(pe_model);
                                }

                                foreach (var tag in ep_2.list_tag_value)
                                {
                                    int property_id_ref = 0;
                                    int property_id = 0;
                                    string str = "/" + tag;
                                    var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                    foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                    {
                                        element_variation model = new element_variation();
                                        model.product_variation_id = get_id_pv.product_variation_id;
                                        model.element_variation_name = tag;
                                        model.product_variation_property_id = 0;
                                        if (ep_2.property_id != 0 && ep_2.property_name == null) //Thuộc tính tồn tại
                                        {
                                            model.product_variation_property_id = ep_2.property_id;
                                        }
                                        else //Thêm thuộc tính mới
                                        {
                                            product_variation_property obj = new product_variation_property();
                                            obj.product_variation_property_name = ep_2.property_name;
                                            if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                            {
                                                model.product_variation_property_id = property_id_ref;
                                            }
                                        }
                                        ev_provider.insertElementVariation(model);
                                        property_id = (int)model.product_variation_property_id;
                                    }

                                    product_element pe_model = new product_element();
                                    pe_model.product_id = product_id;
                                    pe_model.element_value = tag;
                                    pe_model.property_id = property_id;
                                    pe_provider.insertProductElement(pe_model);
                                }
                            }
                            if (ep_1.list_tag_value != null && ep_2.list_tag_value != null && ep_3.list_tag_value != null)
                            {
                                foreach (var tag in ep_1.list_tag_value)
                                {
                                    int property_id_ref = 0;
                                    int property_id = 0;
                                    string str = tag + "/";
                                    var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                    foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                    {
                                        element_variation model = new element_variation();
                                        model.product_variation_id = get_id_pv.product_variation_id;
                                        model.element_variation_name = tag;
                                        model.product_variation_property_id = 0;
                                        if (ep_1.property_id != 0 && ep_1.property_name == null) //Thuộc tính tồn tại
                                        {
                                            model.product_variation_property_id = ep_1.property_id;
                                        }
                                        else //Thêm thuộc tính mới
                                        {
                                            product_variation_property obj = new product_variation_property();
                                            obj.product_variation_property_name = ep_1.property_name;
                                            if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                            {
                                                model.product_variation_property_id = property_id_ref;
                                            }
                                        }
                                        ev_provider.insertElementVariation(model);
                                        property_id = (int)model.product_variation_property_id;
                                    }

                                    product_element pe_model = new product_element();
                                    pe_model.product_id = product_id;
                                    pe_model.element_value = tag;
                                    pe_model.property_id = property_id;
                                    pe_provider.insertProductElement(pe_model);
                                }

                                foreach (var tag in ep_2.list_tag_value)
                                {
                                    int property_id_ref = 0;
                                    int property_id = 0;
                                    string str = "/" + tag;
                                    var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                    foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                    {
                                        element_variation model = new element_variation();
                                        model.product_variation_id = get_id_pv.product_variation_id;
                                        model.element_variation_name = tag;
                                        model.product_variation_property_id = 0;
                                        if (ep_2.property_id != 0 && ep_2.property_name == null) //Thuộc tính tồn tại
                                        {
                                            model.product_variation_property_id = ep_2.property_id;
                                        }
                                        else //Thêm thuộc tính mới
                                        {
                                            product_variation_property obj = new product_variation_property();
                                            obj.product_variation_property_name = ep_2.property_name;
                                            if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                            {
                                                model.product_variation_property_id = property_id_ref;
                                            }
                                        }
                                        ev_provider.insertElementVariation(model);
                                        property_id = (int)model.product_variation_property_id;
                                    }

                                    product_element pe_model = new product_element();
                                    pe_model.product_id = product_id;
                                    pe_model.element_value = tag;
                                    pe_model.property_id = property_id;
                                    pe_provider.insertProductElement(pe_model);
                                }

                                foreach (var tag in ep_3.list_tag_value)
                                {
                                    int property_id_ref = 0;
                                    int property_id = 0;
                                    string str = "/" + tag;
                                    var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                    foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                    {
                                        element_variation model = new element_variation();
                                        model.product_variation_id = get_id_pv.product_variation_id;
                                        model.element_variation_name = tag;
                                        model.product_variation_property_id = 0;
                                        if (ep_3.property_id != 0 && ep_3.property_name == null) //Thuộc tính tồn tại
                                        {
                                            model.product_variation_property_id = ep_3.property_id;
                                        }
                                        else //Thêm thuộc tính mới
                                        {
                                            product_variation_property obj = new product_variation_property();
                                            obj.product_variation_property_name = ep_3.property_name;
                                            if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                            {
                                                model.product_variation_property_id = property_id_ref;
                                            }
                                        }
                                        ev_provider.insertElementVariation(model);
                                        property_id = (int)model.product_variation_property_id;
                                    }

                                    product_element pe_model = new product_element();
                                    pe_model.product_id = product_id;
                                    pe_model.element_value = tag;
                                    pe_model.property_id = property_id;
                                    pe_provider.insertProductElement(pe_model);
                                }
                            }
                        }
                        else //Có 1 loại biến thể
                        {
                            foreach (var tag in ep_1.list_tag_value)
                            {
                                int property_id_ref = 0;
                                int property_id = 0;
                                string str = tag;
                                var get_list_pv_by_str_and_product_id = pv_provider.getIdByNameAndProductId(str, product_id);
                                foreach (var get_id_pv in get_list_pv_by_str_and_product_id)
                                {
                                    element_variation model = new element_variation();
                                    model.product_variation_id = get_id_pv.product_variation_id;
                                    model.element_variation_name = tag;
                                    model.product_variation_property_id = 0;
                                    if (ep_1.property_id != 0 && ep_1.property_name == null) //Thuộc tính tồn tại
                                    {
                                        model.product_variation_property_id = ep_1.property_id;
                                    }
                                    else //Thêm thuộc tính mới
                                    {
                                        product_variation_property obj = new product_variation_property();
                                        obj.product_variation_property_name = ep_1.property_name;
                                        if (pvp_provider.insertProductVariationProperty(obj, ref property_id_ref))
                                        {
                                            model.product_variation_property_id = property_id_ref;
                                        }
                                    }
                                    ev_provider.insertElementVariation(model);
                                    property_id = (int)model.product_variation_property_id;
                                }

                                product_element pe_model = new product_element();
                                pe_model.product_id = product_id;
                                pe_model.element_value = tag;
                                pe_model.property_id = property_id;
                                pe_provider.insertProductElement(pe_model);
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        [HttpPost]
        public ActionResult GetProductByProductId(long product_id)
        {
            var product = product_provider.getById(product_id);
            var data = new
            {
                product_name = product.product_name
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductDetail(long id)
        {
            ViewData["list_product_variation_property"] = pvp_provider.getByProductId(id);
            ViewData["list_image_more"] = image_more_provider.getByProductId(id);
            ViewData["list_product_variation"] = pv_provider.getByProductId(id);

            var list_combo_pv = combo_product_provider.getByProductId(id);
            List<long> list_combo_pv_id = new List<long>();
            if (list_combo_pv.Count() > 0)
            {
                foreach (var combo_pv in list_combo_pv)
                {
                    list_combo_pv_id.Add((long)combo_pv.product_variation_id);
                }
            }
            ViewBag.ListComboPvId = list_combo_pv_id;
            ViewBag.ProductId = id;
            ViewBag.PrefixUrl = prefix_url;
            return View(product_provider.getById(id));
        }

        public JsonResult GetListCategoryIdOfProduct(long product_id)
        {
            var list_category_of_product = category_provider.getByProductId(product_id);
            var list_category_id = new List<long>();
            foreach (var item in list_category_of_product)
            {
                list_category_id.Add(item.category_id);
            }
            return Json(new { list_category_id = list_category_id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTagProduct(long product_id)
        {
            var list = tag_provider.getByProductId(product_id);
            ViewBag.ProductId = product_id;
            return PartialView("_PartialViewTagProduct", list);
        }


        [HttpPost]
        public ActionResult DeleteTagProduct(long tag_id, long product_id)
        {
            if (tag_product_provider.deleteTagProductByProductIdAndTagId(tag_id, product_id))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa tag khỏi sản phẩm " + product_provider.getById(product_id).product_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = product_id;
                system_log.system_log_url = "/Product/ProductDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Remove tag Success");
        }

        public ActionResult GetImageCollectionProduct(long product_id)
        {
            var list = image_more_provider.getByProductId(product_id);
            ViewBag.ProductId = product_id;
            return PartialView("_PartialViewImageProduct", list);
        }

        [HttpPost]
        public ActionResult DeleteImageMore(long product_id, int id_to_delete)
        {
            if (image_more_provider.deleteImageMore(id_to_delete))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã xóa hình ảnh trong thư viện khỏi sản phẩm " + product_provider.getById(product_id).product_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = product_id;
                system_log.system_log_url = "/Product/ProductDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Delete Success");
        }

        public ActionResult GetListProductVariationToUpdate(long product_id)
        {
            ViewData["list_product_variation_property"] = pvp_provider.getByProductId(product_id);
            var list = pv_provider.getByProductId(product_id);
            ViewBag.ProductId = product_id;
            return PartialView("_PartialViewUpdateListProductVariation", list);
        }

        [HttpPost]
        public ActionResult GetListProductVariationCombo(List<long> list_product_variation_id_selected)
        {
            var list_combo_view = new List<product_variation>();
            if (list_product_variation_id_selected != null)
            {
                list_combo_view = pv_provider.getByListPvId(list_product_variation_id_selected);
            }
            return PartialView("_PartialViewGetCombo", list_combo_view);
        }

        public ActionResult GetListProductVariationComboToUpdate(List<long> list_product_variation_id_selected)
        {
            var list_combo_view = new List<product_variation>();
            if (list_product_variation_id_selected != null)
            {
                list_combo_view = pv_provider.getByListPvId(list_product_variation_id_selected);
            }
            return PartialView("_PartialViewGetCombo", list_combo_view);
        }

        [HttpPost]
        public ActionResult GetListProductVariationComboAndUpdateCombo(long product_id, List<long> list_product_variation_id_selected)
        {
            var list_combo_view = new List<product_variation>();
            if (combo_product_provider.updateComboProduct(product_id, list_product_variation_id_selected))
            {
                if (list_product_variation_id_selected != null)
                {
                    list_combo_view = pv_provider.getByListPvId(list_product_variation_id_selected);
                }
                var product = product_provider.getById(product_id);
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật sản phẩm " + product.product_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = product.product_id;
                system_log.system_log_url = "/Product/ProductDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return PartialView("_PartialViewGetCombo", list_combo_view);
        }

        public JsonResult GetListProductVariationIdComboByProductId(long product_id)
        {
            var list_combo_pv = combo_product_provider.getByProductId(product_id);
            List<long> list_combo_pv_id = new List<long>();
            if (list_combo_pv.Count() > 0)
            {
                foreach (var combo_pv in list_combo_pv)
                {
                    list_combo_pv_id.Add((long)combo_pv.product_variation_id);
                }
            }
            return Json(new { list_combo_pv_id = list_combo_pv_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteComboProduct(long product_id, long product_variation_id)
        {
            var list_combo_pv = new List<combo_product>();
            List<long> list_combo_pv_id = new List<long>();
            if (combo_product_provider.deleteComboByProductIdAndPvId(product_id, product_variation_id))
            {
                list_combo_pv = combo_product_provider.getByProductId(product_id);
                if (list_combo_pv.Count() > 0)
                {
                    foreach (var combo_pv in list_combo_pv)
                    {
                        list_combo_pv_id.Add((long)combo_pv.product_variation_id);
                    }
                    var product = product_provider.getById(product_id);
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã cập nhật sản phẩm " + product.product_name;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = product.product_id;
                    system_log.system_log_url = "/Product/ProductDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return Json(new { list_combo_pv_id = list_combo_pv_id }, JsonRequestBehavior.AllowGet);
        }



        public virtual ActionResult GetArrayProductVariationId(long product_id)
        {
            var list = pv_provider.getByProductId(product_id);
            List<long> list_long_id = new List<long>();
            List<ProductVariationModel> list_pv_model = new List<ProductVariationModel>();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    list_long_id.Add(item.product_variation_id);

                    ProductVariationModel pv_model = new ProductVariationModel();
                    pv_model.product_variation_id = item.product_variation_id;
                    pv_model.get_list_ev = ev_provider.getListElementVariationIdByProductVariationId(item.product_variation_id);
                    list_pv_model.Add(pv_model);
                }
            }
            return Json(new { get_list_pv_id = list_long_id, list_pv_model = list_pv_model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateProduct(product model, List<int> list_category_id, List<string> tags, List<HttpPostedFileBase> files_more_image,
                                        List<ProductVariationModel> list_product_variation,
                                        string product_date_to_show, string product_time_to_show, bool is_showing,
                                        bool status_image_change, bool status_video_change, bool is_not_include_video,
                                        List<ElementVariationModel> list_ev,
                                        HttpPostedFileBase video)
        {
            DateTime currentDateTime = System.DateTime.Now;
            var old_model = product_provider.getById(model.product_id);
            product product = new product();
            product.product_name = model.product_name;
            product.product_meta_title = model.seo_alias;
            product.description = model.description;
            product.update_date = currentDateTime;
            product.user_id = getUserLoged().user_id;
            product.content = model.content;
            product.content_2 = model.content_2;
            product.content_3 = model.content_3;
            product.supplier_id = model.supplier_id;
            product.is_video_url = model.is_video_url;

            //SEO
            product.seo_alias = model.seo_alias;
            product.seo_description = model.seo_description;
            product.seo_header = model.seo_header;

            DateTime show_datetime = new DateTime();
            if (is_showing == false)
            {
                if (product_date_to_show != "" && product_time_to_show != "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else if (product_date_to_show != "" && product_time_to_show == "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else if (product_date_to_show == "" && product_time_to_show != "")
                {
                    product.status = true;
                    string str = product_date_to_show.Replace('/', '-') + " " + product_time_to_show;
                    show_datetime = DateTime.ParseExact(str, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
                    product.show_datetime = show_datetime;
                }
                else
                {
                    product.status = false;
                    product.show_datetime = null;
                }
            }
            else
            {
                product.status = true;
                product.show_datetime = System.DateTime.Now;
            }


            if (status_image_change == true)
            {
                HttpPostedFileBase file = Request.Files["product_image"];
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), fileName);
                    //Đổi sang đường dẫn ảo
                    string path_e = GetVirtualPath(filePath);
                    file.SaveAs(filePath);
                    product.product_image = path_e;
                }
            }
            else
            {
                product.product_image = old_model.product_image;
                product.alt_image = old_model.alt_image;
            }

            if (is_not_include_video == true)
            {
                product.is_video_url = true;
                product.video_filename = null;
                product.product_video = model.product_video;
            }
            else
            {
                if (status_video_change == true)
                {
                    HttpPostedFileBase get_video = Request.Files["video"];
                    if (get_video != null)
                    {
                        var fileName = Path.GetFileName(get_video.FileName);
                        var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/video/product_video")), fileName);
                        //Đổi sang đường dẫn ảo
                        string path_e = GetVirtualPath(filePath);
                        get_video.SaveAs(filePath);
                        product.product_video = path_e;
                        product.video_filename = fileName;
                        product.is_video_url = false;
                    }
                    else
                    {
                        product.product_video = model.product_video;
                        product.video_filename = null;
                        product.is_video_url = true;
                    }
                }
                else
                {
                    product.product_video = model.product_video;
                    product.video_filename = old_model.video_filename;
                    product.is_video_url = old_model.is_video_url;
                }
            }

            long tagId = 0;
            if (product_provider.updateProduct(product, model.product_id))
            {
                category_product_provider.deleteCategoryProduct(model.product_id);
                if (list_category_id != null)
                {
                    foreach (var category_id in list_category_id)
                    {
                        category_product obj = new category_product();
                        obj.category_id = category_id;
                        obj.product_id = model.product_id;
                        category_product_provider.insertCategoryProduct(obj);
                    }
                }
                if (files_more_image != null)
                {
                    var list_img_more = image_more_provider.getListMoreImageByProductId(model.product_id);
                    int i = list_img_more.Count();
                    foreach (HttpPostedFileBase image_more_of_product in files_more_image)
                    {
                        i++;
                        var fileName_moreImage = Path.GetFileName(image_more_of_product.FileName);
                        var filePath_moreImage = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), fileName_moreImage);
                        string path_of_more_img = GetVirtualPath(filePath_moreImage);
                        image_more_of_product.SaveAs(filePath_moreImage);
                        image_more image_more_model = new image_more();
                        image_more_model.image_more_url = path_of_more_img;
                        image_more_model.image_more_alt = model.alt_image + "-" + i;
                        image_more_model.product_id = model.product_id;
                        image_more_provider.insertImageMore(image_more_model);
                    }
                }
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        if (tag_provider.insertTag(tag, ref tagId))
                        {
                            tag_product_provider.insertTagProduct(model.product_id, tagId);
                        }
                    }
                }
                if (list_product_variation != null)
                {
                    foreach (var item in list_product_variation)
                    {
                        string path_of_product_variation_image = "";
                        if (item.is_change_image == true)
                        {
                            if (item.product_variation_image != null)
                            {
                                var file_name_product_variation_image = Path.GetFileName(item.product_variation_image.FileName);
                                var file_path_product_variation_image = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/product_image")), file_name_product_variation_image);
                                path_of_product_variation_image = GetVirtualPath(file_path_product_variation_image);
                                item.product_variation_image.SaveAs(file_path_product_variation_image);
                            }
                            else
                            {
                                path_of_product_variation_image = null;
                            }
                        }
                        else
                        {
                            var old_pv = pv_provider.getById(item.product_variation_id);
                            path_of_product_variation_image = old_pv.product_variation_image;
                        }

                        pv_provider.updatedOverviewProductVariation(item.product_variation_id, path_of_product_variation_image, item.product_variation_code, item.product_variation_price, item.product_variation_rent_cost, item.product_variation_in_stock);
                    }

                }
                if (list_ev != null || list_ev.Count() > 0)
                {
                    //Update Element Variation
                    foreach (var ev in list_ev)
                    {
                        ev_provider.updateElementVariationName(ev.element_variation_id, ev.element_variation_name);
                    }
                }

                system_log system_log = new system_log();
                system_log.system_log_description = "đã cập nhật sản phẩm " + model.product_name;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = model.product_id;
                system_log.system_log_url = "/Product/ProductDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json(new { product_id = model.product_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteProduct(List<long> list_product_id, int? page_num, string search_string, string quick_filter_value, string size_of_page, string sort_list_return)
        {
            foreach (var product_id in list_product_id)
            {
                var product_name_to_delete = product_provider.getById(product_id).product_name;
                if (product_provider.deleteProduct(product_id))
                {
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã xóa sản phẩm " + product_name_to_delete;
                    system_log.user_id = getUserLoged().user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_url = "/Product/Index";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return RedirectToAction("Index", new { search_string = search_string, page_num = page_num, quick_filter_value = quick_filter_value, size_of_page = size_of_page, sort_list_return = sort_list_return });
        }

        //PRODUCT VARIATION
        public ActionResult GetFormAddProductVarition(long product_id)
        {
            var list_property = pvp_provider.getByProductId(product_id);
            ViewBag.ProductId = product_id;
            return PartialView("_ChildViewAddProductVariation", list_property);
        }

        [HttpPost]
        public JsonResult GetListPropertyJson(long product_id)
        {
            var list_property_data = pvp_provider.getByProductId(product_id);
            var list_return = new List<ProductVariationPropertyModel>();
            foreach (var property in list_property_data)
            {
                ProductVariationPropertyModel model = new ProductVariationPropertyModel();
                model.product_variation_property_id = property.product_variation_property_id;
                model.product_variation_property_name = property.product_variation_property_name;
                list_return.Add(model);
            }
            return Json(new { list_property = list_return }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewProductVariation(long product_id, ProductVariationModel product_variation, List<ElementVariationModel> list_ev)
        {
            bool status = false;
            var product = product_provider.getById(product_id);
            if (list_ev != null)
            {
                long product_varation_id_ref = 0;
                product_variation pv_model = new product_variation();
                pv_model.product_id = product_id;
                pv_model.product_variation_name = product_variation.product_variation_name;
                pv_model.product_variation_code = product_variation.product_variation_code;
                pv_model.product_variation_barcode = product_variation.product_variation_barcode;
                pv_model.product_variation_weight = product_variation.product_variation_weight;
                pv_model.product_variation_price = product_variation.product_variation_price;
                pv_model.product_variation_price_comparison = product_variation.product_variation_price_comparison;
                pv_model.product_variation_rent_cost = product_variation.product_variation_rent_cost;
                pv_model.product_variation_rent_cost_comparision = product_variation.product_variation_rent_cost;
                pv_model.product_variation_image_alt = product_variation.product_variation_image_alt;
                pv_model.allow_delivery = product_variation.allow_delivery;
                if (pv_model.allow_delivery == null)
                {
                    pv_model.allow_delivery = true;
                }
                pv_model.allow_ordering_while_out_of_stock_status = product_variation.allow_ordering_while_out_of_stock_status;
                if (pv_model.allow_ordering_while_out_of_stock_status == null)
                {
                    pv_model.allow_ordering_while_out_of_stock_status = true;
                }
                pv_model.product_variation_in_stock = product_variation.product_variation_in_stock;
                if (pv_provider.insertVariationForProductRefId(pv_model, ref product_varation_id_ref))
                {
                    product_provider.updateManagementWarehouseStatus(product_id, product_variation.warehouse_management_status);
                    foreach (var ev in list_ev)
                    {
                        element_variation ev_model = new element_variation();
                        ev_model.product_variation_id = product_varation_id_ref;
                        ev_model.product_variation_property_id = ev.product_variation_property_id;
                        ev_model.element_variation_name = ev.element_variation_name;
                        if (ev_provider.insertElementVariation(ev_model))
                        {
                            product_element pe_model = new product_element();
                            pe_model.product_id = product_id;
                            pe_model.element_value = ev.element_variation_name;
                            pe_model.property_id = ev.product_variation_property_id;
                            if (pe_provider.insertProductElement(pe_model))
                            {
                                status = true;
                            }
                        }
                    }
                }

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePrice(long product_id, decimal new_price)
        {

        }
    }
}