using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingProduct), Schema = BM.Constant.Database.DbSchema.Booking)]

    public class BookingProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productID { get; set; }

        public string productName { get; set; }
        public string productDescription { get; set; }
        public double productPrice { get; set; }
        public string productStatus { get; set; }
        public string productImage { get; set; }

        public int categoryID { get; set; }
        public virtual BookingCategory BookingCategory { get; set; }
        public virtual ICollection<BookingProductImage> BookingProductImages { get; set; }
        public virtual ICollection<BookingProductDetail> BookingProductDetails { get; set; }

    }

    public class BookingCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int categoryID { get; set; }

        public string categoryName { get; set; }
        public string categoryDescription { get; set; }
        public double categoryPrice { get; set; }
        public string categoryStatus { get; set; }
        public string categoryImage { get; set; }

        public virtual ICollection<BookingProduct> BookingProducts { get; set; }
    }

    //public class BookingProductDescription
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int productDescriptionID { get; set; }

    //    public string productImage { get; set; }
    //    public string productName { get; set; }
    //    public string productDescription { get; set; }
    //    public string productStatus { get; set; }
    //    public string productColor { get; set; }
    //    public string productSize { get; set; }
    //    public string productType { get; set; }
    //    public virtual ICollection< BookingProductDetail> BookingProductDetails { get; set; }
    //}

    public class BookingProductDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productDetailID { get; set; }
        public int productID { get; set; }
        public double productPrice { get; set; }
        public int productQuantity { get; set; }
        public string productStatus { get; set; }
        public string productColor { get; set; }
        public string productSize { get; set; }
        public string productType { get; set; }
        public string productDescription { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; } = DateTime.Now;
        public string productNote { get; set; }

        public virtual BookingProduct BookingProduct { get; set; }
        public virtual ICollection<BookingSupplierProductDetail> BookingSupplierProductDetail { get; set; }

        //public virtual BookingProductDescription BookingProductDescription { get; set; }
        public virtual ICollection<BookingOrderProduct> BookingOrderProducts { get; set; }

    }
    public class BookingProductImage {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productImageID { get; set; }
        public string srcImage { get; set; }
        public int productID { get; set; }
        public virtual BookingProduct BookingProduct { get; set; }
        
    }
    public class BookingSupplierProductDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int supplierProductDetailID { get; set; }
        public int productDetailID { get; set; }
        public int supplierID { get; set; }
        public double productPrice { get; set; }
        public int productQuantity { get; set; }
        public string productStatus { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; } = DateTime.Now;
        public BookingSupplier BookingSupplier { get; set; }
        public BookingProductDetail BookingProductDetail { get; set; }
    }



    public class BookingOrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderProductID { get; set; }

        public int orderID { get; set; }
        public int productDetailID { get; set; }

        public double productPrice { get; set; }
        public int quantity { get; set; }

        public virtual BookingProductDetail BookingProductDetail { get; set; }
        public virtual BookingOrder BookingOrder { get; set; }
    }
    public class BookingSupplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int supplierID { get; set; }

        public string supplierName { get; set; }
        public string supplierEmail { get; set; }
        public string supplierPhone { get; set; }
        public string supplierAddress { get; set; }
        public string supplierStatus { get; set; }
        public string supplierNote { get; set; }
        public string supplierImage { get; set; }

        public virtual ICollection<BookingSupplierProductDetail> BookingSupplierProductDetail { get; set; }
    }


    public class BookingProductDetailCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productDetailCartID { get; set; }
        public int productDetailID { get; set; }
        public int quantity { get; set; }
        public double productPrice { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual BookingProductDetail BookingProductDetail { get; set; }

    }
    public class BookingOrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderStatusID { get; set; }
        public string orderStatusName { get; set; }
        public string orderStatusDescription { get; set; }
       
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; } = DateTime.Now;
        public virtual ICollection<BookingOrder> BookingOrders { get; set; }
    }




}
