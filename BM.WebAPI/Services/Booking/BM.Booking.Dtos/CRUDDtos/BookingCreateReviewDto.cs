using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDdtos
{
    public class BookingCreateReviewDto
    {
        public int orderID { get; set; }
        public int rating { get; set; }
        public string reviewContent { get; set; }
        public DateTime reviewDate { get; set; }
        public string reviewStatus { get; set; }
    }
    public class BookingUpdateReviewDto
    {
        public int reviewID { get; set; }
        public int orderID { get; set; }
        public int rating { get; set; }
        public string reviewContent { get; set; }
        public DateTime reviewDate { get; set; }
        public string reviewStatus { get; set; }
    }
    public class BookingReadReviewDto
    {
        public int reviewID { get; set; }
        public int orderID { get; set; }
        public int rating { get; set; }
        public string reviewContent { get; set; }
        public DateTime reviewDate { get; set; }
        public string reviewStatus { get; set; }
    }
}
