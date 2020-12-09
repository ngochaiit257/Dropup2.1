using Common;
using Data;
using Data.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DropupManagement.Models;
using System.Web.Security;

namespace DropupManagement.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {

        LocalProvider local_provider = new LocalProvider();
        UserProvider user_provider = new UserProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(user model, string returnUrl, bool? isRememberMe)
        {
            returnUrl = returnUrl.Replace("%2f", "/");
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                var userValid = user_provider.GetByUsernamePassword(model.username, Models.EndCodeModel.Encrypt(model.password));

                // User found in the database
                if (userValid != null)
                {
                    if (userValid.role_id != 3)
                    {
                        if (isRememberMe == true)
                        {
                            FormsAuthentication.SetAuthCookie(model.username, false);
                            system_log system_log = new system_log();
                            system_log.system_log_description = "đã đăng nhập";
                            system_log.user_id = getUserLoged().user_id;
                            system_log.system_log_time = System.DateTime.Now;
                            system_log.system_log_url = "#";
                            system_log_provider.insertSystemLog(system_log);

                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                var action = Url.Action("Index", "Home");
                                Response.RemoveOutputCacheItem(action);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(model.username, true);
                            system_log system_log = new system_log();
                            system_log.system_log_description = "đã đăng nhập";
                            system_log.user_id = getUserLoged().user_id;
                            system_log.system_log_time = System.DateTime.Now;
                            system_log.system_log_url = "#";
                            system_log_provider.insertSystemLog(system_log);

                            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                var action = Url.Action("Index", "Home");
                                Response.RemoveOutputCacheItem(action);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    else
                    {
                        TempData["LoginMessages"] = "Bạn không có quyền truy cập vào hệ thống!";
                    }

                }
                else
                {
                    TempData["LoginMessages"] = "Sai tên đăng nhập hoặc mật khẩu!";
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            system_log system_log = new system_log();
            system_log.system_log_description = "đã đăng xuất";
            system_log.user_id = getUserLoged().user_id;
            system_log.system_log_time = System.DateTime.Now;
            system_log.system_log_url = "#";
            system_log_provider.insertSystemLog(system_log);

            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public ActionResult InsertUser()
        {
            ViewData["ListRole"] = user_provider.getListRole();
            ViewData["ListProvince"] = local_provider.GetAllProvince();
            return View();
        }

        [HttpPost]
        public bool checkDuplicate(string username)
        {

            return user_provider.checkDuplicate(username);
        }

        [HttpPost]
        public bool checkOldPassword(int user_id, string old_password)
        {
            return user_provider.checkOldPassword(user_provider.getByUserId(user_id), Models.EndCodeModel.Encrypt(old_password));
        }

        [HttpPost]
        public async Task<ActionResult> InsertUser(user model)
        {

            user user = new user();
            user.email = model.email;
            user.username = model.username;
            user.password = Models.EndCodeModel.Encrypt(model.username);
            user.role_id = model.role_id;

            if (model.firstname != null || model.lastname != null || model.gender_id != null || model.birthday != null || model.phone_number != null || model.province_id != null || model.district != null || model.ward != null)
            {
                user.firstname = model.firstname;
                user.lastname = model.lastname;
                user.gender_id = model.gender_id;
                user.birthday = model.birthday;
                user.phone_number = model.phone_number;
                user.province_id = model.province_id;
                user.district_id = model.district_id;
                user.ward_id = model.ward_id;
            }

            HttpPostedFileBase file = Request.Files["avatar"];
            if (file == null)
            {
                if (model.gender_id != null)
                {
                    if (model.gender_id == 1)
                    {
                        user.avatar = "../Assets/images/gender/male.png";
                    }
                    else if (model.gender_id == 2)
                    {
                        user.avatar = "../Assets/images/gender/female.png";
                    }
                }
                else
                {
                    user.avatar = "../Assets/images/gender/notGender.png";
                }
            }
            else
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(Url.Content("~/Assets/img/avatar")), fileName);
                //Đổi sang đường dẫn ảo
                string path_e = GetVirtualPath(filePath);
                file.SaveAs(filePath);
                user.avatar = path_e;
            }

            long user_id = 0;
            string username = "";

            if (user_provider.insertUser(user, ref user_id, ref username))
            {
                //await sendEmail(user);
                system_log system_log = new system_log();
                system_log.system_log_description = "đã cấp tài khoản " + username;
                system_log.user_id = getUserLoged().user_id;
                system_log.system_log_time = System.DateTime.Now;
                system_log.system_log_object_id = user_id;
                system_log.system_log_url = "/User/UserDetail";
                system_log_provider.insertSystemLog(system_log);
            }
            return Json("Insert Success");
        }

        public async Task<bool> sendEmail(user user)
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/template/ConfirmOrderEmail.html"));

            content = content.Replace("{{CustomerName}}", "<a style='color: red'" + ">Mr.Hải</a><br/>");
            content = content.Replace("{{Phone}}", "(094) 734 5396");
            content = content.Replace("{{Email}}", "ngochaiit257@gmail.com");
            content = content.Replace("{{Address}}", "Hà Nội");
            content = content.Replace("{{Total}}", "1.000.000 đ");
            var toEmail = "muoptv@gmail.com";
            //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            //new MailHelper().SendMail("ngochaiit257@gmail.com", "Đơn hàng mới từ FreshMart", content);
            await new MailHelper().SendMail(toEmail, "Đơn hàng mới từ TEST", content);
            return true;
        }

        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }

        public ActionResult Index()
        {
            ViewData["ListAdmin"] = user_provider.getAdmin();
            return View();
        }

        public ActionResult DeleteUser(int user_id)
        {
            var username_to_del = user_provider.getByUserId(user_id).username;
            user model_to_delete = user_provider.getByUserId(user_id);
            if (user_provider.deleteUser(model_to_delete))
            {
                system_log model = new system_log();

                model.system_log_description = "đã xóa tài khoản " + username_to_del;
                model.user_id = getUserLoged().user_id;
                model.system_log_time = System.DateTime.Now;
                model.system_log_url = "/User/Index";
                system_log_provider.insertSystemLog(model);
            }
            return RedirectToAction("Index", "User");
        }

        public ActionResult UpdateRole()
        {
            ViewData["ListUserManagement"] = user_provider.getAdmin();
            return View();
        }

    }
}