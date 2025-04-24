using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BM.Constant.Dto
{
    public class BookingGetCustAndEmpByOrderDto
    {
        public int custID { get; set; }
        public double totalMoney { get; set; }
        public List <BookingEmpMoney> empMoney { get; set; }
    }
    public class BookingEmpMoney
    { 
        public int empID { get; set; }
        public double money { get; set; }
        //public int rating { get; set; }
      
    }
    public class CustomerPromotionDto
    {
        public CustomerDto CustomerData { get; set; } // Renamed from Customer
        public PromotionDto PromotionData { get; set; } // Renamed from Promotion
    }

    public class CustomerDto
    {
        public int CustomerID { get; set; }
        public int UserID { get; set; }
        public int VipID { get; set; }
        public double LoyaltyPoints { get; set; }
        public string CustomerType { get; set; }
        public string CustomerStatus { get; set; }
        public double TotalSpent { get; set; }
        public double PercentDiscount { get; set; }
    }

    public class PromotionDto
    {
        public int PromoID { get; set; }
        public string PromoName { get; set; }
        public string PromoDescription { get; set; }
        public double PromoDiscount { get; set; }
        public DateTime PromoStart { get; set; }
        public DateTime PromoEnd { get; set; }
        public string PromoStatus { get; set; }
    }
}
