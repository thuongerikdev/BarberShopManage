using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDDtos
{
    public class BookingCreateInvoiceDto
    {
        public int orderID { get; set; } // Khóa ngoại liên kết với đơn hàng
        public DateTime invoiceDate { get; set; } // Ngày lập hóa đơn
        public double totalAmount { get; set; } // Tổng số tiền phải thanh toán
        public string paymentTerms { get; set; } // Điều khoản thanh toán
        public string status { get; set; } // Trạng thái hóa đơn
        public string paymentMethod { get; set; }
    }
    public class BookingReadInvoiceDto
    {
        public int invoiceID { get; set; } // Mã hóa đơn duy nhất
        public int orderID { get; set; } // Khóa ngoại liên kết với đơn hàng
        public DateTime invoiceDate { get; set; } // Ngày lập hóa đơn
        public double totalAmount { get; set; } // Tổng số tiền phải thanh toán
        public string paymentTerms { get; set; } // Điều khoản thanh toán
        public string status { get; set; } // Trạng thái hóa đơn
        public string paymentMethod { get; set; }
    }
    public class BookingUpdateInvoiceDto {
        public int invoiceID { get; set; } // Mã hóa đơn duy nhất
        public int orderID { get; set; } // Khóa ngoại liên kết với đơn hàng
        public DateTime invoiceDate { get; set; } // Ngày lập hóa đơn
        public double totalAmount { get; set; } // Tổng số tiền phải thanh toán
        public string paymentTerms { get; set; } // Điều khoản thanh toán
        public string status { get; set; } // Trạng thái hóa đơn
        public string paymentMethod { get; set; }
    }
}
