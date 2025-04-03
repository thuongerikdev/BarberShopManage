using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthLoginDto
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    
    public class AuthReadUserDto
    {
        public int userID { get; set; }
        public int roleID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fullName { get; set; }
        public string avatar { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
    }
    public class AuthRegisterDto
    {
        public int roleID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fullName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        //public bool isEmailVerified { get; set; } = false;  // New property
        //public string emailVerificationToken { get; set; }  // New property for verification token

    }
    public class AuthRegisteEmprDto
    {
        public int roleID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fullName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
        public int branchID { get; set; }

        public DateTime? startDate { get; set; }
      

    }
    public class AuthUpdateUserDto
    {
        public int userID { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fullName { get; set; }
        public string avatar { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public bool isEmailVerified { get; set; } = false;  // New property
        public string emailVerificationToken { get; set; }  // New property for verification token
    }
    public class AuthUpdateAvatarDto
    {
        public int userID { get; set; }
        public IFormFile file { get; set; }
    }
    
}
