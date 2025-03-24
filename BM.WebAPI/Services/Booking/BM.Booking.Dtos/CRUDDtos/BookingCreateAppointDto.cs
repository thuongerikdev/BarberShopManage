using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDdtos
{
    public class BookingCreateAppointDto
    {
        public int servID { get; set; }
        public int empID { get; set; }
        public int orderID { get; set; }
        public string appStatus { get; set; }
    }
    //public class BookingCreateBussinessAppointDto
    //{
    //    public int servID { get; set; }
    //    public int empID { get; set; }
    //    public int orderID { get; set; }
    //    public string appStatus { get; set; }
    //}

    public class BookingUpdateAppointDto
    {
        public int appID { get; set; }
        public int servID { get; set; }
        public int empID { get; set; }
        public int orderID { get; set; }
        public string appStatus { get; set; }
    }
    public class BookingReadAppointDto
    {
        public int appID { get; set; }
        public int servID { get; set; }
        public int empID { get; set; }
        public int orderID { get; set; }
        public string appStatus { get; set; }
    }
}
