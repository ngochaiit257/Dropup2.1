using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class UnitProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<unit> getAll() {
            try
            {
                return db.units.OrderBy(u => u.unit_id).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
