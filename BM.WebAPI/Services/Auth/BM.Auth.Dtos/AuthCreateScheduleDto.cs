using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class AuthCreateScheduleDto
    {
        public string? scheduleName { get; set; }
        public string? description { get; set; }
        public string? note { get; set; }
        public int percent { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
    public class AuthReadScheduleDto
    {
        public int scheduleID { get; set; }
        public string? scheduleName { get; set; }
        public string? description { get; set; }
        public string? note { get; set; }
        public int percent { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
    public class AuthUpdateScheduleDto
    {
        public int scheduleID { get; set; }
        public string? scheduleName { get; set; }
        public string? description { get; set; }
        public string? note { get; set; }
        public int percent { get; set; }
        public string? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
