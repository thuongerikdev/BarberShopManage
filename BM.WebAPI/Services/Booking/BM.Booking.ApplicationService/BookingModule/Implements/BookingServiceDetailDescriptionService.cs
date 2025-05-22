using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using BM.Constant.Dto;
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BM.Booking.ApplicationService.BookingModule.Implements
{
    public class BookingServiceDetailDescriptionService : BookingServiceBase  , IBookingServiceDetailDescriptionService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingServiceDetailDescriptionService(
            ILogger<BookingServiceDetailDescriptionService> logger,
            ICloudinaryService cloudinaryService,
            BookingDbContext dbContext) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateServiceDetailDescription(BookingCreateServiceDetailDescriptionDto createBookingServiceDetailDescriptionDto)
        {
            _logger.LogInformation("BookingCreateServiceDetailDescription");
            try
            {
                // Upload ảnh và lấy URL
                var img = await _cloudinaryService.UploadImageAsync(createBookingServiceDetailDescriptionDto.servImage);
                var bookingServiceDetailDescription = new BookingServiceDetailDescription
                {
                    serviceDetailID = createBookingServiceDetailDescriptionDto.serviceDetailID,
                    servDescription = createBookingServiceDetailDescriptionDto.servDescription,
                    servImage = img,
                    servStatus = createBookingServiceDetailDescriptionDto.servStatus,
                    servName = createBookingServiceDetailDescriptionDto.servName,
                    servType = createBookingServiceDetailDescriptionDto.servType,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                };
                _dbContext.bookingServiceDetailDescriptions.Add(bookingServiceDetailDescription);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo service detail description thành công", bookingServiceDetailDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateServiceDetailDescription(BookingUpdateServiceDetailDescriptionDto updateBookingServiceDetailDescriptionDto)
        {
            _logger.LogInformation("BookingUpdateServiceDetailDescription");
            try
            {
                var bookingServiceDetailDescription = await _dbContext.bookingServiceDetailDescriptions.FindAsync(updateBookingServiceDetailDescriptionDto.serviceDetailDescriptionID);
                if (bookingServiceDetailDescription == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service detail description");
                }
                // Upload ảnh và lấy URL
                var img = await _cloudinaryService.UploadImageAsync(updateBookingServiceDetailDescriptionDto.servImage);
                bookingServiceDetailDescription.serviceDetailID = updateBookingServiceDetailDescriptionDto.serviceDetailID;
                bookingServiceDetailDescription.servName = updateBookingServiceDetailDescriptionDto.servName;
                bookingServiceDetailDescription.servImage = img;
                bookingServiceDetailDescription.servStatus = updateBookingServiceDetailDescriptionDto.servStatus;
                bookingServiceDetailDescription.servType = updateBookingServiceDetailDescriptionDto.servType;
                bookingServiceDetailDescription.servDescription = updateBookingServiceDetailDescriptionDto.servDescription;
                bookingServiceDetailDescription.updateAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật service detail description thành công", bookingServiceDetailDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetServiceDetailDescriptionByID(int serviceDetailDescriptionID)
        {
            _logger.LogInformation("BookingGetServiceDetailDescriptionById");
            try
            {
                var bookingServiceDetailDescription = await _dbContext.bookingServiceDetailDescriptions.FindAsync(serviceDetailDescriptionID);
                if (bookingServiceDetailDescription == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service detail description");
                }
                return ErrorConst.Success("Lấy service detail description thành công", bookingServiceDetailDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllServiceDetailDescription()
        {
            _logger.LogInformation("BookingGetAllServiceDetailDescription");
            try
            {
                var bookingServiceDetailDescriptions = await _dbContext.bookingServiceDetailDescriptions.ToListAsync();
                return ErrorConst.Success("Lấy tất cả service detail description thành công", bookingServiceDetailDescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteServiceDetailDescription(int serviceDetailDescriptionID)
        {
            _logger.LogInformation("BookingDeleteServiceDetailDescription");
            try
            {
                var bookingServiceDetailDescription = await _dbContext.bookingServiceDetailDescriptions.FindAsync(serviceDetailDescriptionID);
                if (bookingServiceDetailDescription == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service detail description");
                }
                _dbContext.bookingServiceDetailDescriptions.Remove(bookingServiceDetailDescription);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa service detail description thành công", bookingServiceDetailDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

        public async Task<ResponeDto> BookingGetServiceDetailDescriptionByServiceDetailID(int serviceDetailID)
        {
            _logger.LogInformation("BookingGetServiceDetailDescriptionByServiceDetailID");
            try
            {
                var bookingServiceDetailDescriptions = await _dbContext.bookingServiceDetailDescriptions
                    .Where(x => x.serviceDetailID == serviceDetailID)
                    .Include(x => x.BookingServiceDetails)
                    .Select(x => new BookingGetServiceDetailAndDescriptions
                    {
                        serviceDetailID = x.serviceDetailID,
                        servDesDescription = x.servDescription,
                        servImage = x.servImage,
                        servDesName = x.servName,
                        servStatus = x.servStatus,
                        servType = x.servType,
                        servDescription = x.BookingServiceDetails.servDescription,
                        servName = x.BookingServiceDetails.servName,
                        serviceDetailDescriptionID = x.serviceDetailDescriptionID,
                        servID = x.BookingServiceDetails.servID,
                        servPrice = x.BookingServiceDetails.servPrice,



                    })
                    .ToListAsync();
                if (bookingServiceDetailDescriptions == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service detail description");
                }
                return ErrorConst.Success("Lấy tất cả service detail description thành công", bookingServiceDetailDescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
