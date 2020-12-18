using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class BlogCategoryProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        public List<blog_category> getAll()
        {
            try
            {
                return db.blog_categories.ToList();
            }
            catch
            {
                return null;
            }
        }

        public blog_category getBySeoAlias(string seo_alias)
        {
            try
            {
                return db.blog_categories.Where(bc => bc.blog_category_seo_alias == seo_alias);
            }
            catch
            {
                return null;
            }
        }
    }
}
