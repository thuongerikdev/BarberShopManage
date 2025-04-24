using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDdtos;
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
    public class BookingAppointService : BookingServiceBase, IBookingAppointService
    {
        public BookingAppointService(ILogger<BookingAppointService> logger, BookingDbContext bookingDbContext) : base(logger, bookingDbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateAppoint(List<BookingCreateAppointDto> bookingCreateAppointDto)
        {
            _logger.LogInformation("BookingCreateAppointService called");
            try
            {
                var appoint = bookingCreateAppointDto.Select(a => new BookingAppointment
                {
                    appStatus = a.appStatus,
                    empID = a.empID,
                    serviceDetailID = a.servID,
                    orderID = a.orderID,

                }).ToList();
                _dbContext.BookingAppointments.AddRange(appoint);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo dịch vụ thành công", appoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateAppoint(BookingUpdateAppointDto bookingUpdateAppointDto)
        {
            _logger.LogInformation("BookingUpdateAppointService called");
            try
            {
                var appoint = await _dbContext.BookingAppointments.FindAsync(bookingUpdateAppointDto.appID);
                if (appoint == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                appoint.appStatus = bookingUpdateAppointDto.appStatus;
                appoint.empID = bookingUpdateAppointDto.empID;
                appoint.serviceDetailID = bookingUpdateAppointDto.servID;
                appoint.orderID = bookingUpdateAppointDto.orderID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật dịch vụ thành công", appoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteAppoint(int appID)
        {
            _logger.LogInformation("BookingDeleteAppointService called");
            try
            {
                var appoint = await _dbContext.BookingAppointments.FindAsync(appID);
                if (appoint == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                _dbContext.BookingAppointments.Remove(appoint);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa dịch vụ thành công", appoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAppoint(int appID)
        {
            _logger.LogInformation("BookingGetAppointService called");
            try
            {
                var appoint = await _dbContext.BookingAppointments.FindAsync(appID);
                if (appoint == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy dịch vụ");
                }
                return ErrorConst.Success("Lấy dịch vụ thành công", appoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllAppoint()
        {
            _logger.LogInformation("BookingGetAllAppointService called");
            try
            {
                var appoints = await _dbContext.BookingAppointments.ToListAsync();
                return ErrorConst.Success("Lấy danh sách dịch vụ thành công", appoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAppointByOrderID(int orderID)
        {
            _logger.LogInformation("BookingGetAppointByOrderID called");
            try
            {
                var appoints = await _dbContext.BookingAppointments.Where(a => a.orderID == orderID).ToListAsync();
                return ErrorConst.Success("Lấy danh sách dịch vụ thành công", appoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
