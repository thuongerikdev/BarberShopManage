using BM.Booking.ApplicationService.Common;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
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

namespace BM.Booking.ApplicationService.PaymentModule.Implements
{
    public class BookingPromoService : BookingServiceBase, IBookingPromoService
    {
        public BookingPromoService(ILogger<BookingPromoService> logger, BookingDbContext dbContext) : base(logger, dbContext)
        {
        }
        public async Task<ResponeDto> BookingCreatePromo(BookingCreatePromotDto bookingCreatePromoDto)
        {
            _logger.LogInformation("BookingCreatePromo");
            try
            {
                var promo = new BookingPromotion
                {
                    promoName = bookingCreatePromoDto.promoName,
                    promoDescription = bookingCreatePromoDto.promoDescription,
                    promoDiscount = bookingCreatePromoDto.promoDiscount,
                    promoStatus = bookingCreatePromoDto.promoStatus,
                    promoStart = bookingCreatePromoDto.promoStart,
                    promoEnd = bookingCreatePromoDto.promoEnd
                };
                _dbContext.BookingPromotions.Add(promo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo promo thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdatePromo(BookingUpdatePromotDto bookingUpdatePromoDto)
        {
            _logger.LogInformation("BookingUpdatePromo");
            try
            {
                var promo = await _dbContext.BookingPromotions.FindAsync(bookingUpdatePromoDto.promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy promo");
                }
                promo.promoName = bookingUpdatePromoDto.promoName;
                promo.promoDescription = bookingUpdatePromoDto.promoDescription;
                promo.promoDiscount = bookingUpdatePromoDto.promoDiscount;
                promo.promoStatus = bookingUpdatePromoDto.promoStatus;
                promo.promoStart = bookingUpdatePromoDto.promoStart;
                promo.promoEnd = bookingUpdatePromoDto.promoEnd;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật promo thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeletePromo(int promoID)
        {
            _logger.LogInformation("BookingDeletePromo");
            try
            {
                var promo = await _dbContext.BookingPromotions.FindAsync(promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy promo");
                }
                _dbContext.BookingPromotions.Remove(promo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa promo thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetPromo(int promoID)
        {
            _logger.LogInformation("BookingGetPromo");
            try
            {
                var promo = await _dbContext.BookingPromotions.FindAsync(promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy promo");
                }
                return ErrorConst.Success("Lấy promo thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllPromo()
        {
            _logger.LogInformation("BookingGetAllPromo");
            try
            {
                var promos = await _dbContext.BookingPromotions.ToListAsync();
                return ErrorConst.Success("Lấy danh sách promo thành công", promos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
