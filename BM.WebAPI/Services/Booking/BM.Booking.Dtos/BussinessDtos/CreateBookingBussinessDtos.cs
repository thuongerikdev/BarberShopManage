using BM.Booking.Dtos.CRUDdtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.BussinessDtos
{
    public class CreateBookingBussinessDtos
    {
        public int appID { get; set; }
        public int servID { get; set; }
        public int empID { get; set; }
        public int orderID { get; set; }
        public string appStatus { get; set; }
    }

    public class BookingCreateBussinessAppointDto
    {
        //public int appID { get; set; }
        public int servID { get; set; }
        public int empID { get; set; }
        public string appStatus { get; set; }
    }

    public class BookingCreateOrderBussinessDto
    {
        public int custID { get; set; }
        public DateTime createAt { get; set; }
        public DateTime orderDate { get; set; }
    }

    public class PaymentInformationModel
    {
        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
    }

    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }

    public class BookingCreateInvoiceBussinessDto
    {
        public int orderID { get; set; } // Khóa ngoại liên kết với đơn hàng
        public DateTime invoiceDate { get; set; } // Ngày lập hóa đơn
        public decimal totalAmount { get; set; } // Tổng số tiền phải thanh toán
        public string paymentTerms { get; set; } // Điều khoản thanh toán
        public string status { get; set; } // Trạng thái hóa đơn
    }

    public class BookingCreateOrderRequestDto
    {
        public List<BookingCreateBussinessAppointDto>? Appoint { get; set; }
        public BookingCreateOrderBussinessDto? Order { get; set; }
        public int PromoID { get; set; }
    }
}