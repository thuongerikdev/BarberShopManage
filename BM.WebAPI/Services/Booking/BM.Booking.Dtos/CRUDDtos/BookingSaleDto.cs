using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDDtos
{
    namespace BM.Booking.Application.DTOs
    {
        // -------------------- BookingProduct DTOs --------------------
        public class BookingCreateProductDto
        {
            public string productName { get; set; }
            public string productDescription { get; set; }
            public double productPrice { get; set; }
            public string productStatus { get; set; }
            public IFormFile productImage { get; set; }
            public int categoryID { get; set; }
        }

        public class BookingUpdateProductDto : BookingCreateProductDto
        {
            public int productID { get; set; }
        }

        public class BookingReadProductDto : BookingUpdateProductDto
        {
            public string categoryName { get; set; }
        }

        // -------------------- BookingCategory DTOs --------------------
        public class BookingCreateCategoryDto
        {
            public string categoryName { get; set; }
            public string categoryDescription { get; set; }
            public double categoryPrice { get; set; }
            public string categoryStatus { get; set; }
            public IFormFile categoryImage { get; set; }
        }

        public class BookingUpdateCategoryDto : BookingCreateCategoryDto
        {
            public int categoryID { get; set; }
        }

        public class BookingReadCategoryDto : BookingUpdateCategoryDto
        {
        }

        // -------------------- BookingProductDescription DTOs --------------------
        public class BookingCreateProductDescriptionDto
        {
            public IFormFile productImage { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public string productStatus { get; set; }
            public string productColor { get; set; }
            public string productSize { get; set; }
            public string productType { get; set; }
        }

        public class BookingUpdateProductDescriptionDto : BookingCreateProductDescriptionDto
        {
            public int productDescriptionID { get; set; }
        }

        public class BookingReadProductDescriptionDto : BookingUpdateProductDescriptionDto
        {
        }

        // -------------------- BookingProductDetail DTOs --------------------
        public class BookingCreateProductDetailDto
        {
            public int productID { get; set; }
            public int productDescriptionID { get; set; }
            public int supplierID { get; set; }
            public double productPrice { get; set; }
            public int productQuantity { get; set; }
        }

        public class BookingUpdateProductDetailDto : BookingCreateProductDetailDto
        {
            public int productDetailID { get; set; }
        }

        public class BookingReadProductDetailDto : BookingUpdateProductDetailDto
        {
            public string productName { get; set; }
            public string supplierName { get; set; }
        }

        // -------------------- BookingOrderProduct DTOs --------------------
        public class BookingCreateOrderProductDto
        {
            public int orderID { get; set; }
            public int productDetailID { get; set; }
            public double productPrice { get; set; }
            public int quantity { get; set; }
        }

        public class BookingUpdateOrderProductDto : BookingCreateOrderProductDto
        {
            public int orderProductID { get; set; }
        }

        public class BookingReadOrderProductDto : BookingUpdateOrderProductDto
        {
            public string productName { get; set; }
        }

        // -------------------- BookingSupplier DTOs --------------------
        public class BookingCreateSupplierDto
        {
            public string supplierName { get; set; }
            public string supplierEmail { get; set; }
            public string supplierPhone { get; set; }
            public string supplierAddress { get; set; }
            public string supplierStatus { get; set; }
            public string supplierNote { get; set; }
            public IFormFile supplierImage {  get; set; }  
        }

        public class BookingUpdateSupplierDto : BookingCreateSupplierDto
        {
            public int supplierID { get; set; }
        }

        public class BookingReadSupplierDto : BookingUpdateSupplierDto
        {
        }
    }

}
