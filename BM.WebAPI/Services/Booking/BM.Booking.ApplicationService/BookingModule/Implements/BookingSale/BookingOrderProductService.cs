using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Implements.BookingSale
{
    public class BookingOrderProductService : BookingServiceBase , IBookingOrderProductService
    {
        public BookingOrderProductService(ILogger<BookingOrderProductService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateOrderProduct(List<BookingCreateOrderProductDto> bookingCreateOrderProductDto)
        {
            _logger.LogInformation("BookingCreateOrderProduct");
            try
            {
                var bookingOrderProduct =  bookingCreateOrderProductDto.Select (x => new BookingOrderProduct
                {
                    orderID = x.orderID,
                    productDetailID = x.productDetailID,
                    productPrice = x.productPrice,
                    quantity = x.quantity,
                    
                });
                
                _dbContext.BookingOrderProducts.AddRange(bookingOrderProduct);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao order product thanh cong", bookingOrderProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateOrderProduct(BookingUpdateOrderProductDto bookingUpdateOrderProductDto)
        {
            _logger.LogInformation("BookingUpdateOrderProduct");
            try
            {
                var bookingOrderProduct = await _dbContext.BookingOrderProducts.FindAsync(bookingUpdateOrderProductDto.orderProductID);
                if (bookingOrderProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay order product");
                }
                bookingOrderProduct.orderID = bookingUpdateOrderProductDto.orderID;
                bookingOrderProduct.productDetailID = bookingUpdateOrderProductDto.productDetailID;
                bookingOrderProduct.productPrice = bookingUpdateOrderProductDto.productPrice;
                bookingOrderProduct.quantity = bookingUpdateOrderProductDto.quantity;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat order product thanh cong", bookingOrderProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteOrderProduct(int orderProductID)
        {
            _logger.LogInformation("BookingDeleteOrderProduct");
            try
            {
                var bookingOrderProduct = await _dbContext.BookingOrderProducts.FindAsync(orderProductID);
                if (bookingOrderProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay order product");
                }
                _dbContext.BookingOrderProducts.Remove(bookingOrderProduct);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa order product thanh cong", bookingOrderProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetOrderProduct(int orderProductID)
        {
            _logger.LogInformation("BookingGetOrderProduct");
            try
            {
                var bookingOrderProduct = await _dbContext.BookingOrderProducts.FindAsync(orderProductID);
                if (bookingOrderProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay order product");
                }
                return ErrorConst.Success("lay order product thanh cong", bookingOrderProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> BookingGetAllOrderProduct()
        {
            _logger.LogInformation("BookingGetAllOrderProduct");
            try
            {
                var bookingOrderProducts = await _dbContext.BookingOrderProducts.ToListAsync();
                return ErrorConst.Success("lay danh sach order product thanh cong", bookingOrderProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
