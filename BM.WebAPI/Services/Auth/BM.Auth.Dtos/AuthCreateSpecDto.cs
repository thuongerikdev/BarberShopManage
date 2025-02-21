using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreateSpecDto
    {
        public string specialtyName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public int percent { get; set; }
    }
    public class AuthReadSpecDto
    {
        public int specialtyID { get; set; }
        public string specialtyName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public int percent { get; set; }
    }
    public class AuthUpdateSpecDto
    {
        public int specialtyID { get; set; }
        public string specialtyName { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public int percent { get; set; }
    }
}
