using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.BussinessModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Domain;
using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Dtos.CRUDDtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using BM.Constant.Dto;
using BM.Shared.ApplicationService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingAppointment = BM.Booking.Domain.BookingAppointment;

namespace BM.Booking.ApplicationService.BussinessModule.Implements
{
    public class BookingBussinessService : BookingServiceBase , IBookingBussinessService , ISendOrderData
    {
        protected readonly IBookingAppointService _bookingAppointService;
        protected  readonly IBookingOrderService _bookingOrderService;
        protected readonly IBookingPromoService _bookingPromoService;
        protected readonly IBookingServService _bookingServService;
        protected readonly IBookingInvoiceService _bookingInvoiceService;
        private readonly IConfiguration _configuration;
        public BookingBussinessService( 
            ILogger<BookingBussinessService> logger,
            BookingDbContext dbContext , 
            IBookingAppointService bookingAppointService,
            IBookingPromoService bookingPromoService,
            IConfiguration configuration,
            IBookingServService bookingServService,
            IBookingInvoiceService bookingInvoiceService,
            IBookingOrderService bookingOrderService) : base(logger, dbContext)
        {
            _bookingAppointService = bookingAppointService;
            _bookingOrderService = bookingOrderService;
            _bookingPromoService = bookingPromoService;
            _bookingServService = bookingServService;
            _configuration = configuration;
            _bookingInvoiceService = bookingInvoiceService;
        }
        public async Task <ResponeDto> CreateOrderAppoint (List<BookingCreateBussinessAppointDto> appoint , BookingCreateOrderBussinessDto order, int promoID)
        {
            try
            {
                //Lấy danh sách các mã ưu đãi
                var promo = await _bookingPromoService.BookingGetPromo(promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy promo");
                }
                var listAppoint = new List<int>();
                //Lặp để lấy danh sách các mã dịch vụ
                foreach (var item in appoint)
                {
                    listAppoint.Add(item.servID);
                }
                //Lấy danh sách dịch vụ
                var service = await _dbContext.BookingServices.Where(s => listAppoint.Contains(s.servID)).ToListAsync();
                if (service.Count != appoint.Count)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                //Lặp để lấy tổng tiền
                double totalMoneyofAppoint = 0;
                foreach (var item in service)
                {
                    totalMoneyofAppoint += item.servPrice;
                }

                var promoData = promo.Data as BookingPromotion;

                //Lấy phần trăm ưu đãi
                var percent = promoData.promoDiscount;
                var TotalMoney = totalMoneyofAppoint - (totalMoneyofAppoint * percent / 100);   
                var orderFinal = new BookingCreateOrderDto
                {
                    orderDate = DateTime.Now,
                    orderStatus = "UnConfirm",
                    orderTotal = TotalMoney,
                    createAt = DateTime.Now,
                    custID = order.custID,


                };
                //Tạo order
                var orderCreate = await _bookingOrderService.BookingCreateOrder(orderFinal);
                if (orderCreate.ErrorCode != 200)
                {
                    return orderCreate;
                }
                var orderData = orderCreate.Data as BookingOrder;

                var createAppoint = appoint.Select(a => new BookingCreateAppointDto
                {
                    appStatus = a.appStatus,
                    empID = a.empID,
                    servID = a.servID,
                    orderID = orderData.orderID,

                }).ToList();
                //Tao invoice 
                var invoice = new BookingCreateInvoiceDto
                {
                    orderID = orderData.orderID,
                    paymentTerms = "**Điều khoản thanh toán:**\r\n\r\n1. **Thời gian thanh toán:** Hóa đơn này phải được thanh toán trong vòng 30 ngày kể từ ngày lập hóa đơn (Invoice Date). Nếu không nhận được thanh toán trong thời gian quy định, chúng tôi có quyền áp dụng lãi suất 1.5% mỗi tháng trên số tiền chưa thanh toán.\r\n\r\n2. **Phương thức thanh toán:** Khách hàng có thể thực hiện thanh toán qua các phương thức sau:\r\n   - Chuyển khoản ngân hàng trực tiếp vào tài khoản ngân hàng của chúng tôi.\r\n   - Thanh toán bằng thẻ tín dụng thông qua hệ thống thanh toán trực tuyến của chúng tôi.\r\n   - Tiền mặt tại văn phòng của chúng tôi trong giờ làm việc.\r\n\r\n3. **Chi phí phát sinh:** Tất cả các chi phí phát sinh liên quan đến việc thu hồi khoản nợ sẽ do khách hàng chịu. Điều này bao gồm nhưng không giới hạn ở chi phí pháp lý, phí thu hồi nợ, và các chi phí khác phát sinh trong quá trình thu hồi.\r\n\r\n4. **Khách hàng có trách nhiệm:** Khách hàng có trách nhiệm kiểm tra thông tin trên hóa đơn và thông báo ngay cho chúng tôi nếu có bất kỳ sự không chính xác nào trong vòng 7 ngày kể từ ngày nhận hóa đơn.\r\n\r\n5. **Điều kiện hủy bỏ:** Nếu khách hàng không thanh toán đúng hạn, chúng tôi có quyền hủy bỏ hoặc tạm dừng tất cả các dịch vụ đang cung cấp cho khách hàng cho đến khi khoản nợ được thanh toán đầy đủ.",
                    status = "UnPaid",
                    totalAmount = TotalMoney,
                    invoiceDate = DateTime.Now,
                   
                };
                var invoiceCreate = await _bookingInvoiceService.BookingCreateInvoice(invoice);

                //Tạo Appointment
                await _bookingAppointService.BookingCreateAppoint(createAppoint);
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Tạo order và dịch vụ thành công");
            }
            catch (Exception ex)
            {
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> GetOrderToOtherDomain(int orderID)
        {
            _logger.LogInformation("GetOrderToOtherDomain");
            try
            {
                var order = await _bookingOrderService.BookingGetOrder(orderID);
                if (order == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                var orderData = order.Data as BookingOrder;


                var appoint = await _bookingAppointService.BookingGetAppointByOrderID(orderID);

                var appointData = appoint.Data as List<BookingAppointment>;

                var listAppoint = new List<int>();
                //Lặp để lấy danh sách các mã dịch vụ
                foreach (var item in appointData)
                {
                    listAppoint.Add(item.servID);
                }
                //Lấy danh sách dịch vụ
                var service = await _dbContext.BookingServices.Where(s => listAppoint.Contains(s.servID)).ToListAsync();

                //var review = await _dbContext.BookingReviews.Where(r => listAppoint.Contains(r.appID)).ToListAsync();

                var empMoneyList = new List<BookingEmpMoney>();
                foreach (var appointment in appointData)
                {
                    // Tìm dịch vụ tương ứng với servID
                    var services = service.FirstOrDefault(s => s.servID == appointment.servID);
                    //var reviews = review.FirstOrDefault(r => r.appID == appointment.appID);
                    if (service != null)
                    {
                        // Giả sử service có thuộc tính Price hoặc bạn cần logic tính tiền
                        double money = services.servPrice; // Thay bằng logic thực tế nếu cần
                        //int rate = reviews.rating;

                        // Thêm thông tin empID và money vào danh sách
                        empMoneyList.Add(new BookingEmpMoney
                        {
                            empID = appointment.empID,
                            money = money,
                            //rating = rate
                        });
                    }
                }

                // Nhóm theo empID để tổng hợp tiền nếu nhân viên xuất hiện nhiều lần
                //var groupedEmpMoney = empMoneyList
                //    .GroupBy(e => e.empID)
                //    .Select(g => new BookingEmpMoney
                //    {
                //        empID = g.Key,
                //        money = g.Sum(e => e.money)
                //    })
                //    .ToList();

                // Tạo DTO kết quả
                var data = new BookingGetCustAndEmpByOrderDto
                {
                    custID = orderData.custID,
                    totalMoney = orderData.orderTotal,
                    empMoney = empMoneyList
                };
                return ErrorConst.Success("Lấy thông tin order thành công", order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

       public async Task <string> CreatePaymentUrl(int orderiD, HttpContext context)
        {


            var order = await  _bookingOrderService.BookingGetOrder(orderiD);
            if (order == null)
            {
                return null;
            }
            var orderData = order.Data as BookingOrder;
            if (orderData.orderStatus == "Confirmed")
            {
                return "Đơn hàng đã được thanh toán";
            }
            
            var amount = orderData.orderTotal;
            var infor = "thanh toán hóa đơn cho dịch vụ của shop" + " với số tiền " + amount + " VNĐ " + " orderID " + orderiD  ;
            var orderType = "other";

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var baseUrl = _configuration["ApplicationSettings:BaseUrl"];
            var urlCallBack = $"{baseUrl}/api/BookingBussiness/vnpay/PaymentExecute";

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", infor);
            pay.AddRequestData("vnp_OrderType", orderType);
            pay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }


        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }
    }
}
