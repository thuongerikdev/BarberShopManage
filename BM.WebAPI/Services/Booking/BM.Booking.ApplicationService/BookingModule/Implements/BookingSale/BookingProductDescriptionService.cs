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
    public class BookingProductDescriptionService : BookingServiceBase, IBookingProductDescriptionService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingProductDescriptionService( ICloudinaryService cloudinaryService,  ILogger<BookingProductDescriptionService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateProductDescription(BookingCreateProductDescriptionDto bookingCreateProductDescriptionDto)
        {
            _logger.LogInformation("BookingCreateProductDescription");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingCreateProductDescriptionDto.productImage);

                var bookingProductDescription = new BookingProductDescription
                {
                    productColor = bookingCreateProductDescriptionDto.productColor,
                    productSize = bookingCreateProductDescriptionDto.productSize,
                    productImage = image,
                    productDescription = bookingCreateProductDescriptionDto.productDescription,
                    productName = bookingCreateProductDescriptionDto.productName,
                    productType = bookingCreateProductDescriptionDto.productType,
                    productStatus = bookingCreateProductDescriptionDto.productStatus,
                };
                _dbContext.BookingProductDescriptions.Add(bookingProductDescription);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao product description thanh cong", bookingProductDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateProductDescription(BookingUpdateProductDescriptionDto bookingUpdateProductDescriptionDto)
        {
            _logger.LogInformation("BookingUpdateProductDescription");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingUpdateProductDescriptionDto.productImage);
                var bookingProductDescription = await _dbContext.BookingProductDescriptions.FindAsync(bookingUpdateProductDescriptionDto.productDescriptionID);
                if (bookingProductDescription == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product description");
                }
                bookingProductDescription.productColor = bookingUpdateProductDescriptionDto.productColor;
                bookingProductDescription.productSize = bookingUpdateProductDescriptionDto.productSize;
                bookingProductDescription.productImage = image;
                bookingProductDescription.productDescription = bookingUpdateProductDescriptionDto.productDescription;
                bookingProductDescription.productName = bookingUpdateProductDescriptionDto.productName;
                bookingProductDescription.productType = bookingUpdateProductDescriptionDto.productType;
                bookingProductDescription.productStatus = bookingUpdateProductDescriptionDto.productStatus;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat product description thanh cong", bookingProductDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteProductDescription(int productDescriptionID)
        {
            _logger.LogInformation("BookingDeleteProductDescription");
            try
            {
                var bookingProductDescription = await _dbContext.BookingProductDescriptions.FindAsync(productDescriptionID);
                if (bookingProductDescription == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product description");
                }
                _dbContext.BookingProductDescriptions.Remove(bookingProductDescription);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa product description thanh cong", bookingProductDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetProductDescription(int productDescriptionID)
        {
            _logger.LogInformation("BookingGetProductDescription");
            try
            {
                var bookingProductDescription = await _dbContext.BookingProductDescriptions.FindAsync(productDescriptionID);
                if (bookingProductDescription == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product description");
                }
                return ErrorConst.Success("lay product description thanh cong", bookingProductDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllProductDescription()
        {
            _logger.LogInformation("BookingGetAllProductDescription");
            try
            {
                var bookingProductDescriptions = await _dbContext.BookingProductDescriptions.ToListAsync();
                return ErrorConst.Success("lay danh sach product description thanh cong", bookingProductDescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }



    }
}
