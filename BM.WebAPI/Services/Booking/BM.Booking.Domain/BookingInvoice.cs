using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingInvoice), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int invoiceID { get; set; } // Mã hóa đơn duy nhất
        public int orderID { get; set; } // Khóa ngoại liên kết với đơn hàng
        public DateTime invoiceDate { get; set; } // Ngày lập hóa đơn
        public double totalAmount { get; set; } // Tổng số tiền phải thanh toán
        public string paymentTerms { get; set; } // Điều khoản thanh toán
        public string status { get; set; } // Trạng thái hóa đơn
        public string paymentMethod { get; set; } // Phương thức thanh toán
        public virtual BookingOrder BookingOrder { get; set; } // Liên kết với đơn hàng
    }
}
