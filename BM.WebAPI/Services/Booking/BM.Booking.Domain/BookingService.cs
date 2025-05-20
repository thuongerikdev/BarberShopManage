using BM.Constant.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Domain
{
    [Table(nameof(BookingService), Schema = BM.Constant.Database.DbSchema.Booking)]
    public class BookingService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int servID { get; set; }
        [MaxLength(50)]
        public string servName { get; set; }
        [MaxLength(50)]
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        [MaxLength(50)]
        public string servStatus { get; set; }
        public string servImage { get; set; }

        //public virtual ICollection<BookingServPro> BookingServPros { get; set; }
        public virtual ICollection<BookingServiceDetail> BookingServiceDetails { get; set; }


    }
    public class BookingServiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceDetailID { get; set; }
        public int servID { get; set; }
        public double servPrice { get; set; }
        public string servImage { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public string servStatus { get; set; }

        public virtual BookingService BookingService { get; set; }
        public virtual ICollection<BookingAppointment> BookingAppointments { get; set; }
        public virtual ICollection<BookingServiceDetailDescription> BookingServiceDetailDescriptions { get; set; }


    }
    public class  BookingServiceDetailDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceDetailDescriptionID { get; set; }
        public int serviceDetailID { get; set; }
        public string servImage { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public string servStatus { get; set; }
        public string servType { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
        public virtual BookingServiceDetail BookingServiceDetails { get; set; }

    }
}
