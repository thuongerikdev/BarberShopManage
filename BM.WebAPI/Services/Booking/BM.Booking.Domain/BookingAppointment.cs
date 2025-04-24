using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingAppointment), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int appID { get; set; }
        public int serviceDetailID { get; set; }
        public int empID { get; set; }
        public int orderID { get; set; }
        [MaxLength(50)]
        public string appStatus { get; set; }

        public virtual BookingServiceDetail BookingServiceDetails { get; set; } //ok
        //public virtual BookingReview BookingReviews { get; set; } //ok
        public virtual BookingOrder BookingOrder { get; set; } //ok

        //public virtual BookingEmployee BookingEmployee { get; set; }



    }
}
