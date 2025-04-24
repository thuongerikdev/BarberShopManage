using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Abtracts
{
    public interface IBookingProductService
    {
        public Task<ResponeDto> BookingCreateProduct(BookingCreateProductDto bookingCreateProductDto);
        public Task<ResponeDto> BookingUpdateProduct(BookingUpdateProductDto bookingUpdateProductDto);
        public Task<ResponeDto> BookingDeleteProduct(int productID);
        public Task<ResponeDto> BookingGetProduct(int productID);
        public Task<ResponeDto> BookingGetAllProduct();
    }
    public interface IBookingCategoryService
    {
        public Task<ResponeDto> BookingCreateCategory(BookingCreateCategoryDto bookingCreateCategoryDto);
        public Task<ResponeDto> BookingUpdateCategory(BookingUpdateCategoryDto bookingUpdateCategoryDto);
        public Task<ResponeDto> BookingDeleteCategory(int categoryID);
        public Task<ResponeDto> BookingGetCategory(int categoryID);
        public Task<ResponeDto> BookingGetAllCategory();
    }
    public interface IBookingProductDescriptionService
    {
        public Task<ResponeDto> BookingCreateProductDescription(BookingCreateProductDescriptionDto bookingCreateProductDescriptionDto);
        public Task<ResponeDto> BookingUpdateProductDescription(BookingUpdateProductDescriptionDto bookingUpdateProductDescriptionDto);
        public Task<ResponeDto> BookingDeleteProductDescription(int productDescriptionID);
        public Task<ResponeDto> BookingGetProductDescription(int productDescriptionID);
        public Task<ResponeDto> BookingGetAllProductDescription();
    }
    public interface IBookingProductDetailService
    {
        public Task<ResponeDto> BookingCreateProductDetail(BookingCreateProductDetailDto bookingCreateProductDetailDto);
        public Task<ResponeDto> BookingUpdateProductDetail(BookingUpdateProductDetailDto bookingUpdateProductDetailDto);
        public Task<ResponeDto> BookingDeleteProductDetail(int productDetailID);
        public Task<ResponeDto> BookingGetProductDetail(int productDetailID);
        public Task<ResponeDto> BookingGetAllProductDetail();
    }
    public interface IBookingOrderProductService
    {
        public Task<ResponeDto> BookingCreateOrderProduct(BookingCreateOrderProductDto bookingCreateOrderProductDto);
        public Task<ResponeDto> BookingUpdateOrderProduct(BookingUpdateOrderProductDto bookingUpdateOrderProductDto);
        public Task<ResponeDto> BookingDeleteOrderProduct(int orderProductID);
        public Task<ResponeDto> BookingGetOrderProduct(int orderProductID);
        public Task<ResponeDto> BookingGetAllOrderProduct();
    }
    public interface IBookingSupplierService
    {
        public Task<ResponeDto> BookingCreateSupplier(BookingCreateSupplierDto bookingCreateSupplierDto);
        public Task<ResponeDto> BookingUpdateSupplier(BookingUpdateSupplierDto bookingUpdateSupplierDto);
        public Task<ResponeDto> BookingDeleteSupplier(int supplierID);
        public Task<ResponeDto> BookingGetSupplier(int supplierID);
        public Task<ResponeDto> BookingGetAllSupplier();
    }
    
}
