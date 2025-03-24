using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDdtos
{
    public class BookingCreateServiceDto
    {
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public string servImage { get; set; }
    }
    public class BookingUpdateServiceDto
    {
        public int servID { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public string servImage { get; set; }
    }
    public class BookingReadServiceDto
    {
        public int servID { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public string servImage { get; set; }
    }
}
