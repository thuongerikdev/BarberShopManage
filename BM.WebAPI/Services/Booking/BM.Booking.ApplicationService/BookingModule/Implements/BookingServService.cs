using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Implements
{
    public class BookingServService : BookingServiceBase, IBookingServService
    {
        public BookingServService(ILogger<BookingServService> logger, BookingDbContext bookingDbContext) : base(logger, bookingDbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateService(BookingCreateServiceDto bookingCreateServiceDto)
        {
            _logger.LogInformation("BookingCreateService called");
            try
            {
                var service = new BookingService
                {
                    servName = bookingCreateServiceDto.servName,
                    servPrice = bookingCreateServiceDto.servPrice,
                    servDescription = bookingCreateServiceDto.servDescription,
                    servStatus = bookingCreateServiceDto.servStatus,
                    servImage = bookingCreateServiceDto.servImage,                  
                };
                _dbContext.BookingServices.Add(service);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo dịch vụ thành công", service);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateService(BookingUpdateServiceDto bookingUpdateServiceDto)
        {
            _logger.LogInformation("BookingUpdateService called");
            try
            {
                var service = await _dbContext.BookingServices.FindAsync(bookingUpdateServiceDto.servID);
                if (service == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                service.servName = bookingUpdateServiceDto.servName;
                service.servPrice = bookingUpdateServiceDto.servPrice;
                service.servDescription = bookingUpdateServiceDto.servDescription;
                service.servStatus = bookingUpdateServiceDto.servStatus;
                service.servImage = bookingUpdateServiceDto.servImage;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật dịch vụ thành công", service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteService(int serviceID)
        {
            _logger.LogInformation("BookingDeleteService called");
            try
            {
                var service = await _dbContext.BookingServices.FindAsync(serviceID);
                if (service == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                _dbContext.BookingServices.Remove(service);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa dịch vụ thành công", service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetService(int serviceID)
        {
            _logger.LogInformation("BookingGetService called");
            try
            {
                var service = await _dbContext.BookingServices.FindAsync(serviceID);
                if (service == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                return ErrorConst.Success("Lấy dịch vụ thành công", service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllService()
        {
            _logger.LogInformation("BookingGetAllService called");
            try
            {
                var services = await _dbContext.BookingServices.ToListAsync();
                return ErrorConst.Success("Lấy danh sách dịch vụ thành công", services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
