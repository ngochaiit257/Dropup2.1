using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Providers
{
    public class StatisticsProvider
    {
        FreshMartDatabaseDataContext db = new FreshMartDatabaseDataContext();
        OrderProvider order_provider = new OrderProvider();
        TransportProvider transport_provider = new TransportProvider();
        ProductVariationInOrderProvider pvio_provider = new ProductVariationInOrderProvider();
        NewProductInOrderProvider cpio_provider = new NewProductInOrderProvider();
        ProductVariationProvider pv_provider = new ProductVariationProvider();

        public List<order> getAllOrder()
        {
            try
            {
                return db.orders.OrderByDescending(o => o.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Đếm số hóa đơn theo ngày
        /// </summary>
        /// <param name="date_to_get"></param>
        /// <returns></returns>
        public int getNumberOfOrderByDay(DateTime date_to_get)
        {
            try
            {
                return db.orders.Where(o => o.create_datetime.Date == date_to_get.Date && o.draft_status == false).Count();
            }
            catch
            {
                return -1;
            }
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }


        /// <summary>
        /// Thống kê số lượng hóa đơn theo từng ngày
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        public int[] numberOfOrdersByTimeLine(string timeline)
        {
            try
            {
                DateTime currentDate = System.DateTime.Now;
                var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMinutes(-1);

                //get Number of Month in this month
                int monthsApart = Math.Abs(12 * (firstDayOfMonth.Year - lastDayOfMonth.Year) + firstDayOfMonth.Day - lastDayOfMonth.Day);
                List<order> list_order_in_this_month = db.orders.Where(o => o.create_datetime.Date >= firstDayOfMonth && o.create_datetime <= lastDayOfMonth).ToList();

                List<int> listInt = new List<int>();

                if (timeline == null)
                {
                    timeline = "this_month";
                }

                if (timeline == "this_month")
                {
                    if (currentDate.Date >= firstDayOfMonth && currentDate.Date <= lastDayOfMonth)
                    {
                        foreach (DateTime date in getListDateFromFirstDateOfMonthToCurrent())
                        {
                            listInt.Add(order_provider.getAllOfficialOrderPerDay(date).Count());
                        }
                    }
                }
                if (timeline == "last_month")
                {
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-1).Month))
                    {
                        listInt.Add(order_provider.getAllOfficialOrderPerDay(date).Count());
                    }
                }
                return listInt.ToArray();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public static IEnumerable<DateTime> Daily(TimeSpan ts, DayOfWeek startDayOfWeek = DayOfWeek.Monday, DateTime? checkDay = null)
        //{
        //    var compDate = checkDay ?? DateTime.UtcNow;
        //    var days = startDayOfWeek - compDate.DayOfWeek;
        //    days = days > 0 ? days - 7 : days;
        //    var startDate = compDate.AddDays(days);

        //    for (var i = 0; i < 7; i++)
        //    {
        //        yield return startDate.AddDays(i).Date + ts;
        //    }
        //}


        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }


        public Decimal[] getRevenueByTimeLine(string timeline)
        {
            try
            {
                DateTime currentDate = System.DateTime.Now;
                var firstDayOfWeek = GetFirstDayOfWeek(currentDate);
                var allDateOfWeek = Enumerable.Range(0, 7).Select(d => firstDayOfWeek.AddDays(d)).ToList();

                List<Decimal> listDec = new List<Decimal>();

                if (timeline == null)
                {
                    timeline = "this_week";
                }

                //else if (timeline == "this_week")
                //{
                //    foreach(DateTime date in allDateOfWeek)
                //    {
                //        var listOrderInDayByThisWeek = db.orders.Where(o => o.create_datetime.Date == date.Date).ToList();
                //        Decimal sumRevenueInDayByThisWeek = 0;
                //        foreach (var order in listOrderInDayByThisWeek)
                //        {
                //            var listOrderDetailByOrder = db.order_details.Where(od => od.order_id == order.order_id).ToList();
                //            foreach (var order_detail in listOrderDetailByOrder)
                //            {
                //                var product = db.products.FirstOrDefault(p => p.product_id == order_detail.product_id);
                //                sumRevenueInDayByThisWeek += (Decimal)(order_detail.quantity * product.promotion_price);
                //            }
                //        }
                //        listDec.Add(sumRevenueInDayByThisWeek);
                //    }
                //}
                //else if (timeline == "last_week")
                //{
                //    foreach (DateTime date in Enumerable.Range(0, 7).Select(d => firstDayOfWeek.AddDays(-(d+1))).ToList())
                //    {
                //        var listOrderInDayByThisWeek = db.orders.Where(o => o.create_datetime.Date == date.Date).ToList();
                //        Decimal sumRevenueInDayByThisWeek = 0;
                //        foreach (var order in listOrderInDayByThisWeek)
                //        {
                //            var listOrderDetailByOrder = db.order_details.Where(od => od.order_id == order.order_id).ToList();
                //            foreach (var order_detail in listOrderDetailByOrder)
                //            {
                //                var product = db.products.FirstOrDefault(p => p.product_id == order_detail.product_id);
                //                sumRevenueInDayByThisWeek += (Decimal)(order_detail.quantity * product.promotion_price);
                //            }
                //        }
                //        listDec.Add(sumRevenueInDayByThisWeek);
                //    }
                //}

                else if (timeline == "this_month")
                {
                    listDec = getListRevenueFromFirstDateOfMonthToCurrent();
                }

                else if (timeline == "last_month")
                {
                    foreach (DateTime date in AllDatesInMonth(currentDate.Year, currentDate.AddMonths(-1).Month))
                    {
                        var listOrderPerDayByLastMonth = order_provider.getAllOfficialOrderPerDay(date);
                        Decimal sumRevenueInDayByLastMonth = 0;
                        if (listOrderPerDayByLastMonth != null)
                        {
                            foreach (var order in listOrderPerDayByLastMonth)
                            {
                                sumRevenueInDayByLastMonth += getRevenuePerOrder(order.order_id);
                            }
                            listDec.Add(sumRevenueInDayByLastMonth);
                        }

                    }
                }

                return listDec.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public int[] getOrderByHour()
        {
            try
            {
                List<int> listInt = new List<int>();
                foreach (int hour in Enumerable.Range(0, 24))
                {
                    var list_order_by_hour = db.orders.Where(o => o.create_datetime.Hour == hour && o.draft_status == false).ToList();
                    listInt.Add(list_order_by_hour.Count());
                }
                return listInt.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tính doanh thu trên mỗi đơn hàng
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public decimal getRevenuePerOrder(long order_id)
        {
            try
            {
                decimal revenue = 0;
                var order = order_provider.getById(order_id);
                var transport_of_this_order = transport_provider.getByOrderId(order_id);
                var list_pvio = pvio_provider.getByOrderId(order_id);
                var list_cpio = cpio_provider.getByOrderId(order_id);
                decimal shipping_fee = 0;

                if (transport_of_this_order != null)
                {
                    shipping_fee = (decimal)transport_of_this_order.shipping_fee;
                }
                else
                {
                    shipping_fee = (decimal)order.cost_of_shipping;
                }
                decimal cost_by_all_product_in_order = 0;
                if (list_pvio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_pvio.Sum(pv => pv.product_variation_quantity * pv.product_variation_price);
                }
                if (list_cpio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_cpio.Sum(pv => pv.custom_product_in_order_quantity * pv.custom_product_in_order_price);
                }
                revenue = cost_by_all_product_in_order;
                return revenue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính doanh thu thuần trên mỗi đơn hàng
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public decimal getNetRevenuePerOrder(long order_id)
        {
            try
            {
                decimal net_revenue = 0;
                var order = order_provider.getById(order_id);
                var transport_of_this_order = transport_provider.getByOrderId(order_id);
                var list_pvio = pvio_provider.getByOrderId(order_id);
                var list_cpio = cpio_provider.getByOrderId(order_id);
                decimal shipping_fee = 0;

                if (transport_of_this_order != null)
                {
                    shipping_fee = (decimal)transport_of_this_order.shipping_fee;
                }
                else
                {
                    shipping_fee = (decimal)order.cost_of_shipping;
                }
                decimal cost_by_all_product_in_order = 0;
                decimal repay = 0;
                if (list_pvio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_pvio.Sum(pv => pv.product_variation_quantity * pv.product_variation_price);
                    repay += (decimal)list_pvio.Sum(pv => pv.product_variation_price_repay);
                }
                if (list_cpio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_cpio.Sum(pv => pv.custom_product_in_order_quantity * pv.custom_product_in_order_price);
                    repay += (decimal)list_cpio.Sum(cp => cp.custom_product_price_repay);
                }
                net_revenue = cost_by_all_product_in_order - (decimal)order.discount_amount;
                return net_revenue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính tổng giá trị trên mỗi đơn hàng
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public decimal getTotalValuePerOrder(long order_id)
        {
            try
            {
                decimal total_value = 0;
                var order = order_provider.getById(order_id);
                var transport_of_this_order = transport_provider.getByOrderId(order_id);
                var list_pvio = pvio_provider.getByOrderId(order_id);
                var list_cpio = cpio_provider.getByOrderId(order_id);
                decimal shipping_fee = 0;

                if (transport_of_this_order != null)
                {
                    shipping_fee = (decimal)transport_of_this_order.shipping_fee;
                }
                else
                {
                    shipping_fee = (decimal)order.cost_of_shipping;
                }
                decimal cost_by_all_product_in_order = 0;
                decimal repay = 0;
                if (list_pvio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_pvio.Sum(pv => pv.product_variation_quantity * pv.product_variation_price);
                    repay += (decimal)list_pvio.Sum(pv => pv.product_variation_price_repay);
                }
                if (list_cpio != null)
                {
                    cost_by_all_product_in_order += (decimal)list_cpio.Sum(pv => pv.custom_product_in_order_quantity * pv.custom_product_in_order_price);
                    repay += (decimal)list_cpio.Sum(cp => cp.custom_product_price_repay);
                }
                total_value = cost_by_all_product_in_order + (decimal)order.cost_of_shipping - (decimal)order.discount_amount;
                return total_value;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính số tiền đã thu được trên mỗi đơn hàng
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public decimal getAmountReceivedPerOrder(long order_id)
        {
            try
            {
                decimal amount_received = 0;
                var order = order_provider.getById(order_id);
                var transport_of_this_order = transport_provider.getByOrderId(order_id);

                if (order.payment_method_id == 3)
                {
                    //Thu COD
                    if (order.payment_status == true)
                    {
                        amount_received = getRevenuePerOrder(order_id);
                    }
                }
                else
                {
                    //Chuyển khoản ngân hàng
                    if (transport_of_this_order != null)
                    {
                        if (order.payment_status == true && transport_of_this_order.get_money_status_id == true)
                        {
                            amount_received = (decimal)transport_of_this_order.get_cost_cod;
                        }
                    }
                }
                return amount_received;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính tổng số sản phẩm mỗi đơn hàng
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public int getTotalProductPerOrder(long order_id)
        {
            try
            {
                int total_product = 0;
                var order = order_provider.getById(order_id);
                var transport_of_this_order = transport_provider.getByOrderId(order_id);
                var list_pvio = pvio_provider.getByOrderId(order_id);
                var list_cpio = cpio_provider.getByOrderId(order_id);

                if (list_pvio != null)
                {
                    total_product += (int)list_pvio.Sum(l => l.product_variation_quantity);
                }
                if (list_cpio != null)
                {
                    total_product += (int)list_cpio.Sum(l => l.custom_product_in_order_quantity);
                }
                return total_product;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính doanh thu từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public decimal getRevenuePerDay(DateTime day)
        {
            try
            {
                decimal revenue = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        revenue += getRevenuePerOrder(order.order_id);
                    }
                }
                return revenue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính doanh thu THUẦN từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public decimal getNetRevenuePerDay(DateTime day)
        {
            try
            {
                decimal net_revenue = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        net_revenue += getNetRevenuePerOrder(order.order_id);
                    }
                }
                return net_revenue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính TỔNG GIÁ TRỊ CÁC ĐƠN HÀNG THEO từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public decimal getTotalValuePerDay(DateTime day)
        {
            try
            {
                decimal total_value = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        total_value += getTotalValuePerOrder(order.order_id);
                    }
                }
                return total_value;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính SỐ TIỀN ĐÃ THU ĐƯỢC THEO từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public decimal getAmountReceivedPerDay(DateTime day)
        {
            try
            {
                decimal amount_received = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        amount_received += getAmountReceivedPerOrder(order.order_id);
                    }
                }
                return amount_received;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính tổng số sản phẩm theo từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int getTotalProductPerDay(DateTime day)
        {
            try
            {
                int count_product = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        count_product += getTotalProductPerOrder(order.order_id);
                    }
                }
                return count_product;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính giảm giá theo từng ngày
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public decimal getDiscountPerDay(DateTime day)
        {
            try
            {
                decimal discount = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderPerDay(day);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        discount += (decimal)order.discount_amount;
                    }
                }
                return discount;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính doanh thu theo khoảng thời gian
        /// </summary>
        /// <param name="start_datetime"></param>
        /// <param name="end_datetime"></param>
        /// <returns></returns>
        public decimal getRevenueByTimeSegment(DateTime start_datetime, DateTime end_datetime)
        {
            try
            {
                decimal revenue = 0;
                var list_order_per_day = order_provider.getAllOfficialOrderByTimeSegment(start_datetime, end_datetime);
                if (list_order_per_day != null)
                {
                    foreach (var order in list_order_per_day)
                    {
                        revenue += getRevenuePerOrder(order.order_id);
                    }
                }
                return revenue;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Tính doanh thu tháng hiện tại
        /// </summary>
        /// <returns></returns>
        public decimal getRevenueThisMonth()
        {
            try
            {
                DateTime currentDateTime = System.DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
                //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMinutes(-1);

                return getRevenueByTimeSegment(firstDayOfMonth, currentDateTime);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy các ngày từ đầu tháng này đến hiện tại
        /// </summary>
        /// <returns></returns>
        public List<DateTime> getListDateFromFirstDateOfMonthToCurrent()
        {
            try
            {
                List<DateTime> listDateTime = new List<DateTime>();
                DateTime currentDateTime = System.DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
                //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMinutes(-1);
                for (DateTime d = firstDayOfMonth; d <= currentDateTime; d = d.AddDays(1))
                {
                    listDateTime.Add(d);
                }
                return listDateTime;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tính doanh thu từ đầu tháng này đến hiện tại
        /// </summary>
        /// <returns></returns>
        public List<decimal> getListRevenueFromFirstDateOfMonthToCurrent()
        {
            try
            {
                List<decimal> listRevenue = new List<decimal>();
                foreach (var day in getListDateFromFirstDateOfMonthToCurrent())
                {
                    listRevenue.Add(getRevenuePerDay(day));
                }
                return listRevenue;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách doanh thu tháng này để hiển thị thống kê
        /// </summary>
        /// <returns></returns>
        public List<RevenuePerDayModel> getListRevenueThisMonthToShow()
        {
            try
            {
                List<RevenuePerDayModel> list_return = new List<RevenuePerDayModel>();
                foreach (var day in getListDateFromFirstDateOfMonthToCurrent())
                {
                    var list_order_in_day = order_provider.getAllOfficialOrderPerDay(day);
                    if (list_order_in_day != null && list_order_in_day.Count() > 0)
                    {
                        RevenuePerDayModel model = new RevenuePerDayModel();
                        model.create_datetime = day;
                        model.count_order = list_order_in_day.Count();
                        model.revenue = getRevenuePerDay(day);
                        model.net_revenue = getNetRevenuePerDay(day);
                        model.discount = getDiscountPerDay(day);
                        model.shipping_fee = 0;
                        foreach (var order in list_order_in_day)
                        {
                            model.shipping_fee += (decimal)order.cost_of_shipping;
                        }
                        model.total_cost_order = getTotalValuePerDay(day);
                        model.amount_received = getAmountReceivedPerDay(day);
                        model.count_product = getTotalProductPerDay(day);
                        model.amount_repay = 0;
                        model.count_product_repay = 0;
                        list_return.Add(model);
                    }
                }
                return list_return.OrderByDescending(l => l.create_datetime).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Đếm số sản phẩm đã bán được của từng biến thể sp
        /// </summary>
        /// <param name="product_variation_id"></param>
        /// <returns></returns>
        public int countNumSoldPerProductVariation(long product_variation_id)
        {
            try
            {
                int num = 0;
                foreach (var order in order_provider.getAllOfficialOrder())
                {
                    var list_pvio = pvio_provider.getByOrderId(order.order_id);
                    if (list_pvio != null)
                    {
                        foreach (var pv in list_pvio)
                        {
                            if (pv.product_variation_id == product_variation_id)
                            {
                                num += (int)pv.product_variation_quantity;
                            }
                        }
                    }
                }
                return num;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy 10 sản phẩm bán chạy nhất
        /// </summary>
        /// <returns></returns>
        public List<TopProductVariationSaleModel> getTop10ProductVariationSale()
        {
            try
            {
                var list_return = new List<TopProductVariationSaleModel>();
                var list_pv = pv_provider.getAll();
                foreach (var pv in list_pv)
                {
                    TopProductVariationSaleModel model = new TopProductVariationSaleModel();
                    model.product_id = pv.product_id;
                    model.product_variation_id = pv.product_variation_id;
                    if (pv.product_variation_name == "Default Title")
                    {
                        model.show_name = pv.product.product_name;
                    }
                    else
                    {
                        model.show_name = pv.product.product_name + " - " + pv.product_variation_name;
                    }
                    model.price = (decimal)pv.product_variation_price;
                    model.num_of_sold = countNumSoldPerProductVariation(pv.product_variation_id);
                    if (pv.product_variation_in_stock != null)
                    {
                        model.total_num = model.num_of_sold + (int)pv.product_variation_in_stock;
                    }
                    model.sell_status = (bool)pv.product.status;
                    model.img = pv.product_variation_image;
                    model.img_alt = pv.product_variation_image_alt;
                    list_return.Add(model);
                }
                return list_return.OrderByDescending(pv => pv.num_of_sold).Take(10).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
