using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreatePositionDto
    {
        public string positionName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public double DefaultSalary { get; set; }
    }
    public class AuthReadPositionDto
    {
        public int positionID { get; set; }
        public string positionName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public double DefaultSalary { get; set; }
    }
    public class AuthUpdatePositionDto
    {
        public int positionID { get; set; }
        public string positionName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public double DefaultSalary { get; set; }
    }

}
