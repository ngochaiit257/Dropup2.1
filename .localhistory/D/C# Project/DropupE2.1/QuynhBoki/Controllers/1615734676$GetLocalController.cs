using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuynhBoki.Controllers
{
    public class GetLocalController : Controller
    {
        // GET: GetLocal
        public class GetLocalController : Controller
        {
            LocalProvider local_provider = new LocalProvider();

            public virtual ActionResult GetAllCountries()
            {
                return Json(local_provider.GetAllCountries(), JsonRequestBehavior.AllowGet);
            }

            public virtual ActionResult GetAllProvinceByCountryId(int? id = 237)
            {
                return Json(local_provider.GetAllProvinceByCountryId(id), JsonRequestBehavior.AllowGet);
            }

            public ActionResult GetAllDistrictByProvinceId(int id)
            {
                List<district> list_district = local_provider.GetAllDistrictByProvinceId(id);
                List<DistrictModel> list_return = new List<DistrictModel>();
                foreach (var district in list_district)
                {
                    DistrictModel district_model = new DistrictModel();
                    district_model.district_id = district.Id;
                    district_model.district_name = district.Name;
                    district_model.district_type = district.Type;
                    list_return.Add(district_model);
                }
                return Json(list_return, JsonRequestBehavior.AllowGet);
            }

            public ActionResult GetAllWardByDistrictId(int id)
            {
                List<ward> list_ward = local_provider.GetAllWardByDistrictId(id);
                List<WardModel> list_return = new List<WardModel>();
                foreach (var ward in list_ward)
                {
                    WardModel ward_model = new WardModel();
                    ward_model.ward_id = ward.Id;
                    ward_model.ward_name = ward.Name;
                    ward_model.ward_type = ward.Type;
                    list_return.Add(ward_model);
                }
                return Json(list_return, JsonRequestBehavior.AllowGet);
            }

            public ActionResult GetAllProvince()
            {
                var list_province = local_provider.GetAllProvince();
                List<ProvinceModel> list_return = new List<ProvinceModel>();
                foreach (var province in list_province)
                {
                    ProvinceModel model = new ProvinceModel();
                    model.province_id = province.Id;
                    model.province_name = province.Name;
                    list_return.Add(model);
                }
                return Json(list_return, JsonRequestBehavior.AllowGet);
            }
        }

    }
}