using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class UserProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        public List<user> getAll()
        {
            try
            {
                return db.users.OrderByDescending(p => p.role_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<user> getAdmin()
        {
            try
            {
                return db.users.Where(p => (p.role_id == 1 || p.role_id == 2)).OrderBy(p => p.role_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<role> getListRole()
        {
            try
            {
                return db.roles.OrderBy(r => r.role_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public user getByUserId(long user_id)
        {
            try
            {
                return db.users.FirstOrDefault(p => p.user_id == user_id);
            }
            catch
            {
                return null;
            }
        }

        public user GetByUserName(string username)
        {
            try
            {
                return db.users.FirstOrDefault(u => u.username == username);
            }
            catch
            {
                return null;
            }
        }

        public user GetByUserEmail(string email)
        {
            try
            {
                return db.users.FirstOrDefault(u => u.email == email);
            }
            catch
            {
                return null;
            }
        }

        public user GetByUsernamePassword(string username, string password)
        {
            try
            {
                return db.users.FirstOrDefault(u => u.username == username && u.password == password);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool insertUser(user model, ref long user_id, ref string username)
        {
            try
            {
                if (checkDuplicate(model.username))
                {
                    var model_exist = GetByUserName(model.username);
                    user_id = model_exist.user_id;
                    username = model_exist.username;
                }
                else
                {
                    db.users.InsertOnSubmit(model);
                    db.SubmitChanges();
                    user_id = model.user_id;
                    username = model.username;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkDuplicate(string username)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.username.ToLower() == username.ToLower())
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

        public bool checkDuplicateEmail(string email)
        {
            try
            {
                bool status = false;
                foreach (var obj in getAll())
                {
                    if (obj.email.ToLower() == email.ToLower())
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
        /// <param name="model"></param>
        /// <param name="old_password"></param>
        /// <returns></returns>
        public bool checkOldPassword(user model, string old_password)
        {
            try
            {
                bool status = true;
                var model_to_check = getByUserId(model.user_id);
                if (model_to_check.password != old_password || old_password == "")
                {
                    status = false;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool updateUser(user model)
        {
            try
            {
                var old_model = getByUserId(model.user_id);
                old_model.firstname = model.firstname;
                old_model.lastname = model.lastname;
                old_model.email = model.email;
                old_model.phone_number = model.phone_number;
                old_model.birthday = model.birthday;
                old_model.address_detail = model.address_detail;
                old_model.gender_id = model.gender_id;
                old_model.role_id = model.role_id;
                if (model.avatar != null || model.avatar != "")
                {
                    old_model.avatar = model.avatar;
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool updatePasswordUser(user model)
        {
            try
            {
                var model_to_update_password = getByUserId(model.user_id);
                model_to_update_password.password = model.password;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool checkConfirmPassword(string new_password, string confirm_password)
        {
            try
            {
                bool status = true;
                if (new_password != confirm_password)
                {
                    status = false;
                }
                return status;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteUser(user model)
        {
            try
            {
                var model_to_delete = getByUserId(model.user_id);
                db.users.DeleteOnSubmit(model_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool updateRoleUser(user model)
        {
            try
            {
                var model_to_update_role = getByUserId(model.user_id);
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
        public List<user> quickFilter(string search_string, string quickFilterValue)
        {
            try
            {
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quickFilterValue == "admin")
                {
                    return db.users.Where(s => ((s.firstname + s.lastname).Contains(search_string) || s.username.Contains(search_string)) && s.role_id < 3).OrderByDescending(s => s.join_date).ToList();
                }
                else if (quickFilterValue == "customer")
                {
                    return db.users.Where(s => ((s.firstname + s.lastname).Contains(search_string) || s.username.Contains(search_string)) && s.role_id == 3).OrderByDescending(s => s.join_date).ToList();

                }
                else
                {
                    return db.users.Where(s => (s.firstname + s.lastname).Contains(search_string) || s.username.Contains(search_string)).OrderByDescending(s => s.join_date).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
