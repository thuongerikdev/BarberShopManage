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
    public class BookingSupplierService : BookingServiceBase, IBookingSupplierService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingSupplierService(ILogger<BookingSupplierService> logger, BookingDbContext dbContext , ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> BookingCreateSupplier(BookingCreateSupplierDto bookingCreateSupplierDto)
        {
            _logger.LogInformation("BookingCreateSupplier");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingCreateSupplierDto.supplierImage);
                var bookingSupplier = new BookingSupplier
                {
                    supplierAddress = bookingCreateSupplierDto.supplierAddress,
                    supplierName = bookingCreateSupplierDto.supplierName,
                    supplierEmail = bookingCreateSupplierDto.supplierEmail,
                    supplierNote = bookingCreateSupplierDto.supplierNote,
                    supplierPhone = bookingCreateSupplierDto.supplierPhone,
                    supplierStatus = bookingCreateSupplierDto.supplierStatus,
                    supplierImage = image
                };
                _dbContext.BookingSuppliers.Add(bookingSupplier);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao supplier thanh cong", bookingSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateSupplier(BookingUpdateSupplierDto bookingUpdateSupplierDto)
        {
            _logger.LogInformation("BookingUpdateSupplier");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingUpdateSupplierDto.supplierImage);
                var bookingSupplier = await _dbContext.BookingSuppliers.FindAsync(bookingUpdateSupplierDto.supplierID);
                if (bookingSupplier == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier");
                }
                bookingSupplier.supplierAddress = bookingUpdateSupplierDto.supplierAddress;
                bookingSupplier.supplierName = bookingUpdateSupplierDto.supplierName;
                bookingSupplier.supplierEmail = bookingUpdateSupplierDto.supplierEmail;
                bookingSupplier.supplierNote = bookingUpdateSupplierDto.supplierNote;
                bookingSupplier.supplierPhone = bookingUpdateSupplierDto.supplierPhone;
                bookingSupplier.supplierStatus = bookingUpdateSupplierDto.supplierStatus;
                bookingSupplier.supplierImage = image;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat supplier thanh cong", bookingSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteSupplier(int supplierID)
        {
            _logger.LogInformation("BookingDeleteSupplier");
            try
            {
                var bookingSupplier = await _dbContext.BookingSuppliers.FindAsync(supplierID);
                if (bookingSupplier == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier");
                }
                _dbContext.BookingSuppliers.Remove(bookingSupplier);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa supplier thanh cong", bookingSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetSupplier(int supplierID)
        {
            _logger.LogInformation("BookingGetSupplier");
            try
            {
                var bookingSupplier = await _dbContext.BookingSuppliers.FindAsync(supplierID);
                if (bookingSupplier == null)
                {
                    return ErrorConst.Error(500, "khong tim thay supplier");
                }
                return ErrorConst.Success("lay supplier thanh cong", bookingSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
        public async Task<ResponeDto> BookingGetAllSupplier()
        {
            _logger.LogInformation("BookingGetAllSupplier");
            try
            {
                var bookingSuppliers = await _dbContext.BookingSuppliers.ToListAsync();
                return ErrorConst.Success("lay danh sach supplier thanh cong", bookingSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
