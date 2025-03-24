using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.Dtos
{
    public class BookingOrder
    {
      
        public int orderID { get; set; }
        public int custID { get; set; }
        public double orderTotal { get; set; }
        public string orderStatus { get; set; }
        public DateTime createAt { get; set; }
        public DateTime orderDate { get; set; }

    }
}
