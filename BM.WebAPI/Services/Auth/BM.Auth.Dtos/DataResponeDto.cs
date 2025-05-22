using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
    public class CuspromoDataGet
    {
        public int cusPromoID { get; set; }
        public int customerID { get; set; }
        public int promoID { get; set; }
        public string cusPromoStatus { get; set; }
        public int promoCount { get; set; }
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public int pointToGet { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
        public string promoType { get; set; }
        public string promoImage { get; set; }
        public int userID { get; set; }
        public int vipID { get; set; }
        public double loyaltyPoints { get; set; }

    }
    public class PromoWithOwnershipDto
    {
        public int PromoID { get; set; }
        public string PromoName { get; set; }
        public string PromoDescription { get; set; }
        public double PromoDiscount { get; set; }
        public int PointToGet { get; set; }
        public DateTime PromoStart { get; set; }
        public DateTime PromoEnd { get; set; }
        public string PromoStatus { get; set; }
        public string PromoType { get; set; }
        public string PromoImage { get; set; }
        public bool IsAssociatedWithCustomer { get; set; }
        public List<AuthCusPromoDto> CustomerPromos { get; set; }
    }

    // DTO for AuthCusPromo to avoid exposing navigation properties
    public class AuthCusPromoDto
    {
        public int CusPromoID { get; set; }
        public int CustomerID { get; set; }
        public int PromoID { get; set; }
        public string CusPromoStatus { get; set; }
        public int PromoCount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class CustomerVipResponseDto
    {
        public int CustomerID { get; set; }
        public double TotalSpent { get; set; }
        public double LoyaltyPoints { get; set; }
        public int VipID { get; set; }
        public string VipType { get; set; }
        // Include other relevant properties, excluding navigation properties like AuthCustomers
    }
    public class UpdateVipImageDto
    {
        public int VipID { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
    public class UpdateBasicUserInfoDto
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string userPhone { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; }
        public string fullName { get; set; }

    }
   
}
