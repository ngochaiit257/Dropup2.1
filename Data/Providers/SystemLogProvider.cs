using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class SystemLogProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<system_log> getAll()
        {
            try
            {
                return db.system_logs.OrderByDescending(s => s.system_log_time).ToList();
            }
            catch
            {
                return null;
            }
        }

        public system_log getBySystemLogId(long system_log_id)
        {
            try
            {
                return db.system_logs.FirstOrDefault(s => s.system_log_id == system_log_id);
            }
            catch
            {
                return null;
            }
        }

        public bool insertSystemLog(system_log model)
        {
            try
            {
                db.system_logs.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<system_log> getSystemLogByUserId(long user_id)
        {
            try
            {
                return db.system_logs.Where(s => s.user_id == user_id).OrderByDescending(s => s.system_log_time).ToList();
            }
            catch
            {
                return null;
            }
        }

    }
}
