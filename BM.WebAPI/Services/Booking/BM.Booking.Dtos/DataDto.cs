using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos
{
    public class BookingGetServiceDetailAndDescriptions
    {
        public int serviceDetailID { get; set; }
        public int servID { get; set; }
        public double servPrice { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public string servStatus { get; set; }

        public int serviceDetailDescriptionID { get; set; }
        public string servImage { get; set; }
        public string servDesName { get; set; }
        public string servDesDescription { get; set; }
        public string servType { get; set; }



    }
}
