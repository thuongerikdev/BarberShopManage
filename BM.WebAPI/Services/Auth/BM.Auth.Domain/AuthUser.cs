using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Domain
{
    [Table(nameof(AuthUser), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
        public int roleID { get; set; }
     
        [MaxLength(50)]
        public string userName { get; set; }
    
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }

        [MaxLength(50)]
        public string email { get; set; }
        [MaxLength(20)]
        public string phoneNumber { get; set; }
        [MaxLength(50)]
        public string fullName { get; set; }
        public string avatar { get; set; }
        public string token { get; set; }

        public bool isEmp { get; set; } 
        public DateTime? dateOfBirth { get; set; }
        [MaxLength(10)]
        public string gender { get; set; }
        public bool isEmailVerified { get; set; } = false;  // New property
        public string emailVerificationToken { get; set; }  // New property for verification token

        public virtual AuthEmp AuthEmp { get; set; }
        public virtual AuthCustomer AuthCustomer { get; set; }
        public virtual AuthRole AuthRoles { get; set; }
    }

    [Table(nameof(AuthEmp), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthEmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int empID { get; set; }
        public int userID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
        public string empCode { get; set; }
        public double salary { get; set; }
        public double bonusSalary { get; set; }
        public double rate { get; set; }
        public string status { get; set; }
        public int branchID { get; set; }
        public DateTime? startDate { get; set; }

        public virtual AuthUser AuthUser { get; set; }
        public virtual AuthPosition AuthPosition { get; set; }
        public virtual AuthSpecialty AuthSpecialty { get; set; }
        public virtual AuthBranches AuthBranches { get; set; }
        public virtual ICollection<AuthScheEmp> AuthScheEmps { get; set; }

    }
    [Table(nameof(AuthCustomer), Schema = BM.Constant.Database.DbSchema.Auth)]
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
        public virtual  AuthVip AuthVip { get; set; }

        public virtual AuthUser AuthUser { get; set; }
        public virtual ICollection<AuthCusPromo> AuthCusPromos { get; set; }
        public virtual ICollection<AuthCustomerCheckIn> AuthCustomerCheckIns { get; set; }


    }
    //[Table(nameof(LoyaltyPoint), Schema = BM.Constant.Database.DbSchema.Auth)]
    //public class LoyaltyPoint
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int loyaltyPointID { get; set; }
    //    public int customerID { get; set; }
    //    public double loyaltyPoints { get; set; }
    //    public DateTime? expiredDate { get; set; }
    //    public string? status { get; set; }
    //    public virtual AuthCustomer AuthCustomer { get; set; }
    //}
    public class EmailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string BaseUrl { get; set; }  // Add this for verification link
    }


    public class AuthVip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int vipID { get; set; }
        public string vipType { get; set; }
        public string vipStatus { get; set; }
        public double vipCost { get; set; }
        public double vipDiscount { get; set; }
        public string vipImage { get; set; }
        public virtual ICollection<AuthCustomer> AuthCustomers { get; set; }

    }

    public class AuthBranches
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int branchID { get; set; }
        public string branchName { get; set; }
        public string branchType { get; set; }
        public string branchStatus { get; set; }
        public double branchArea { get; set; }
        public string branchHotline { get; set; }
        public string startWork { get; set; }
        public string endWork { get; set; }
        public string location { get; set; }
        public string branchImage { get; set; }
        public virtual ICollection <AuthEmp> AuthEmps { get; set; }


    }
    public class AuthCusPromo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cusPromoID { get;  set; }
        public int customerID { get; set; }
        public int promoID { get; set; }
        public string cusPromoStatus { get; set; }
        public int promoCount { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
        public virtual AuthCustomer AuthCustomer { get; set; }
        public virtual AuthPromotion AuthPromotion { get; set; }
    }

    public class AuthPromotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int promoID { get; set; }
        [MaxLength(50)]
        public string promoName { get; set; }
        [MaxLength(50)]
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public int pointToGet { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
        public string promoType { get; set; }
        public string promoImage { get; set; }
        public virtual ICollection<AuthCusPromo> AuthCusPromos { get; set; }

    }
    public class AuthCustomerCheckIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customerCheckInID { get; set; }
        public int customerID { get; set; }
        public DateTime checkInDate { get; set; }
        public string checkInStatus { get; set; }
        public string checkInType { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
        public virtual AuthCustomer AuthCustomer { get; set; }
    }

}
