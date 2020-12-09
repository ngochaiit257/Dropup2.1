using Data;
using Data.Providers;
using Dino.Models;
using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dino.Controllers
{
    public class UserController : Controller
    {
        UserProvider user_provider = new UserProvider();
        CustomerProvider customer_provider = new CustomerProvider();
        CartProvider cart_provider = new CartProvider();
        SystemLogProvider system_log_provider = new SystemLogProvider();
        DeliveryAddressProvider da_provider = new DeliveryAddressProvider();


        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public user getUserLoged()
        {
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            return (new UserProvider().GetByUserName(username));
        }

        public ActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public long Login(string username, string password)
        {
            long user_id_loged = 0;
            if (ModelState.IsValid)
            {
                var userValid = user_provider.GetByUsernamePassword(username, EndCode.Encrypt(password));
                // User found in the database
                if (userValid != null)
                {
                    FormsAuthentication.SetAuthCookie(username, true);
                    var action = Url.Action("Index", "Home");
                    Response.RemoveOutputCacheItem(action);
                    user_id_loged = userValid.user_id;
                }
            }
            return user_id_loged;
        }

        [HttpPost]
        public long Register(user model)
        {
            long user_id = 0;
            if (user_provider.checkDuplicate(model.email) == false)
            {
                user user = new user();
                user.email = model.email;
                user.username = model.email;
                user.password = Models.EndCodeModel.Encrypt(model.password);
                user.role_id = 3;
                user.province_id = model.province_id;
                user.district_id = model.district_id;
                user.ward_id = model.ward_id;
                user.address_detail = model.address_detail;
                user.lastname = model.lastname;
                user.phone_number = model.phone_number;
                user.join_date = DateTime.Now;

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
                        user.avatar = "../Assets/images/gender/avatar.png";
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

                string username = "";
                if (user_provider.insertUser(user, ref user_id, ref username))
                {
                    customer customer = new customer();
                    customer.customer_last_name = user.lastname;
                    customer.customer_phone_number = user.phone_number;
                    customer.customer_email = user.email;
                    customer.province_id = user.province_id;
                    customer.district_id = user.district_id;
                    customer.ward_id = user.ward_id;
                    customer.address_detail = user.address_detail;
                    customer.user_id = user_id;
                    customer.customer_note = "Khách truy cập website";
                    customer.marketing_status = true;
                    customer.black_list_status = false;
                    customer.create_datetime = System.DateTime.Now;
                    long customer_id = 0;
                    if (customer_provider.insertCustomer(customer, ref customer_id))
                    {
                        delivery_address da_model_insert = new delivery_address();
                        da_model_insert.last_name = user.lastname;
                        da_model_insert.phone_number = user.phone_number;
                        da_model_insert.province_id = user.province_id;
                        da_model_insert.district_id = user.district_id;
                        da_model_insert.ward_id = user.ward_id;
                        da_model_insert.address_detail = user.address_detail;
                        da_model_insert.customer_id = customer_id;
                        da_model_insert.default_status = true;
                        int delivery_address_id_in_order = 0;
                        da_provider.insertDeliveryAddress(da_model_insert, ref delivery_address_id_in_order);
                    }
                    FormsAuthentication.SetAuthCookie(user.email, true);
                    system_log system_log = new system_log();
                    system_log.system_log_description = "đã tạo tài khoản " + username + " từ website";
                    system_log.user_id = user_id;
                    system_log.system_log_time = System.DateTime.Now;
                    system_log.system_log_object_id = user_id;
                    system_log.system_log_url = "/User/UserDetail";
                    system_log_provider.insertSystemLog(system_log);
                }
            }
            return user_id;
        }


        public string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "../" + physicalPath;
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            var action = Url.Action("Index", "Home");
            Response.RemoveOutputCacheItem(action);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult GoogleLogin(string email, string first_name, string last_name, string avatar_url)
        {
            long user_id_return = 0;
            if (!user_provider.checkDuplicate(email) && !user_provider.checkDuplicateEmail(email))
            {
                user model = new user();
                model.username = email;
                model.email = email;
                model.firstname = first_name;
                model.lastname = last_name;
                model.avatar = avatar_url;
                model.role_id = 3;
                model.join_date = DateTime.Now;
                long user_id = 0;
                string username = "";
                if (user_provider.insertUser(model, ref user_id, ref username))
                {
                    user_id_return = user_id;
                    customer customer = new customer();
                    customer.customer_first_name = first_name;
                    customer.customer_last_name = last_name;
                    customer.customer_email = email;
                    customer.user_id = user_id;
                    customer.create_datetime = DateTime.Now;
                    customer.black_list_status = false;
                    customer.customer_note = "Đăng nhập tài khoản Google";
                    customer.marketing_status = true;
                    long customer_id = 0;
                    if (customer_provider.insertCustomer(customer, ref customer_id))
                    {
                        FormsAuthentication.SetAuthCookie(username, true);
                    }
                }
            }
            else
            {
                var user_to_login = user_provider.GetByUserEmail(email);
                user_id_return = user_to_login.user_id;
                FormsAuthentication.SetAuthCookie(user_to_login.username, true);
            }
            return Json(new { user_id = user_id_return }, JsonRequestBehavior.AllowGet);
        }

        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "664509467832505",
                client_secret = "e94f07d7a44144ac7e79d6961f2c63c9",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            long user_id_loged = 0;
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "664509467832505",
                client_secret = "e94f07d7a44144ac7e79d6961f2c63c9",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;
            if (!user_provider.checkDuplicate(email) && !user_provider.checkDuplicateEmail(email))
            {
                user model = new user();
                model.username = email;
                model.email = email;
                model.firstname = me.first_name;
                model.lastname = me.last_name;
                model.avatar = me.picture.data.url;
                model.role_id = 3;
                model.join_date = DateTime.Now;
                long user_id = 0;
                string username = "";
                if (user_provider.insertUser(model, ref user_id, ref username))
                {
                    user_id_loged = user_id;
                    customer customer = new customer();
                    customer.customer_first_name = me.first_name;
                    customer.customer_last_name = me.last_name;
                    customer.customer_email = email;
                    customer.user_id = user_id;
                    customer.create_datetime = DateTime.Now;
                    customer.black_list_status = false;
                    customer.customer_note = "Đăng nhập tài khoản Facebook.";
                    customer.marketing_status = true;
                    long customer_id = 0;
                    if (customer_provider.insertCustomer(customer, ref customer_id))
                    {
                        FormsAuthentication.SetAuthCookie(username, true);
                    }
                }
            }
            else
            {
                var user_to_login = user_provider.GetByUserEmail(email);
                user_id_loged = user_to_login.user_id;
                FormsAuthentication.SetAuthCookie(user_to_login.username, true);
            }

            if (Request.Cookies["DINO_CART"] != null)
            {
                UpdateCartWhenLogin(user_id_loged, GetListFromCookie("DINO_CART"));
            }
            else
            {
                HttpCookie myCookie = new HttpCookie("DINO_CART");
                DateTime now = DateTime.Now;

                var list_get = cart_provider.getCartForUser(user_id_loged);
                var list_to_update_cookie = new List<ProductVariationInCartModel>();
                if (list_get != null)
                {
                    foreach (var item in list_get)
                    {
                        var model = new ProductVariationInCartModel();
                        model.id = item.product_variation_id;
                        model.quantity = item.quantity;
                        list_to_update_cookie.Add(model);
                    }
                }

                //convert từ List<> to =>  json string
                string jsonItem = JsonConvert.SerializeObject(list_to_update_cookie, Formatting.Indented);
                //myCookie.Value = Server.UrlEncode(jsonItem);//Encode dịch  mã  các ký tự đặc biệt, từ string  => tham khảo http://www.aspnut.com/reference/encoding.asp
                myCookie.Value = Uri.EscapeUriString(jsonItem);//Encode dịch  mã  các ký tự đặc biệt, từ string  => tham khảo http://www.aspnut.com/reference/encoding.asp
                myCookie.Expires = now.AddDays(365);
                HttpContext.Response.Cookies.Add(myCookie);// cookie mới đè lên cookie cũ
            }
            //string url_return =  System.Configuration.ConfigurationManager.AppSettings["currentUrl"];
            //return Json(new { user_id = TempData["UserIdLoged"] });
            return Redirect(TempData["CurrentUrl"].ToString());
        }


        public List<ProductVariationInCartModel> GetListFromCookie(string name)
        {
            List<ProductVariationInCartModel> list_return_from_cookie = new List<ProductVariationInCartModel>();
            if (Request.Cookies["DINO_CART"] != null)
            {
                string objCartListString = HttpUtility.UrlDecode(Request.Cookies["DINO_CART"].Value.ToString());
                list_return_from_cookie = JsonConvert.DeserializeObject<List<ProductVariationInCartModel>>(objCartListString);
            }
            return list_return_from_cookie;
        }

        public bool UpdateCartWhenLogin(long user_id, List<ProductVariationInCartModel> list_from_cookie)
        {
            bool status = false;
            var list_to_update = new List<cart_product_variation>();
            foreach (var item in list_from_cookie)
            {
                var model = new cart_product_variation();
                model.product_variation_id = item.id;
                model.quantity = item.quantity;
                list_to_update.Add(model);
            }
            cart_provider.updateCartForUser(user_id, list_to_update);
            return status;
        }
    }
}