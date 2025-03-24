using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.BussinessDtos
{
    public class AuthUser
    {
        public int userID { get; set; }
        public int roleID { get; set; }
         
        public string userName { get; set; }
         
        public string password { get; set; }
         
        public string email { get; set; }
   
        public string phoneNumber { get; set; }
         
        public string fullName { get; set; }
        public string avatar { get; set; }
        public string token { get; set; }

        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public bool isEmailVerified { get; set; } = false;  // New property
        public string emailVerificationToken { get; set; }  // New property for verification token

    }
    public class AuthCustomer
    {
      
        public int customerID { get; set; }
        public int userID { get; set; }
        public int loyaltyPoints { get; set; }
        public string? customerType { get; set; }
        public string? customerStatus { get; set; }
        public virtual AuthUser AuthUser { get; set; }

    }
}
