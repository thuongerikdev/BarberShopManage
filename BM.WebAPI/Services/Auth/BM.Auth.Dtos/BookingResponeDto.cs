using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class BookingOrder
    {
      
        public int orderID { get; set; }
        public int custID { get; set; }
        public double orderTotal { get; set; }
        public string orderStatus { get; set; }
        public DateTime createAt { get; set; }
        public DateTime orderDate { get; set; }

    }
    public class CusPromoResponseDto
    {
        public int CustomerID { get; set; }
        public int PromoID { get; set; }
        public string CusPromoStatus { get; set; }
        public int PromoCount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        // Add other relevant properties, but avoid navigation properties
    }
    public class CustomerCheckInResponseDto
    {
        public int CustomerId { get; set; }
        public string CheckInStatus { get; set; }
        public DateTime CheckInDate { get; set; }
        public string CheckInType { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int PointsAdded { get; set; }
        public double TotalLoyaltyPoints { get; set; }
    }
}
