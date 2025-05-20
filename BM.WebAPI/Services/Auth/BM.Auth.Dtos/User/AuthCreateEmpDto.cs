using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos.User
{
    public class AuthCreateEmpDto
    {
        public int userID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
      
        public int branchID { get; set; }
        public DateTime? startDate { get; set; }

    }
    public class AuthReadEmpDto
    {
        public int empID { get; set; }
        public int userID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
        public string? empCode { get; set; }
        public double salary { get; set; }
        public double rate { get; set; }
        public DateTime? startDate { get; set; }
    }
    public class AuthUpdateEmpDto
    {
        public int empID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
        public int branchID { get; set; }
        //public double bonusSalary { get; set; }

    }

    public class AuthEmpDto
    {
        public int empID { get; set; }
        public string empCode { get; set; }
        public int positionID { get; set; }
        public string positionName { get; set; } // Chỉ lấy thông tin cần thiết từ AuthPosition
        public int specialtyID { get; set; }
        public double salary { get; set; }
        public DateTime? startDate { get; set; }
        public int userID { get; set; }
        public double bonusSalary { get; set; }
        public string status { get; set; }
        public int branchID { get; set; }
    }

    public class AuthGetEmpDto
    {
        public int empID { get; set; }
        public string empCode { get; set; }
        public int positionID { get; set; }
        public string positionName { get; set; } // Chỉ lấy thông tin cần thiết từ AuthPosition
        public int specialtyID { get; set; }
        public string specialtyName { get; set; } // Chỉ lấy thông tin cần thiết từ AuthSpecialty
        public double salary { get; set; }
        public DateTime? startDate { get; set; }
        //public List<AuthGetScheEmpDto> schedule { get; set; } // Chỉ lấy thông tin cần thiết từ AuthSchedule
        public int userID { get; set; }
        public double bonusSalary { get; set; }
        public string status { get; set; }
        public int branchID { get; set; }
        public string image { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

        

        //public string address { get; set; }
        public string fullName { get; set; }
        public string gender { get; set; }
        public DateTime? dateOfBirth { get; set; }

        public string branchName { get; set; } // Chỉ lấy thông tin cần thiết từ AuthBranch
        public string branchesImage { get; set; } // Chỉ lấy thông tin cần thiết từ AuthBranch

    }
    //public class AuthGetScheEmpDto
    //{
    //    public int empID { get; set; }
    //    public int scheduleID { get; set; }
    //    public string? status { get; set; }
    //    public string? note { get; set; }
    //    public DateTime? startDate { get; set; }
    //    public DateTime? endDate { get; set; }
    //}

}
