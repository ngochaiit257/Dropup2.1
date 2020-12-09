using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data;
using Data.Providers;

namespace DropupManagement.Controllers
{
    public class SupplierController : Controller
    {
        SupplierProvider supplier_provider = new SupplierProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult InsertSupplier()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertSupplier(supplier model)
        {

            supplier supplier = new supplier();
            supplier.supplier_name = model.supplier_name;
            supplier.supplier_email = model.supplier_email;
            supplier.supplier_address = model.supplier_address;
            supplier.supplier_phone_number = model.supplier_phone_number;
            supplier.date_of_contract = model.date_of_contract;

            HttpPostedFileBase file = Request.Files["supplier_image"];

            if (file == null)
            {
                supplier.supplier_image = "../Assets/images/gender/male.png";
            }
            else
            {
                //file.SaveAs(HttpContext.Server.MapPath("~/assets/img/thumbnail") + file.FileName);
                //supplier.supplier_img_thumb = file.FileName;
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/supplier_image")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                supplier.supplier_image = path_e;
            }

            if (supplier_provider.insertSupplier(supplier))
            {
                system_log system_log = new system_log();
                system_log.system_log_description = "đã thêm 1 nhà cung cấp";
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log_provider.insertSystemLog(system_log);
            }

            return Json("Insert Success.");
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        [HttpPost]
        public bool checkSupplierNameDuplicate(string supplier_name)
        {
            return supplier_provider.checkSupplierNameDuplicate(supplier_name);
        }

    }
}