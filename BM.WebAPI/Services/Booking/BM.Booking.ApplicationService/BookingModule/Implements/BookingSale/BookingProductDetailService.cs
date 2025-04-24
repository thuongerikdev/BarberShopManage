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
    public class BookingProductDetailService : BookingServiceBase, IBookingProductDetailService
    {
        public BookingProductDetailService(ILogger<BookingProductDetailService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateProductDetail(BookingCreateProductDetailDto bookingCreateProductDetailDto)
        {
            _logger.LogInformation("BookingCreateProductDetail");
            try
            {
                var bookingProductDetail = new BookingProductDetail
                {
                    productDescriptionID = bookingCreateProductDetailDto.productDescriptionID,
                    productID = bookingCreateProductDetailDto.productID,
                    productQuantity = bookingCreateProductDetailDto.productQuantity,
                    productPrice = bookingCreateProductDetailDto.productPrice,
                    supplierID = bookingCreateProductDetailDto.supplierID,
                };
                _dbContext.BookingProductDetails.Add(bookingProductDetail);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao product detail thanh cong", bookingProductDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateProductDetail(BookingUpdateProductDetailDto bookingUpdateProductDetailDto)
        {
            _logger.LogInformation("BookingUpdateProductDetail");
            try
            {
                var bookingProductDetail = await _dbContext.BookingProductDetails.FindAsync(bookingUpdateProductDetailDto.productDetailID);
                if (bookingProductDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product detail");
                }
                bookingProductDetail.productDescriptionID = bookingUpdateProductDetailDto.productDescriptionID;
                bookingProductDetail.productID = bookingUpdateProductDetailDto.productID;
                bookingProductDetail.productQuantity = bookingUpdateProductDetailDto.productQuantity;
                bookingProductDetail.productPrice = bookingUpdateProductDetailDto.productPrice;
                bookingProductDetail.supplierID = bookingUpdateProductDetailDto.supplierID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat product detail thanh cong", bookingProductDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteProductDetail(int productDetailID)
        {
            _logger.LogInformation("BookingDeleteProductDetail");
            try
            {
                var bookingProductDetail = await _dbContext.BookingProductDetails.FindAsync(productDetailID);
                if (bookingProductDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product detail");
                }
                _dbContext.BookingProductDetails.Remove(bookingProductDetail);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa product detail thanh cong", bookingProductDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetProductDetail(int productDetailID)
        {
            _logger.LogInformation("BookingGetProductDetail");
            try
            {
                var bookingProductDetail = await _dbContext.BookingProductDetails.FindAsync(productDetailID);
                if (bookingProductDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product detail");
                }
                return ErrorConst.Success("lay product detail thanh cong", bookingProductDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllProductDetail()
        {
            _logger.LogInformation("BookingGetAllProductDetail");
            try
            {
                var bookingProductDetails = await _dbContext.BookingProductDetails.ToListAsync();
                return ErrorConst.Success("lay danh sach product detail thanh cong", bookingProductDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
