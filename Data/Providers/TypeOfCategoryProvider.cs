using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class TypeOfCategoryProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<type_of_category> getAll()
        {
            try
            {
                return db.type_of_categories.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<type_of_category> getByShowStatusTrue()
        {
            try
            {
                return db.type_of_categories.Where(toc => toc.show_status == true).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
