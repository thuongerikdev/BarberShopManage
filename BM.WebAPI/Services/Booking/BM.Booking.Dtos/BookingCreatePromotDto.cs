using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos
{
    public class BookingCreatePromotDto
    {
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
    }
    public class BookingUpdatePromotDto
    {
        public int promoID { get; set; }
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
    }
    public class BookingReadPromotDto
    {
        public int promoID { get; set; }
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
    }
}
