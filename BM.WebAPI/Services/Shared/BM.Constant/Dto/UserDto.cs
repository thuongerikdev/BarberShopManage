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
}
