using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM.Booking.ApplicationService.BookingModule.Implements.BookingSale
{
    public class BookingSupplierProductDetailService : BookingServiceBase , IBookingSupplierProductDetailService
    {
      
        public BookingSupplierProductDetailService(
            ILogger<BookingSupplierProductDetailService> logger,
            BookingDbContext dbContext) : base(logger, dbContext)
        {
          
        }
        public async Task<ResponeDto> BookingCreateSupplierProductDetail(BookingCreateSupplierProductDetailDto bookingCreateSupplierProductDto)
        {
            _logger.LogInformation("BookingCreateSupplierProduct");
            try
            {
                var bookingSupplierProduct = new BookingSupplierProductDetail
                {
                    supplierID = bookingCreateSupplierProductDto.supplierID,
                    productDetailID = bookingCreateSupplierProductDto.productDetailID,
                    productPrice = bookingCreateSupplierProductDto.productPrice,
                    productQuantity = bookingCreateSupplierProductDto.productQuantity,
                    productStatus = bookingCreateSupplierProductDto.productStatus,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                };
                var bookingDetail = await _dbContext.BookingProductDetails
                    .Where(x => x.productDetailID == bookingCreateSupplierProductDto.productDetailID)
                    .Select(x => new BookingProductDetail
                    {
                        productQuantity = x.productQuantity
                    })
                    .FirstOrDefaultAsync();


                
                bookingDetail.productQuantity += bookingCreateSupplierProductDto.productQuantity;

              

                _dbContext.BookingSupplierProductDetails.Add(bookingSupplierProduct);
                _dbContext.SaveChanges();
                return ErrorConst.Success("Tạo supplier product thành công", bookingSupplierProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateSupplierProductDetail(BookingUpdateSupplierProductDetailDto bookingUpdateSupplierProductDto)
        {
            _logger.LogInformation("BookingUpdateSupplierProduct");
            try
            {
                var bookingSupplierProduct = await _dbContext.BookingSupplierProductDetails.FindAsync(bookingUpdateSupplierProductDto.supplierProductDetailID);
                if (bookingSupplierProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier product");
                }
                bookingSupplierProduct.supplierID = bookingUpdateSupplierProductDto.supplierID;
                bookingSupplierProduct.productDetailID = bookingUpdateSupplierProductDto.productDetailID;
                bookingSupplierProduct.productPrice = bookingUpdateSupplierProductDto.productPrice;
                bookingSupplierProduct.productQuantity = bookingUpdateSupplierProductDto.productQuantity;
                bookingSupplierProduct.productStatus = bookingUpdateSupplierProductDto.productStatus;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật supplier product thành công", bookingSupplierProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteSupplierProductDetail(int supplierProductDetailID)
        {
            _logger.LogInformation("BookingDeleteSupplierProduct");
            try
            {
                var bookingSupplierProduct = await _dbContext.BookingSupplierProductDetails.FindAsync(supplierProductDetailID);
                if (bookingSupplierProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier product");
                }
                _dbContext.BookingSupplierProductDetails.Remove(bookingSupplierProduct);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa supplier product thành công", bookingSupplierProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetSupplierProductDetail(int supplierProductDetailID)
        {
            _logger.LogInformation("BookingGetSupplierProduct");
            try
            {
                var bookingSupplierProduct = await _dbContext.BookingSupplierProductDetails.FindAsync(supplierProductDetailID);
                if (bookingSupplierProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier product");
                }
                return ErrorConst.Success("Lấy thông tin supplier product thành công", bookingSupplierProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> BookingGetAllSupplierProductDetail()
        {
            _logger.LogInformation("BookingGetAllSupplierProduct");
            try
            {
                var bookingSupplierProducts = await _dbContext.BookingSupplierProductDetails.ToListAsync();
                if (bookingSupplierProducts == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier product");
                }
                return ErrorConst.Success("Lấy tất cả thông tin supplier product thành công", bookingSupplierProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
