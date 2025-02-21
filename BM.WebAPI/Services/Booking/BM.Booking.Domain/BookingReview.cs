using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingReview), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reviewID { get; set; }
        public int appID { get; set; }
        public int rating { get; set; }
        [MaxLength(50)]
        public string reviewContent { get; set; }
        public DateTime reviewDate { get; set; }
        [MaxLength(50)]
        public string reviewStatus { get; set; }

        public virtual BookingAppointment BookingAppointment { get; set; }



    }
}
