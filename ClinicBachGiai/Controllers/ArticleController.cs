using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using Data.Providers;

namespace ClinicBachGiai.Controllers
{
    public class ArticleController : Controller
    {
        ProductProvider product_provider = new ProductProvider();
        // GET: Article
        public ActionResult ArticleDetail(string seo_alias)
        {
            //var product = product_provider.getBySEOalias(seo_alias);
            //var list_related = new ProductProvider().getListRelatedByProductId(product.product_id);
            return View(product_provider.getBySEOalias(seo_alias));
        }
    }
}