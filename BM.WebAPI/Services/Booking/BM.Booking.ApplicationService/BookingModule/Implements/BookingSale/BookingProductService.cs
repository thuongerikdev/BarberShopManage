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
    public class BookingProductService : BookingServiceBase, IBookingProductService
    {
        private readonly ICloudinaryService _cloudinaryService;
        public BookingProductService(ILogger<BookingProductService> logger, ICloudinaryService cloudinaryService, BookingDbContext dbContext) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;

        }
        public async Task<ResponeDto> BookingCreateProduct(BookingCreateProductDto bookingCreateProductDto)
        {
            _logger.LogInformation("BookingCreateProduct");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingCreateProductDto.productImage);
                var bookingProduct = new BookingProduct
                {
                    productName = bookingCreateProductDto.productName,
                    productDescription = bookingCreateProductDto.productDescription,
                    productPrice = bookingCreateProductDto.productPrice,
                    productStatus = bookingCreateProductDto.productStatus,
                    productImage = image,
                    categoryID = bookingCreateProductDto.categoryID
                };
                _dbContext.BookingProducts.Add(bookingProduct);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("tao product thanh cong", bookingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateProduct(BookingUpdateProductDto bookingUpdateProductDto)
        {
            _logger.LogInformation("BookingUpdateProduct");
            try
            {
                var image = await _cloudinaryService.UploadImageAsync(bookingUpdateProductDto.productImage);
                var bookingProduct = await _dbContext.BookingProducts.FindAsync(bookingUpdateProductDto.productID);
                if (bookingProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product");
                }
                bookingProduct.productName = bookingUpdateProductDto.productName;
                bookingProduct.productDescription = bookingUpdateProductDto.productDescription;
                bookingProduct.productPrice = bookingUpdateProductDto.productPrice;
                bookingProduct.productStatus = bookingUpdateProductDto.productStatus;
                bookingProduct.productImage = image;
                bookingProduct.categoryID = bookingUpdateProductDto.categoryID;

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("cap nhat product thanh cong", bookingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteProduct(int productID)
        {
            _logger.LogInformation("BookingDeleteProduct");
            try
            {
                var bookingProduct = await _dbContext.BookingProducts.FindAsync(productID);
                if (bookingProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product");
                }
                _dbContext.BookingProducts.Remove(bookingProduct);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("xoa product thanh cong", bookingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetProduct(int productID)
        {
            _logger.LogInformation("BookingGetProduct");
            try
            {
                var bookingProduct = await _dbContext.BookingProducts.FindAsync(productID);
                if (bookingProduct == null)
                {
                    return ErrorConst.Error(500, "khong tim thay product");
                }
                return ErrorConst.Success("lay product thanh cong", bookingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetAllProduct()
        {
            _logger.LogInformation("BookingGetAllProduct");
            try
            {
                var bookingProducts = await _dbContext.BookingProducts.ToListAsync();
                return ErrorConst.Success("lay danh sach product thanh cong", bookingProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
    }
}
