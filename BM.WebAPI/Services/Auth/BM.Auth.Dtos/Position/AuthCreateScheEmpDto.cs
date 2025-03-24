using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreateScheEmpDto
    {
        public int empID { get; set; }
        public int scheduleID { get; set; }
        public string? status { get; set; }
        public string? note { get; set; }
    }
    public class AuthReadScheEmpDto
    {
        public int scheEmpID { get; set; }
        public int empID { get; set; }
        public int scheduleID { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? status { get; set; }
        public string? note { get; set; }
    }
    public class AuthUpdateScheEmpDto
    {
        public int scheEmpID { get; set; }
        public int empID { get; set; }
        public int scheduleID { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? status { get; set; }
        public string? note { get; set; }
    }
}
