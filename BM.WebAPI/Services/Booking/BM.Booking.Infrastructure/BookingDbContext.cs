using BM.Booking.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public DbSet<BookingOrder> BookingOrders { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<BookingServPro> BookingServPros { get; set; }
        public DbSet<BookingReview> BookingReviews { get; set; }
        public DbSet<BookingPromotion> BookingPromotions { get; set; }
        public DbSet<BookingAppointment> BookingAppointments { get; set; }
        public DbSet<BookingInvoice> BookingInvoices { get; set; }
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingService>()
                .HasMany(bs => bs.BookingAppointments)
                .WithOne(ba => ba.BookingService)
                .HasForeignKey(ba => ba.servID); //(1-n)

            modelBuilder.Entity<BookingService>()
                .HasMany(bs => bs.BookingServPros)
                .WithOne(bsp => bsp.BookingService)
                .HasForeignKey(bs => bs.servID); //(1-n)

            modelBuilder.Entity<BookingAppointment>()
                .HasOne(ba => ba.BookingReviews)
                .WithOne(bs => bs.BookingAppointment)
                .HasForeignKey<BookingReview>(ba => ba.appID); //(1-1)

            modelBuilder.Entity<BookingAppointment>()
                .HasOne(ba => ba.BookingOrder)
                .WithMany(bo => bo.BookingAppointments)
                .HasForeignKey(ba => ba.orderID); //(1-n)

            modelBuilder.Entity<BookingPromotion>()
                .HasMany(bp => bp.BookingServPros)
                .WithOne(bsp => bsp.BookingPromotion)
                .HasForeignKey(bsp => bsp.promoID); //(1-n)

            // Sửa mối quan hệ 1-1 giữa BookingInvoice và BookingOrder
            modelBuilder.Entity<BookingInvoice>()
                .HasOne(bi => bi.BookingOrder)
                .WithOne(bo => bo.BookingInvoice)
                .HasForeignKey<BookingInvoice>(bi => bi.orderID); // Khóa ngoại nằm ở BookingInvoice

            base.OnModelCreating(modelBuilder);
        }
    }
}
