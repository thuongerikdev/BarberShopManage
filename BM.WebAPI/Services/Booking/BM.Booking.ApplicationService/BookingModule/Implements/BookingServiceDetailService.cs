using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDdtos;
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

namespace BM.Booking.ApplicationService.BookingModule.Implements
{
    public class BookingServiceDetailService : BookingServiceBase , IBookingServiceDetailService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingServiceDetailService(ILogger<BookingServiceDetailService> logger , BookingDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateServiceDetail(BookingCreateServiceDetailDto bookingCreateServiceDetailDto)
        {
            _logger.LogInformation("BookingCreateServiceDetail");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingCreateServiceDetailDto.servImage);
                var bookingServiceDetail = new BookingServiceDetail
                {
                    servName = bookingCreateServiceDetailDto.servName,
                    servID = bookingCreateServiceDetailDto.servID,
                    servPrice = bookingCreateServiceDetailDto.servPrice,
                    servDescription = bookingCreateServiceDetailDto.servDescription,
                    servStatus = bookingCreateServiceDetailDto.servStatus,
                    servImage = image
                };
                _dbContext.BookingServiceDetails.Add(bookingServiceDetail);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao service detail thanh cong", bookingServiceDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateServiceDetail(BookingUpdateServiceDetailDto bookingUpdateServiceDetailDto)
        {
            _logger.LogInformation("BookingUpdateServiceDetail");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingUpdateServiceDetailDto.servImage);
                var bookingServiceDetail = await _dbContext.BookingServiceDetails.FindAsync(bookingUpdateServiceDetailDto.serviceDetailID);
                if (bookingServiceDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay service detail");
                }
                bookingServiceDetail.servName = bookingUpdateServiceDetailDto.servName;
                bookingServiceDetail.servPrice = bookingUpdateServiceDetailDto.servPrice;
                bookingServiceDetail.servDescription = bookingUpdateServiceDetailDto.servDescription;
                bookingServiceDetail.servStatus = bookingUpdateServiceDetailDto.servStatus;
                bookingServiceDetail.servImage = image;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat service detail thanh cong", bookingServiceDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteServiceDetail(int serviceDetailID)
        {
            _logger.LogInformation("BookingDeleteServiceDetail");
            try
            {
                var bookingServiceDetail = await _dbContext.BookingServiceDetails.FindAsync(serviceDetailID);
                if (bookingServiceDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay service detail");
                }
                _dbContext.BookingServiceDetails.Remove(bookingServiceDetail);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa service detail thanh cong", bookingServiceDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetServiceDetailByID(int serviceDetailID)
        {
            _logger.LogInformation("BookingGetServiceDetailByID");
            try
            {
                var bookingServiceDetail = await _dbContext.BookingServiceDetails.FindAsync(serviceDetailID);
                if (bookingServiceDetail == null)
                {
                    return ErrorConst.Error(500, "khong tim thay service detail");
                }
                return ErrorConst.Success("tim thay service detail", bookingServiceDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllServiceDetail()
        {
            _logger.LogInformation("BookingGetAllServiceDetail");
            try
            {
                var bookingServiceDetails = await _dbContext.BookingServiceDetails.ToListAsync();
                return ErrorConst.Success("lay danh sach service detail thanh cong", bookingServiceDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task <ResponeDto> BookingGetServiceDetailByServiceID (int serviceID)
        {
            _logger.LogInformation("BookingGetServiceDetailByServiceID");
            try
            {
                var bookingServiceDetails = await _dbContext.BookingServiceDetails.Where(x => x.servID == serviceID).ToListAsync();
                if (bookingServiceDetails == null)
                {
                    return ErrorConst.Error(500, "khong tim thay service detail");
                }
                return ErrorConst.Success("tim thay service detail", bookingServiceDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
