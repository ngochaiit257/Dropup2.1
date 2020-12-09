using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class ShippingServiceProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        LocalProvider local_provider = new LocalProvider();

        public bool insertLocalShippingService(local_shipping_service model, ref int local_shipping_service_id)
        {
            try
            {
                db.local_shipping_services.InsertOnSubmit(model);
                db.SubmitChanges();
                local_shipping_service_id = model.local_shipping_service_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertCostOfLocalShipping(cost_of_local_shipping model, ref int cost_of_local_shipping_id)
        {
            try
            {
                db.cost_of_local_shippings.InsertOnSubmit(model);
                db.SubmitChanges();
                cost_of_local_shipping_id = model.cost_of_local_shipping_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertCostForDistrictShipping(int cost_of_local_shipping_id, decimal price_of_cost, int province_id)
        {
            try
            {
                var list_district = local_provider.GetAllDistrictByProvinceId(province_id);
                foreach (var district in list_district)
                {
                    cost_for_district_shipping model = new cost_for_district_shipping();
                    model.district_id = district.Id;
                    model.cost_of_local_shipping_id = cost_of_local_shipping_id;
                    model.price_for_district_shipping_final = price_of_cost;
                    model.price_for_district_shipping_adjust = 0;
                    model.status_district_shipping = true;
                    db.cost_for_district_shippings.InsertOnSubmit(model);
                }

                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string getNameStandardForShippingByStandardId(int standard_id)
        {
            try
            {
                return db.standard_for_shippings.FirstOrDefault(s => s.standard_for_shipping_id == standard_id).standard_name;
            }
            catch
            {
                return "";
            }
        }

        public List<local_shipping_service> getAllLocalShippingService()
        {
            try
            {
                return db.local_shipping_services.OrderBy(l => l.province.Name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<cost_of_local_shipping> getAllCostOfLocalShipping()
        {
            try
            {
                return db.cost_of_local_shippings.OrderBy(l => l.cost_of_local_shipping_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<cost_of_local_shipping> getAllCostOfLocalShippingByLocalShippingServiceId(int local_shipping_service_id)
        {
            try
            {
                var list_return = db.cost_of_local_shippings.Where(l => l.local_shipping_service_id == local_shipping_service_id).OrderBy(l => l.cost_of_local_shipping_id).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<cost_for_district_shipping> getAllCostForDistrictShipping()
        {
            try
            {
                return db.cost_for_district_shippings.OrderBy(l => l.district.Name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool updateCostForDistrictShipping(cost_for_district_shipping model)
        {
            try
            {
                var old_model = db.cost_for_district_shippings.FirstOrDefault(d => d.cost_for_district_shipping_id == model.cost_for_district_shipping_id);

                old_model.price_for_district_shipping_adjust = model.price_for_district_shipping_adjust;
                old_model.price_for_district_shipping_final = model.price_for_district_shipping_final;
                old_model.status_district_shipping = model.status_district_shipping;

                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<cost_for_district_shipping> getAllCostForDistrictShippingByCostOfLocalShippingID(int cost_of_local_shipping_id)
        {
            try
            {
                return db.cost_for_district_shippings.Where(d => d.cost_of_local_shipping_id == cost_of_local_shipping_id).OrderBy(d => d.district.Name).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<standard_for_shipping> getAllStandardForShipping()
        {
            try
            {
                return db.standard_for_shippings.OrderBy(s => s.standard_for_shipping_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool checkDuplicateProvinceShipping(int province_id)
        {
            try
            {
                bool status = false;
                foreach (var local_shipping_service in getAllLocalShippingService())
                {
                    if (local_shipping_service.province_id == province_id)
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

        public cost_of_local_shipping getCostOfLocalShippingById(int cost_of_loacal_shipping_id)
        {
            try
            {
                return db.cost_of_local_shippings.FirstOrDefault(c => c.cost_of_local_shipping_id == cost_of_loacal_shipping_id);
            }
            catch
            {
                return null;
            }
        }

        public string getProvinceNameByCostOfLocalShippingID(int cost_of_loacal_shipping_id)
        {
            try
            {
                return db.cost_of_local_shippings.FirstOrDefault(c => c.cost_of_local_shipping_id == cost_of_loacal_shipping_id).local_shipping_service.province.Name;
            }
            catch
            {
                return "";
            }
        }

        public string getProvinceNameByLocalShippingServiceID(int local_shipping_service_id)
        {
            try
            {
                return db.local_shipping_services.FirstOrDefault(c => c.local_shipping_service_id == local_shipping_service_id).province.Name;
            }
            catch
            {
                return "";
            }
        }

        public int getProvinceIdByCostOfLocalShippingID(int cost_of_loacal_shipping_id)
        {
            try
            {
                return db.cost_of_local_shippings.FirstOrDefault(c => c.cost_of_local_shipping_id == cost_of_loacal_shipping_id).local_shipping_service.province.Id;
            }
            catch
            {
                return -1;
            }
        }

        public local_shipping_service getLocalShippingServiceById(int local_shipping_service_id)
        {
            try
            {
                return db.local_shipping_services.FirstOrDefault(l => l.local_shipping_service_id == local_shipping_service_id);
            }
            catch
            {
                return null;
            }
        }

        public bool deleteLocalShippingService(int local_shipping_service_id, ref string province_name)
        {
            try
            {
                var local_shipping_service_to_delete = getLocalShippingServiceById(local_shipping_service_id);
                province_name = local_shipping_service_to_delete.province.Name;
                var list_cost_of_local_shipping = getAllCostOfLocalShippingByLocalShippingServiceId(local_shipping_service_id);
                List<cost_for_district_shipping> list_cost_for_district_to_remove = new List<cost_for_district_shipping>();
                foreach (var obj in list_cost_of_local_shipping)
                {
                    var list_cost_for_district_by_cost_of_local_shipping = getAllCostForDistrictShippingByCostOfLocalShippingID(obj.cost_of_local_shipping_id);
                    foreach (var item in list_cost_for_district_by_cost_of_local_shipping)
                    {
                        list_cost_for_district_to_remove.Add(item);
                    }
                }
                db.cost_for_district_shippings.DeleteAllOnSubmit(list_cost_for_district_to_remove);
                db.cost_of_local_shippings.DeleteAllOnSubmit(list_cost_of_local_shipping);
                db.local_shipping_services.DeleteOnSubmit(local_shipping_service_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string getNameStandardById(int standard_id)
        {
            try
            {
                return db.standard_for_shippings.FirstOrDefault(s => s.standard_for_shipping_id == standard_id).standard_name;
            }
            catch
            {
                return "";
            }
        }

        public bool deleteCostOfLocalShipping(int cost_of_local_shipping_id, ref string name_of_cost, ref string province_name, ref int count_cost_of_local_shipping_remaining, ref int local_shipping_service_id)
        {
            try
            {
                var model_to_delete = getCostOfLocalShippingById(cost_of_local_shipping_id);
                name_of_cost = model_to_delete.name_of_cost;
                province_name = model_to_delete.local_shipping_service.province.Name;
                local_shipping_service_id = (Int32)model_to_delete.local_shipping_service_id;
                var list_cost_for_district_by_cost_of_local_shipping = getAllCostForDistrictShippingByCostOfLocalShippingID(cost_of_local_shipping_id);
                var list_cost_of_local_shipping = getAllCostOfLocalShippingByLocalShippingServiceId(local_shipping_service_id);
                count_cost_of_local_shipping_remaining = list_cost_of_local_shipping.Count();

                db.cost_for_district_shippings.DeleteAllOnSubmit(list_cost_for_district_by_cost_of_local_shipping);
                db.cost_of_local_shippings.DeleteOnSubmit(model_to_delete);

                count_cost_of_local_shipping_remaining -= 1;
                if (count_cost_of_local_shipping_remaining == 0)
                {
                    var model_local_shipping_service_to_delete = getLocalShippingServiceById(local_shipping_service_id);
                    db.local_shipping_services.DeleteOnSubmit(model_local_shipping_service_to_delete);
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateTitleAndStandard(int cost_of_local_shipping_id, string name_of_cost, int standard_for_shipping_id,
            decimal min_value_for_standard, decimal max_value_for_standard)
        {
            try
            {
                var model = getCostOfLocalShippingById(cost_of_local_shipping_id);
                model.name_of_cost = name_of_cost;
                model.standard_for_shipping_id = standard_for_shipping_id;
                model.min_value_for_standard = min_value_for_standard;
                model.max_value_for_standard = max_value_for_standard;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateCostForLocalShippingAndCostForDistrictShipping(int cost_of_local_shipping_id, decimal price_of_cost,
            List<cost_for_district_shipping> list_cost_for_district_shipping)
        {
            try
            {
                var cost_of_local_to_update = getCostOfLocalShippingById(cost_of_local_shipping_id);
                cost_of_local_to_update.price_of_cost = price_of_cost;
                var list_cost_for_district_old = new List<cost_for_district_shipping>();
                if (list_cost_for_district_shipping != null)
                {
                    foreach (var new_obj in list_cost_for_district_shipping)
                    {
                        var old_cost_for_district = db.cost_for_district_shippings.FirstOrDefault(d => d.cost_for_district_shipping_id == new_obj.cost_for_district_shipping_id);
                        list_cost_for_district_old.Add(old_cost_for_district);
                    }
                }
                foreach (var new_obj in list_cost_for_district_shipping)
                {
                    foreach (var old_obj in list_cost_for_district_old)
                    {
                        if (new_obj.cost_for_district_shipping_id == old_obj.cost_for_district_shipping_id)
                        {
                            old_obj.status_district_shipping = new_obj.status_district_shipping;
                            old_obj.price_for_district_shipping_adjust = new_obj.price_for_district_shipping_adjust;
                            old_obj.price_for_district_shipping_final = new_obj.price_for_district_shipping_final;
                        }
                    }
                }
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<cost_of_local_shipping> getCostOfLocalShippingForOrder(int province_id, decimal value_for_standard)
        {
            try
            {
                var list_return = new List<cost_of_local_shipping>();
                local_shipping_service lss = db.local_shipping_services.FirstOrDefault(l => l.province_id == province_id);
                if(lss != null)
                {
                    var list_cost_of_local_shipping = getAllCostOfLocalShippingByLocalShippingServiceId(lss.local_shipping_service_id);
                    if (list_cost_of_local_shipping != null)
                    {
                        foreach (var obj in list_cost_of_local_shipping)
                        {
                            if (value_for_standard >= obj.min_value_for_standard && value_for_standard <= obj.max_value_for_standard)
                            {
                                list_return.Add(obj);
                            }
                            else if (obj.min_value_for_standard == 0 && obj.standard_for_shipping_id == 2)
                            {
                                list_return.Add(obj);
                            }
                            else if (obj.min_value_for_standard == 0 && obj.standard_for_shipping_id == 1)
                            {
                                list_return.Add(obj);
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<cost_for_district_shipping> getCostForDistrictShippingForOrder(int district_id, decimal value_for_standard)
        {
            try
            {
                var list_return = new List<cost_for_district_shipping>();
                var list_cost_for_district = db.cost_for_district_shippings.Where(d => d.district_id == district_id).ToList();
                if(list_cost_for_district != null)
                {
                    foreach (var obj in list_cost_for_district)
                    {
                        if (obj.status_district_shipping == true)
                        {
                            if (obj.cost_of_local_shipping.min_value_for_standard <= value_for_standard && obj.cost_of_local_shipping.max_value_for_standard >= value_for_standard)
                            {
                                list_return.Add(obj);
                            }
                            else if (obj.cost_of_local_shipping.min_value_for_standard == 0 && obj.cost_of_local_shipping.standard_for_shipping_id == 1)
                            {
                                list_return.Add(obj);
                            }
                            else if (obj.cost_of_local_shipping.min_value_for_standard == 0 && obj.cost_of_local_shipping.standard_for_shipping_id == 2)
                            {
                                list_return.Add(obj);
                            }
                        }
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public cost_of_local_shipping getCostShippingAllOfCountry()
        {
            try
            {
                return db.cost_of_local_shippings.FirstOrDefault(c => c.local_shipping_service_id == null && c.status == true);
            }
            catch
            {
                return null;
            }
        }
    }
}
