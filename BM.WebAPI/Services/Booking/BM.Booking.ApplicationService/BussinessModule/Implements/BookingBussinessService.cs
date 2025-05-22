using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.BussinessModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Domain;
using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Dtos.CRUDDtos;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
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
using System.Text.Json;
using System.Threading.Tasks;
using BookingAppointment = BM.Booking.Domain.BookingAppointment;

namespace BM.Booking.ApplicationService.BussinessModule.Implements
{
    public class BookingBussinessService : BookingServiceBase , IBookingBussinessService , ISendOrderData
    {
        protected readonly IBookingAppointService _bookingAppointService;
        protected  readonly IBookingOrderService _bookingOrderService;
        //protected readonly IBookingPromoService _bookingPromoService;
        protected readonly IGetPromotionShared  _getPromotionShared;
        protected readonly IBookingServService _bookingServService;
        protected readonly IBookingInvoiceService _bookingInvoiceService;
        protected readonly IBookingOrderProductService _bookingOrderProductService;
        private readonly IConfiguration _configuration;
        public BookingBussinessService( 
            ILogger<BookingBussinessService> logger,
            BookingDbContext dbContext , 
            IBookingAppointService bookingAppointService,
            IGetPromotionShared getPromotionShared,
            IBookingOrderProductService bookingOrderProductService,
            //IBookingPromoService bookingPromoService,
            IConfiguration configuration,
            IBookingServService bookingServService,
            IBookingInvoiceService bookingInvoiceService,
            IBookingOrderService bookingOrderService) : base(logger, dbContext)
        {
            _bookingAppointService = bookingAppointService;
            _bookingOrderService = bookingOrderService;
            //_bookingPromoService = bookingPromoService;
            _bookingServService = bookingServService;
            _configuration = configuration;
            _bookingInvoiceService = bookingInvoiceService;
            _getPromotionShared = getPromotionShared;
            _bookingOrderProductService = bookingOrderProductService;
        }
        public async Task<ResponeDto> CreateOrderAppoint(List<BookingCreateBussinessAppointDto> appoint, BookingCreateOrderBussinessDto order, int? promoID)
        {
            try
            {
                double percentDiscount = 0;
                CustomerPromotionDto promoData = null;

                // Kiểm tra và lấy thông tin khuyến mãi nếu có promoID
                if (promoID.HasValue && promoID > 0)
                {
                    var promo = await _getPromotionShared.GetCustomerAndPromotionAsync(order.custID, promoID.Value);
                    if (promo == null || promo.ErrorCode != 200)
                    {
                        _logger.LogWarning($"Failed to get promotion for customerID: {order.custID}, promoID: {promoID}");
                        return ErrorConst.Error(404, "Không tìm thấy thông tin khuyến mãi hoặc khách hàng.");
                    }

                    // Ép kiểu promo.Data sang CustomerPromotionDto
                    promoData = promo.Data as CustomerPromotionDto;
                    if (promoData == null || promoData.CustomerData == null)
                    {
                        _logger.LogError($"Invalid promo data structure. Data: {JsonSerializer.Serialize(promo.Data)}");
                        return ErrorConst.Error(500, "Dữ liệu khuyến mãi không hợp lệ.");
                    }

                    // Lấy phần trăm ưu đãi từ CustomerData
                    percentDiscount = promoData.PromotionData.PromoDiscount;
                    if (percentDiscount < 0 || percentDiscount > 100)
                    {
                        _logger.LogWarning($"Invalid percentDiscount value: {percentDiscount} for customerID: {order.custID}");
                        return ErrorConst.Error(400, "Phần trăm ưu đãi không hợp lệ.");
                    }
                }

                // Lấy danh sách servID từ appoint
                var listAppoint = appoint.Select(item => item.servID).ToList();
                if (!listAppoint.Any())
                {
                    _logger.LogWarning("No services provided in the appointment list.");
                    return ErrorConst.Error(400, "Danh sách dịch vụ trống.");
                }

                // Lấy danh sách dịch vụ
                var services = await _dbContext.BookingServiceDetails
                    .Where(s => listAppoint.Contains(s.serviceDetailID))
                    .ToListAsync();
                if (services.Count != listAppoint.Count)
                {
                    _logger.LogWarning($"Mismatch in service count. Expected: {listAppoint.Count}, Found: {services.Count}");
                    return ErrorConst.Error(404, "Không tìm thấy một số dịch vụ.");
                }

                // Tính tổng tiền từ danh sách dịch vụ
                double totalMoneyOfAppoint = services.Sum(item => item.servPrice);
                if (totalMoneyOfAppoint <= 0)
                {
                    _logger.LogWarning("Total money calculated as zero or negative.");
                    return ErrorConst.Error(400, "Tổng tiền dịch vụ không hợp lệ.");
                }

                // Tính tổng tiền sau khi áp dụng ưu đãi (nếu có)
                double totalMoney = totalMoneyOfAppoint - (totalMoneyOfAppoint * percentDiscount / 100);
                if (totalMoney < 0)
                {
                    _logger.LogWarning($"Total money after discount is negative: {totalMoney}");
                    return ErrorConst.Error(500, "Tổng tiền sau giảm giá không hợp lệ.");
                }

                // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    // Tạo order
                    var orderFinal = new BookingCreateOrderDto
                    {
                        orderDate = order.orderDate,
                        orderStatus = "UnConfirm",
                        orderTotal = totalMoney,
                        createAt = DateTime.Now,
                        custID = order.custID,
                    };

                    var orderCreate = await _bookingOrderService.BookingCreateOrder(orderFinal);
                    if (orderCreate.ErrorCode != 200)
                    {
                        _logger.LogError($"Failed to create order: {orderCreate.ErrorMessager}");
                        await transaction.RollbackAsync();
                        return orderCreate;
                    }

                    var orderData = orderCreate.Data as BookingOrder;
                    if (orderData == null)
                    {
                        _logger.LogError("Failed to retrieve order data after creation.");
                        await transaction.RollbackAsync();
                        return ErrorConst.Error(500, "Không thể lấy thông tin đơn hàng sau khi tạo.");
                    }

                    // Tạo danh sách appointment
                    var createAppoint = appoint.Select(a => new BookingCreateAppointDto
                    {
                        appStatus = a.appStatus,
                        empID = a.empID,
                        servID = a.servID,
                        orderID = orderData.orderID,
                    }).ToList();

                    // Tạo appointment
                    var appointCreate = await _bookingAppointService.BookingCreateAppoint(createAppoint);
                    if (appointCreate.ErrorCode != 200)
                    {
                        _logger.LogError($"Failed to create appointments: {appointCreate.ErrorMessager}");
                        await transaction.RollbackAsync();
                        return appointCreate;
                    }

                    // Cập nhật trạng thái khuyến mãi nếu có sử dụng promo
                    if (promoID.HasValue && promoID > 0)
                    {
                        var cusPromo = await _getPromotionShared.AuthDecreasePromotion(promoData.cusPromoID);
                        if (cusPromo == null || cusPromo.ErrorCode != 200)
                        {
                            _logger.LogWarning($"Failed to decrease promotion for customerID: {order.custID}, promoID: {promoID}");
                            await transaction.RollbackAsync();
                            return ErrorConst.Error(404, "Không tìm thấy thông tin khuyến mãi hoặc khách hàng.");
                        }
                    }

                    // Lưu thay đổi vào database
                    await _dbContext.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    _logger.LogInformation($"Successfully created order {orderData.orderID} for customer {order.custID}");
                    return ErrorConst.Success("Tạo order, hóa đơn và dịch vụ thành công");
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error while creating order and appointments within transaction");
                    return ErrorConst.Error(500, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order and appointments");
                return ErrorConst.Error(500, ex.Message);
            }
        }


        public async Task<ResponeDto> CreateOrderSale(List<BookingCreateOrderProductDto> orderproduct, BookingCreateOrderBussinessDto order, int promoID)
        {
            try
            {
                // Lấy thông tin khuyến mãi và khách hàng
                var promo = await _getPromotionShared.GetCustomerAndPromotionAsync(order.custID, promoID);
                if (promo == null || promo.ErrorCode != 200)
                {
                    _logger.LogWarning($"Failed to get promotion for customerID: {order.custID}, promoID: {promoID}");
                    return ErrorConst.Error(404, "Không tìm thấy thông tin khuyến mãi hoặc khách hàng.");
                }

                // Ép kiểu promo.Data sang CustomerPromotionDto
                var promoData = promo.Data as CustomerPromotionDto;
                if (promoData == null || promoData.CustomerData == null)
                {
                    _logger.LogError($"Invalid promo data structure. Data: {JsonSerializer.Serialize(promo.Data)}");
                    return ErrorConst.Error(500, "Dữ liệu khuyến mãi không hợp lệ.");
                }

                // Lấy phần trăm ưu đãi từ CustomerData
                double percentDiscount = promoData.PromotionData.PromoDiscount;
                if (percentDiscount < 0 || percentDiscount > 100)
                {
                    _logger.LogWarning($"Invalid percentDiscount value: {percentDiscount} for customerID: {order.custID}");
                    return ErrorConst.Error(400, "Phần trăm ưu đãi không hợp lệ.");
                }

                // Lấy danh sách servID từ appoint
                var listProduct = orderproduct.Select(item => item.productDetailID).ToList();
                if (!listProduct.Any())
                {
                    _logger.LogWarning("No products provided in the appointment list.");
                    return ErrorConst.Error(400, "Danh sách dịch vụ trống.");
                }

                // Lấy danh sách dịch vụ
                var products = await  _dbContext.BookingProductDetails.Where(x =>  listProduct.Contains(x.productDetailID))
                    .ToListAsync();
                if (products.Count != listProduct.Count)
                {
                    _logger.LogWarning($"Mismatch in service count. Expected: {listProduct.Count}, Found: {products.Count}");
                    return ErrorConst.Error(404, "Không tìm thấy một số dịch vụ.");
                }

                // Tính tổng tiền từ danh sách dịch vụ
                double totalMoneyOfAppoint = products.Sum(item => item.productPrice);
                if (totalMoneyOfAppoint <= 0)
                {
                    _logger.LogWarning("Total money calculated as zero or negative.");
                    return ErrorConst.Error(400, "Tổng tiền dịch vụ không hợp lệ.");
                }

                // Tính tổng tiền sau khi áp dụng ưu đãi
                double totalMoney = totalMoneyOfAppoint - (totalMoneyOfAppoint * percentDiscount / 100);
                if (totalMoney < 0)
                {
                    _logger.LogWarning($"Total money after discount is negative: {totalMoney}");
                    return ErrorConst.Error(500, "Tổng tiền sau giảm giá không hợp lệ.");
                }

                // Tạo order
                var orderFinal = new BookingCreateOrderDto
                {
                    orderDate = DateTime.Now,
                    orderStatus = "UnConfirm",
                    orderTotal = totalMoney,
                    createAt = DateTime.Now,
                    custID = order.custID,
                };

                var orderCreate = await _bookingOrderService.BookingCreateOrder(orderFinal);
                if (orderCreate.ErrorCode != 200)
                {
                    _logger.LogError($"Failed to create order: {orderCreate.ErrorMessager}");
                    return orderCreate;
                }

                var orderData = orderCreate.Data as BookingOrder;
                if (orderData == null)
                {
                    _logger.LogError("Failed to retrieve order data after creation.");
                    return ErrorConst.Error(500, "Không thể lấy thông tin đơn hàng sau khi tạo.");
                }

                // Tạo danh sách appointment
                var createAppoint = orderproduct.Select(x => new BookingCreateOrderProductDto
                {
                    orderID = x.orderID,
                    productDetailID = x.productDetailID,
                    productPrice = x.productPrice,
                    quantity = x.quantity,
                }).ToList();


                // Tạo appointment
                var appointCreate = await _bookingOrderProductService.BookingCreateOrderProduct(createAppoint);
                if (appointCreate.ErrorCode != 200)
                {
                    _logger.LogError($"Failed to create appointments: {appointCreate.ErrorMessager}");
                    return appointCreate;
                }

                // Lưu thay đổi vào database
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Successfully created order {orderData.orderID} for customer {order.custID}");
                return ErrorConst.Success("Tạo order, hóa đơn và dịch vụ thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order and appointments");
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
                    listAppoint.Add(item.serviceDetailID);
                }
                //Lấy danh sách dịch vụ
                var service = await _dbContext.BookingServices.Where(s => listAppoint.Contains(s.servID)).ToListAsync();

                //var review = await _dbContext.BookingReviews.Where(r => listAppoint.Contains(r.appID)).ToListAsync();

                var empMoneyList = new List<BookingEmpMoney>();
                foreach (var appointment in appointData)
                {
                    // Tìm dịch vụ tương ứng với servID
                    var services = service.FirstOrDefault(s => s.servID == appointment.serviceDetailID);
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
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
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
