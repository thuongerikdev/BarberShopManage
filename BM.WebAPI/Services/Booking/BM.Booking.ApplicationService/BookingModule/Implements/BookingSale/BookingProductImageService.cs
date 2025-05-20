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
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM.Booking.ApplicationService.BookingModule.Implements.BookingSale
{
    public class BookingProductImageService : BookingServiceBase , IBookingProductImageService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingProductImageService(ILogger<BookingProductImageService> logger, BookingDbContext bookingDbContext , ICloudinaryService cloudinaryService) : base(logger, bookingDbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task <ResponeDto> BookingCreateProductImage(BookingCreateProductImageDto bookingCreateProductInageDto)
        {
            _logger.LogInformation("BookingCreateProductImage");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(bookingCreateProductInageDto.srcImage);
                var bookingProductImage = new BookingProductImage
                {
                    productID = bookingCreateProductInageDto.productID,
                    srcImage = img,
                };
                _dbContext.BookingProductImages.Add(bookingProductImage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao product image thanh cong", bookingProductImage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BookingCreateProductImage");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateProductImage(BookingUpdateProductImageDto bookingUpdateProductImageDto)
        {
            _logger.LogInformation("BookingUpdateProductImage");
            try
            {
                var bookingProductImage = await _dbContext.BookingProductImages.FindAsync(bookingUpdateProductImageDto.productImageID);
                if (bookingProductImage == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product image");
                }
                bookingProductImage.productID = bookingUpdateProductImageDto.productID;
                var img = await _cloudinaryService.UploadImageAsync(bookingUpdateProductImageDto.srcImage);
                bookingProductImage.srcImage = img;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("update product image thanh cong", bookingProductImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BookingUpdateProductImage");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetProductImage(int productImageID)
        {
            _logger.LogInformation("BookingGetProductImage");
            try
            {
                var bookingProductImage = await _dbContext.BookingProductImages.FindAsync(productImageID);
                if (bookingProductImage == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product image");
                }
                return ErrorConst.Success("get product image thanh cong", bookingProductImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BookingGetProductImage");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllProductImage()
        {
            _logger.LogInformation("BookingGetAllProductImage");
            try
            {
                var bookingProductImages = await _dbContext.BookingProductImages.ToListAsync();
                if (bookingProductImages == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product image");
                }
                return ErrorConst.Success("get all product image thanh cong", bookingProductImages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BookingGetAllProductImage");
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteProductImage(int productImageID)
        {
            _logger.LogInformation("BookingDeleteProductImage");
            try
            {
                var bookingProductImage = await _dbContext.BookingProductImages.FindAsync(productImageID);
                if (bookingProductImage == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product image");
                }
                _dbContext.BookingProductImages.Remove(bookingProductImage);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("delete product image thanh cong", bookingProductImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BookingDeleteProductImage");
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
