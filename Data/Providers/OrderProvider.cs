using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class OrderProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        ProductVariationProvider product_variation_provider = new ProductVariationProvider();

        ProductVariationInOrderProvider pvio_provider = new ProductVariationInOrderProvider();
        CustomProductInOrder cpio_provider = new CustomProductInOrder();
        OrderLogProvider ol_provider = new OrderLogProvider();
        OrderTagProvider ot_provider = new OrderTagProvider();
        TransportProvider transport_provider = new TransportProvider();

        PaymentInformationProvider pi_provider = new PaymentInformationProvider();
        TransportLogProvider tl_provider = new TransportLogProvider();

        public List<order> getAll()
        {
            try
            {
                return db.orders.OrderBy(o => o.order_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public order getById(long order_id)
        {
            try
            {
                return db.orders.FirstOrDefault(o => o.order_id == order_id);
            }
            catch
            {
                return null;
            }
        }

        public List<order> getByCustomerId(long customer_id)
        {
            try
            {
                return db.orders.OrderBy(o => o.customer_id == customer_id).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool insertOrderDraft(order model, ref long order_id)
        {
            try
            {
                db.orders.InsertOnSubmit(model);
                db.SubmitChanges();
                order_id = model.order_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertProductVariationInOrder(long pv_id, int quantity, long order_id)
        {
            try
            {
                var product_variation = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id);
                product_variation_in_order model = new product_variation_in_order();
                model.product_variation_id = pv_id;
                model.product_variation_name = product_variation.product_variation_name;
                model.product_variation_price = product_variation.product_variation_price;
                model.product_variation_quantity = quantity;
                model.order_id = order_id;
                if (product_variation.product.unit_id != null)
                {
                    model.unit_name = product_variation.product.unit.unit_name;
                }
                model.product_name = db.product_variations.FirstOrDefault(pv => pv.product_variation_id == pv_id).product.product_name;
                model.product_variation_image = product_variation.product_variation_image;
                db.product_variation_in_orders.InsertOnSubmit(model);
                product_variation_provider.updateInStock(product_variation, quantity);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertCustomProductInOrder(custom_product_in_order model)
        {
            try
            {
                db.custom_product_in_orders.InsertOnSubmit(model);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertOrder(order model, ref long order_id)
        {
            try
            {
                db.orders.InsertOnSubmit(model);
                db.SubmitChanges();
                order_id = model.order_id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<order> getAllOfficialOrderPerDay(DateTime day)
        {
            try
            {
                return db.orders.Where(o => o.create_datetime.Date == day.Date && o.draft_status == false && o.confirm_order_status == true).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<order> getAllOfficialOrderByTimeSegment(DateTime start_datetime, DateTime end_datetime)
        {
            try
            {
                return db.orders.Where(o => o.create_datetime >= start_datetime && o.create_datetime <= end_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }


        public List<order> getOfficialOrderByCustomerId(long customer_id)
        {
            try
            {
                return db.orders.Where(o => o.customer_id == customer_id && o.draft_status == false).ToList();
            }
            catch
            {
                return null;
            }
        }

        public order getLastestOfficialOrderByCustomerId(long customer_id)
        {
            try
            {
                var list_order = getOfficialOrderByCustomerId(customer_id);
                return list_order.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public order getOfficialOrderById(long order_id)
        {
            try
            {
                return db.orders.FirstOrDefault(o => o.order_id == order_id && o.draft_status == false);
            }
            catch
            {
                return null;
            }
        }

        public decimal getSumCostByCustomerId(long customer_id)
        {
            try
            {
                var list_order = getOfficialOrderByCustomerId(customer_id);
                decimal sum_cost_to_return = 0;
                foreach (var order in list_order)
                {
                    foreach (var obj in db.product_variation_in_orders.Where(pvo => pvo.order_id == order.order_id).ToList())
                    {
                        sum_cost_to_return += (decimal)(obj.product_variation_price * obj.product_variation_quantity);
                    }
                    foreach (var obj in db.custom_product_in_orders.Where(pvo => pvo.order_id == order.order_id).ToList())
                    {
                        sum_cost_to_return += (decimal)(obj.custom_product_in_order_price * obj.custom_product_in_order_quantity);
                    }
                }
                return sum_cost_to_return;
            }
            catch
            {
                return 0;
            }
        }

        public decimal getSumCostByOrderId(long order_id)
        {
            try
            {
                decimal sum_cost_by_all_product = 0;
                var order = getById(order_id);
                foreach (var obj in db.product_variation_in_orders.Where(pvo => pvo.order_id == order_id).ToList())
                {
                    sum_cost_by_all_product += (decimal)(obj.product_variation_price * obj.product_variation_quantity);
                }
                foreach (var obj in db.custom_product_in_orders.Where(pvo => pvo.order_id == order_id).ToList())
                {
                    sum_cost_by_all_product += (decimal)(obj.custom_product_in_order_price * obj.custom_product_in_order_quantity);
                }
                return (decimal)(sum_cost_by_all_product - order.discount_amount + order.cost_of_shipping);
            }
            catch
            {
                return 0;
            }
        }


        public order getDraftOrderById(long draft_order_id)
        {
            try
            {
                return db.orders.FirstOrDefault(o => o.order_id == draft_order_id && o.draft_status == true);
            }
            catch
            {
                return null;
            }
        }


        public List<product_variation_in_order> getProductVariationInOrderByOrderId(long order_id)
        {
            try
            {
                var list_return = db.product_variation_in_orders.Where(pvo => pvo.order_id == order_id).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<custom_product_in_order> getCustomProductInOrderByOrderId(long order_id)
        {
            try
            {
                var list_return = db.custom_product_in_orders.Where(pvo => pvo.order_id == order_id).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }
        public List<product_variation_in_order> getProductVariationInOrderByOrderId(long order_id, ref decimal total_price_list_pv)
        {
            try
            {
                var list_return = db.product_variation_in_orders.Where(pvo => pvo.order_id == order_id).ToList();
                foreach (var obj in list_return)
                {
                    total_price_list_pv += (decimal)(obj.product_variation_price * obj.product_variation_quantity);
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<custom_product_in_order> getCustomProductInOrderByOrderId(long order_id, ref decimal total_price_list_cp)
        {
            try
            {
                var list_return = db.custom_product_in_orders.Where(pvo => pvo.order_id == order_id).ToList();
                foreach (var obj in list_return)
                {
                    total_price_list_cp += (decimal)(obj.custom_product_in_order_price * obj.custom_product_in_order_quantity);
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public bool updateOrder(order order)
        {
            try
            {
                var old_model = getById(order.order_id);
                old_model.customer_id = order.customer_id;
                old_model.update_datetime = order.update_datetime;
                old_model.source_order = order.source_order;
                old_model.delivery_address_id = order.delivery_address_id;
                old_model.user_id_update = order.user_id_update;
                old_model.coupon_id = order.coupon_id;
                old_model.discount_amount = order.discount_amount;
                old_model.reason_of_promotion = order.reason_of_promotion;
                old_model.cost_of_shipping = order.cost_of_shipping;
                old_model.name_of_shipping = order.name_of_shipping;
                old_model.payment_method_id = order.payment_method_id;
                old_model.draft_status = order.draft_status;
                old_model.confirm_order_status = order.confirm_order_status;
                old_model.payment_status = order.payment_status;
                old_model.completed_status = order.completed_status;
                old_model.order_note = order.order_note;
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteListProductVariationInOrder(long order_id)
        {
            try
            {
                var list_to_delete = getProductVariationInOrderByOrderId(order_id);
                db.product_variation_in_orders.DeleteAllOnSubmit(list_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteListCustomProductInOrder(long order_id)
        {
            try
            {
                var list_to_delete = getCustomProductInOrderByOrderId(order_id);
                db.custom_product_in_orders.DeleteAllOnSubmit(list_to_delete);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updatePaymentStatus(long order_id, long user_id, DateTime update_datetime)
        {
            try
            {
                var order = getById(order_id);
                order.payment_status = !order.payment_status;
                order.user_id_update = user_id;
                order.update_datetime = update_datetime;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateInfoOfUpdate(long order_id, long user_id, DateTime update_datetime)
        {
            try
            {
                var order = getById(order_id);
                order.user_id_update = user_id;
                order.update_datetime = update_datetime;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool updateConfirmStatus(long order_id, long user_id, DateTime update_datetime)
        {
            try
            {
                var order = getById(order_id);
                order.confirm_order_status = !order.confirm_order_status;
                order.user_id_update = user_id;
                order.update_datetime = update_datetime;
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteOrder(long order_id)
        {
            try
            {
                var list_pvio = pvio_provider.getByOrderId(order_id);
                var list_cpio = cpio_provider.getByOrderId(order_id);
                var list_ol = ol_provider.getByOrderId(order_id);
                var list_ot = ot_provider.getByOrderId(order_id);
                var tp = transport_provider.getByOrderId(order_id); 

                if(list_pvio.Count > 0)
                {
                    pvio_provider.deleteListProductVariationInOrder(list_pvio);
                }

                if (list_cpio.Count > 0)
                {
                   cpio_provider.deleteListCustomProductInOrder(list_cpio);
                }

                if (list_ot.Count > 0)
                {
                    ot_provider.deleteListOrderTagInOrder(list_ot);
                }

                if (list_ol.Count > 0)
                {
                    foreach (var ol in list_ol)
                    {
                        var pi_to_delete = pi_provider.getByOrderLogId(ol.order_log_id);
                        if (pi_to_delete != null)
                        {
                            pi_provider.deletePaymentInfomation(pi_to_delete);
                        }
                    }
                    ol_provider.deleteListOrderLogInOrder(list_ol);
                }

                if(tp != null)
                {
                    if (tl_provider.deleteListTranportLogInOrder(tl_provider.getByTransportId(tp.transport_id)))
                    {
                        transport_provider.deleteTransport(tp);
                    }
                }
                db.orders.DeleteOnSubmit(getById(order_id));
                db.SubmitChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public List<order> getAllOfficialOrder()
        {
            try
            {
                return db.orders.Where(o => o.draft_status == false).OrderByDescending(o => o.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<order> getListOrderHaveNotTransport(List<order> list_order)
        {
            try
            {
                var list_return = new List<order>();
                var list_transport = db.transports.ToList();
                List<long> list_order_id_have_transport_daNhanCOD = new List<long>();
                List<order> list_order_have_transport = new List<order>();
                foreach (var transport in list_transport)
                {
                    list_order_id_have_transport_daNhanCOD.Add(transport.order_id);
                }
                foreach (var order_id_have_transport in list_order_id_have_transport_daNhanCOD)
                {
                    list_order_have_transport.Add(getById(order_id_have_transport));
                }
                foreach (var order in list_order)
                {
                    if (!list_order_have_transport.Contains(order))
                    {
                        list_return.Add(order);
                    }
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<order> getByPaymentStatus(string quick_filter_condition)
        {
            try
            {
                var list_return = new List<order>();
                if (quick_filter_condition == "is_paid")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_status == true).ToList();
                }
                else if (quick_filter_condition == "is_pending")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_status == false).ToList();
                }
                else
                {
                    list_return = null;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }


        public List<order> getByShippingStatus(string quick_filter_condition)
        {
            try
            {
                var list_return = new List<order>();
                if (quick_filter_condition == "1")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(1))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "2")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(2))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "3")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(3))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "4")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(4))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "5")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(5))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "6")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(6))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else if (quick_filter_condition == "7")
                {
                    foreach (var transport in transport_provider.getByShippingStatus(7))
                    {
                        list_return.Add(getById(transport.order_id));
                    }
                }
                else
                {
                    list_return = null;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        //GET BY TIME_TO_ORDER
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }
        public List<order> getByTimeToOrder(string quick_filter_condition)
        {
            try
            {
                DateTime currentDate = System.DateTime.Now;
                List<order> list_return = new List<order>();
                if (quick_filter_condition == "time_to_order_this_week")
                {
                    var firstDayOfWeek = GetFirstDayOfWeek(currentDate);
                    var allDateOfWeek = Enumerable.Range(0, 7).Select(d => firstDayOfWeek.AddDays(d)).ToList();
                    foreach (DateTime date in allDateOfWeek)
                    {
                        var list_order_in_day_by_week = getAllOfficialOrder().Where(o => o.create_datetime.Date == date.Date).ToList();
                        foreach (var o in list_order_in_day_by_week)
                        {
                            list_return.Add(o);
                        }
                    }
                }
                else if (quick_filter_condition == "time_to_order_this_month")
                {
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-1).Month))
                    {
                        var list_order_in_day_by_month = getAllOfficialOrder().Where(o => o.create_datetime.Date == date.Date).ToList();
                        foreach (var o in list_order_in_day_by_month)
                        {
                            list_return.Add(o);
                        }
                    }
                }
                else if (quick_filter_condition == "time_to_order_this_3_month")
                {
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-1).Month))
                    {
                        var list_order_in_day_by_month = getAllOfficialOrder().Where(o => o.create_datetime.Date == date.Date).ToList();
                        foreach (var o in list_order_in_day_by_month)
                        {
                            list_return.Add(o);
                        }
                    }
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-2).Month))
                    {
                        var list_order_in_day_by_month = getAllOfficialOrder().Where(o => o.create_datetime.Date == date.Date).ToList();
                        foreach (var o in list_order_in_day_by_month)
                        {
                            list_return.Add(o);
                        }
                    }
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-3).Month))
                    {
                        var list_order_in_day_by_month = getAllOfficialOrder().Where(o => o.create_datetime.Date == date.Date).ToList();
                        foreach (var o in list_order_in_day_by_month)
                        {
                            list_return.Add(o);
                        }
                    }
                }
                else if (quick_filter_condition == "time_to_order_this_year")
                {
                    list_return = getAllOfficialOrder().Where(o => o.create_datetime.Year == currentDate.Year).ToList();
                }
                else if (quick_filter_condition == "time_to_order_today")
                {
                    list_return = getAllOfficialOrder().Where(o => o.create_datetime.Date == currentDate.Date).ToList();
                }
                else if (quick_filter_condition == "time_to_order_yesterday")
                {
                    list_return = getAllOfficialOrder().Where(o => o.create_datetime.Date == currentDate.Date.AddDays(-1)).ToList();
                }
                else
                {
                    list_return = null;
                }

                return list_return;
            }
            catch
            {
                return null;
            }
        }
        //END

        //GET ORDER BY TAG
        public List<order> getByTagName(string order_tag_name)
        {
            try
            {
                List<order> list_return = new List<order>();
                var dbo_link = db.order_tags.Where(ot => ot.order_tag_name == order_tag_name).ToList();
                foreach (var obj in dbo_link)
                {
                    list_return.Add(getById((long)obj.order_id));
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
        //END

        //GET BY GET MONEY CODE STATUS
        public List<order> getByGetMoneyCODStatus(string quick_filter_condition)
        {
            try
            {
                List<order> list_return = new List<order>();
                if (quick_filter_condition == "khongThuCOD")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_method_id == 4).ToList();
                }
                else if (quick_filter_condition == "daNhan")
                {
                    var list_transport = db.transports.ToList();
                    List<long> list_order_id_have_transport_daNhanCOD = new List<long>();
                    List<order> list_order_have_transport = new List<order>();
                    foreach (var transport in list_transport)
                    {
                        if (transport.get_money_status_id == true)
                        {
                            list_order_id_have_transport_daNhanCOD.Add(transport.order_id);
                        }
                    }
                    foreach (var order_id_have_transport in list_order_id_have_transport_daNhanCOD)
                    {
                        list_order_have_transport.Add(getById(order_id_have_transport));
                    }
                    list_return = list_order_have_transport.Where(o => o.payment_method_id == 3).ToList();
                }
                else if (quick_filter_condition == "chuaNhan")
                {
                    var list_transport = db.transports.ToList();
                    List<long> list_order_id_have_transport_daNhanCOD = new List<long>();
                    List<order> list_order_have_transport = new List<order>();
                    foreach (var transport in list_transport)
                    {
                        if (transport.get_money_status_id == false)
                        {
                            list_order_id_have_transport_daNhanCOD.Add(transport.order_id);
                        }
                    }
                    foreach (var order_id_have_transport in list_order_id_have_transport_daNhanCOD)
                    {
                        list_order_have_transport.Add(getById(order_id_have_transport));
                    }
                    list_return = list_order_have_transport.Where(o => o.payment_method_id == 3).ToList();
                }
                else
                {
                    list_return = null;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
        //END

        //GET BY XAC THUC DON HANG
        public List<order> getByConfirmation(string quick_filter_condition)
        {
            try
            {
                List<order> list_return = new List<order>();
                if (quick_filter_condition == "daXacThuc")
                {
                    list_return = getAllOfficialOrder().Where(o => o.confirm_order_status == true).ToList();
                }
                else if (quick_filter_condition == "chuaXacThuc")
                {
                    list_return = getAllOfficialOrder().Where(o => o.confirm_order_status == false).ToList();
                }
                else
                {
                    list_return = null;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
        //END

        //GET BY PAYMENT METHOD
        public List<order> getByPaymentMethod(string quick_filter_condition)
        {
            try
            {
                List<order> list_return = new List<order>();
                if (quick_filter_condition == "3")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_method_id == 3).ToList();
                }
                else if (quick_filter_condition == "4")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_method_id == 4).ToList();
                }
                else
                {
                    list_return = null;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
        //END



        public List<order> getByTabOrder(string quick_filter_tab, List<order> list_order)
        {
            try
            {
                List<order> list_return = new List<order>();
                if (quick_filter_tab == "tab_unconfimred_order")
                {
                    list_return = getAllOfficialOrder().Where(o => o.confirm_order_status == false).ToList();
                }
                else if (quick_filter_tab == "tab_unpaid_order")
                {
                    list_return = getAllOfficialOrder().Where(o => o.payment_status == false).ToList();
                }
                else if (quick_filter_tab == "tab_has_not_transport")
                {
                    list_return = getListOrderHaveNotTransport(list_order);
                }
                else
                {
                    list_return = list_order;
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public List<order> getListOrderFilter(string search_string, string quick_filter_select, string quick_filter_condition,
                                                        decimal quick_filter_value, string quick_filter_tab)
        {
            try
            {
                var list_return = new List<order>();
                if (search_string == null)
                {
                    search_string = "";
                }
                if (quick_filter_condition != null)
                {
                    if (quick_filter_select == "payment_status")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByPaymentStatus(quick_filter_condition));
                    }
                    else if (quick_filter_select == "shipping_status")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByShippingStatus(quick_filter_condition));
                    }
                    else if (quick_filter_select == "time_to_order")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByTimeToOrder(quick_filter_condition));
                    }
                    else if (quick_filter_select == "tag")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByTagName(quick_filter_value.ToString()));
                    }
                    else if (quick_filter_select == "get_money_cod_status")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByGetMoneyCODStatus(quick_filter_condition));
                    }
                    else if (quick_filter_select == "confirm_status")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByConfirmation(quick_filter_condition));
                    }
                    else if (quick_filter_select == "payment_method")
                    {
                        list_return = getByTabOrder(quick_filter_tab, getByPaymentMethod(quick_filter_condition));
                    }
                    else
                    {
                        list_return = getByTabOrder(quick_filter_tab, getAllOfficialOrder());
                    }
                }
                else
                {
                    list_return = getAllOfficialOrder();
                }
                list_return = list_return.Where(o => ("#" + o.order_id + o.customer.customer_first_name + " " + o.customer.customer_last_name + " " + o.customer.customer_email).ToLower().Contains(search_string.ToLower())).OrderByDescending(o => o.create_datetime).ToList();
                return list_return;
            }
            catch
            {
                return null;
            }
        }

        public int getShippingStatusIdTransportByOrderId(long order_id)
        {
            try
            {
                return (int)db.transports.FirstOrDefault(t => t.order_id == order_id).shipping_status_id;
            }
            catch
            {
                return 0;
            }
        }

        public string getShippingStatusDescriptionTransportByOrderId(long order_id)
        {
            try
            {
                return db.transports.FirstOrDefault(t => t.order_id == order_id).shipping_status.shipping_status_description;
            }
            catch
            {
                return "";
            }
        }

        public bool getGetMoneyStatusDescriptionTransportByOrderId(long order_id)
        {
            try
            {
                return (bool)db.transports.FirstOrDefault(t => t.order_id == order_id).get_money_status_id;
            }
            catch
            {
                return false;
            }
        }

        public List<order> getListOrderQua24GioChuaLayHang()
        {
            try
            {
                var list_return = new List<order>();
                DateTime current_dattime = System.DateTime.Now;
                var transport_qua_24_gio_chua_lay = db.transports.Where(t => (current_dattime >= t.create_datetime.Value.AddDays(1)) && t.shipping_status_id == 1).OrderByDescending(t => t.create_datetime).ToList();
                foreach(var transport in transport_qua_24_gio_chua_lay)
                {
                    list_return.Add(getById(transport.order_id));
                }
                return list_return;
            }
            catch
            {
                return null;
            }
        }
    }
}
