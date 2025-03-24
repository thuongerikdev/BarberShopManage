using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingOrder), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderID { get; set; }
        public int custID { get; set; }
        public double orderTotal { get; set; }
        [MaxLength(50)]
        public string orderStatus { get; set; }
        public DateTime createAt { get; set; }
        public DateTime orderDate { get; set; }

        public virtual BookingInvoice BookingInvoice { get; set; }
        public virtual ICollection< BookingAppointment> BookingAppointments { get; set; }
   

    }
}
