using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos
{
    public class BookingCreateServPromoDto
    {
        public int servID { get; set; }
        public int promoID { get; set; }
    }
    public class BookingUpdateServPromoDto
    {
        public int servProID { get; set; }
        public int servID { get; set; }
        public int promoID { get; set; }
    }
    public class BookingReadServPromoDto
    {
        public int servProID { get; set; }
        public int servID { get; set; }
        public int promoID { get; set; }
    }
}
