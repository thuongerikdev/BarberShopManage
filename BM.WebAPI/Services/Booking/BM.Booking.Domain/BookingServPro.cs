using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingServPro), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingServPro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int servProID { get; set; }
        public int servID { get; set; }
        public int promoID { get; set; }
        public virtual BookingService BookingService { get; set; }
        public virtual BookingPromotion BookingPromotion { get; set; }
    }
}
