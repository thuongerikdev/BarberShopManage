using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public DateTime? startDate { get; set; }

        public virtual AuthUser AuthUser { get; set; }
        public virtual AuthPosition AuthPosition { get; set; }
        public virtual AuthSpecialty AuthSpecialty { get; set; }
        public virtual ICollection<AuthScheEmp> AuthScheEmps { get; set; }

    }
    [Table(nameof(AuthCustomer), Schema = BM.Constant.Database.DbSchema.Auth)]
    public class AuthCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int customerID { get; set; }
        public int userID { get; set; }
        public double loyaltyPoints { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
        public double totalSpent { get; set; }
        public double percentDiscount { get; set; }

        public virtual AuthUser AuthUser { get; set; }

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
}
