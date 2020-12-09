using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ImageMoreProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        ProductProvider product_provider = new ProductProvider();

        public List<image_more> getByProductId(long product_id)
        {
            try
            {
                return db.image_mores.Where(im => im.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public image_more get1ByProductId(long product_id)
        {
            try
            {
                var img_more = new image_more();
                var product = product_provider.getById(product_id);
                img_more = getByProductId(product_id).Where(p => p.image_more_url != product.product_image).First();
                return img_more;
            }
            catch
            {
                return null;
            }
        }

        public bool insertImageMore(image_more model)
        {
            try
            {
                db.image_mores.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<image_more> getListMoreImageByProductId(long product_id)
        {
            try
            {
                return db.image_mores.Where(more_img => more_img.product_id == product_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool deleteImageMore(int img_id)
        {
            try
            {
                var img = db.image_mores.FirstOrDefault(im => im.image_more_id == img_id);
                db.image_mores.DeleteOnSubmit(img);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
