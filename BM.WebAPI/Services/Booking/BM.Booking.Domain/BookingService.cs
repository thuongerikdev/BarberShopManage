﻿using BM.Constant.Database;
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

        public virtual ICollection<BookingServPro> BookingServPros { get; set; }
        public virtual ICollection<BookingAppointment> BookingAppointments { get; set; }

    }
}
