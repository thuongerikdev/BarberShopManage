using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.PaymentModule.Implements
{
    public class BookingOrderService : BookingServiceBase, IBookingOrderService
    {
        public BookingOrderService(ILogger<BookingOrderService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateOrder(BookingCreateOrderDto bookingCreateOrderDto)
        {
            _logger.LogInformation("BookingCreateOrder");
            try
            {

                var order = new BookingOrder
                {
                    orderDate = bookingCreateOrderDto.orderDate,
                    orderStatus = bookingCreateOrderDto.orderStatus,
                    orderTotal = bookingCreateOrderDto.orderTotal,
                    createAt = bookingCreateOrderDto.createAt,
                    custID = bookingCreateOrderDto.custID,
 
                };
                _dbContext.BookingOrders.Add(order);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo order thành công", order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateOrder(BookingUpdateOrderDto bookingUpdateOrderDto)
        {
            _logger.LogInformation("BookingUpdateOrder");
            try
            {
                var order = await _dbContext.BookingOrders.FindAsync(bookingUpdateOrderDto.orderID);
                if (order == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                order.orderDate = bookingUpdateOrderDto.orderDate;
                order.orderStatus = bookingUpdateOrderDto.orderStatus;
                order.orderTotal = bookingUpdateOrderDto.orderTotal;
                order.createAt = bookingUpdateOrderDto.createAt;
                order.custID = bookingUpdateOrderDto.custID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật order thành công", order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteOrder(int orderID)
        {
            _logger.LogInformation("BookingDeleteOrder");
            try
            {
                var order = await _dbContext.BookingOrders.FindAsync(orderID);
                if (order == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                _dbContext.BookingOrders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa order thành công", order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetOrder(int orderID)
        {
            _logger.LogInformation("BookingGetOrder");
            try
            {
                var order = await _dbContext.BookingOrders.FindAsync(orderID);
                if (order == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                return ErrorConst.Success("Lấy order thành công", order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
        public async Task<ResponeDto> BookingGetAllOrder()
        {
            _logger.LogInformation("BookingGetAllOrder");
            try
            {
                var orders = await _dbContext.BookingOrders.ToListAsync();
                return ErrorConst.Success("Lấy danh sách order thành công", orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingConfirmOrder(int orderID)
        {
            _logger.LogInformation("BookingGetAllOrder");
            try
            {
                var orders = await _dbContext.BookingOrders.FindAsync(orderID);
                if (orders == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                orders.orderStatus = "Completed";
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Lấy danh sách order thành công", orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetOrderByCustomerID(int customerID)
        {
            _logger.LogInformation("BookingGetOrderByCustomerID");
            try
            {
                var orders = await _dbContext.BookingOrders.Where(x => x.custID == customerID).ToListAsync();
                if (orders == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy order");
                }
                return ErrorConst.Success("Lấy danh sách order thành công", orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
    }
   
}
