using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Constant.Dto
{
    public class AuthUser
    {
        public int userID { get; set; }
        public int roleID { get; set; }
        public string userName { get; set; }
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fullName { get; set; }
        public string avatar { get; set; }
        public string token { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public bool isEmailVerified { get; set; } = false;  // New property
        public string emailVerificationToken { get; set; }  // New property for verification toke
     
    }


    public class AuthCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customerID { get; set; }
        public int userID { get; set; }
        public int vipID { get; set; }
        public double loyaltyPoints { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
        public double totalSpent { get; set; }
        public double percentDiscount { get; set; }
     
    }

    public class AuthCusPromo
    {
       
        public int cusPromoID { get; set; }
        public int customerID { get; set; }
        public int promoID { get; set; }
        public string cusPromoStatus { get; set; }
    
    }

    public class AuthPromotion
    {
        public int promoID { get; set; }
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
    }
    public class CustomerPromotionDtos
    {
        public AuthCustomer Customer { get; set; }
        public AuthPromotion Promotion { get; set; }
    }
}
