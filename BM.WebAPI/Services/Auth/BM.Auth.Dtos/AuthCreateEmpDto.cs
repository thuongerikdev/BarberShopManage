using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreateEmpDto
    {
        public int userID { get; set; }
        public int positionID { get; set; }
        public int specialtyID { get; set; }
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

    }

}
