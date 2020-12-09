using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class LocalProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();

        /// <summary>
        /// Hàm lấy danh sách Country
        /// Test OK
        /// </summary>
        /// <returns></returns>
        public List<country> GetAllCountries()
        {
            try
            {
                return db.countries.OrderBy(x => x.CommonName).ToList();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Hàm lấy danh sách tỉnh thành theo CountryId. 
        /// Id = 237 là của Việt Nam. Do database mình quy định vậy
        /// Test OK
        /// </summary>
        /// <param name="id">Id của country</param>
        /// <returns></returns>
        public List<province> GetAllProvinceByCountryId(int? id = 237)
        {
            return db.provinces.Where(x => x.CountryId == id).OrderBy(x => x.Id).ToList();
        }
        /// <summary>
        /// Hàm lấy tất cả danh sách quận huyện
        /// Id = 1 là Hà Nội, do database mình quy định vậy
        /// Test OK
        /// </summary>
        /// <param name="id">Id = provinceId</param>
        /// <returns></returns>
        public List<district> GetAllDistrictByProvinceId(int id)
        {
            return db.districts.Where(x => x.ProvinceId == id).OrderBy(x => x.Name).ToList();
        }
        /// <summary>
        /// Hàm lấy danh sách xã phường theo quận huyện
        /// Id= 1 là Ba Đình. Do database quy định
        /// Test OK
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ward> GetAllWardByDistrictId(int id)
        {
            return db.wards.Where(x => x.DistrictID == id).OrderBy(x => x.Name).ToList();
        }


        public List<province> GetAllProvince()
        {
            try
            {
                var list_province = db.provinces.OrderBy(x => x.Name).ToList();
                return list_province;
            }
            catch
            {
                return null;
            }
        }

        public string getNameOfProvinceByProvinceId(int province_id)
        {
            try
            {
                return db.provinces.FirstOrDefault(p => p.Id == province_id).Name;
            }
            catch
            {
                return "";
            }
        }

        public string getNameOfDistrictByDistrictId(int district_id)
        {
            try
            {
                return db.districts.FirstOrDefault(d => d.Id == district_id).Name;
            }
            catch
            {
                return "";
            }
        }
        public string getNameOfWardByWardId(int ward_id)
        {
            try
            {
                return db.wards.FirstOrDefault(d => d.Id == ward_id).Name;
            }
            catch
            {
                return "";
            }
        }

    }
}
