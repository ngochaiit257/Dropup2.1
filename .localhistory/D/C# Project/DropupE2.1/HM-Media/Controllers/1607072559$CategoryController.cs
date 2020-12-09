﻿using Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HM_Media.Controllers
{
    public class CategoryController : Controller
    {
        CategoryProvider category_provider = new CategoryProvider();
        //GET: Product
        //[OutputCache(Duration = 2592000, VaryByParam = "none", SqlDependency = "SiteCache:product;SiteCache:category;SiteCache:category_product;" +
        //   "SiteCache:cart_of_user;SiteCache:cart_product_variation;SiteCache:blog;SiteCache:system_log;" +
        //   "SiteCache:store_logo;SiteCache:store_banner;SiteCache:store_slide;SiteCache:store_membership;SiteCache:store_customer_say;" +
        //   "SiteCache:store_partner;SiteCache:store_contact")]
        public ActionResult CategoryDetail(string seo_alias)
        {
            return View(category_provider.getBySeoAlias(seo_alias));
        }
    }
}