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
using static System.Net.Mime.MediaTypeNames;

namespace BM.Booking.ApplicationService.BookingModule.Implements.BookingSale
{
    public class BookingCategoryService : BookingServiceBase , IBookingCategoryService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingCategoryService(ILogger<BookingCategoryService> logger, BookingDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateCategory(BookingCreateCategoryDto bookingCreateCategoryDto)
        {
            _logger.LogInformation("BookingCreateCategory");
            try
            {
                var image = await  _cloudinaryService.UploadImageAsync(bookingCreateCategoryDto.categoryImage);
                var bookingCategory = new BookingCategory
                {
                    categoryName = bookingCreateCategoryDto.categoryName,
                    categoryDescription = bookingCreateCategoryDto.categoryDescription,
                    categoryPrice = bookingCreateCategoryDto.categoryPrice,
                    categoryStatus = bookingCreateCategoryDto.categoryStatus,
                    categoryImage = image
                };
                _dbContext.BookingCategories.Add(bookingCategory);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao category thanh cong", bookingCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateCategory(BookingUpdateCategoryDto bookingUpdateCategoryDto)
        {
            _logger.LogInformation("BookingUpdateCategory");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingUpdateCategoryDto.categoryImage);
                var bookingCategory = await _dbContext.BookingCategories.FindAsync(bookingUpdateCategoryDto.categoryID);
                if (bookingCategory == null)
                {
                    return ErrorConst.Error(500, "khong tim thay category");
                }
                bookingCategory.categoryName = bookingUpdateCategoryDto.categoryName;
                bookingCategory.categoryDescription = bookingUpdateCategoryDto.categoryDescription;
                bookingCategory.categoryPrice = bookingUpdateCategoryDto.categoryPrice;
                bookingCategory.categoryStatus = bookingUpdateCategoryDto.categoryStatus;
                bookingCategory.categoryImage = image;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat category thanh cong", bookingCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteCategory(int categoryID)
        {
            _logger.LogInformation("BookingDeleteCategory");
            try
            {
                var bookingCategory = await _dbContext.BookingCategories.FindAsync(categoryID);
                if (bookingCategory == null)
                {
                    return ErrorConst.Error(500, "khong tim thay category");
                }
                _dbContext.BookingCategories.Remove(bookingCategory);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa category thanh cong", bookingCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetCategory(int categoryID)
        {
            _logger.LogInformation("BookingGetCategory");
            try
            {
                var bookingCategory = await _dbContext.BookingCategories.FindAsync(categoryID);
                if (bookingCategory == null)
                {
                    return ErrorConst.Error(500, "khong tim thay category");
                }
                return ErrorConst.Success("lay category thanh cong", bookingCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllCategory()
        {
            _logger.LogInformation("BookingGetAllCategory");
            try
            {
                var bookingCategories = await _dbContext.BookingCategories.ToListAsync();
                return ErrorConst.Success("lay danh sach category thanh cong", bookingCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
