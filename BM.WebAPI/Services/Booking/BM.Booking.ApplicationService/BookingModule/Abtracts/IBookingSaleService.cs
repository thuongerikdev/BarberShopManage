﻿using BM.Booking.Dtos.CRUDDtos.BM.Booking.Application.DTOs;
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
        public Task<ResponeDto> BookingGetProductByCategory(int categoryID);
    }
    public interface IBookingCategoryService
    {
        public Task<ResponeDto> BookingCreateCategory(BookingCreateCategoryDto bookingCreateCategoryDto);
        public Task<ResponeDto> BookingUpdateCategory(BookingUpdateCategoryDto bookingUpdateCategoryDto);
        public Task<ResponeDto> BookingDeleteCategory(int categoryID);
        public Task<ResponeDto> BookingGetCategory(int categoryID);
        public Task<ResponeDto> BookingGetAllCategory();
    }
    //public interface IBookingProductDescriptionService
    //{
    //    public Task<ResponeDto> BookingCreateProductDescription(BookingCreateProductDescriptionDto bookingCreateProductDescriptionDto);
    //    public Task<ResponeDto> BookingUpdateProductDescription(BookingUpdateProductDescriptionDto bookingUpdateProductDescriptionDto);
    //    public Task<ResponeDto> BookingDeleteProductDescription(int productDescriptionID);
    //    public Task<ResponeDto> BookingGetProductDescription(int productDescriptionID);
    //    public Task<ResponeDto> BookingGetAllProductDescription();
    //}
    public interface IBookingProductImageService
    {
        public Task<ResponeDto> BookingCreateProductImage(BookingCreateProductImageDto bookingCreateProductInageDto);
        public Task<ResponeDto> BookingUpdateProductImage(BookingUpdateProductImageDto bookingUpdateProductImageDto);
        public Task<ResponeDto> BookingGetProductImage(int productImageID);
        public Task<ResponeDto> BookingDeleteProductImage(int productImageID);
        public Task<ResponeDto> BookingGetAllProductImage();
    }

    public interface IBookingProductDetailService
    {
        public Task<ResponeDto> BookingCreateProductDetail(BookingCreateProductDetailDto bookingCreateProductDetailDto);
        public Task<ResponeDto> BookingUpdateProductDetail(BookingUpdateProductDetailDto bookingUpdateProductDetailDto);
        public Task<ResponeDto> BookingDeleteProductDetail(int productDetailID);
        public Task<ResponeDto> BookingGetProductDetail(int productDetailID);
        public Task<ResponeDto> BookingGetAllProductDetail();
        public Task<ResponeDto> BookingGetProductDetailByProductID(int productID);
    }
    public interface IBookingOrderProductService
    {
        public Task<ResponeDto> BookingCreateOrderProduct(List<BookingCreateOrderProductDto> bookingCreateOrderProductDto);
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
    public interface IBookingSupplierProductDetailService
    {
        public Task<ResponeDto> BookingCreateSupplierProductDetail(BookingCreateSupplierProductDetailDto bookingCreateSupplierProductDto);
        public Task<ResponeDto> BookingUpdateSupplierProductDetail(BookingUpdateSupplierProductDetailDto bookingUpdateSupplierProductDto);
        public Task<ResponeDto> BookingDeleteSupplierProductDetail(int supplierProductID);
        public Task<ResponeDto> BookingGetSupplierProductDetail(int supplierProductID);
        public Task<ResponeDto> BookingGetAllSupplierProductDetail();
    }
}
