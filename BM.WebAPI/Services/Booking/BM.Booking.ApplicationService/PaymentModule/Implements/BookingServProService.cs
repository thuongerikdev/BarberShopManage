using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
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

namespace BM.Booking.ApplicationService.PaymentModule.Implements
{
    public class BookingServProService : BookingServiceBase, IBookingServProService
    {
        public BookingServProService(ILogger<BookingServProService> logger, BookingDbContext bookingDbContext) : base(logger, bookingDbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateServPro(BookingCreateServPromoDto bookingCreateServProDto)
        {
            _logger.LogInformation("BookingCreateServPro called");
            try
            {
                var servPro = new BookingServPro
                {
                    servID = bookingCreateServProDto.servID,
                    promoID = bookingCreateServProDto.promoID,
                };
                _dbContext.BookingServPros.Add(servPro);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo service provider thành công", servPro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateServPro(BookingUpdateServPromoDto bookingUpdateServProDto)
        {
            _logger.LogInformation("BookingUpdateServPro called");
            try
            {
                var servPro = await _dbContext.BookingServPros.FindAsync(bookingUpdateServProDto.servProID);
                if (servPro == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service provider");
                }
                servPro.servID = bookingUpdateServProDto.servID;
                servPro.promoID = bookingUpdateServProDto.promoID;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật service provider thành công", servPro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteServPro(int servProID)
        {
            _logger.LogInformation("BookingDeleteServPro called");
            try
            {
                var servPro = await _dbContext.BookingServPros.FindAsync(servProID);
                if (servPro == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service provider");
                }
                _dbContext.BookingServPros.Remove(servPro);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa service provider thành công", servPro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetServPro(int servProID)
        {
            _logger.LogInformation("BookingGetServPro called");
            try
            {
                var servPro = await _dbContext.BookingServPros.FindAsync(servProID);
                if (servPro == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy service provider");
                }
                return ErrorConst.Success("Lấy service provider thành công", servPro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllServPro()
        {
            _logger.LogInformation("BookingGetAllServPro called");
            try
            {
                var servPros = await _dbContext.BookingServPros.ToListAsync();
                return ErrorConst.Success("Lấy danh sách service provider thành công", servPros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
