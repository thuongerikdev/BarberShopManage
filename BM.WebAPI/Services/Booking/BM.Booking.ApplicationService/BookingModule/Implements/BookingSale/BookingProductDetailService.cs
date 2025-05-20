using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Booking.Infrastructure;
using BM.Constant;
using BM.Shared.ApplicationService;
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
        private readonly ICloudinaryService _cloudinaryService;
        public BookingProductDetailService(ILogger<BookingProductDetailService> logger, BookingDbContext dbContext , ICloudinaryService  cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateProductDetail(BookingCreateProductDetailDto bookingCreateProductDetailDto)
        {
            _logger.LogInformation("BookingCreateProductDetail");
            try
            {
                // Upload ảnh và lấy URL
                var img = await _cloudinaryService.UploadImageAsync(bookingCreateProductDetailDto.productImage);

                var bookingProductDetail = new BookingProductDetail
                {
                    productID = bookingCreateProductDetailDto.productID,
                    productQuantity = bookingCreateProductDetailDto.productQuantity,
                    productPrice = bookingCreateProductDetailDto.productPrice,
                    productStatus = bookingCreateProductDetailDto.productStatus,
                    productColor = bookingCreateProductDetailDto.productColor,
                    productSize = bookingCreateProductDetailDto.productSize,
                    productType = bookingCreateProductDetailDto.productType,
                    productDescription = bookingCreateProductDetailDto.productDescription,
                    productName = bookingCreateProductDetailDto.productName,
                    productImage = img, // hoặc .Url.ToString() tùy service
                    productNote = bookingCreateProductDetailDto.productNote,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                };

                _dbContext.BookingProductDetails.Add(bookingProductDetail);
                await _dbContext.SaveChangesAsync();

                return ErrorConst.Success("Tạo product detail thành công", bookingProductDetail);
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
                    return ErrorConst.Error(500, "không tìm thấy product detail");
                }
                var img = await _cloudinaryService
                    .UploadImageAsync(bookingUpdateProductDetailDto.productImage);

                // Cập nhật tất cả các thuộc tính
                bookingProductDetail.productID = bookingUpdateProductDetailDto.productID;
                bookingProductDetail.productPrice = bookingUpdateProductDetailDto.productPrice;
                bookingProductDetail.productQuantity = bookingUpdateProductDetailDto.productQuantity;
                bookingProductDetail.productStatus = bookingUpdateProductDetailDto.productStatus;
                bookingProductDetail.productColor = bookingUpdateProductDetailDto.productColor;
                bookingProductDetail.productSize = bookingUpdateProductDetailDto.productSize;
                bookingProductDetail.productType = bookingUpdateProductDetailDto.productType;
                bookingProductDetail.productDescription = bookingUpdateProductDetailDto.productDescription;
                bookingProductDetail.productName = bookingUpdateProductDetailDto.productName;
                bookingProductDetail.productImage = img;
                bookingProductDetail.productNote = bookingUpdateProductDetailDto.productNote;
                bookingProductDetail.updateAt = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật product detail thành công", bookingProductDetail);
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
        public async Task<ResponeDto> BookingGetProductDetailByProductID(int productID)
        {
            _logger.LogInformation("BookingGetProductDetail");
            try
            {
                var bookingProductDetail = await _dbContext.BookingProductDetails.Where(x => x.productID == productID).ToListAsync();
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
    }
}
