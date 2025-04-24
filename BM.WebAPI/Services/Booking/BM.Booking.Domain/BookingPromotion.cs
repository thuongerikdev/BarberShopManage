//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BM.Booking.Domain
//{
//    [Table(nameof(BookingPromotion), Schema = BM.Constant.Database.DbSchema.Booking)]
//    public class BookingPromotion
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int promoID { get; set; }
//        [MaxLength(50)]
//        public string promoName { get; set; }
//        [MaxLength(50)]
//        public string promoDescription { get; set; }
//        public double promoDiscount { get; set; }
//        public DateTime promoStart { get; set; }
//        public DateTime promoEnd { get; set; }
//        public string promoStatus { get; set; }
//        //public virtual ICollection<BookingServPro> BookingServPros { get; set; }


//    }
//}
